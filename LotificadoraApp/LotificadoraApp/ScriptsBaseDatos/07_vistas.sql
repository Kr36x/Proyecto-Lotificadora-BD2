use Grupo8
go

CREATE OR ALTER VIEW dbo.vw_lotes_disponibles
AS
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
WHERE l.estadoLote = 'disponible';
GO

CREATE OR ALTER VIEW dbo.vw_creditos_activos_cliente
AS
SELECT
    c.idCliente,
    c.nombres + ' ' + c.apellidos AS cliente,
    v.idVenta,
    vc.idVentaCredito,
    vc.plazoAnios,
    vc.tasaInteresAnual,
    vc.estadoCredito,
    pp.idPlanPago,
    pp.totalPlan,
    dbo.fn_credito_saldo_pendiente(vc.idVentaCredito) AS saldoPendiente
FROM VentaCredito vc
INNER JOIN Venta v ON v.idVenta = vc.idVenta
INNER JOIN Cliente c ON c.idCliente = v.idCliente
INNER JOIN PlanPago pp ON pp.idVentaCredito = vc.idVentaCredito
WHERE vc.estadoCredito IN ('activo', 'moroso');
GO
