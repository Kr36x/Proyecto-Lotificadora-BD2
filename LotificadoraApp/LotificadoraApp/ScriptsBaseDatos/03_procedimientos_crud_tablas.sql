USE Grupo8
GO

-- =======================================================
--         PROCEDIMIENTOS PARA PROYECTO
-- =======================================================

--INSERT
CREATE PROCEDURE sp_proyecto_insertar
    @nombreProyecto VARCHAR(150),
    @descripcion VARCHAR(255) = NULL,
    @fechaInicio DATE,
    @fechaFinEstimada DATE = NULL,
    @areaTotalV2 DECIMAL(18,2),
    @maxAniosFinanciamiento INT,
    @estado VARCHAR(30) = 'activo'
AS
BEGIN
    BEGIN TRY
        INSERT INTO Proyecto (nombreProyecto, descripcion, fechaInicio, fechaFinEstimada, areaTotalV2, maxAniosFinanciamiento, estado)
        VALUES (@nombreProyecto, @descripcion, @fechaInicio, @fechaFinEstimada, @areaTotalV2, @maxAniosFinanciamiento, @estado);
        
        -- Opcional: Devolver el ID del proyecto recién insertado
        SELECT SCOPE_IDENTITY() AS idProyectoGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

-- ACTUALIZAR
CREATE PROCEDURE sp_proyecto_actualizar
    @idProyecto INT,
    @nombreProyecto VARCHAR(150),
    @descripcion VARCHAR(255) = NULL,
    @fechaInicio DATE,
    @fechaFinEstimada DATE = NULL,
    @areaTotalV2 DECIMAL(18,2),
    @maxAniosFinanciamiento INT,
    @estado VARCHAR(30)
AS
BEGIN
    BEGIN TRY
        UPDATE Proyecto
        SET nombreProyecto = @nombreProyecto,
            descripcion = @descripcion,
            fechaInicio = @fechaInicio,
            fechaFinEstimada = @fechaFinEstimada,
            areaTotalV2 = @areaTotalV2,
            maxAniosFinanciamiento = @maxAniosFinanciamiento,
            estado = @estado
        WHERE idProyecto = @idProyecto;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--ELIMINAR
CREATE PROCEDURE sp_proyecto_eliminar
    @idProyecto INT
AS
BEGIN
    BEGIN TRY
        -- Eliminación física. Si el registro tiene dependencias (llaves foráneas), 
        -- el bloque CATCH capturará el error de integridad referencial.
        DELETE FROM Proyecto
        WHERE idProyecto = @idProyecto;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--OBTENER
CREATE PROCEDURE sp_proyecto_obtener
    @idProyecto INT
AS
BEGIN
    SELECT 
        idProyecto, 
        nombreProyecto, 
        descripcion, 
        fechaInicio, 
        fechaFinEstimada, 
        areaTotalV2, 
        maxAniosFinanciamiento, 
        estado
    FROM Proyecto
    WHERE idProyecto = @idProyecto;
END;
GO

--LISTAR
CREATE PROCEDURE sp_proyecto_listar
AS
BEGIN
    SELECT 
        idProyecto, 
        nombreProyecto, 
        descripcion, 
        fechaInicio, 
        fechaFinEstimada, 
        areaTotalV2, 
        maxAniosFinanciamiento, 
        estado
    FROM Proyecto
    ORDER BY idProyecto DESC; -- Los ordenamos del más reciente al más antiguo
END;
GO

-- =======================================================
--                 PROCEDIMIENTOS PARA ETAPA
-- =======================================================

--INSERTAR
CREATE PROCEDURE sp_etapa_insertar
    @idProyecto INT,
    @nombreEtapa VARCHAR(100),
    @fechaInicio DATE,
    @fechaFinEstimada DATE = NULL,
    @areaTotalV2 DECIMAL(18,2),
    @porcentajeAreaVerde DECIMAL(5,2),
    @porcentajeAreaComun DECIMAL(5,2),
    @porcentajeAreaLotes DECIMAL(5,2),
    @precioVaraCuadrada DECIMAL(18,2),
    @tasaInteresAnual DECIMAL(5,2),
    @estado VARCHAR(30) = 'activa'
AS
BEGIN
    BEGIN TRY
        INSERT INTO Etapa (
            idProyecto, nombreEtapa, fechaInicio, fechaFinEstimada, 
            areaTotalV2, porcentajeAreaVerde, porcentajeAreaComun, 
            porcentajeAreaLotes, precioVaraCuadrada, tasaInteresAnual, estado
        )
        VALUES (
            @idProyecto, @nombreEtapa, @fechaInicio, @fechaFinEstimada, 
            @areaTotalV2, @porcentajeAreaVerde, @porcentajeAreaComun, 
            @porcentajeAreaLotes, @precioVaraCuadrada, @tasaInteresAnual, @estado
        );
        
        SELECT SCOPE_IDENTITY() AS idEtapaGenerada;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--ACTUALIZAR
CREATE PROCEDURE sp_etapa_actualizar
    @idEtapa INT,
    @idProyecto INT,
    @nombreEtapa VARCHAR(100),
    @fechaInicio DATE,
    @fechaFinEstimada DATE = NULL,
    @areaTotalV2 DECIMAL(18,2),
    @porcentajeAreaVerde DECIMAL(5,2),
    @porcentajeAreaComun DECIMAL(5,2),
    @porcentajeAreaLotes DECIMAL(5,2),
    @precioVaraCuadrada DECIMAL(18,2),
    @tasaInteresAnual DECIMAL(5,2),
    @estado VARCHAR(30)
AS
BEGIN
    BEGIN TRY
        UPDATE Etapa
        SET idProyecto = @idProyecto,
            nombreEtapa = @nombreEtapa,
            fechaInicio = @fechaInicio,
            fechaFinEstimada = @fechaFinEstimada,
            areaTotalV2 = @areaTotalV2,
            porcentajeAreaVerde = @porcentajeAreaVerde,
            porcentajeAreaComun = @porcentajeAreaComun,
            porcentajeAreaLotes = @porcentajeAreaLotes,
            precioVaraCuadrada = @precioVaraCuadrada,
            tasaInteresAnual = @tasaInteresAnual,
            estado = @estado
        WHERE idEtapa = @idEtapa;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--ELIMINAR
CREATE PROCEDURE sp_etapa_eliminar
    @idEtapa INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Etapa
        WHERE idEtapa = @idEtapa;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--OBTENER
CREATE PROCEDURE sp_etapa_obtener
    @idEtapa INT
AS
BEGIN
    SELECT 
        idEtapa, idProyecto, nombreEtapa, fechaInicio, fechaFinEstimada, 
        areaTotalV2, porcentajeAreaVerde, porcentajeAreaComun, 
        porcentajeAreaLotes, precioVaraCuadrada, tasaInteresAnual, estado
    FROM Etapa
    WHERE idEtapa = @idEtapa;
END;
GO

--LISTAR
CREATE PROCEDURE sp_etapa_listar
AS
BEGIN
    SELECT 
        idEtapa, idProyecto, nombreEtapa, fechaInicio, fechaFinEstimada, 
        areaTotalV2, porcentajeAreaVerde, porcentajeAreaComun, 
        porcentajeAreaLotes, precioVaraCuadrada, tasaInteresAnual, estado
    FROM Etapa
    ORDER BY idEtapa DESC;
END;
GO

-- =======================================================
--            PROCEDIMIENTOS PARA BLOQUE
-- =======================================================

--INSERTAR
CREATE PROCEDURE sp_bloque_insertar
    @idEtapa INT,
    @nombreBloque VARCHAR(50),
    @descripcion VARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO Bloque (idEtapa, nombreBloque, estado, descripcion)
        VALUES (@idEtapa, @nombreBloque, 4, @descripcion);
        
        -- Devuelve el ID generado automáticamente
        SELECT SCOPE_IDENTITY() AS idBloqueGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--Actualizar
CREATE PROCEDURE sp_bloque_actualizar
    @idBloque INT,
    @idEtapa INT,
    @nombreBloque VARCHAR(50),
    @descripcion VARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE Bloque
        SET idEtapa = @idEtapa,
            nombreBloque = @nombreBloque,
            descripcion = @descripcion
        WHERE idBloque = @idBloque;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--ELIMINAR
CREATE PROCEDURE sp_bloque_eliminar
    @idBloque INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Bloque
        WHERE idBloque = @idBloque;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--OBTENER POR ID
CREATE PROCEDURE sp_bloque_obtener
    @idBloque INT
AS
BEGIN
    SELECT 
        idBloque, 
        idEtapa, 
        nombreBloque, 
        descripcion
    FROM Bloque
    WHERE idBloque = @idBloque;
END;
GO

--LISTAR
CREATE PROCEDURE sp_bloque_listar
AS
BEGIN
    SELECT 
        idBloque, 
        idEtapa, 
        nombreBloque, 
        descripcion
    FROM Bloque
    ORDER BY idBloque DESC;
END;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA LOTE
-- =======================================================

--INSERTAR
CREATE PROCEDURE sp_lote_insertar
    @idBloque INT,
    @numeroLote VARCHAR(20),
    @areaV2 DECIMAL(18,2),
    @esEsquina BIT = 0,
    @cercaParque BIT = 0,
    @calleCerrada BIT = 0,
    @precioBase DECIMAL(18,2) = 0,
    @recargoTotal DECIMAL(18,2) = 0,
    @precioFinal DECIMAL(18,2) = 0,
    @estadoLote VARCHAR(30) = 'disponible'
AS
BEGIN
    BEGIN TRY
        INSERT INTO Lote (idBloque, numeroLote, areaV2, esEsquina, cercaParque, calleCerrada, precioBase, recargoTotal, precioFinal, estadoLote)
        VALUES (@idBloque, @numeroLote, @areaV2, @esEsquina, @cercaParque, @calleCerrada, @precioBase, @recargoTotal, @precioFinal, @estadoLote);
        SELECT SCOPE_IDENTITY() AS idLoteGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--ACTUALIZAR
CREATE PROCEDURE sp_lote_actualizar
    @idLote INT,
    @idBloque INT,
    @numeroLote VARCHAR(20),
    @areaV2 DECIMAL(18,2),
    @esEsquina BIT,
    @cercaParque BIT,
    @calleCerrada BIT,
    @precioBase DECIMAL(18,2),
    @recargoTotal DECIMAL(18,2),
    @precioFinal DECIMAL(18,2),
    @estadoLote VARCHAR(30)
AS
BEGIN
    BEGIN TRY
        UPDATE Lote
        SET idBloque = @idBloque, numeroLote = @numeroLote, areaV2 = @areaV2, esEsquina = @esEsquina, 
            cercaParque = @cercaParque, calleCerrada = @calleCerrada, precioBase = @precioBase, 
            recargoTotal = @recargoTotal, precioFinal = @precioFinal, estadoLote = @estadoLote
        WHERE idLote = @idLote;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--ELIMINAR
CREATE PROCEDURE sp_lote_eliminar
    @idLote INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Lote WHERE idLote = @idLote;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

--OBTENER
CREATE PROCEDURE sp_lote_obtener
    @idLote INT
AS
BEGIN
    SELECT * FROM Lote WHERE idLote = @idLote;
END;
GO

--LISTAR
CREATE PROCEDURE sp_lote_listar
AS
BEGIN
    SELECT * FROM Lote ORDER BY idLote DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_listar_lotes_disponibles
AS
BEGIN
    SELECT
        idLote,
        idProyecto,
        nombreProyecto,
        idEtapa,
        nombreEtapa,
        idBloque,
        nombreBloque,
        numeroLote,
        precioFinalCalculado
    FROM dbo.vw_lotes_disponibles
    ORDER BY idProyecto, idEtapa, idBloque, numeroLote;
END;
GO

--exec sp_listar_lotes_disponibles;

CREATE OR ALTER PROCEDURE dbo.sp_obtener_detalle_lote_disponible
    @idLote INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.idLote,
        v.idProyecto,
        v.nombreProyecto,
        v.idEtapa,
        v.nombreEtapa,
        v.idBloque,
        v.nombreBloque,
        v.numeroLote,
        v.precioFinalCalculado,
        e.tasaInteresAnual
    FROM dbo.vw_lotes_disponibles v
    INNER JOIN Lote l ON l.idLote = v.idLote
    INNER JOIN Bloque b ON b.idBloque = l.idBloque
    INNER JOIN Etapa e ON e.idEtapa = b.idEtapa
    WHERE v.idLote = @idLote;
END;
GO
--exec dbo.sp_obtener_detalle_lote_disponible @idLote = 4;


-- =======================================================
-- PROCEDIMIENTOS PARA CLIENTE
-- =======================================================
CREATE PROCEDURE sp_cliente_insertar
    @identidad VARCHAR(20),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @fechaNacimiento DATE = NULL,
    @telefono VARCHAR(20) = NULL,
    @correo VARCHAR(120) = NULL,
    @direccion VARCHAR(255) = NULL,
    @estadoCivilId INT = NULL,
    @rtn VARCHAR(20) = NULL,
    @estado VARCHAR(20) = 'activo'
AS
BEGIN
    BEGIN TRY
        INSERT INTO Cliente (identidad, nombres, apellidos, fechaNacimiento, telefono, correo, direccion, estadoCivilId, rtn, estado)
        VALUES (@identidad, @nombres, @apellidos, @fechaNacimiento, @telefono, @correo, @direccion, @estadoCivilId, @rtn, @estado);
        SELECT SCOPE_IDENTITY() AS idClienteGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_cliente_actualizar
    @idCliente INT,
    @identidad VARCHAR(20),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @fechaNacimiento DATE = NULL,
    @telefono VARCHAR(20) = NULL,
    @correo VARCHAR(120) = NULL,
    @direccion VARCHAR(255) = NULL,
    @estadoCivilId INT = NULL,
    @rtn VARCHAR(20) = NULL,
    @estado VARCHAR(20)
AS
BEGIN
    BEGIN TRY
        UPDATE Cliente
        SET identidad = @identidad, nombres = @nombres, apellidos = @apellidos, fechaNacimiento = @fechaNacimiento, 
            telefono = @telefono, correo = @correo, direccion = @direccion, estadoCivilId = @estadoCivilId, rtn = @rtn, estado = @estado
        WHERE idCliente = @idCliente;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_cliente_eliminar
    @idCliente INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Cliente WHERE idCliente = @idCliente;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_cliente_obtener
    @idCliente INT
AS
BEGIN
    SELECT
        c.idCliente,
        c.identidad,
        c.nombres,
        c.apellidos,
        c.fechaNacimiento,
        c.telefono,
        c.correo,
        c.direccion,
        c.estadoCivilId,
        ec.descripcion AS estadoCivil,
        c.rtn,
        c.estado
    FROM Cliente c
    LEFT JOIN EstadoCivil ec
        ON c.estadoCivilId = ec.id
    WHERE c.idCliente = @idCliente;
END;
GO


CREATE OR ALTER PROCEDURE sp_cliente_listar
AS
BEGIN
    SELECT
        c.idCliente,
        c.identidad,
        c.nombres,
        c.apellidos,
        c.fechaNacimiento,
        c.telefono,
        c.correo,
        c.direccion,
        c.estadoCivilId,
        ec.descripcion AS estadoCivil,
        c.rtn,
        c.estado
    FROM Cliente c
    LEFT JOIN EstadoCivil ec
        ON c.estadoCivilId = ec.id
    ORDER BY c.idCliente DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_cliente_listar_activo
AS
BEGIN
    SELECT
        idCliente,
        CONCAT(idCliente, ' - ', nombres, ' ', apellidos) AS nombreCompleto
        FROM Cliente
        WHERE estado = 'activo'
        ORDER BY nombres, apellidos;
END;
GO

--exec sp_cliente_listar_activo;

CREATE OR ALTER PROCEDURE dbo.sp_obtener_resumen_cliente_capacidad_pago
    @idCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.nombres + ' ' + c.apellidos AS cliente,
        dl.ingresoMensual
    FROM Cliente c
    INNER JOIN DatosLaboralesCliente dl
        ON dl.idCliente = c.idCliente
    WHERE c.idCliente = @idCliente
      AND c.estado = 'activo';
END;
GO
-- exec dbo.sp_obtener_resumen_cliente_capacidad_pago @idCliente = @idCliente

-- =======================================================
-- PROCEDIMIENTOS PARA AVAL
-- =======================================================
CREATE PROCEDURE sp_aval_insertar
    @identidad VARCHAR(20),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @telefono VARCHAR(20) = NULL,
    @direccion VARCHAR(255) = NULL,
    @lugarTrabajo VARCHAR(150) = NULL,
    @ingresoMensual DECIMAL(18,2) = NULL,
    @parentescoId INT = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO Aval (identidad, nombres, apellidos, telefono, direccion, lugarTrabajo, ingresoMensual, parentescoId)
        VALUES (@identidad, @nombres, @apellidos, @telefono, @direccion, @lugarTrabajo, @ingresoMensual, @parentescoId);
        SELECT SCOPE_IDENTITY() AS idAvalGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_aval_actualizar
    @idAval INT,
    @identidad VARCHAR(20),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @telefono VARCHAR(20) = NULL,
    @direccion VARCHAR(255) = NULL,
    @lugarTrabajo VARCHAR(150) = NULL,
    @ingresoMensual DECIMAL(18,2) = NULL,
    @parentescoId INT = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE Aval
        SET identidad = @identidad, nombres = @nombres, apellidos = @apellidos, telefono = @telefono, 
            direccion = @direccion, lugarTrabajo = @lugarTrabajo, ingresoMensual = @ingresoMensual, parentescoId = @parentescoId
        WHERE idAval = @idAval;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_aval_eliminar
    @idAval INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Aval WHERE idAval = @idAval;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_aval_obtener
    @idAval INT
AS
BEGIN
    SELECT * FROM Aval WHERE idAval = @idAval;
END;
GO

CREATE PROCEDURE sp_aval_listar
AS
BEGIN
    SELECT * FROM Aval ORDER BY idAval DESC;
END;
GO

CREATE PROCEDURE sp_aval_listar_comboBox
AS
BEGIN
    SELECT
        idAval,
        CONCAT(idAval, ' - ', nombres, ' ', apellidos) AS nombreCompleto
        FROM Aval
        ORDER BY nombres, apellidos;
END;
GO
exec sp_aval_listar_comboBox;



-- =======================================================
-- PROCEDIMIENTOS PARA BENEFICIARIO
-- =======================================================
CREATE PROCEDURE sp_beneficiario_insertar
    @identidad VARCHAR(20),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @telefono VARCHAR(20) = NULL,
    @parentescoId INT = NULL,
    @direccion VARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO Beneficiario (identidad, nombres, apellidos, telefono, parentescoId, direccion)
        VALUES (@identidad, @nombres, @apellidos, @telefono, @parentescoId, @direccion);
        SELECT SCOPE_IDENTITY() AS idBeneficiarioGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_beneficiario_actualizar
    @idBeneficiario INT,
    @identidad VARCHAR(20),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @telefono VARCHAR(20) = NULL,
    @parentescoId INT = NULL,
    @direccion VARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE Beneficiario
        SET identidad = @identidad, nombres = @nombres, apellidos = @apellidos, 
            telefono = @telefono, parentescoId = @parentescoId, direccion = @direccion
        WHERE idBeneficiario = @idBeneficiario;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_beneficiario_eliminar
    @idBeneficiario INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Beneficiario WHERE idBeneficiario = @idBeneficiario;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_beneficiario_obtener
    @idBeneficiario INT
AS
BEGIN
    SELECT * FROM Beneficiario WHERE idBeneficiario = @idBeneficiario;
END;
GO

CREATE PROCEDURE sp_beneficiario_listar
AS
BEGIN
    SELECT * FROM Beneficiario ORDER BY idBeneficiario DESC;
END;
GO

CREATE PROCEDURE sp_beneficiario_listar_comboBox
AS
BEGIN
     SELECT
        idBeneficiario,
        CONCAT(idBeneficiario, ' - ', nombres, ' ', apellidos) AS nombreCompleto
        FROM Beneficiario
        ORDER BY nombres, apellidos;
END;
GO

--exec sp_beneficiario_listar_comboBox






-- =======================================================
-- PROCEDIMIENTOS PARA BANCO
-- =======================================================
CREATE PROCEDURE sp_banco_insertar
    @nombreBanco VARCHAR(100),
    @estado VARCHAR(20) = 'activo'
AS
BEGIN
    BEGIN TRY
        INSERT INTO Banco (nombreBanco, estado)
        VALUES (@nombreBanco, @estado);
        SELECT SCOPE_IDENTITY() AS idBancoGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_banco_actualizar
    @idBanco INT,
    @nombreBanco VARCHAR(100),
    @estado VARCHAR(20)
AS
BEGIN
    BEGIN TRY
        UPDATE Banco
        SET nombreBanco = @nombreBanco, estado = @estado
        WHERE idBanco = @idBanco;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_banco_eliminar
    @idBanco INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Banco WHERE idBanco = @idBanco;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_banco_obtener
    @idBanco INT
AS
BEGIN
    SELECT * FROM Banco WHERE idBanco = @idBanco;
END;
GO

CREATE PROCEDURE sp_banco_listar
AS
BEGIN
    SELECT * FROM Banco ORDER BY idBanco DESC;
END;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA CUENTA BANCARIA
-- =======================================================
CREATE PROCEDURE sp_cuenta_bancaria_insertar
    @idBanco INT,
    @idEtapa INT,
    @numeroCuenta VARCHAR(50),
    @tipoCuenta VARCHAR(30),
    @saldoActual DECIMAL(18,2) = 0,
    @estado VARCHAR(20) = 'activa'
AS
BEGIN
    BEGIN TRY
        INSERT INTO CuentaBancaria (idBanco, idEtapa, numeroCuenta, tipoCuenta, saldoActual, estado)
        VALUES (@idBanco, @idEtapa, @numeroCuenta, @tipoCuenta, @saldoActual, @estado);
        SELECT SCOPE_IDENTITY() AS idCuentaBancariaGenerada;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_cuenta_bancaria_actualizar
    @idCuentaBancaria INT,
    @idBanco INT,
    @idEtapa INT,
    @numeroCuenta VARCHAR(50),
    @tipoCuenta VARCHAR(30),
    @saldoActual DECIMAL(18,2),
    @estado VARCHAR(20)
AS
BEGIN
    BEGIN TRY
        UPDATE CuentaBancaria
        SET idBanco = @idBanco, idEtapa = @idEtapa, numeroCuenta = @numeroCuenta, 
            tipoCuenta = @tipoCuenta, saldoActual = @saldoActual, estado = @estado
        WHERE idCuentaBancaria = @idCuentaBancaria;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_cuenta_bancaria_eliminar
    @idCuentaBancaria INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM CuentaBancaria WHERE idCuentaBancaria = @idCuentaBancaria;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_cuenta_bancaria_obtener
    @idCuentaBancaria INT
AS
BEGIN
    SELECT * FROM CuentaBancaria WHERE idCuentaBancaria = @idCuentaBancaria;
END;
GO

CREATE PROCEDURE sp_cuenta_bancaria_listar
AS
BEGIN
    SELECT * FROM CuentaBancaria ORDER BY idCuentaBancaria DESC;
END;
GO

USE Grupo8;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA TIPO DE GASTO
-- =======================================================

-- 1. INSERTAR TIPO DE GASTO
CREATE PROCEDURE sp_tipo_gasto_insertar
    @nombreTipoGasto VARCHAR(100),
    @estado VARCHAR(20) = 'activo'
AS
BEGIN
    BEGIN TRY
        INSERT INTO TipoGasto (nombreTipoGasto, estado)
        VALUES (@nombreTipoGasto, @estado);
        
        SELECT SCOPE_IDENTITY() AS idTipoGastoGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

-- 2. ACTUALIZAR TIPO DE GASTO
CREATE PROCEDURE sp_tipo_gasto_actualizar
    @idTipoGasto INT,
    @nombreTipoGasto VARCHAR(100),
    @estado VARCHAR(20)
AS
BEGIN
    BEGIN TRY
        UPDATE TipoGasto
        SET nombreTipoGasto = @nombreTipoGasto,
            estado = @estado
        WHERE idTipoGasto = @idTipoGasto;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

-- 3. ELIMINAR TIPO DE GASTO
CREATE PROCEDURE sp_tipo_gasto_eliminar
    @idTipoGasto INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM TipoGasto 
        WHERE idTipoGasto = @idTipoGasto;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

-- 4. LISTAR TIPOS DE GASTO
CREATE PROCEDURE sp_tipo_gasto_listar
AS
BEGIN
    SELECT 
        idTipoGasto, 
        nombreTipoGasto, 
        estado
    FROM TipoGasto
    ORDER BY idTipoGasto DESC;
END;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA PARENTESCO
-- =======================================================

CREATE OR ALTER PROCEDURE dbo.sp_parentesco_listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id,
        descripcion
    FROM Parentesco
    ORDER BY id;
END;
GO

--exec dbo.sp_parentesco_listar;
-- =======================================================
-- PROCEDIMIENTOS PARA ESTADO CIVIL
-- =======================================================

CREATE OR ALTER PROCEDURE dbo.sp_estado_civil_listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id,
        descripcion
    FROM EstadoCivil
    ORDER BY descripcion;
END;
GO

--exec dbo.sp_estado_civil_listar