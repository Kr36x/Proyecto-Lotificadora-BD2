use Grupo8
go

-- -------------------------
-- FUNCIONES ESCALARES
-- -------------------------

CREATE OR ALTER FUNCTION dbo.fn_lote_precio_base
(
    @idLote INT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @precio DECIMAL(18,2) = 0;

    SELECT
        @precio = ROUND(l.areaV2 * e.precioVaraCuadrada, 2)
    FROM Lote l
    INNER JOIN Bloque b ON b.idBloque = l.idBloque
    INNER JOIN Etapa e ON e.idEtapa = b.idEtapa
    WHERE l.idLote = @idLote;

    RETURN ISNULL(@precio, 0);
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_lote_recargo_total
(
    @idLote INT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @base DECIMAL(18,2) = dbo.fn_lote_precio_base(@idLote);
    DECLARE @recargoBanderas DECIMAL(18,2) = 0;
    DECLARE @recargoCatalogo DECIMAL(18,2) = 0;

    SELECT
        @recargoBanderas =
            (CASE WHEN l.esEsquina = 1 THEN @base * 0.05 ELSE 0 END) +
            (CASE WHEN l.cercaParque = 1 THEN @base * 0.04 ELSE 0 END) +
            (CASE WHEN l.calleCerrada = 1 THEN @base * 0.03 ELSE 0 END)
    FROM Lote l
    WHERE l.idLote = @idLote;

    SELECT
        @recargoCatalogo = ISNULL(SUM(
            CASE
                WHEN cl.tipoRecargo = 'porcentaje' THEN @base * (cl.valorRecargo / 100.0)
                ELSE cl.valorRecargo
            END
        ), 0)
    FROM LoteCaracteristica lc
    INNER JOIN CaracteristicaLote cl ON cl.idCaracteristica = lc.idCaracteristica
    WHERE lc.idLote = @idLote
      AND cl.estado = 'activo';

    RETURN ROUND(ISNULL(@recargoBanderas, 0) + ISNULL(@recargoCatalogo, 0), 2);
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_lote_precio_final
(
    @idLote INT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN ROUND(
        dbo.fn_lote_precio_base(@idLote) + dbo.fn_lote_recargo_total(@idLote),
        2
    );
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_cuota_saldo_pendiente
(
    @idCuota INT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @montoCuota DECIMAL(18,2) = 0;
    DECLARE @pagado DECIMAL(18,2) = 0;
    DECLARE @saldo DECIMAL(18,2) = 0;

    SELECT @montoCuota = montoCuota
    FROM Cuota
    WHERE idCuota = @idCuota;

    SELECT @pagado = ISNULL(SUM(montoAplicado), 0)
    FROM DetallePagoCuota
    WHERE idCuota = @idCuota;

    SET @saldo = ISNULL(@montoCuota, 0) - ISNULL(@pagado, 0);
    IF @saldo < 0 SET @saldo = 0;

    RETURN @saldo;
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_credito_saldo_pendiente
(
    @idVentaCredito INT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @totalPlan DECIMAL(18,2) = 0;
    DECLARE @pagado DECIMAL(18,2) = 0;
    DECLARE @saldo DECIMAL(18,2) = 0;

    SELECT @totalPlan = ISNULL(pp.totalPlan, 0)
    FROM PlanPago pp
    WHERE pp.idVentaCredito = @idVentaCredito;

    SELECT @pagado = ISNULL(SUM(dpc.montoAplicado), 0)
    FROM PlanPago pp
    INNER JOIN Cuota c ON c.idPlanPago = pp.idPlanPago
    INNER JOIN DetallePagoCuota dpc ON dpc.idCuota = c.idCuota
    WHERE pp.idVentaCredito = @idVentaCredito;

    SET @saldo = @totalPlan - @pagado;
    IF @saldo < 0 SET @saldo = 0;

    RETURN @saldo;
END;
GO

-- -------------------------
-- FUNCIONES TIPO TABLA
-- -------------------------

CREATE OR ALTER FUNCTION dbo.fn_tvf_lotes_disponibles
(
    @idEtapa INT = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        p.idProyecto,
        p.nombreProyecto,
        e.idEtapa,
        e.nombreEtapa,
        b.idBloque,
        b.nombreBloque,
        l.idLote,
        l.numeroLote,
        l.areaV2,
        dbo.fn_lote_precio_base(l.idLote) AS precioBaseCalculado,
        dbo.fn_lote_recargo_total(l.idLote) AS recargoCalculado,
        dbo.fn_lote_precio_final(l.idLote) AS precioFinalCalculado
    FROM Lote l
    INNER JOIN Bloque b ON b.idBloque = l.idBloque
    INNER JOIN Etapa e ON e.idEtapa = b.idEtapa
    INNER JOIN Proyecto p ON p.idProyecto = e.idProyecto
    WHERE l.estadoLote = 'disponible'
      AND (@idEtapa IS NULL OR e.idEtapa = @idEtapa)
);
GO

CREATE OR ALTER FUNCTION dbo.fn_tvf_estado_cuenta_cliente
(
    @idCliente INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        c.idCliente,
        c.nombres + ' ' + c.apellidos AS cliente,
        v.idVenta,
        vc.idVentaCredito,
        pp.idPlanPago,
        cu.idCuota,
        cu.numeroCuota,
        cu.fechaVencimiento,
        cu.montoCuota,
        ISNULL(pg.totalPagado, 0) AS totalPagado,
        dbo.fn_cuota_saldo_pendiente(cu.idCuota) AS saldoPendiente,
        cu.estadoCuota
    FROM Cliente c
    INNER JOIN Venta v ON v.idCliente = c.idCliente AND v.tipoVenta = 'credito'
    INNER JOIN VentaCredito vc ON vc.idVenta = v.idVenta
    INNER JOIN PlanPago pp ON pp.idVentaCredito = vc.idVentaCredito
    INNER JOIN Cuota cu ON cu.idPlanPago = pp.idPlanPago
    OUTER APPLY
    (
        SELECT SUM(dpc.montoAplicado) AS totalPagado
        FROM DetallePagoCuota dpc
        WHERE dpc.idCuota = cu.idCuota
    ) pg
    WHERE c.idCliente = @idCliente
);
GO

CREATE OR ALTER FUNCTION dbo.fn_tvf_plan_pago_por_credito
(
    @idVentaCredito INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        pp.idPlanPago,
        cu.idCuota,
        cu.numeroCuota,
        cu.fechaVencimiento,
        cu.saldoInicial,
        cu.capitalProgramado,
        cu.interesProgramado,
        cu.montoCuota,
        cu.saldoFinal,
        ISNULL(pg.totalPagado, 0) AS totalPagado,
        dbo.fn_cuota_saldo_pendiente(cu.idCuota) AS saldoPendiente,
        cu.estadoCuota
    FROM PlanPago pp
    INNER JOIN Cuota cu ON cu.idPlanPago = pp.idPlanPago
    OUTER APPLY
    (
        SELECT SUM(dpc.montoAplicado) AS totalPagado
        FROM DetallePagoCuota dpc
        WHERE dpc.idCuota = cu.idCuota
    ) pg
    WHERE pp.idVentaCredito = @idVentaCredito
);
GO

CREATE OR ALTER FUNCTION dbo.fn_tvf_pagos_por_rango
(
    @fechaInicio DATE,
    @fechaFin DATE,
    @idEtapa INT = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        pa.idPago,
        CAST(pa.fechaPago AS DATE) AS fechaPago,
        pa.formaPago,
        pa.montoTotal,
        pa.numeroReferencia,
        v.idVenta,
        e.idEtapa,
        e.nombreEtapa,
        p.nombreProyecto,
        cb.numeroCuenta
    FROM Pago pa
    INNER JOIN Venta v ON v.idVenta = pa.idVenta
    INNER JOIN Lote l ON l.idLote = v.idLote
    INNER JOIN Bloque b ON b.idBloque = l.idBloque
    INNER JOIN Etapa e ON e.idEtapa = b.idEtapa
    INNER JOIN Proyecto p ON p.idProyecto = e.idProyecto
    LEFT JOIN CuentaBancaria cb ON cb.idCuentaBancaria = pa.idCuentaBancaria
    WHERE CAST(pa.fechaPago AS DATE) BETWEEN @fechaInicio AND @fechaFin
      AND (@idEtapa IS NULL OR e.idEtapa = @idEtapa)
);
GO

CREATE OR ALTER FUNCTION dbo.fn_tvf_gastos_proyecto
(
    @idProyecto INT,
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        g.idGasto,
        g.idProyecto,
        p.nombreProyecto,
        g.idEtapa,
        e.nombreEtapa,
        tg.nombreTipoGasto,
        g.fechaGasto,
        g.descripcion,
        g.monto,
        g.estado,
        cb.numeroCuenta
    FROM GastoProyecto g
    INNER JOIN Proyecto p ON p.idProyecto = g.idProyecto
    INNER JOIN Etapa e ON e.idEtapa = g.idEtapa
    INNER JOIN TipoGasto tg ON tg.idTipoGasto = g.idTipoGasto
    INNER JOIN CuentaBancaria cb ON cb.idCuentaBancaria = g.idCuentaBancaria
    WHERE g.idProyecto = @idProyecto
      AND (@fechaInicio IS NULL OR CAST(g.fechaGasto AS DATE) >= @fechaInicio)
      AND (@fechaFin IS NULL OR CAST(g.fechaGasto AS DATE) <= @fechaFin)
);
GO