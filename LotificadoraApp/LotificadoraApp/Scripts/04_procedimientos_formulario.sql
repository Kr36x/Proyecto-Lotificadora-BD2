use Grupo8
go

-- =========================================
-- 1. Ventas a crédito activas para combo
-- =========================================
CREATE OR ALTER PROCEDURE dbo.sp_listar_ventas_credito_activas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idVenta,
        idVentaCredito,
        idCliente,
        CONCAT(
            'Venta ', idVenta,
            ' | Cliente ', cliente,
            ' | Lote ', numeroLote
        ) AS descripcion
    FROM dbo.vw_creditos_activos_cliente
    ORDER BY idVenta DESC;
END;
GO
    
-- =========================================
-- 2. Detalle de una venta crédito
-- =========================================
CREATE OR ALTER PROCEDURE dbo.sp_obtener_detalle_venta_credito
    @idVenta INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.idVenta,
        vc.idVentaCredito,
        c.nombres + ' ' + c.apellidos AS cliente,
        l.numeroLote AS lote,
        e.idEtapa,
        e.nombreEtapa
    FROM Venta v
    INNER JOIN VentaCredito vc ON vc.idVenta = v.idVenta
    INNER JOIN Cliente c ON c.idCliente = v.idCliente
    INNER JOIN Lote l ON l.idLote = v.idLote
    INNER JOIN Bloque b ON b.idBloque = l.idBloque
    INNER JOIN Etapa e ON e.idEtapa = b.idEtapa
    WHERE v.idVenta = @idVenta;
END;
GO

-- =========================================
-- 3. Cuotas pendientes por venta
-- =========================================
CREATE OR ALTER PROCEDURE dbo.sp_listar_cuotas_pendientes_por_venta
    @idVenta INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.idCuota,
        c.numeroCuota,
        c.fechaVencimiento,
        c.montoCuota,
        ISNULL(SUM(dpc.montoAplicado), 0) AS totalPagado,
        c.montoCuota - ISNULL(SUM(dpc.montoAplicado), 0) AS saldoPendiente,
        c.estadoId,
        e.nombre AS estadoCuota,
        CONCAT(
            'Cuota ', c.numeroCuota,
            ' | Vence ', CONVERT(varchar(10), c.fechaVencimiento, 103),
            ' | Saldo ', CONVERT(varchar(30), CAST(c.montoCuota - ISNULL(SUM(dpc.montoAplicado), 0) AS decimal(18,2)))
        ) AS descripcion
    FROM Venta v
    INNER JOIN VentaCredito vc ON vc.idVenta = v.idVenta
    INNER JOIN PlanPago pp ON pp.idVentaCredito = vc.idVentaCredito
    INNER JOIN Cuota c ON c.idPlanPago = pp.idPlanPago
    INNER JOIN Estado e ON e.id = c.estadoId
    LEFT JOIN DetallePagoCuota dpc ON dpc.idCuota = c.idCuota
    WHERE v.idVenta = @idVenta
    GROUP BY
        c.idCuota,
        c.numeroCuota,
        c.fechaVencimiento,
        c.montoCuota,
        c.estadoId,
        e.nombre
    HAVING c.montoCuota - ISNULL(SUM(dpc.montoAplicado), 0) > 0
    ORDER BY c.numeroCuota;
END;
GO

-- =========================================
-- 4. Cuentas bancarias activas por etapa
-- =========================================
CREATE OR ALTER PROCEDURE dbo.sp_listar_cuentas_bancarias_por_etapa
    @idEtapa INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idCuentaBancaria,
        CONCAT(numeroCuenta, ' | ', tipoCuenta) AS descripcion
    FROM CuentaBancaria
    WHERE idEtapa = @idEtapa
      AND estadoId = 4
    ORDER BY numeroCuenta;
END;
GO


-- ==================================================================
-- 5. OBTENER RESUMEN CREDITO PARA CONSULTA PLAN PAGO
-- ==================================================================
CREATE OR ALTER PROCEDURE dbo.sp_obtener_resumen_credito
    @idVentaCredito INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        vc.idVentaCredito,
        c.nombres + ' ' + c.apellidos AS cliente,
        pp.idPlanPago,
        pp.totalPlan,
        dbo.fn_credito_saldo_pendiente(vc.idVentaCredito) AS saldoPendiente,
        e.nombre AS estadoCredito
    FROM VentaCredito vc
    INNER JOIN Venta v ON v.idVenta = vc.idVenta
    INNER JOIN Cliente c ON c.idCliente = v.idCliente
    INNER JOIN PlanPago pp ON pp.idVentaCredito = vc.idVentaCredito
    INNER JOIN Estado e ON e.id = vc.estadoId
    WHERE vc.idVentaCredito = @idVentaCredito;
END;
GO

-- ==================================================================
-- 6. registra una venta al crédito
-- ==================================================================
create or alter procedure sp_registrar_venta_credito
    @idLote int,
    @idCliente int,
    @idAval int,
    @idBeneficiario int,
    @fechaVenta date,
    @precioLote decimal(18,2),
    @descuento decimal(18,2),
    @recargo decimal(18,2),
    @prima decimal(18,2),
    @financiaTotal bit,
    @plazoAnios int,
    @fechaInicioPago date
as
begin
    set nocount on

    -- variables de control
    declare @idVenta int
    declare @idVentaCredito int
    declare @tasaInteresAnual decimal(5,2)
    declare @maxAniosFinanciamiento int
    declare @totalVenta decimal(18,2)
    declare @montoFinanciado decimal(18,2)
    declare @numeroCuotas int
    declare @cuotaMensualEstimada decimal(18,2)
    declare @totalInteres decimal(18,2)
    declare @totalPlan decimal(18,2)
    declare @tasaMensual decimal(18,8)

    -- nuevas variables para validación por ingreso
    declare @ingresoMensual decimal(18,2)
    declare @porcentajeMaxIngreso decimal(5,2)
    declare @capacidadMaxima decimal(18,2)

    begin try
        begin transaction

        -- validar cliente
        if not exists (
            select 1
            from Cliente
            where idCliente = @idCliente
              and estadoId = 1
        )
        begin
            raiserror('el cliente no existe o está inactivo', 16, 1)
            rollback transaction
            return
        end

        -- validar aval
        if not exists (
            select 1
            from Aval
            where idAval = @idAval
        )
        begin
            raiserror('el aval no existe', 16, 1)
            rollback transaction
            return
        end

        -- validar beneficiario
        if not exists (
            select 1
            from Beneficiario
            where idBeneficiario = @idBeneficiario
        )
        begin
            raiserror('el beneficiario no existe', 16, 1)
            rollback transaction
            return
        end

        -- obtener tasa de interés de la etapa y máximo de años del proyecto
        select
            @tasaInteresAnual = e.tasaInteresAnual,
            @maxAniosFinanciamiento = p.maxAniosFinanciamiento
        from Lote l
        inner join Bloque b on l.idBloque = b.idBloque
        inner join Etapa e on b.idEtapa = e.idEtapa
        inner join Proyecto p on e.idProyecto = p.idProyecto
        where l.idLote = @idLote

        -- validar lote
        if @tasaInteresAnual is null or @maxAniosFinanciamiento is null
        begin
            raiserror('el lote no existe o no está asociado correctamente', 16, 1)
            rollback transaction
            return
        end

        -- validar plazo
        if @plazoAnios > @maxAniosFinanciamiento
        begin
            raiserror('el plazo supera el máximo permitido por el proyecto', 16, 1)
            rollback transaction
            return
        end

        -- calcular total de la venta
        set @totalVenta = @precioLote - @descuento + @recargo

        if @totalVenta <= 0
        begin
            raiserror('el total de la venta no es válido', 16, 1)
            rollback transaction
            return
        end

        -- calcular monto financiado
        if @financiaTotal = 1
        begin
            set @prima = 0
            set @montoFinanciado = @totalVenta
        end
        else
        begin
            if @prima <= 0
            begin
                raiserror('si no financia el total, la prima debe ser mayor que cero', 16, 1)
                rollback transaction
                return
            end

            if @prima >= @totalVenta
            begin
                raiserror('la prima no puede ser mayor o igual al total de la venta', 16, 1)
                rollback transaction
                return
            end

            set @montoFinanciado = @totalVenta - @prima
        end

        -- calcular resumen del plan de pago
        set @numeroCuotas = @plazoAnios * 12
        set @tasaMensual = @tasaInteresAnual / 12.0 / 100.0

        if @tasaMensual = 0
        begin
            set @cuotaMensualEstimada = round(@montoFinanciado / @numeroCuotas, 2)
        end
        else
        begin
            set @cuotaMensualEstimada = round(
                @montoFinanciado *
                (
                    @tasaMensual * power(1 + @tasaMensual, @numeroCuotas)
                ) /
                (
                    power(1 + @tasaMensual, @numeroCuotas) - 1
                )
            , 2)
        end

        set @totalPlan = round(@cuotaMensualEstimada * @numeroCuotas, 2)
        set @totalInteres = round(@totalPlan - @montoFinanciado, 2)

        -- nueva validación: capacidad de pago del cliente
        select
            @ingresoMensual = dl.ingresoMensual
        from DatosLaboralesCliente dl
        where dl.idCliente = @idCliente

        if @ingresoMensual is null
        begin
            raiserror('el cliente no tiene datos laborales registrados', 16, 1)
            rollback transaction
            return
        end

        set @porcentajeMaxIngreso = 30
        set @capacidadMaxima = @ingresoMensual * (@porcentajeMaxIngreso / 100.0)

        if @cuotaMensualEstimada > @capacidadMaxima
        begin
            raiserror('la cuota estimada supera el 30%% del ingreso mensual del cliente', 16, 1)
            rollback transaction
            return
        end

        -- insertar venta general
        insert into Venta (
            idLote,
            idCliente,
            fechaVenta,
            tipoVenta,
            precioLote,
            descuento,
            recargo,
            totalVenta,
            estadoId
        )
        values (
            @idLote,
            @idCliente,
            @fechaVenta,
            'credito',
            @precioLote,
            @descuento,
            @recargo,
            @totalVenta, 4)

        set @idVenta = scope_identity()

        -- insertar detalle de venta a crédito
        insert into VentaCredito (
            idVenta,
            idAval,
            idBeneficiario,
            prima,
            financiaTotal,
            montoFinanciado,
            plazoAnios,
            tasaInteresAnual,
            fechaInicioPago,
            estadoId
        )
        values (
            @idVenta,
            @idAval,
            @idBeneficiario,
            @prima,
            @financiaTotal,
            @montoFinanciado,
            @plazoAnios,
            @tasaInteresAnual,
            @fechaInicioPago, 1)

        set @idVentaCredito = scope_identity()

        -- insertar cabecera del plan de pago
        insert into PlanPago (
            idVentaCredito,
            fechaGeneracion,
            totalCapital,
            totalInteres,
            totalPlan,
            cuotaMensualEstimada,
            estadoId
        )
        values (
            @idVentaCredito,
            getdate(),
            @montoFinanciado,
            @totalInteres,
            @totalPlan,
            @cuotaMensualEstimada, 1)

        commit transaction

        -- mostrar resumen
        select
            @idVenta as idVenta,
            @idVentaCredito as idVentaCredito,
            @totalVenta as totalVenta,
            @montoFinanciado as montoFinanciado,
            @tasaInteresAnual as tasaInteresAnual,
            @numeroCuotas as numeroCuotas,
            @cuotaMensualEstimada as cuotaMensualEstimada,
            @totalInteres as totalInteres,
            @totalPlan as totalPlan,
            @ingresoMensual as ingresoMensual,
            @capacidadMaxima as capacidadMaxima
    end try
    begin catch
        if @@trancount > 0
            rollback transaction

        declare @mensajeError varchar(4000)
        set @mensajeError = ERROR_MESSAGE()
        raiserror(@mensajeError, 16, 1)
    end catch
end
go

-- ==================================================================
-- 7. registra un pago y genera su factura
-- ==================================================================
create or alter procedure sp_registrar_pago
    @idVenta int,
    @idCuota int,
    @formaPago varchar(20),
    @montoTotal decimal(18,2),
    @idCuentaBancaria int = null,
    @numeroReferencia varchar(100) = null,
    @observacion varchar(255) = null,
    @idEmpleado INT = NULL
as
begin
    set nocount on

    declare @idPago int
    declare @idFactura int

    declare @montoCuota decimal(18,2)
    declare @estadoCuota int
    declare @capitalProgramado decimal(18,2)
    declare @interesProgramado decimal(18,2)

    declare @totalPagadoAntes decimal(18,2)
    declare @saldoPendiente decimal(18,2)

    declare @montoCapital decimal(18,2)
    declare @montoInteres decimal(18,2)

    declare @nombreCliente varchar(200)
    declare @rtnCliente varchar(20)
    declare @numeroFactura varchar(50)

    begin try
        begin transaction

        if not exists (
            select 1
            from Venta
            where idVenta = @idVenta
              and estadoId = 4
        )
        begin
            raiserror('la venta no existe o no está activa', 16, 1)
            rollback transaction
            return
        end

        select
            @montoCuota = montoCuota,
            @estadoCuota = estadoId,
            @capitalProgramado = capitalProgramado,
            @interesProgramado = interesProgramado
        from Cuota
        where idCuota = @idCuota

        if @montoCuota is null
        begin
            raiserror('la cuota no existe', 16, 1)
            rollback transaction
            return
        end

        if @estadoCuota = 15
        begin
            raiserror('la cuota ya está pagada', 16, 1)
            rollback transaction
            return
        end

        select
            @totalPagadoAntes = isnull(sum(montoAplicado), 0)
        from DetallePagoCuota
        where idCuota = @idCuota

        set @saldoPendiente = @montoCuota - @totalPagadoAntes

        if @montoTotal <= 0
        begin
            raiserror('el monto del pago debe ser mayor que cero', 16, 1)
            rollback transaction
            return
        end

        if @montoTotal > @saldoPendiente
        begin
            raiserror('el monto excede el saldo pendiente de la cuota', 16, 1)
            rollback transaction
            return
        end

        if @formaPago not in ('efectivo', 'deposito')
        begin
            raiserror('la forma de pago no es válida', 16, 1)
            rollback transaction
            return
        end

        if @formaPago = 'deposito' and @idCuentaBancaria is null
        begin
            raiserror('debe indicar la cuenta bancaria cuando el pago es por depósito', 16, 1)
            rollback transaction
            return
        end

        if @formaPago = 'deposito' and (isnull(ltrim(rtrim(@numeroReferencia)), '') = '')
        begin
            raiserror('debe indicar el número de referencia cuando el pago es por depósito', 16, 1)
            rollback transaction
            return
        end

        if @montoTotal <= @interesProgramado
        begin
            set @montoInteres = @montoTotal
            set @montoCapital = 0
        end
        else
        begin
            set @montoInteres = @interesProgramado
            set @montoCapital = @montoTotal - @montoInteres
        end

        insert into Pago (
            idVenta,
            fechaPago,
            formaPago,
            montoTotal,
            idCuentaBancaria,
            numeroReferencia,
            depositadoCaja,
            observacion
        )
        values (
            @idVenta,
            getdate(),
            @formaPago,
            @montoTotal,
            @idCuentaBancaria,
            @numeroReferencia,
            case when @formaPago = 'efectivo' then 0 else 1 end,
            @observacion
        )

        set @idPago = scope_identity()

        insert into DetallePagoCuota (
            idPago,
            idCuota,
            montoCapital,
            montoInteres,
            montoAplicado
        )
        values (
            @idPago,
            @idCuota,
            @montoCapital,
            @montoInteres,
            @montoTotal
        )

        select
            @nombreCliente = c.nombres + ' ' + c.apellidos,
            @rtnCliente = c.rtn
        from Venta v
        inner join Cliente c on v.idCliente = c.idCliente
        where v.idVenta = @idVenta

        set @numeroFactura = 'FAC-' + cast(@idPago as varchar(20))

        insert into Factura (
            idPago,
            numeroFactura,
            fechaFactura,
            nombreCliente,
            rtnCliente,
            totalFactura
        )
        values (
            @idPago,
            @numeroFactura,
            getdate(),
            @nombreCliente,
            @rtnCliente,
            @montoTotal
        )

        set @idFactura = scope_identity()

        insert into DetalleFactura (
            idFactura,
            descripcion,
            capital,
            interes,
            subtotal
        )
        values (
            @idFactura,
            'pago de cuota',
            @montoCapital,
            @montoInteres,
            @montoTotal
        )

        if @formaPago = 'efectivo'
        begin
            insert into ControlCaja (
                idPago,
                idDepositoCaja,
                idEmpleado,
                fechaMovimiento,
                tipoMovimiento,
                monto,
                observacion
            )
            values (
                @idPago,
                null,
                @idEmpleado,
                getdate(),
                'recepcion_efectivo',
                @montoTotal,
                @observacion
            )
        end

        commit transaction

        select
            @idPago as idPago,
            @idFactura as idFactura,
            @montoTotal as montoPagado,
            @montoCapital as montoCapital,
            @montoInteres as montoInteres,
            @numeroFactura as numeroFactura
    end try
    begin catch
        if @@trancount > 0
            rollback transaction

        declare @mensajeError varchar(4000)
        set @mensajeError = error_message()

        raiserror(@mensajeError, 16, 1)
    end catch
end
go

-- ==================================================================
-- 8. funcion para validar los lotes disponibles por cliente
-- ==================================================================

CREATE OR ALTER FUNCTION dbo.fn_tvf_lotes_aptos_por_cliente
(
    @idCliente INT,
    @prima DECIMAL(18,2),
    @plazoAnios INT,
    @porcentajeMaxIngreso DECIMAL(5,2)
)
RETURNS TABLE
AS
RETURN
(
        WITH ClienteIngresos AS
    (
        SELECT
            c.idCliente,
            c.nombres + ' ' + c.apellidos AS cliente,
            dl.ingresoMensual
        FROM Cliente c
        INNER JOIN DatosLaboralesCliente dl
            ON dl.idCliente = c.idCliente
        WHERE c.idCliente = @idCliente
          AND c.estadoId = 1
    ),
    LotesDisponibles AS
    (
        SELECT
            v.idProyecto,
            v.nombreProyecto,
            v.idEtapa,
            v.nombreEtapa,
            v.idBloque,
            v.nombreBloque,
            v.idLote,
            v.numeroLote,
            v.areaV2,
            v.precioFinalCalculado,
            e.tasaInteresAnual,
            p.maxAniosFinanciamiento
        FROM dbo.vw_lotes_disponibles v
        INNER JOIN Lote l
            ON l.idLote = v.idLote
        INNER JOIN Bloque b
            ON b.idBloque = l.idBloque
        INNER JOIN Etapa e
            ON e.idEtapa = b.idEtapa
        INNER JOIN Proyecto p
            ON p.idProyecto = e.idProyecto
        WHERE @plazoAnios <= p.maxAniosFinanciamiento
    )
    SELECT
        ci.idCliente,
        ci.cliente,
        ci.ingresoMensual,
        ld.idProyecto,
        ld.nombreProyecto,
        ld.idEtapa,
        ld.nombreEtapa,
        ld.idBloque,
        ld.nombreBloque,
        ld.idLote,
        ld.numeroLote,
        ld.areaV2,
        ld.precioFinalCalculado AS precioFinalLote,

        CASE
            WHEN @prima < 0 THEN ld.precioFinalCalculado
            WHEN @prima >= ld.precioFinalCalculado THEN 0
            ELSE ld.precioFinalCalculado - @prima
        END AS montoFinanciado,

        ld.tasaInteresAnual,

        CAST(
            CASE
                WHEN @plazoAnios <= 0 THEN 0
                WHEN
                    CASE
                        WHEN @prima < 0 THEN ld.precioFinalCalculado
                        WHEN @prima >= ld.precioFinalCalculado THEN 0
                        ELSE ld.precioFinalCalculado - @prima
                    END <= 0
                THEN 0
                ELSE
                    CASE
                        WHEN (ld.tasaInteresAnual / 12.0 / 100.0) = 0
                        THEN
                            (
                                CASE
                                    WHEN @prima < 0 THEN ld.precioFinalCalculado
                                    WHEN @prima >= ld.precioFinalCalculado THEN 0
                                    ELSE ld.precioFinalCalculado - @prima
                                END
                            ) / (@plazoAnios * 12.0)
                        ELSE
                            (
                                (
                                    CASE
                                        WHEN @prima < 0 THEN ld.precioFinalCalculado
                                        WHEN @prima >= ld.precioFinalCalculado THEN 0
                                        ELSE ld.precioFinalCalculado - @prima
                                    END
                                )
                                *
                                (
                                    (ld.tasaInteresAnual / 12.0 / 100.0)
                                    *
                                    POWER(1 + (ld.tasaInteresAnual / 12.0 / 100.0), @plazoAnios * 12)
                                )
                                /
                                (
                                    POWER(1 + (ld.tasaInteresAnual / 12.0 / 100.0), @plazoAnios * 12) - 1
                                )
                            )
                    END
            END
        AS DECIMAL(18,2)) AS cuotaEstimada,

        CAST(ci.ingresoMensual * (@porcentajeMaxIngreso / 100.0) AS DECIMAL(18,2)) AS capacidadMaxima,

        CAST(
            CASE
                WHEN ci.ingresoMensual <= 0 THEN 0
                ELSE
                    (
                        CAST(
                            CASE
                                WHEN @plazoAnios <= 0 THEN 0
                                WHEN
                                    CASE
                                        WHEN @prima < 0 THEN ld.precioFinalCalculado
                                        WHEN @prima >= ld.precioFinalCalculado THEN 0
                                        ELSE ld.precioFinalCalculado - @prima
                                    END <= 0
                                THEN 0
                                ELSE
                                    CASE
                                        WHEN (ld.tasaInteresAnual / 12.0 / 100.0) = 0
                                        THEN
                                            (
                                                CASE
                                                    WHEN @prima < 0 THEN ld.precioFinalCalculado
                                                    WHEN @prima >= ld.precioFinalCalculado THEN 0
                                                    ELSE ld.precioFinalCalculado - @prima
                                                END
                                            ) / (@plazoAnios * 12.0)
                                        ELSE
                                            (
                                                (
                                                    CASE
                                                        WHEN @prima < 0 THEN ld.precioFinalCalculado
                                                        WHEN @prima >= ld.precioFinalCalculado THEN 0
                                                        ELSE ld.precioFinalCalculado - @prima
                                                    END
                                                )
                                                *
                                                (
                                                    (ld.tasaInteresAnual / 12.0 / 100.0)
                                                    *
                                                    POWER(1 + (ld.tasaInteresAnual / 12.0 / 100.0), @plazoAnios * 12)
                                                )
                                                /
                                                (
                                                    POWER(1 + (ld.tasaInteresAnual / 12.0 / 100.0), @plazoAnios * 12) - 1
                                                )
                                            )
                                    END
                            END
                        AS DECIMAL(18,2)) / ci.ingresoMensual
                    ) * 100
            END
        AS DECIMAL(18,2)) AS porcentajeIngresoComprometido,

        CASE
            WHEN ci.ingresoMensual <= 0 THEN 'NO APTO'
            WHEN
                CAST(
                    CASE
                        WHEN @plazoAnios <= 0 THEN 0
                        WHEN
                            CASE
                                WHEN @prima < 0 THEN ld.precioFinalCalculado
                                WHEN @prima >= ld.precioFinalCalculado THEN 0
                                ELSE ld.precioFinalCalculado - @prima
                            END <= 0
                        THEN 0
                        ELSE
                            CASE
                                WHEN (ld.tasaInteresAnual / 12.0 / 100.0) = 0
                                THEN
                                    (
                                        CASE
                                            WHEN @prima < 0 THEN ld.precioFinalCalculado
                                            WHEN @prima >= ld.precioFinalCalculado THEN 0
                                            ELSE ld.precioFinalCalculado - @prima
                                        END
                                    ) / (@plazoAnios * 12.0)
                                ELSE
                                    (
                                        (
                                            CASE
                                                WHEN @prima < 0 THEN ld.precioFinalCalculado
                                                WHEN @prima >= ld.precioFinalCalculado THEN 0
                                                ELSE ld.precioFinalCalculado - @prima
                                            END
                                        )
                                        *
                                        (
                                            (ld.tasaInteresAnual / 12.0 / 100.0)
                                            *
                                            POWER(1 + (ld.tasaInteresAnual / 12.0 / 100.0), @plazoAnios * 12)
                                        )
                                        /
                                        (
                                            POWER(1 + (ld.tasaInteresAnual / 12.0 / 100.0), @plazoAnios * 12) - 1
                                        )
                                    )
                            END
                    END
                AS DECIMAL(18,2))
                <= CAST(ci.ingresoMensual * (@porcentajeMaxIngreso / 100.0) AS DECIMAL(18,2))
            THEN 'APTO'
            ELSE 'NO APTO'
        END AS resultado
    FROM ClienteIngresos ci
    CROSS JOIN LotesDisponibles ld
);
GO

-- ==================================================================
-- 9. SP para obtener datos laborales por id del cliente
-- ==================================================================


CREATE OR ALTER PROCEDURE dbo.sp_datos_laborales_obtener_por_cliente
    @idCliente INT
AS
BEGIN
    SELECT TOP 1 *
    FROM DatosLaboralesCliente
    WHERE idCliente = @idCliente;
END;
GO



-- ==================================================================
-- 10. SP para consultar recaudación
-- ==================================================================
CREATE OR ALTER PROCEDURE dbo.sp_consulta_sp_recaudacion_etapa
    @idEtapa INT,
    @fechaInicio DATE,
    @fechaFin DATE
AS
BEGIN
    SELECT
        e.idEtapa,
        e.nombreEtapa,
        COUNT(pa.idPago) AS totalPagos,
        SUM(pa.montoTotal) AS montoRecaudado
    FROM Pago pa
    INNER JOIN Venta v ON v.idVenta = pa.idVenta
    INNER JOIN Lote l ON l.idLote = v.idLote
    INNER JOIN Bloque b ON b.idBloque = l.idBloque
    INNER JOIN Etapa e ON e.idEtapa = b.idEtapa
    WHERE e.idEtapa = @idEtapa
      AND CAST(pa.fechaPago AS DATE) BETWEEN @fechaInicio AND @fechaFin
    GROUP BY e.idEtapa, e.nombreEtapa;
END;
GO

-- ==================================================================
-- 11. SP para insertar datos laborales
-- ==================================================================
CREATE OR ALTER PROCEDURE dbo.sp_datos_laborales_insertar
    @idCliente INT,
    @empresa VARCHAR(150),
    @cargo VARCHAR(100),
    @ingresoMensual DECIMAL(18,2),
    @antiguedadLaboral INT = NULL,
    @telefonoTrabajo VARCHAR(20) = NULL,
    @direccionTrabajo VARCHAR(255) = NULL
AS
BEGIN
    INSERT INTO DatosLaboralesCliente
    (
        idCliente,
        empresa,
        cargo,
        ingresoMensual,
        antiguedadLaboral,
        telefonoTrabajo,
        direccionTrabajo
    )
    VALUES
    (
        @idCliente,
        @empresa,
        @cargo,
        @ingresoMensual,
        @antiguedadLaboral,
        @telefonoTrabajo,
        @direccionTrabajo
    );

    SELECT SCOPE_IDENTITY() AS idDatosLaboralesGenerado;
END;
GO

-- ==================================================================
-- 12. SP para actualizar datos laborales
-- ==================================================================
CREATE OR ALTER PROCEDURE dbo.sp_datos_laborales_actualizar
    @idDatosLaborales INT,
    @idCliente INT,
    @empresa VARCHAR(150),
    @cargo VARCHAR(100),
    @ingresoMensual DECIMAL(18,2),
    @antiguedadLaboral INT = NULL,
    @telefonoTrabajo VARCHAR(20) = NULL,
    @direccionTrabajo VARCHAR(255) = NULL
AS
BEGIN
    UPDATE DatosLaboralesCliente
    SET
        idCliente = @idCliente,
        empresa = @empresa,
        cargo = @cargo,
        ingresoMensual = @ingresoMensual,
        antiguedadLaboral = @antiguedadLaboral,
        telefonoTrabajo = @telefonoTrabajo,
        direccionTrabajo = @direccionTrabajo
    WHERE idDatosLaborales = @idDatosLaborales;
END;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA ESTADO CIVIL
-- =======================================================

CREATE OR ALTER PROCEDURE dbo.sp_estado_civil_listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id,
        descripcion
    FROM EstadoCivil
    ORDER BY descripcion;
END;
GO

--exec dbo.sp_estado_civil_listar

-- =======================================================
-- PROCEDIMIENTOS PARA PARENTESCO
-- =======================================================

CREATE OR ALTER PROCEDURE dbo.sp_parentesco_listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id,
        descripcion
    FROM Parentesco
    ORDER BY id;
END;
GO

CREATE PROCEDURE sp_beneficiario_listar_comboBox
AS
BEGIN
     SELECT
        idBeneficiario,
        CONCAT(idBeneficiario, ' - ', nombres, ' ', apellidos) AS nombreCompleto
        FROM Beneficiario
        ORDER BY nombres, apellidos;
END;
GO
-- =======================================================
-- PROCEDIMIENTOS EXTRAS PARA AVAL
-- =======================================================

CREATE PROCEDURE sp_aval_listar_comboBox
AS
BEGIN
    SELECT
        idAval,
        CONCAT(idAval, ' - ', nombres, ' ', apellidos) AS nombreCompleto
        FROM Aval
        ORDER BY nombres, apellidos;
END;
GO
-- =======================================================
-- PROCEDIMIENTOS EXTRAS PARA CLIENTE
-- =======================================================

CREATE OR ALTER PROCEDURE sp_cliente_listar_activo
AS
BEGIN
    SELECT
        idCliente,
        CONCAT(idCliente, ' - ', nombres, ' ', apellidos) AS nombreCompleto
        FROM Cliente
        WHERE estadoId = 1 -- 1 = activo
        ORDER BY nombres, apellidos;
END;
GO

--exec sp_cliente_listar_activo;

CREATE OR ALTER PROCEDURE dbo.sp_obtener_resumen_cliente_capacidad_pago
    @idCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.nombres + ' ' + c.apellidos AS cliente,
        dl.ingresoMensual
    FROM Cliente c
    INNER JOIN DatosLaboralesCliente dl
        ON dl.idCliente = c.idCliente
    WHERE c.idCliente = @idCliente
      AND c.estadoId = 1; -- 1 = activo
END;
GO
-- exec dbo.sp_obtener_resumen_cliente_capacidad_pago @idCliente = @idCliente
-- =======================================================
-- PROCEDIMIENTOS EXTRAS PARA LOTES
-- =======================================================

CREATE OR ALTER PROCEDURE dbo.sp_listar_lotes_disponibles
AS
BEGIN
    SELECT
        idLote,
        idProyecto,
        nombreProyecto,
        idEtapa,
        nombreEtapa,
        idBloque,
        nombreBloque,
        numeroLote,
        precioFinalCalculado
    FROM dbo.vw_lotes_disponibles
    ORDER BY idProyecto, idEtapa, idBloque, numeroLote;
END;
GO

--exec sp_listar_lotes_disponibles;

CREATE OR ALTER PROCEDURE dbo.sp_obtener_detalle_lote_disponible
    @idLote INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.idLote,
        v.idProyecto,
        v.nombreProyecto,
        v.idEtapa,
        v.nombreEtapa,
        v.idBloque,
        v.nombreBloque,
        v.numeroLote,
        v.precioFinalCalculado,
        e.tasaInteresAnual
    FROM dbo.vw_lotes_disponibles v
    INNER JOIN Lote l ON l.idLote = v.idLote
    INNER JOIN Bloque b ON b.idBloque = l.idBloque
    INNER JOIN Etapa e ON e.idEtapa = b.idEtapa
    WHERE v.idLote = @idLote;
END;
GO
--exec dbo.sp_obtener_detalle_lote_disponible @idLote = 4;

create or alter procedure dbo.sp_listar_ventas_por_cliente
    @idCliente int
as
begin
    set nocount on;

    select
        vc.idVenta,
        concat(
            'Venta #', vc.idVenta,
            ' - Lote ', l.numeroLote,
            ' - Fecha ', convert(varchar(10), v.fechaVenta, 120)
        ) as descripcion
    from VentaCredito vc
    inner join Venta v
        on vc.idVenta = v.idVenta
    inner join Lote l
        on v.idLote = l.idLote
    where v.idCliente = @idCliente
    order by vc.idVenta desc;
end
go

create or alter procedure dbo.sp_consultar_pagos_cliente_venta
    @idCliente int,
    @idVenta int = null,
    @fechaInicio date,
    @fechaFin date
as
begin
    set nocount on;

    select
        p.idPago,
        v.idVenta,
        c.nombres + ' ' + c.apellidos as cliente,
        cu.numeroCuota,
        p.fechaPago,
        p.montoTotal as montoPagado,
        p.formaPago,
        p.numeroReferencia,
        f.numeroFactura,
        p.observacion
    from Pago p
    inner join Venta v
        on p.idVenta = v.idVenta
    inner join Cliente c
        on v.idCliente = c.idCliente
    left join DetallePagoCuota dpc
        on dpc.idPago = p.idPago
    left join Cuota cu
        on cu.idCuota = dpc.idCuota
    left join Factura f
        on f.idPago = p.idPago
    where v.idCliente = @idCliente
      and (@idVenta is null or v.idVenta = @idVenta)
      and cast(p.fechaPago as date) between @fechaInicio and @fechaFin
    order by p.fechaPago desc, p.idPago desc;
end
go

--SP PARA CONTROL DE FLUJO DE CAJA
CREATE OR ALTER PROCEDURE dbo.sp_control_caja_registrar_pago_efectivo
    @idPago INT,
    @idEmpleado INT,
    @observacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS
        (
            SELECT 1
            FROM Pago
            WHERE idPago = @idPago
              AND formaPago = 'efectivo'
        )
        BEGIN
            RAISERROR('El pago no existe o no es en efectivo.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS
        (
            SELECT 1
            FROM Empleado
            WHERE id = @idEmpleado
        )
        BEGIN
            RAISERROR('El empleado no existe.', 16, 1);
            RETURN;
        END

        INSERT INTO ControlCaja
        (
            idPago,
            idDepositoCaja,
            idEmpleado,
            fechaMovimiento,
            tipoMovimiento,
            monto,
            observacion
        )
        SELECT
            p.idPago,
            NULL,
            @idEmpleado,
            p.fechaPago,
            'recepcion_efectivo',
            p.montoTotal,
            COALESCE(@observacion, p.observacion)
        FROM Pago p
        WHERE p.idPago = @idPago
          AND p.formaPago = 'efectivo';
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_control_caja_listar
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.idControlCaja,
        cc.idPago,
        cc.idDepositoCaja,
        cc.idEmpleado,
        e.nombres,
        e.apellidos,
        cc.fechaMovimiento,
        cc.tipoMovimiento,
        cc.monto,
        cc.observacion
    FROM ControlCaja cc
    INNER JOIN Empleado e
        ON cc.idEmpleado = e.id
    WHERE (@fechaInicio IS NULL OR cc.fechaMovimiento >= @fechaInicio)
      AND (@fechaFin IS NULL OR cc.fechaMovimiento < DATEADD(DAY, 1, @fechaFin))
    ORDER BY cc.fechaMovimiento DESC, cc.idControlCaja DESC;
END;
GO
--Para la consulta de la vista de creditos activos
CREATE OR ALTER PROCEDURE dbo.sp_resumen_creditos_activos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idVentaCredito,
        idVenta,
        cliente,
        numeroLote,
        nombreProyecto,
        nombreEtapa,
        montoFinanciado,
        cuotaMensualEstimada,
        saldoPendiente,
        cuotasPendientes,
        cuotasVencidas,
        estadoId
    FROM dbo.vw_creditos_activos_cliente
    ORDER BY idVenta DESC;
END;
GO

--Para el estaod cuenta de un cliente en la consulta del mismo
CREATE OR ALTER PROCEDURE dbo.sp_resumen_estado_cuenta_cliente @IdCliente int 
AS
BEGIN
    SELECT 
        ec.idCliente,
        ec.cliente,
        ec.idCuota,
        ec.numeroCuota,
        ec.fechaVencimiento,
        ec.montoCuota,
        ec.saldoPendiente,
        e.nombre AS estado
        FROM dbo.fn_tvf_estado_cuenta_cliente(@idCliente) ec
        INNER JOIN dbo.Estado e
        ON ec.estadoId = e.id
END
GO
