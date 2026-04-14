use Grupo8;
go

-- -------------------------
-- 1
-- -------------------------
create or alter function dbo.fn_tvf_lotes_disponibles
(
    @idEtapa int = null
)
returns table
as
return (
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

        -- base
        (l.areaV2 * e.precioVaraCuadrada) as precioBaseCalculado,

        -- recargo
        isnull(sum(
            case cl.tipoRecargo
                when 'monto' then cl.valorRecargo
                when 'porcentaje' then (l.areaV2 * e.precioVaraCuadrada * cl.valorRecargo / 100.0)
                else 0
            end
        ),0) as recargoCalculado,

        -- total
        (l.areaV2 * e.precioVaraCuadrada) +
        isnull(sum(
            case cl.tipoRecargo
                when 'monto' then cl.valorRecargo
                when 'porcentaje' then (l.areaV2 * e.precioVaraCuadrada * cl.valorRecargo / 100.0)
                else 0
            end
        ),0) as precioFinalCalculado,

        l.estadoId

    from Lote l
    join Bloque b on l.idBloque = b.idBloque
    join Etapa e on b.idEtapa = e.idEtapa
    join Proyecto p on e.idProyecto = p.idProyecto
    left join LoteCaracteristica lc on lc.idLote = l.idLote
    left join CaracteristicaLote cl 
        on cl.idCaracteristica = lc.idCaracteristica 
       and cl.estado = 'activo'

    where l.estadoId = 7
      and e.estadoId <> 5
      and (@idEtapa is null or e.idEtapa = @idEtapa)

    group by
        p.idProyecto, p.nombreProyecto,
        e.idEtapa, e.nombreEtapa, e.precioVaraCuadrada, e.tasaInteresAnual,
        b.idBloque, b.nombreBloque,
        l.idLote, l.numeroLote, l.areaV2,
        l.esEsquina, l.cercaParque, l.calleCerrada,
        l.estadoId
);
go

-- -------------------------
-- 2
-- -------------------------
create or alter function dbo.fn_tvf_estado_cuenta_cliente
(
    @idCliente int
)
returns table
as
return (
    select
        c.idCliente,
        c.nombres + ' ' + c.apellidos as cliente,
        v.idVenta,
        vc.idVentaCredito,
        pp.idPlanPago,
        cu.idCuota,
        cu.numeroCuota,
        cu.fechaVencimiento,
        cu.montoCuota,
        isnull(pg.totalPagado, 0) as totalPagado,

        case 
            when (cu.montoCuota - isnull(pg.totalPagado,0)) < 0 then 0
            else (cu.montoCuota - isnull(pg.totalPagado,0))
        end as saldoPendiente,

        cu.estadoId

    from Cliente c
    join Venta v on v.idCliente = c.idCliente and v.tipoVenta = 'credito'
    join VentaCredito vc on vc.idVenta = v.idVenta
    join PlanPago pp on pp.idVentaCredito = vc.idVentaCredito
    join Cuota cu on cu.idPlanPago = pp.idPlanPago
    outer apply (
        select sum(montoAplicado) as totalPagado
        from DetallePagoCuota
        where idCuota = cu.idCuota
    ) pg
    where c.idCliente = @idCliente
);
go

-- -------------------------
-- 3
-- -------------------------
create or alter function dbo.fn_tvf_plan_pago_por_credito
(
    @idVentaCredito int
)
returns table
as
return (
    select
        pp.idPlanPago,
        c.idCliente,
        c.identidad as identidadCliente,
        c.nombres + ' ' + c.apellidos as nombreCliente,
        c.telefono,
        pp.totalCapital,
        pp.totalInteres,
        pp.totalPlan,
        pp.cuotaMensualEstimada,
        pp.estadoId as estadoPlan,
        cu.idCuota,
        cu.numeroCuota,
        cu.fechaVencimiento,
        cu.saldoInicial,
        cu.capitalProgramado,
        cu.interesProgramado,
        cu.montoCuota,
        cu.saldoFinal,
        isnull(pg.totalPagado, 0) as totalPagado,

        case 
            when (cu.montoCuota - isnull(pg.totalPagado,0)) < 0 then 0
            else (cu.montoCuota - isnull(pg.totalPagado,0))
        end as saldoPendiente,

        cu.estadoId

    from PlanPago pp
    join VentaCredito vc on vc.idVentaCredito = pp.idVentaCredito
    join Venta v on v.idVenta = vc.idVenta
    join Cliente c on c.idCliente = v.idCliente
    join Cuota cu on cu.idPlanPago = pp.idPlanPago
    outer apply (
        select sum(montoAplicado) as totalPagado
        from DetallePagoCuota
        where idCuota = cu.idCuota
    ) pg
    where pp.idVentaCredito = @idVentaCredito
);
go

-- -------------------------
-- 4
-- -------------------------
create or alter function dbo.fn_tvf_pagos_por_rango
(
    @fechaInicio date,
    @fechaFin date,
    @idEtapa int = null
)
returns table
as
return (
    select
        pa.idPago,
        pa.fechaPago,
        pa.formaPago,
        pa.montoTotal,
        pa.numeroReferencia,
        cl.idCliente,
        cl.nombres + ' ' + cl.apellidos as nombreCliente,
        v.idVenta,
        e.idEtapa,
        e.nombreEtapa,
        p.nombreProyecto,
        cb.numeroCuenta
    from Pago pa
    join Venta v on v.idVenta = pa.idVenta
    join Cliente cl on cl.idCliente = v.idCliente
    join Lote l on l.idLote = v.idLote
    join Bloque b on b.idBloque = l.idBloque
    join Etapa e on e.idEtapa = b.idEtapa
    join Proyecto p on p.idProyecto = e.idProyecto
    left join CuentaBancaria cb on cb.idCuentaBancaria = pa.idCuentaBancaria
    where pa.fechaPago >= @fechaInicio
      and pa.fechaPago < dateadd(day,1,@fechaFin)
      and (@idEtapa is null or e.idEtapa = @idEtapa)
);
go

-- -------------------------
-- 5
-- -------------------------
create or alter function dbo.fn_tvf_gastos_proyecto
(
    @idProyecto int,
    @fechaInicio date = null,
    @fechaFin date = null
)
returns table
as
return (
    select
        g.idGasto,
        g.idProyecto,
        p.nombreProyecto,
        g.idEtapa,
        e.nombreEtapa,
        tg.nombreTipoGasto,
        g.fechaGasto,
        g.descripcion,
        g.monto,
        g.estadoId,
        cb.numeroCuenta,
        bk.nombreBanco
    from GastoProyecto g
    join Proyecto p on p.idProyecto = g.idProyecto
    join Etapa e on e.idEtapa = g.idEtapa
    join TipoGasto tg on tg.idTipoGasto = g.idTipoGasto
    join CuentaBancaria cb on cb.idCuentaBancaria = g.idCuentaBancaria
    join Banco bk on bk.idBanco = cb.idBanco
    where g.idProyecto = @idProyecto
      and g.estadoId = 1
      and (@fechaInicio is null or g.fechaGasto >= @fechaInicio)
      and (@fechaFin is null or g.fechaGasto < dateadd(day,1,@fechaFin))
);
go

-- -------------------------
-- 6
-- -------------------------
create or alter function dbo.fn_lotes_disponibles_por_proyecto
(
    @idProyecto int
)
returns table
as
return (
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
        l.precioBase,
        l.recargoTotal,
        l.precioFinal,
        l.estadoId
    from Lote l
    join Bloque b on l.idBloque = b.idBloque
    join Etapa e on b.idEtapa = e.idEtapa
    join Proyecto p on e.idProyecto = p.idProyecto
    where p.idProyecto = @idProyecto
      and l.estadoId = 7
      and e.estadoId <> 5
);
go

-- -------------------------
-- 7
-- -------------------------
create or alter function dbo.fn_historial_pagos_cliente
(
    @idCliente int
)
returns table
as
return (
    select
        c.idCliente,
        c.identidad as identidadCliente,
        c.nombres + ' ' + c.apellidos as nombreCliente,
        v.idVenta,
        v.tipoVenta,
        v.fechaVenta,
        p.idPago,
        p.fechaPago,
        p.formaPago,
        p.montoTotal,
        p.numeroReferencia,
        case when p.depositadoCaja = 1 then 'Si' else 'No' end as depositadoCaja,
        dpc.idCuota,
        dpc.montoCapital,
        dpc.montoInteres,
        dpc.montoAplicado,
        f.idFactura,
        f.numeroFactura,
        f.fechaFactura,
        f.totalFactura
    from Cliente c
    join Venta v on c.idCliente = v.idCliente
    join Pago p on v.idVenta = p.idVenta
    join DetallePagoCuota dpc on p.idPago = dpc.idPago
    left join Factura f on p.idPago = f.idPago
    where c.idCliente = @idCliente
);
go

-- -------------------------
-- 8
-- -------------------------
create or alter function dbo.fn_gastos_por_proyecto
(
    @idProyecto int
)
returns table
as
return (
    select * from dbo.fn_tvf_gastos_proyecto(@idProyecto, null, null)
);
go


