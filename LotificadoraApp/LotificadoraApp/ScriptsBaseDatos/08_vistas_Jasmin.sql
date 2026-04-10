use Grupo8
go

create or alter view dbo.vw_lotes_disponibles
as
select
    p.idProyecto,
    p.nombreProyecto,
    e.idEtapa,
    e.nombreEtapa,
    e.precioVaraCuadrada,
    e.tasaInteresAnual,
    b.idBloque,
    b.nombreBloque,
    l.idLote,
    l.numeroLote,
    l.areaV2,
    case when l.esEsquina = 1 then 'Si' else 'No' end as esEsquina,
    case when l.cercaParque = 1 then 'Si' else 'No' end as cercaParque,
    case when l.calleCerrada = 1 then 'Si' else 'No' end as calleCerrada,

    (l.areaV2 * e.precioVaraCuadrada) as precioBaseCalculado,

    isnull(sum(
        case cl.tipoRecargo
            when 'monto' then cl.valorRecargo
            when 'porcentaje' then (l.areaV2 * e.precioVaraCuadrada * cl.valorRecargo / 100.0)
            else 0
        end
    ),0) as recargoCalculado,

    (l.areaV2 * e.precioVaraCuadrada) +
    isnull(sum(
        case cl.tipoRecargo
            when 'monto' then cl.valorRecargo
            when 'porcentaje' then (l.areaV2 * e.precioVaraCuadrada * cl.valorRecargo / 100.0)
            else 0
        end
    ),0) as precioFinalCalculado,

    l.estadoLote,
    u.departamento,
    u.municipio,
    u.aldeaColonia

from Lote l
join Bloque b on l.idBloque = b.idBloque
join Etapa e on b.idEtapa = e.idEtapa
join Proyecto p on e.idProyecto = p.idProyecto
left join UbicacionProyecto u on p.idProyecto = u.idProyecto
left join LoteCaracteristica lc on lc.idLote = l.idLote
left join CaracteristicaLote cl 
    on cl.idCaracteristica = lc.idCaracteristica 
   and cl.estado = 'activo'

where l.estadoLote = 'disponible'
  and p.estado = 'activo'
  and e.estado <> 'inactiva'

group by
    p.idProyecto,p.nombreProyecto,
    e.idEtapa,e.nombreEtapa,e.precioVaraCuadrada,e.tasaInteresAnual,
    b.idBloque,b.nombreBloque,
    l.idLote,l.numeroLote,l.areaV2,
    l.esEsquina,l.cercaParque,l.calleCerrada,
    l.estadoLote,
    u.departamento,u.municipio,u.aldeaColonia;
go

create or alter view dbo.vw_creditos_activos_cliente
as
select
    vc.idVentaCredito,
    v.idVenta,
    v.fechaVenta,
    c.idCliente,
    c.identidad as identidadCliente,
    c.nombres + ' ' + c.apellidos as cliente,
    c.telefono,
    c.correo,
    l.idLote,
    l.numeroLote,
    b.nombreBloque,
    e.nombreEtapa,
    p.nombreProyecto,
    vc.prima,
    vc.montoFinanciado,
    vc.plazoAnios,
    vc.tasaInteresAnual,
    vc.fechaInicioPago,
    vc.estadoCredito,
    pp.idPlanPago,
    pp.totalCapital,
    pp.totalInteres,
    pp.totalPlan,
    pp.cuotaMensualEstimada,

    isnull(sum(
        case 
            when (cu.montoCuota - isnull(pg.totalPagado,0)) < 0 then 0
            else (cu.montoCuota - isnull(pg.totalPagado,0))
        end
    ),0) as saldoPendiente,

    sum(case when cu.estadoCuota in ('pendiente','parcial','vencida') then 1 else 0 end) as cuotasPendientes,
    sum(case when cu.estadoCuota = 'vencida' then 1 else 0 end) as cuotasVencidas

from VentaCredito vc
join Venta v on vc.idVenta = v.idVenta
join Cliente c on v.idCliente = c.idCliente
join Lote l on v.idLote = l.idLote
join Bloque b on l.idBloque = b.idBloque
join Etapa e on b.idEtapa = e.idEtapa
join Proyecto p on e.idProyecto = p.idProyecto
left join PlanPago pp on vc.idVentaCredito = pp.idVentaCredito
left join Cuota cu on cu.idPlanPago = pp.idPlanPago
outer apply (
    select sum(montoAplicado) as totalPagado
    from DetallePagoCuota
    where idCuota = cu.idCuota
) pg

where vc.estadoCredito in ('activo','moroso')

group by
    vc.idVentaCredito,v.idVenta,v.fechaVenta,
    c.idCliente,c.identidad,c.nombres,c.apellidos,c.telefono,c.correo,
    l.idLote,l.numeroLote,b.nombreBloque,e.nombreEtapa,p.nombreProyecto,
    vc.prima,vc.montoFinanciado,vc.plazoAnios,vc.tasaInteresAnual,vc.fechaInicioPago,vc.estadoCredito,
    pp.idPlanPago,pp.totalCapital,pp.totalInteres,pp.totalPlan,pp.cuotaMensualEstimada;
go


create or alter view dbo.vw_pagos_del_dia
as
select
    p.idPago,
    p.fechaPago,
    p.formaPago,
    p.montoTotal,
    p.numeroReferencia,
    case when p.depositadoCaja = 1 then 'Si' else 'No' end as depositadoCaja,
    c.idCliente,
    c.identidad as identidadCliente,
    c.nombres + ' ' + c.apellidos as nombreCliente,
    v.idVenta,
    v.tipoVenta,
    l.numeroLote,
    b.nombreBloque,
    e.nombreEtapa,
    cb.numeroCuenta,
    bk.nombreBanco,
    f.idFactura,
    f.numeroFactura,
    f.totalFactura
from Pago p
join Venta v on p.idVenta = v.idVenta
join Cliente c on v.idCliente = c.idCliente
join Lote l on v.idLote = l.idLote
join Bloque b on l.idBloque = b.idBloque
join Etapa e on b.idEtapa = e.idEtapa
left join CuentaBancaria cb on p.idCuentaBancaria = cb.idCuentaBancaria
left join Banco bk on cb.idBanco = bk.idBanco
left join Factura f on p.idPago = f.idPago
where p.fechaPago >= cast(getdate() as date)
  and p.fechaPago < dateadd(day,1,cast(getdate() as date));
go


create or alter view dbo.vw_gastos_por_proyecto
as
select
    gp.idGasto,
    p.idProyecto,
    p.nombreProyecto,
    e.idEtapa,
    e.nombreEtapa,
    tg.nombreTipoGasto,
    bk.nombreBanco,
    cb.numeroCuenta,
    cb.saldoActual as saldoActualCuenta,
    gp.fechaGasto,
    gp.descripcion,
    gp.monto,
    gp.estado as estadoGasto,
    u.departamento,
    u.municipio
from GastoProyecto gp
join Proyecto p on gp.idProyecto = p.idProyecto
join Etapa e on gp.idEtapa = e.idEtapa
join TipoGasto tg on gp.idTipoGasto = tg.idTipoGasto
join CuentaBancaria cb on gp.idCuentaBancaria = cb.idCuentaBancaria
join Banco bk on cb.idBanco = bk.idBanco
left join UbicacionProyecto u on p.idProyecto = u.idProyecto
where gp.estado = 'activo';
go


create or alter view dbo.vw_cuentas_bancarias_por_etapa
as
select
    cb.idCuentaBancaria,
    cb.numeroCuenta,
    cb.tipoCuenta,
    cb.saldoActual,
    cb.estado as estadoCuenta,
    bk.idBanco,
    bk.nombreBanco,
    e.idEtapa,
    e.nombreEtapa,
    e.estado as estadoEtapa,
    p.idProyecto,
    p.nombreProyecto,
    p.estado as estadoProyecto
from CuentaBancaria cb
join Banco bk on cb.idBanco = bk.idBanco
join Etapa e on cb.idEtapa = e.idEtapa
join Proyecto p on e.idProyecto = p.idProyecto
where cb.estado = 'activa';
go