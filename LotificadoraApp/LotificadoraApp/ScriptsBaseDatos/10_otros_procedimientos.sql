use Grupo8;
go

-- -------------------------
-- PROCEDIMIENTOS TRANSACCIONALES ADICIONALES
-- -------------------------

CREATE OR ALTER PROCEDURE dbo.sp_registrar_venta_contado_transaccional
    @idLote INT,
    @idCliente INT,
    @fechaVenta DATE,
    @precioLote DECIMAL(18,2),
    @descuento DECIMAL(18,2) = 0,
    @recargo DECIMAL(18,2) = 0,
    @fechaPago DATE = NULL,
    @observacion VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idVenta INT;
    DECLARE @totalVenta DECIMAL(18,2);

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @fechaPago IS NULL
            SET @fechaPago = @fechaVenta;

        IF NOT EXISTS
        (
            SELECT 1
            FROM Cliente
            WHERE idCliente = @idCliente
              AND estado = 'activo'
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
            estadoVenta
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
            'activa'
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

        COMMIT TRANSACTION;

        SELECT
            @idVenta AS idVenta,
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

CREATE OR ALTER PROCEDURE dbo.sp_procesar_deposito_caja_transaccional
    @idCuentaBancaria INT,
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
              AND estado = 'activa'
        )
        BEGIN
            RAISERROR('La cuenta bancaria no existe o esta inactiva.', 16, 1);
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
            estado
        )
        VALUES
        (
            @idProyecto,
            @idEtapa,
            @idTipoGasto,
            @idCuentaBancaria,
            GETDATE(),
            @descripcion,
            @monto,
            'activo'
        );

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

-- -------------------------
-- CRUD ADICIONAL FALTANTE
-- -------------------------

CREATE OR ALTER PROCEDURE dbo.sp_ubicacion_proyecto_insertar
    @idProyecto INT,
    @departamento VARCHAR(100),
    @municipio VARCHAR(100),
    @aldeaColonia VARCHAR(150) = NULL,
    @direccionDetalle VARCHAR(255) = NULL,
    @claveCatastral VARCHAR(100) = NULL,
    @observacionLegal VARCHAR(255) = NULL
AS
BEGIN
    INSERT INTO UbicacionProyecto
    (
        idProyecto,
        departamento,
        municipio,
        aldeaColonia,
        direccionDetalle,
        claveCatastral,
        observacionLegal
    )
    VALUES
    (
        @idProyecto,
        @departamento,
        @municipio,
        @aldeaColonia,
        @direccionDetalle,
        @claveCatastral,
        @observacionLegal
    );

    SELECT SCOPE_IDENTITY() AS idUbicacionGenerada;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ubicacion_proyecto_actualizar
    @idUbicacion INT,
    @idProyecto INT,
    @departamento VARCHAR(100),
    @municipio VARCHAR(100),
    @aldeaColonia VARCHAR(150) = NULL,
    @direccionDetalle VARCHAR(255) = NULL,
    @claveCatastral VARCHAR(100) = NULL,
    @observacionLegal VARCHAR(255) = NULL
AS
BEGIN
    UPDATE UbicacionProyecto
    SET
        idProyecto = @idProyecto,
        departamento = @departamento,
        municipio = @municipio,
        aldeaColonia = @aldeaColonia,
        direccionDetalle = @direccionDetalle,
        claveCatastral = @claveCatastral,
        observacionLegal = @observacionLegal
    WHERE idUbicacion = @idUbicacion;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ubicacion_proyecto_eliminar
    @idUbicacion INT
AS
BEGIN
    DELETE FROM UbicacionProyecto
    WHERE idUbicacion = @idUbicacion;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ubicacion_proyecto_obtener
    @idUbicacion INT
AS
BEGIN
    SELECT *
    FROM UbicacionProyecto
    WHERE idUbicacion = @idUbicacion;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ubicacion_proyecto_listar
AS
BEGIN
    SELECT *
    FROM UbicacionProyecto
    ORDER BY idUbicacion DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_caracteristica_lote_insertar
    @nombreCaracteristica VARCHAR(100),
    @tipoRecargo VARCHAR(30),
    @valorRecargo DECIMAL(18,2),
    @estado VARCHAR(20) = 'activo'
AS
BEGIN
    INSERT INTO CaracteristicaLote
    (
        nombreCaracteristica,
        tipoRecargo,
        valorRecargo,
        estado
    )
    VALUES
    (
        @nombreCaracteristica,
        @tipoRecargo,
        @valorRecargo,
        @estado
    );

    SELECT SCOPE_IDENTITY() AS idCaracteristicaGenerada;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_caracteristica_lote_actualizar
    @idCaracteristica INT,
    @nombreCaracteristica VARCHAR(100),
    @tipoRecargo VARCHAR(30),
    @valorRecargo DECIMAL(18,2),
    @estado VARCHAR(20)
AS
BEGIN
    UPDATE CaracteristicaLote
    SET
        nombreCaracteristica = @nombreCaracteristica,
        tipoRecargo = @tipoRecargo,
        valorRecargo = @valorRecargo,
        estado = @estado
    WHERE idCaracteristica = @idCaracteristica;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_caracteristica_lote_eliminar
    @idCaracteristica INT
AS
BEGIN
    DELETE FROM CaracteristicaLote
    WHERE idCaracteristica = @idCaracteristica;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_caracteristica_lote_obtener
    @idCaracteristica INT
AS
BEGIN
    SELECT *
    FROM CaracteristicaLote
    WHERE idCaracteristica = @idCaracteristica;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_caracteristica_lote_listar
AS
BEGIN
    SELECT *
    FROM CaracteristicaLote
    ORDER BY idCaracteristica DESC;
END;
GO

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

CREATE OR ALTER PROCEDURE dbo.sp_datos_laborales_eliminar
    @idDatosLaborales INT
AS
BEGIN
    DELETE FROM DatosLaboralesCliente
    WHERE idDatosLaborales = @idDatosLaborales;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_datos_laborales_obtener
    @idDatosLaborales INT
AS
BEGIN
    SELECT *
    FROM DatosLaboralesCliente
    WHERE idDatosLaborales = @idDatosLaborales;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_datos_laborales_listar
AS
BEGIN
    SELECT *
    FROM DatosLaboralesCliente
    ORDER BY idDatosLaborales DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_lote_caracteristica_insertar
    @idLote INT,
    @idCaracteristica INT
AS
BEGIN
    INSERT INTO LoteCaracteristica (idLote, idCaracteristica)
    VALUES (@idLote, @idCaracteristica);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_lote_caracteristica_eliminar
    @idLote INT,
    @idCaracteristica INT
AS
BEGIN
    DELETE FROM LoteCaracteristica
    WHERE idLote = @idLote
      AND idCaracteristica = @idCaracteristica;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_lote_caracteristica_listar_por_lote
    @idLote INT
AS
BEGIN
    SELECT
        lc.idLote,
        lc.idCaracteristica,
        cl.nombreCaracteristica,
        cl.tipoRecargo,
        cl.valorRecargo
    FROM LoteCaracteristica lc
    INNER JOIN CaracteristicaLote cl ON cl.idCaracteristica = lc.idCaracteristica
    WHERE lc.idLote = @idLote
    ORDER BY lc.idCaracteristica;
END;
GO

-- -------------------------
-- CONSULTAS PARA FORMULARIOS
-- -------------------------

-- 2 formularios usando vistas
CREATE OR ALTER PROCEDURE dbo.sp_consulta_vista_lotes_disponibles
    @idProyecto INT = NULL,
    @idEtapa INT = NULL
AS
BEGIN
    SELECT *
    FROM dbo.vw_lotes_disponibles
    WHERE (@idProyecto IS NULL OR idProyecto = @idProyecto)
      AND (@idEtapa IS NULL OR idEtapa = @idEtapa)
    ORDER BY idProyecto, idEtapa, idBloque, numeroLote;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_consulta_vista_creditos_activos
    @idCliente INT = NULL
AS
BEGIN
    SELECT *
    FROM dbo.vw_creditos_activos_cliente
    WHERE (@idCliente IS NULL OR idCliente = @idCliente)
    ORDER BY idCliente, idVentaCredito;
END;
GO

-- 2 formularios usando procedimientos almacenados
CREATE OR ALTER PROCEDURE dbo.sp_consulta_sp_estado_cuenta_cliente
    @idCliente INT
AS
BEGIN
    SELECT
        c.idCliente,
        c.nombres + ' ' + c.apellidos AS cliente,
        cu.idCuota,
        cu.numeroCuota,
        cu.fechaVencimiento,
        cu.montoCuota,
        dbo.fn_cuota_saldo_pendiente(cu.idCuota) AS saldoPendiente,
        cu.estadoCuota
    FROM Cliente c
    INNER JOIN Venta v ON v.idCliente = c.idCliente AND v.tipoVenta = 'credito'
    INNER JOIN VentaCredito vc ON vc.idVenta = v.idVenta
    INNER JOIN PlanPago pp ON pp.idVentaCredito = vc.idVentaCredito
    INNER JOIN Cuota cu ON cu.idPlanPago = pp.idPlanPago
    WHERE c.idCliente = @idCliente
    ORDER BY cu.numeroCuota;
END;
GO

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

-- 2 formularios usando funciones tipo tabla
CREATE OR ALTER PROCEDURE dbo.sp_consulta_fn_lotes_disponibles
    @idEtapa INT = NULL
AS
BEGIN
    SELECT *
    FROM dbo.fn_tvf_lotes_disponibles(@idEtapa)
    ORDER BY idProyecto, idEtapa, idBloque, numeroLote;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_consulta_fn_estado_cuenta_cliente
    @idCliente INT
AS
BEGIN
    SELECT *
    FROM dbo.fn_tvf_estado_cuenta_cliente(@idCliente)
    ORDER BY numeroCuota;
END;
GO
