use Grupo8
go

-- registra una venta al crédito
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
    set nocount on;

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

    begin try
        begin transaction

        -- validar cliente
        if not exists (
            select 1
            from Cliente
            where idCliente = @idCliente
              and estado = 'activo'
        )
        begin
            raiserror('el cliente no existe o está inactivo', 16, 1)
            rollback transaction;
            return;
        end

        -- validar aval
        if not exists (
            select 1
            from Aval
            where idAval = @idAval
        )
        begin
            raiserror('el aval no existe', 16, 1)
            rollback transaction;
            return;
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
            estadoVenta
        )
        values (
            @idLote,
            @idCliente,
            @fechaVenta,
            'credito',
            @precioLote,
            @descuento,
            @recargo,
            @totalVenta,
            'activa'
        );

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
            estadoCredito
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
            @fechaInicioPago,
            'activo'
        );

        set @idVentaCredito = scope_identity()

        -- insertar cabecera del plan de pago
        insert into PlanPago (
            idVentaCredito,
            fechaGeneracion,
            totalCapital,
            totalInteres,
            totalPlan,
            cuotaMensualEstimada,
            estado
        )
        values (
            @idVentaCredito,
            getdate(),
            @montoFinanciado,
            @totalInteres,
            @totalPlan,
            @cuotaMensualEstimada,
            'activo'
        );

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
            @totalPlan as totalPlan;
    end try
    begin catch
        if @@trancount > 0
            rollback transaction

        declare @mensajeError varchar(4000)
        set @mensajeError = ERROR_MESSAGE()
        raiserror(@mensajeError, 16, 1);
    end catch
end
go