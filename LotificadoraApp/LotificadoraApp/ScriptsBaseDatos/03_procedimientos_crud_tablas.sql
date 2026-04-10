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
        INSERT INTO Bloque (idEtapa, nombreBloque, descripcion)
        VALUES (@idEtapa, @nombreBloque, @descripcion);
        
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
    @estadoCivil VARCHAR(30) = NULL,
    @rtn VARCHAR(20) = NULL,
    @estado VARCHAR(20) = 'activo'
AS
BEGIN
    BEGIN TRY
        INSERT INTO Cliente (identidad, nombres, apellidos, fechaNacimiento, telefono, correo, direccion, estadoCivil, rtn, estado)
        VALUES (@identidad, @nombres, @apellidos, @fechaNacimiento, @telefono, @correo, @direccion, @estadoCivil, @rtn, @estado);
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
    @estadoCivil VARCHAR(30) = NULL,
    @rtn VARCHAR(20) = NULL,
    @estado VARCHAR(20)
AS
BEGIN
    BEGIN TRY
        UPDATE Cliente
        SET identidad = @identidad, nombres = @nombres, apellidos = @apellidos, fechaNacimiento = @fechaNacimiento, 
            telefono = @telefono, correo = @correo, direccion = @direccion, estadoCivil = @estadoCivil, rtn = @rtn, estado = @estado
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

CREATE PROCEDURE sp_cliente_obtener
    @idCliente INT
AS
BEGIN
    SELECT * FROM Cliente WHERE idCliente = @idCliente;
END;
GO

CREATE PROCEDURE sp_cliente_listar
AS
BEGIN
    SELECT * FROM Cliente ORDER BY idCliente DESC;
END;
GO

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
    @parentescoCliente VARCHAR(50) = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO Aval (identidad, nombres, apellidos, telefono, direccion, lugarTrabajo, ingresoMensual, parentescoCliente)
        VALUES (@identidad, @nombres, @apellidos, @telefono, @direccion, @lugarTrabajo, @ingresoMensual, @parentescoCliente);
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
    @parentescoCliente VARCHAR(50) = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE Aval
        SET identidad = @identidad, nombres = @nombres, apellidos = @apellidos, telefono = @telefono, 
            direccion = @direccion, lugarTrabajo = @lugarTrabajo, ingresoMensual = @ingresoMensual, parentescoCliente = @parentescoCliente
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

-- =======================================================
-- PROCEDIMIENTOS PARA BENEFICIARIO
-- =======================================================
CREATE PROCEDURE sp_beneficiario_insertar
    @identidad VARCHAR(20),
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @telefono VARCHAR(20) = NULL,
    @parentesco VARCHAR(50) = NULL,
    @direccion VARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO Beneficiario (identidad, nombres, apellidos, telefono, parentesco, direccion)
        VALUES (@identidad, @nombres, @apellidos, @telefono, @parentesco, @direccion);
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
    @parentesco VARCHAR(50) = NULL,
    @direccion VARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE Beneficiario
        SET identidad = @identidad, nombres = @nombres, apellidos = @apellidos, 
            telefono = @telefono, parentesco = @parentesco, direccion = @direccion
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