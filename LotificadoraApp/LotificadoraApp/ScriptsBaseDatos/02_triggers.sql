use Grupo8
go

-- valida que el lote esté disponible antes de vender
create trigger tr_venta_validar_lote_disponible
on Venta
for insert
as
begin
    set nocount on

    if exists (
        select 1
        from inserted i
        inner join Lote l on i.idLote = l.idLote
        where l.estadoId <> 7
    )
    begin
        raiserror('el lote no está disponible para la venta', 16, 1);
        rollback transaction
        return
    end
end
go


-- cambia el estado del lote después de registrar la venta
create trigger tr_venta_marcar_lote_vendido
on Venta
for insert
as
begin
    set nocount on

    update l
    set l.estadoId = 9
    from Lote l
    inner join inserted i on i.idLote = l.idLote
end
go

-- valida que la cuenta bancaria corresponda a la etapa del lote
create trigger tr_pago_validar_cuenta_por_etapa
on Pago
for insert
as
begin
    set nocount on

    -- si el pago es depósito, debe llevar cuenta bancaria
    if exists (
        select 1
        from inserted
        where formaPago = 'deposito'
          and idCuentaBancaria is null
    )
    begin
        raiserror('el pago por depósito debe llevar cuenta bancaria', 16, 1);
        rollback transaction
        return
    end

    -- si es efectivo, no debería traer cuenta bancaria
    if exists (
        select 1
        from inserted
        where formaPago = 'efectivo'
          and idCuentaBancaria is not null
    )
    begin
        raiserror('el pago en efectivo no debe llevar cuenta bancaria', 16, 1);
        rollback transaction
        return
    end

    -- la cuenta bancaria debe ser de la misma etapa del lote vendido
    if exists (
        select 1
        from inserted i
        inner join Venta v on i.idVenta = v.idVenta
        inner join Lote l on v.idLote = l.idLote
        inner join Bloque b on l.idBloque = b.idBloque
        inner join CuentaBancaria cb on i.idCuentaBancaria = cb.idCuentaBancaria
        where i.formaPago = 'deposito'
          and b.idEtapa <> cb.idEtapa
    )
    begin
        raiserror('la cuenta bancaria no corresponde a la etapa del lote', 16, 1);
        rollback transaction
        return
    end
end
go

-- actualiza el estado de la cuota según lo pagado
create trigger tr_pago_actualizar_estado_cuota
on DetallePagoCuota
for insert
as
begin
    set nocount on

    update c
    set estadoId =
        case
            when (
                select isnull(sum(d.montoAplicado), 0)
                from DetallePagoCuota d
                where d.idCuota = c.idCuota
            ) = 0 then 13
            when (
                select isnull(sum(d.montoAplicado), 0)
                from DetallePagoCuota d
                where d.idCuota = c.idCuota
            ) < c.montoCuota then 14
            else 15
        end
    from Cuota c
    inner join inserted i on c.idCuota = i.idCuota;
end
go

-- -------------------------
-- TRIGGERS COMPLEMENTARIOS
-- -------------------------

CREATE OR ALTER TRIGGER dbo.tr_etapa_validar_porcentaje_area
ON Etapa
FOR INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        WHERE ABS((i.porcentajeAreaVerde + i.porcentajeAreaComun + i.porcentajeAreaLotes) - 100) > 0.01
    )
    BEGIN
        RAISERROR('La suma de porcentajeAreaVerde + porcentajeAreaComun + porcentajeAreaLotes debe ser 100.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_lote_autocalcular_precios
ON Lote
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    UPDATE l
    SET
        l.precioBase = dbo.fn_lote_precio_base(l.idLote),
        l.recargoTotal = dbo.fn_lote_recargo_total(l.idLote),
        l.precioFinal = dbo.fn_lote_precio_final(l.idLote)
    FROM Lote l
    INNER JOIN (SELECT DISTINCT idLote FROM inserted) i
        ON i.idLote = l.idLote;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_lote_caracteristica_recalcular
ON LoteCaracteristica
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    ;WITH lotesAfectados AS
    (
        SELECT idLote FROM inserted
        UNION
        SELECT idLote FROM deleted
    )
    UPDATE l
    SET
        l.recargoTotal = dbo.fn_lote_recargo_total(l.idLote),
        l.precioFinal = dbo.fn_lote_precio_final(l.idLote)
    FROM Lote l
    INNER JOIN lotesAfectados la ON la.idLote = l.idLote;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_detalle_pago_cuota_validar_monto
ON DetallePagoCuota
FOR INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        WHERE ABS((ISNULL(i.montoCapital,0) + ISNULL(i.montoInteres,0)) - ISNULL(i.montoAplicado,0)) > 0.01
    )
    BEGIN
        RAISERROR('montoAplicado debe ser igual a montoCapital + montoInteres.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM Cuota c
        INNER JOIN (SELECT DISTINCT idCuota FROM inserted) i ON i.idCuota = c.idCuota
        OUTER APPLY
        (
            SELECT ISNULL(SUM(d.montoAplicado), 0) AS totalAplicado
            FROM DetallePagoCuota d
            WHERE d.idCuota = c.idCuota
        ) x
        WHERE x.totalAplicado > c.montoCuota + 0.01
    )
    BEGIN
        RAISERROR('El pago aplicado excede el monto de la cuota.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_pago_deposito_incrementar_saldo
ON Pago
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE cb
    SET cb.saldoActual = cb.saldoActual + x.totalMonto
    FROM CuentaBancaria cb
    INNER JOIN
    (
        SELECT idCuentaBancaria, SUM(montoTotal) AS totalMonto
        FROM inserted
        WHERE formaPago = 'deposito'
          AND idCuentaBancaria IS NOT NULL
        GROUP BY idCuentaBancaria
    ) x ON x.idCuentaBancaria = cb.idCuentaBancaria;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_detalle_deposito_caja_validar
ON DetalleDepositoCaja
FOR INSERT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN Pago p ON p.idPago = i.idPago
        WHERE p.formaPago <> 'efectivo'
           OR p.depositadoCaja = 1
           OR ABS(i.monto - p.montoTotal) > 0.01
    )
    BEGIN
        RAISERROR('Solo se pueden depositar pagos en efectivo no depositados y por el monto exacto.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN DetalleDepositoCaja d ON d.idPago = i.idPago
        GROUP BY i.idPago
        HAVING COUNT(*) > 1
    )
    BEGIN
        RAISERROR('Un pago en efectivo no puede incluirse en mas de un detalle de deposito.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_detalle_deposito_caja_aplicar
ON DetalleDepositoCaja
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET
        p.depositadoCaja = 1,
        p.idCuentaBancaria = dcb.idCuentaBancaria
    FROM Pago p
    INNER JOIN inserted i ON i.idPago = p.idPago
    INNER JOIN DepositoCajaBanco dcb ON dcb.idDepositoCaja = i.idDepositoCaja;

    UPDATE cb
    SET cb.saldoActual = cb.saldoActual + x.totalMonto
    FROM CuentaBancaria cb
    INNER JOIN
    (
        SELECT
            dcb.idCuentaBancaria,
            SUM(i.monto) AS totalMonto
        FROM inserted i
        INNER JOIN DepositoCajaBanco dcb ON dcb.idDepositoCaja = i.idDepositoCaja
        GROUP BY dcb.idCuentaBancaria
    ) x ON x.idCuentaBancaria = cb.idCuentaBancaria;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_gasto_proyecto_validar
ON GastoProyecto
FOR INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN CuentaBancaria cb ON cb.idCuentaBancaria = i.idCuentaBancaria
        WHERE cb.idEtapa <> i.idEtapa
    )
    BEGIN
        RAISERROR('La cuenta bancaria del gasto no pertenece a la etapa indicada.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_gasto_proyecto_aplicar_insert
ON GastoProyecto
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN CuentaBancaria cb ON cb.idCuentaBancaria = i.idCuentaBancaria
        WHERE i.estadoId = 1
          AND cb.saldoActual < i.monto
    )
    BEGIN
        RAISERROR('Saldo insuficiente para registrar el gasto.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    UPDATE cb
    SET cb.saldoActual = cb.saldoActual - x.totalGasto
    FROM CuentaBancaria cb
    INNER JOIN
    (
        SELECT idCuentaBancaria, SUM(monto) AS totalGasto
        FROM inserted
        WHERE estadoId = 1
        GROUP BY idCuentaBancaria
    ) x ON x.idCuentaBancaria = cb.idCuentaBancaria;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_gasto_proyecto_revertir_delete
ON GastoProyecto
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE cb
    SET cb.saldoActual = cb.saldoActual + x.totalReintegro
    FROM CuentaBancaria cb
    INNER JOIN
    (
        SELECT idCuentaBancaria, SUM(monto) AS totalReintegro
        FROM deleted
        WHERE estadoId = 1
        GROUP BY idCuentaBancaria
    ) x ON x.idCuentaBancaria = cb.idCuentaBancaria;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_gasto_proyecto_ajustar_update
ON GastoProyecto
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @mov TABLE
    (
        idCuentaBancaria INT PRIMARY KEY,
        ajuste DECIMAL(18,2)
    );

    INSERT INTO @mov(idCuentaBancaria, ajuste)
    SELECT idCuentaBancaria, SUM(ajuste) AS ajuste
    FROM
    (
        SELECT d.idCuentaBancaria, SUM(d.monto) AS ajuste
        FROM deleted d
        WHERE d.estadoId = 1
        GROUP BY d.idCuentaBancaria
        UNION ALL
        SELECT i.idCuentaBancaria, -SUM(i.monto) AS ajuste
        FROM inserted i
        WHERE i.estadoId = 1
        GROUP BY i.idCuentaBancaria
    ) t
    GROUP BY idCuentaBancaria;

    IF EXISTS
    (
        SELECT 1
        FROM @mov m
        INNER JOIN CuentaBancaria cb ON cb.idCuentaBancaria = m.idCuentaBancaria
        WHERE m.ajuste < 0
          AND (cb.saldoActual + m.ajuste) < 0
    )
    BEGIN
        RAISERROR('La actualizacion del gasto deja saldo bancario negativo.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    UPDATE cb
    SET cb.saldoActual = cb.saldoActual + m.ajuste
    FROM CuentaBancaria cb
    INNER JOIN @mov m ON m.idCuentaBancaria = cb.idCuentaBancaria;
END;
GO



