use DB20192002534

--1. Cargar bloques para el combo del lote
CREATE OR ALTER PROCEDURE sp_lote_cargar_bloques
AS
BEGIN
    SELECT 
        b.idBloque,
        b.nombreBloque,
        e.precioVaraCuadrada
    FROM Bloque b
    INNER JOIN Etapa e
        ON b.idEtapa = e.idEtapa
    ORDER BY b.idBloque;
END;
GO
--2. Obtener lote por id para editar
CREATE OR ALTER PROCEDURE sp_lote_obtener_por_id
    @idLote INT
AS
BEGIN
    SELECT
        idLote,
        idBloque,
        numeroLote,
        areaV2,
        esEsquina,
        cercaParque,
        calleCerrada,
        precioBase,
        recargoTotal,
        precioFinal,
        estadoId
    FROM Lote
    WHERE idLote = @idLote;
END;
GO
--3. Listar lotes para gestión
CREATE OR ALTER PROCEDURE sp_lote_listar_gestion
AS
BEGIN
    SELECT
        l.idLote,
        l.idBloque,
        l.numeroLote,
        l.areaV2,
        b.nombreBloque,
        l.esEsquina,
        l.cercaParque,
        l.calleCerrada,
        l.precioBase,
        l.recargoTotal,
        l.precioFinal,
        l.estadoId,
        e.nombre
    FROM Lote l
    INNER JOIN Bloque b
        ON l.idBloque = b.idBloque
    INNER JOIN Estado e
        ON l.estadoId = e.id
    ORDER BY l.idLote DESC;
END;
GO
--4. Buscar lotes por número y estado
CREATE OR ALTER PROCEDURE sp_lote_buscar
    @numeroLote VARCHAR(20) = '',
    @estadoId INT = 0
AS
BEGIN
    SELECT
        l.idLote,
        l.idBloque,
        l.numeroLote,
        l.areaV2,
        b.nombreBloque,
        l.esEsquina,
        l.cercaParque,
        l.calleCerrada,
        l.precioBase,
        l.recargoTotal,
        l.precioFinal,
        l.estadoId,
        e.nombre
    FROM Lote l
    INNER JOIN Bloque b
        ON l.idBloque = b.idBloque
    INNER JOIN Estado e
        ON l.estadoId = e.id
    WHERE
        (@numeroLote = '' OR l.numeroLote LIKE '%' + @numeroLote + '%')
        AND
        (@estadoId = 0 OR l.estadoId = @estadoId)
    ORDER BY l.idLote DESC;
END;
GO

--Listar cuenta bacncaria segun el lote seleccionado para rellanar combobox
CREATE OR ALTER PROCEDURE dbo.sp_cuenta_bancaria_listar_por_lote
    @idLote INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cb.idCuentaBancaria,
        cb.numeroCuenta,
        cb.tipoCuenta,
        cb.saldoActual,
        cb.estadoId,
        b.nombreBanco,
        e.nombreEtapa,
        CONCAT(b.nombreBanco, ' - ', cb.numeroCuenta, ' - ', e.nombreEtapa) AS descripcion
    FROM CuentaBancaria cb
    INNER JOIN Banco b
        ON cb.idBanco = b.idBanco
    INNER JOIN Etapa e
        ON cb.idEtapa = e.idEtapa
    INNER JOIN Bloque bl
        ON bl.idEtapa = e.idEtapa
    INNER JOIN Lote l
        ON l.idBloque = bl.idBloque
    WHERE l.idLote = @idLote
    ORDER BY b.nombreBanco, cb.numeroCuenta;
END;
GO
--Listar lotes disponibles para los comobobox
CREATE OR ALTER PROCEDURE dbo.sp_listar_lotes_disponibles_combo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idLote,
        numeroLote,
        idEtapa,
        nombreEtapa,
        idBloque,
        nombreBloque,
        nombreProyecto,
        CONCAT(numeroLote, ' - ', nombreEtapa, ' - ', nombreBloque) AS descripcion
    FROM dbo.vw_lotes_disponibles
    ORDER BY nombreProyecto, nombreEtapa, nombreBloque, numeroLote;
END;
GO

--CONTROL DE CAJA



--Resumen para grid
CREATE OR ALTER PROCEDURE dbo.sp_control_caja_resumen_movimientos
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.fechaMovimiento,
        cc.tipoMovimiento,
        cc.monto,
        CONCAT(e.nombres, ' ', e.apellidos) AS empleado,
        cc.observacion
    FROM ControlCaja cc
    INNER JOIN Empleado e
        ON cc.idEmpleado = e.id
    WHERE (@fechaInicio IS NULL OR cc.fechaMovimiento >= @fechaInicio)
      AND (@fechaFin IS NULL OR cc.fechaMovimiento < DATEADD(DAY, 1, @fechaFin))
    ORDER BY cc.fechaMovimiento DESC, cc.idControlCaja DESC;
END;
GO

--Resumen para los labels
CREATE OR ALTER PROCEDURE dbo.sp_control_caja_obtener_resumen
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ISNULL(SUM(CASE WHEN tipoMovimiento = 'recepcion_efectivo' THEN monto ELSE 0 END), 0) AS totalRecibido,
        ISNULL(SUM(CASE WHEN tipoMovimiento = 'deposito_banco' THEN monto ELSE 0 END), 0) AS totalDepositado,
        ISNULL(SUM(CASE
            WHEN tipoMovimiento = 'recepcion_efectivo' THEN monto
            WHEN tipoMovimiento = 'deposito_banco' THEN -monto
            ELSE 0
        END), 0) AS saldoCaja,
        COUNT(*) AS totalMovimientos
    FROM ControlCaja
    WHERE (@fechaInicio IS NULL OR fechaMovimiento >= @fechaInicio)
      AND (@fechaFin IS NULL OR fechaMovimiento < DATEADD(DAY, 1, @fechaFin));
END;
GO

--Consultar pendiente por cuenta/etapa
CREATE OR ALTER PROCEDURE dbo.sp_control_caja_consultar_pendiente_deposito
    @idCuentaBancaria INT,
    @fechaOperacion DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idEtapaCuenta INT;

    IF @fechaOperacion IS NULL
        SET @fechaOperacion = CAST(GETDATE() AS DATE);

    SELECT @idEtapaCuenta = idEtapa
    FROM CuentaBancaria
    WHERE idCuentaBancaria = @idCuentaBancaria
      AND estadoId = 4;

    IF @idEtapaCuenta IS NULL
    BEGIN
        RAISERROR('La cuenta bancaria no existe o esta inactiva.', 16, 1);
        RETURN;
    END

    SELECT
        COUNT(1) AS cantidadPagos,
        ISNULL(SUM(p.montoTotal), 0) AS totalPendiente
    FROM Pago p
    INNER JOIN Venta v ON p.idVenta = v.idVenta
    INNER JOIN Lote l ON v.idLote = l.idLote
    INNER JOIN Bloque b ON l.idBloque = b.idBloque
    WHERE p.formaPago = 'efectivo'
      AND p.depositadoCaja = 0
      AND CAST(p.fechaPago AS DATE) = @fechaOperacion
      AND b.idEtapa = @idEtapaCuenta;
END;
GO

--Combo cuentas disponibles
CREATE OR ALTER PROCEDURE dbo.sp_cuenta_bancaria_listar_activas_combo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cb.idCuentaBancaria,
        CONCAT(b.nombreBanco, ' - ', cb.numeroCuenta, ' - ', e.nombreEtapa) AS descripcion
    FROM CuentaBancaria cb
    INNER JOIN Banco b ON cb.idBanco = b.idBanco
    INNER JOIN Etapa e ON cb.idEtapa = e.idEtapa
    WHERE cb.estadoId = 4
    ORDER BY b.nombreBanco, cb.numeroCuenta;
END;
GO

--FACTURAS

CREATE OR ALTER PROCEDURE dbo.sp_factura_listar
    @numeroFactura VARCHAR(50) = NULL,
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL,
    @nombreCliente VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idFactura,
        idPago,
        numeroFactura,
        fechaFactura,
        nombreCliente,
        rtnCliente,
        totalFactura
    FROM Factura
    WHERE (@numeroFactura IS NULL OR numeroFactura LIKE '%' + @numeroFactura + '%')
      AND (@fechaInicio IS NULL OR CAST(fechaFactura AS DATE) >= @fechaInicio)
      AND (@fechaFin IS NULL OR CAST(fechaFactura AS DATE) <= @fechaFin)
      AND (@nombreCliente IS NULL OR nombreCliente LIKE '%' + @nombreCliente + '%')
    ORDER BY idFactura DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_factura_obtener
    @idFactura INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idFactura,
        idPago,
        numeroFactura,
        fechaFactura,
        nombreCliente,
        rtnCliente,
        totalFactura
    FROM Factura
    WHERE idFactura = @idFactura;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_factura_detalle_listar
    @idFactura INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idDetalleFactura,
        idFactura,
        descripcion,
        capital,
        interes,
        subtotal
    FROM DetalleFactura
    WHERE idFactura = @idFactura
    ORDER BY idDetalleFactura ASC;
END;
GO
