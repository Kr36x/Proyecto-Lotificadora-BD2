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
        where l.estadoLote <> 'disponible'
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
    set l.estadoLote = 'vendido'
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
    set estadoCuota =
        case
            when (
                select isnull(sum(d.montoAplicado), 0)
                from DetallePagoCuota d
                where d.idCuota = c.idCuota
            ) = 0 then 'pendiente'
            when (
                select isnull(sum(d.montoAplicado), 0)
                from DetallePagoCuota d
                where d.idCuota = c.idCuota
            ) < c.montoCuota then 'parcial'
            else 'pagada'
        end
    from Cuota c
    inner join inserted i on c.idCuota = i.idCuota;
end
go

