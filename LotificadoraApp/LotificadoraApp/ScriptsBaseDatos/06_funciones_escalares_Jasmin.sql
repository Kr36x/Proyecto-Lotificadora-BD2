use Grupo8
go

-- -------------------------
-- 1
-- -------------------------
create or alter function dbo.fn_lote_precio_base
(
    @idLote int
)
returns decimal(18,2)
as
begin
    declare @precio decimal(18,2) = 0;

    select @precio = round(l.areaV2 * e.precioVaraCuadrada, 2)
    from Lote l
    join Bloque b on l.idBloque = b.idBloque
    join Etapa e on b.idEtapa = e.idEtapa
    where l.idLote = @idLote;

    return isnull(@precio, 0);
end;
go

-- -------------------------
-- 2
-- -------------------------
create or alter function dbo.fn_lote_recargo_total
(
    @idLote int
)
returns decimal(18,2)
as
begin
    declare @recargoTotal decimal(18,2);

    select @recargoTotal = isnull(sum(
        case cl.tipoRecargo
            when 'monto' then cl.valorRecargo
            when 'porcentaje' then (l.areaV2 * e.precioVaraCuadrada * cl.valorRecargo / 100.0)
            else 0
        end
    ), 0)
    from Lote l
    join Bloque b on l.idBloque = b.idBloque
    join Etapa e on b.idEtapa = e.idEtapa
    left join LoteCaracteristica lc on lc.idLote = l.idLote
    left join CaracteristicaLote cl on lc.idCaracteristica = cl.idCaracteristica
    where l.idLote = @idLote
      and (cl.estado = 'activo' or cl.estado is null);

    return round(isnull(@recargoTotal, 0), 2);
end;
go

-- -------------------------
-- 3
-- -------------------------
create or alter function dbo.fn_lote_precio_final
(
    @idLote int
)
returns decimal(18,2)
as
begin
    declare @precioFinal decimal(18,2);

    select @precioFinal = round(
        (l.areaV2 * e.precioVaraCuadrada) +
        isnull(sum(
            case cl.tipoRecargo
                when 'monto' then cl.valorRecargo
                when 'porcentaje' then (l.areaV2 * e.precioVaraCuadrada * cl.valorRecargo / 100.0)
                else 0
            end
        ), 0)
    ,2)
    from Lote l
    join Bloque b on l.idBloque = b.idBloque
    join Etapa e on b.idEtapa = e.idEtapa
    left join LoteCaracteristica lc on lc.idLote = l.idLote
    left join CaracteristicaLote cl on lc.idCaracteristica = cl.idCaracteristica and cl.estado = 'activo'
    where l.idLote = @idLote
    group by l.areaV2, e.precioVaraCuadrada;

    return isnull(@precioFinal, 0);
end;
go

-- -------------------------
-- 4
-- -------------------------
create or alter function dbo.fn_cuota_saldo_pendiente
(
    @idCuota int
)
returns decimal(18,2)
as
begin
    declare @saldo decimal(18,2);

    select @saldo = 
        case 
            when (c.montoCuota - isnull(sum(dpc.montoAplicado),0)) < 0 then 0
            else (c.montoCuota - isnull(sum(dpc.montoAplicado),0))
        end
    from Cuota c
    left join DetallePagoCuota dpc on dpc.idCuota = c.idCuota
    where c.idCuota = @idCuota
    group by c.montoCuota;

    return isnull(@saldo, 0);
end;
go

-- -------------------------
-- 5
-- -------------------------
create or alter function dbo.fn_credito_saldo_pendiente
(
    @idVentaCredito int
)
returns decimal(18,2)
as
begin
    declare @saldo decimal(18,2);

    select @saldo = isnull(sum(
        case 
            when (c.montoCuota - isnull(dpc.totalPagado,0)) < 0 then 0
            else (c.montoCuota - isnull(dpc.totalPagado,0))
        end
    ), 0)
    from PlanPago pp
    join Cuota c on c.idPlanPago = pp.idPlanPago
    outer apply (
        select sum(montoAplicado) as totalPagado
        from DetallePagoCuota
        where idCuota = c.idCuota
    ) dpc
    where pp.idVentaCredito = @idVentaCredito
      and c.estadoCuota in ('pendiente','parcial','vencida');

    return isnull(@saldo, 0);
end;
go