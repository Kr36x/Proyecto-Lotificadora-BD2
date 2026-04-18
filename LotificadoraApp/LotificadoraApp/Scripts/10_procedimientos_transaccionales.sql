use Grupo8;
go

-- -------------------------
-- 1
-- -------------------------

CREATE OR ALTER PROCEDURE dbo.sp_registrar_venta_contado_transaccional
    @idLote INT,
    @idCliente INT,
    @fechaVenta DATE,
    @precioLote DECIMAL(18,2),
    @descuento DECIMAL(18,2) = 0,
    @recargo DECIMAL(18,2) = 0,
    @fechaPago DATE = NULL,
    @formaPago VARCHAR(20),
    @idCuentaBancaria INT = NULL,
    @numeroReferencia VARCHAR(100) = NULL,
    @idEmpleado INT = NULL,
    @observacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idVenta INT;
    DECLARE @idPago INT;
    DECLARE @idFactura INT;
    DECLARE @totalVenta DECIMAL(18,2);
    DECLARE @nombreCliente VARCHAR(200);
    DECLARE @rtnCliente VARCHAR(20);
    DECLARE @numeroFactura VARCHAR(50);

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @fechaPago IS NULL
            SET @fechaPago = @fechaVenta;

        IF NOT EXISTS
        (
            SELECT 1
            FROM Cliente
            WHERE idCliente = @idCliente
              AND estadoId = 1
        )
        BEGIN
            RAISERROR('El cliente no existe o esta inactivo.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        SET @totalVenta = @precioLote - @descuento + @recargo;

        IF @totalVenta <= 0
        BEGIN
            RAISERROR('El total de la venta debe ser mayor que cero.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @formaPago NOT IN ('efectivo', 'deposito')
        BEGIN
            RAISERROR('La forma de pago no es válida.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @formaPago = 'deposito' AND @idCuentaBancaria IS NULL
        BEGIN
            RAISERROR('Debe indicar la cuenta bancaria cuando el pago es por depósito.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @formaPago = 'deposito' AND (ISNULL(LTRIM(RTRIM(@numeroReferencia)), '') = '')
        BEGIN
            RAISERROR('Debe indicar el número de referencia cuando el pago es por depósito.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO Venta
        (
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
        VALUES
        (
            @idLote,
            @idCliente,
            @fechaVenta,
            'contado',
            @precioLote,
            @descuento,
            @recargo,
            @totalVenta,
            4
        );

        SET @idVenta = SCOPE_IDENTITY();

        INSERT INTO VentaContado
        (
            idVenta,
            fechaPago,
            montoPagado,
            observacion
        )
        VALUES
        (
            @idVenta,
            @fechaPago,
            @totalVenta,
            @observacion
        );

        INSERT INTO Pago
        (
            idVenta,
            fechaPago,
            formaPago,
            montoTotal,
            idCuentaBancaria,
            numeroReferencia,
            depositadoCaja,
            observacion
        )
        VALUES
        (
            @idVenta,
            @fechaPago,
            @formaPago,
            @totalVenta,
            @idCuentaBancaria,
            @numeroReferencia,
            CASE WHEN @formaPago = 'efectivo' THEN 0 ELSE 1 END,
            @observacion
        );

        SET @idPago = SCOPE_IDENTITY();

        SELECT
            @nombreCliente = c.nombres + ' ' + c.apellidos,
            @rtnCliente = c.rtn
        FROM Cliente c
        WHERE c.idCliente = @idCliente;

        SET @numeroFactura = 'FAC-' + CAST(@idPago AS VARCHAR(20));

        INSERT INTO Factura
        (
            idPago,
            numeroFactura,
            fechaFactura,
            nombreCliente,
            rtnCliente,
            totalFactura
        )
        VALUES
        (
            @idPago,
            @numeroFactura,
            GETDATE(),
            @nombreCliente,
            @rtnCliente,
            @totalVenta
        );

        SET @idFactura = SCOPE_IDENTITY();

        INSERT INTO DetalleFactura
        (
            idFactura,
            descripcion,
            capital,
            interes,
            subtotal
        )
        VALUES
        (
            @idFactura,
            'venta al contado',
            @totalVenta,
            0,
            @totalVenta
        );

        IF @formaPago = 'efectivo'
        BEGIN
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
            VALUES
            (
                @idPago,
                NULL,
                @idEmpleado,
                GETDATE(),
                'recepcion_efectivo',
                @totalVenta,
                @observacion
            );
        END

        COMMIT TRANSACTION;

        SELECT
            @idVenta AS idVenta,
            @idPago AS idPago,
            @idFactura AS idFactura,
            @totalVenta AS totalVenta;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @msg VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@msg, 16, 1);
    END CATCH
END;
GO
-- -------------------------
-- 2
-- -------------------------
CREATE OR ALTER PROCEDURE dbo.sp_procesar_deposito_caja_transaccional
    @idCuentaBancaria INT,
    @idEmpleado INT,
    @fechaOperacion DATE = NULL,
    @observacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idDepositoCaja INT;
    DECLARE @totalDepositado DECIMAL(18,2);
    DECLARE @cantidadPagos INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @fechaOperacion IS NULL
            SET @fechaOperacion = CAST(GETDATE() AS DATE);

        IF NOT EXISTS
        (
            SELECT 1
            FROM CuentaBancaria
            WHERE idCuentaBancaria = @idCuentaBancaria
              AND estadoId = 4
        )
        BEGIN
            RAISERROR('La cuenta bancaria no existe o esta inactiva.', 16, 1);
            ROLLBACK TRANSACTION;
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
            ROLLBACK TRANSACTION;
            RETURN;
        END

        SELECT
            @cantidadPagos = COUNT(1),
            @totalDepositado = ISNULL(SUM(p.montoTotal), 0)
        FROM Pago p
        WHERE p.formaPago = 'efectivo'
          AND p.depositadoCaja = 0
          AND CAST(p.fechaPago AS DATE) = @fechaOperacion;

        IF ISNULL(@cantidadPagos, 0) = 0
        BEGIN
            RAISERROR('No hay pagos en efectivo pendientes para depositar en la fecha indicada.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO DepositoCajaBanco
        (
            fechaDeposito,
            idCuentaBancaria,
            totalDepositado,
            observacion
        )
        VALUES
        (
            GETDATE(),
            @idCuentaBancaria,
            @totalDepositado,
            @observacion
        );

        SET @idDepositoCaja = SCOPE_IDENTITY();

        INSERT INTO DetalleDepositoCaja
        (
            idDepositoCaja,
            idPago,
            monto
        )
        SELECT
            @idDepositoCaja,
            p.idPago,
            p.montoTotal
        FROM Pago p
        WHERE p.formaPago = 'efectivo'
          AND p.depositadoCaja = 0
          AND CAST(p.fechaPago AS DATE) = @fechaOperacion;

        UPDATE Pago
        SET depositadoCaja = 1
        WHERE formaPago = 'efectivo'
          AND depositadoCaja = 0
          AND CAST(fechaPago AS DATE) = @fechaOperacion;

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
        VALUES
        (
            NULL,
            @idDepositoCaja,
            @idEmpleado,
            GETDATE(),
            'deposito_banco',
            @totalDepositado,
            @observacion
        );

        COMMIT TRANSACTION;

        SELECT
            @idDepositoCaja AS idDepositoCaja,
            @cantidadPagos AS cantidadPagos,
            @totalDepositado AS totalDepositado;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @msg VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@msg, 16, 1);
    END CATCH
END;
GO

-- -------------------------
-- 3
-- -------------------------
CREATE OR ALTER PROCEDURE dbo.sp_registrar_gasto_proyecto_transaccional
    @idProyecto INT,
    @idEtapa INT,
    @idTipoGasto INT,
    @idCuentaBancaria INT,
    @descripcion VARCHAR(255),
    @monto DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idGasto INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM Etapa
            WHERE idEtapa = @idEtapa
              AND idProyecto = @idProyecto
        )
        BEGIN
            RAISERROR('La etapa no pertenece al proyecto indicado.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO GastoProyecto
        (
            idProyecto,
            idEtapa,
            idTipoGasto,
            idCuentaBancaria,
            fechaGasto,
            descripcion,
            monto,
            estadoId
        )
        VALUES
        (
            @idProyecto,
            @idEtapa,
            @idTipoGasto,
            @idCuentaBancaria,
            GETDATE(),
            @descripcion,
            @monto, 1);

        SET @idGasto = SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SELECT @idGasto AS idGasto;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @msg VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@msg, 16, 1);
    END CATCH
END;
GO




