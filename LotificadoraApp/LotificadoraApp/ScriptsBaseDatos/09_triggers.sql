
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
        WHERE i.estado = 'activo'
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
        WHERE estado = 'activo'
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
        WHERE estado = 'activo'
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
        WHERE d.estado = 'activo'
        GROUP BY d.idCuentaBancaria
        UNION ALL
        SELECT i.idCuentaBancaria, -SUM(i.monto) AS ajuste
        FROM inserted i
        WHERE i.estado = 'activo'
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