USE Grupo8;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA ESTADO
-- =======================================================
CREATE OR ALTER PROCEDURE dbo.sp_ObtenerEstados
    @Ids NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.id,
           e.nombre
    FROM dbo.Estado e
    INNER JOIN (
        SELECT DISTINCT TRY_CAST(LTRIM(RTRIM(value)) AS INT) AS id
        FROM STRING_SPLIT(@Ids, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(value)) AS INT) IS NOT NULL
    ) x ON e.id = x.id
    ORDER BY e.id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_estado_listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.id,
           e.nombre
    FROM dbo.Estado e
    order by e.id asc
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_estado_insertar
    @nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Estado (nombre)
        VALUES (@nombre);

        SELECT CAST(SCOPE_IDENTITY() AS INT) AS idEstadoGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_estado_actualizar
    @id INT,
    @nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Estado
        SET nombre = @nombre
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_estado_eliminar
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Estado
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
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
    @estado INT = 1
AS
BEGIN
    BEGIN TRY
        INSERT INTO Proyecto (nombreProyecto, descripcion, fechaInicio, fechaFinEstimada, areaTotalV2, maxAniosFinanciamiento, estadoId)
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
    @estado INT
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
            estadoId = @estado
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
CREATE OR ALTER PROCEDURE sp_proyecto_obtener
    @estadoId INT
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
        T1.nombre AS estado
    FROM Proyecto AS T0
    INNER JOIN [dbo].[Estado] AS T1 ON T0.estadoId = T1.id
    WHERE T0.estadoId = @estadoId
    ORDER BY idProyecto DESC;
END;
GO

--LISTAR
CREATE OR ALTER PROCEDURE sp_proyecto_listar
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
        T1.nombre AS estado
    FROM Proyecto AS T0
    INNER JOIN [dbo].[Estado] AS T1 ON T0.estadoId = T1.id
    ORDER BY idProyecto DESC;
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
    @estado INT = 4
AS
BEGIN
    BEGIN TRY
        INSERT INTO Etapa (
            idProyecto, nombreEtapa, fechaInicio, fechaFinEstimada, 
            areaTotalV2, porcentajeAreaVerde, porcentajeAreaComun, 
            porcentajeAreaLotes, precioVaraCuadrada, tasaInteresAnual, estadoId
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
    @estado INT
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
            estadoId = @estado
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
        porcentajeAreaLotes, precioVaraCuadrada, tasaInteresAnual, estadoId
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
        porcentajeAreaLotes, precioVaraCuadrada, tasaInteresAnual, estadoId
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
        INSERT INTO Bloque (idEtapa, nombreBloque, estadoId, descripcion)
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
CREATE OR ALTER PROCEDURE sp_bloque_listar
AS
BEGIN
    SELECT 
        b.idBloque, 
        b.idEtapa, 
        b.nombreBloque, 
        b.descripcion,
        e.precioVaraCuadrada
    FROM Bloque b
    INNER JOIN Etapa e
        ON b.idEtapa = e.idEtapa
    ORDER BY b.idBloque DESC;
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
    @estadoLote INT = 7
AS
BEGIN
    BEGIN TRY
        INSERT INTO Lote (idBloque, numeroLote, areaV2, esEsquina, cercaParque, calleCerrada, precioBase, recargoTotal, precioFinal, estadoId)
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
    @estadoLote INT
AS
BEGIN
    BEGIN TRY
        UPDATE Lote
        SET idBloque = @idBloque, numeroLote = @numeroLote, areaV2 = @areaV2, esEsquina = @esEsquina, 
            cercaParque = @cercaParque, calleCerrada = @calleCerrada, precioBase = @precioBase, 
            recargoTotal = @recargoTotal, precioFinal = @precioFinal, estadoId = @estadoLote
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
CREATE OR ALTER PROCEDURE sp_lote_obtener
    @bloqueId INT,
    @numeroLote VARCHAR(20)
AS
BEGIN
    SELECT T0.numeroLote AS NumeroLote,
            T0.areaV2 AS AreaV2,
            T1.nombreBloque AS Bloque,
            CASE T0.esEsquina 
                WHEN 0 THEN 'NO'
                WHEN 1 THEN 'SI'
                ELSE 'NO'
            END AS '¿Es Esquina?',
            CASE T0.cercaParque 
                WHEN 0 THEN 'NO'
                WHEN 1 THEN 'SI'
                ELSE 'NO'
            END AS '¿Está cerca del parque?',
            CASE T0.calleCerrada 
                WHEN 0 THEN 'NO'
                WHEN 1 THEN 'SI'
                ELSE 'NO'
            END AS '¿Es calle cerrada?',
            T0.precioBase AS PrecioBase,
            T0.recargoTotal AS RecargoTotal,
            T0.precioFinal AS PrecioFinal,
            T2.nombre AS Estado
    FROM Lote AS T0
    INNER JOIN Bloque AS T1 ON T0.idBloque = T1.idBloque
    INNER JOIN Estado AS T2 ON T0.estadoId = T2.id
    WHERE T0.numeroLote = @numeroLote OR
    T0.idBloque = @bloqueId
    ORDER BY idLote DESC;
END;
GO

--LISTAR
CREATE OR ALTER PROCEDURE sp_lote_listar
AS
BEGIN
    SELECT T0.numeroLote AS NumeroLote,
            T0.areaV2 AS AreaV2,
            T1.nombreBloque AS Bloque,
            CASE T0.esEsquina 
                WHEN 0 THEN 'NO'
                WHEN 1 THEN 'SI'
                ELSE 'NO'
            END AS '¿Es Esquina?',
            CASE T0.cercaParque 
                WHEN 0 THEN 'NO'
                WHEN 1 THEN 'SI'
                ELSE 'NO'
            END AS '¿Está cerca del parque?',
            CASE T0.calleCerrada 
                WHEN 0 THEN 'NO'
                WHEN 1 THEN 'SI'
                ELSE 'NO'
            END AS '¿Es calle cerrada?',
            T0.precioBase AS PrecioBase,
            T0.recargoTotal AS RecargoTotal,
            T0.precioFinal AS PrecioFinal,
            T2.nombre AS Estado
    FROM Lote AS T0
    INNER JOIN Bloque AS T1 ON T0.idBloque = T1.idBloque
    INNER JOIN Estado AS T2 ON T0.estadoId = T2.id
    ORDER BY idLote DESC;
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
    @estadoCivilId INT = NULL,
    @rtn VARCHAR(20) = NULL,
    @estado INT = 1
AS
BEGIN
    BEGIN TRY
        INSERT INTO Cliente (identidad, nombres, apellidos, fechaNacimiento, telefono, correo, direccion, estadoCivilId, rtn, estadoId)
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
    @estado INT
AS
BEGIN
    BEGIN TRY
        UPDATE Cliente
        SET identidad = @identidad, nombres = @nombres, apellidos = @apellidos, fechaNacimiento = @fechaNacimiento, 
            telefono = @telefono, correo = @correo, direccion = @direccion, estadoCivilId = @estadoCivilId, rtn = @rtn, estadoId = @estado
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
        c.estadoId,
        e.nombre AS estado
    FROM Cliente c
    LEFT JOIN EstadoCivil ec
        ON c.estadoCivilId = ec.id
    LEFT JOIN Estado e
        ON c.estadoId = e.id
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
        c.estadoId,
        e.nombre AS estado
    FROM Cliente c
    LEFT JOIN EstadoCivil ec
        ON c.estadoCivilId = ec.id
    LEFT JOIN Estado e
        ON c.estadoId = e.id
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
        WHERE estadoId = 1
        ORDER BY nombres, apellidos;
END;
GO

--exec sp_cliente_listar_activo;

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

-- =======================================================
-- PROCEDIMIENTOS PARA BANCO
-- =======================================================
CREATE PROCEDURE sp_banco_insertar
    @nombreBanco VARCHAR(100),
    @estado INT = 1
AS
BEGIN
    BEGIN TRY
        INSERT INTO Banco (nombreBanco, estadoId)
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
    @estado INT
AS
BEGIN
    BEGIN TRY
        UPDATE Banco
        SET nombreBanco = @nombreBanco, estadoId = @estado
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


CREATE OR ALTER PROCEDURE sp_banco_obtener
    @estadoId INT
AS
BEGIN
     SELECT T0.[idBanco] AS id
      ,T0.[nombreBanco]
      ,T1.nombre as estado
  FROM [dbo].[Banco]  AS T0  
  INNER JOIN [dbo].[Estado] AS T1 ON T0.estadoId = T1.id
  WHERE T1.id = @estadoId
END;
GO

CREATE OR ALTER PROCEDURE sp_banco_listar
AS
BEGIN   
 SELECT T0.[idBanco] AS id
      ,T0.[nombreBanco]
      ,T1.nombre as estado
  FROM [dbo].[Banco]  AS T0  
  INNER JOIN [dbo].[Estado] AS T1 ON T0.estadoId = T1.id
  ORDER BY idBanco ASC;
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
    @estado INT = 4
AS
BEGIN
    BEGIN TRY
        INSERT INTO CuentaBancaria (idBanco, idEtapa, numeroCuenta, tipoCuenta, saldoActual, estadoId)
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
    @estado INT
AS
BEGIN
    BEGIN TRY
        UPDATE CuentaBancaria
        SET idBanco = @idBanco, idEtapa = @idEtapa, numeroCuenta = @numeroCuenta, 
            tipoCuenta = @tipoCuenta, saldoActual = @saldoActual, estadoId = @estado
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

-- =======================================================
-- PROCEDIMIENTOS PARA TIPO DE GASTO
-- =======================================================

-- 1. INSERTAR TIPO DE GASTO
CREATE PROCEDURE sp_tipo_gasto_insertar
    @nombreTipoGasto VARCHAR(100),
    @estado INT = 1
AS
BEGIN
    BEGIN TRY
        INSERT INTO TipoGasto (nombreTipoGasto, estadoId)
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
    @estado INT
AS
BEGIN
    BEGIN TRY
        UPDATE TipoGasto
        SET nombreTipoGasto = @nombreTipoGasto,
            estadoId = @estado
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
        estadoId
    FROM TipoGasto
    ORDER BY idTipoGasto DESC;
END;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA EMPLEADO
-- =======================================================

CREATE PROCEDURE sp_empleado_insertar
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @identidad VARCHAR(14),
    @fechaNacimiento DATE,
    @telefono VARCHAR(20) = NULL,
    @sexoId CHAR(1),
    @fechaIngreso DATETIME,
    @fechaEgreso DATETIME = NULL,
    @salario DECIMAL(18,2),
    @estadoId INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Empleado (
            nombres, apellidos, identidad, fechaNacimiento, telefono,
            sexoId, fechaIngreso, fechaEgreso, salario, estadoId
        )
        VALUES (
            @nombres, @apellidos, @identidad, @fechaNacimiento, @telefono,
            @sexoId, @fechaIngreso, @fechaEgreso, @salario, @estadoId
        );

        SELECT SCOPE_IDENTITY() AS idEmpleadoGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_empleado_actualizar
    @id INT,
    @nombres VARCHAR(100),
    @apellidos VARCHAR(100),
    @identidad VARCHAR(14),
    @fechaNacimiento DATE,
    @telefono VARCHAR(20) = NULL,
    @sexoId CHAR(1),
    @fechaIngreso DATETIME,
    @fechaEgreso DATETIME = NULL,
    @salario DECIMAL(18,2),
    @estadoId INT
AS
BEGIN
    BEGIN TRY
        UPDATE Empleado
        SET nombres = @nombres,
            apellidos = @apellidos,
            identidad = @identidad,
            fechaNacimiento = @fechaNacimiento,
            telefono = @telefono,
            sexoId = @sexoId,
            fechaIngreso = @fechaIngreso,
            fechaEgreso = @fechaEgreso,
            salario = @salario,
            estadoId = @estadoId
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_empleado_eliminar
    @id INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Empleado
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE PROCEDURE sp_empleado_obtener
    @id INT
AS
BEGIN
    SELECT 
        id,
        nombres,
        apellidos,
        identidad,
        fechaNacimiento,
        telefono,
        sexoId,
        fechaIngreso,
        fechaEgreso,
        salario,
        estadoId
    FROM Empleado
    WHERE id = @id;
END;
GO
    
CREATE OR ALTER PROCEDURE sp_empleado_listar
AS
BEGIN
    SELECT
        id,
        nombres,
        apellidos,
        identidad,
        fechaNacimiento,
        telefono,
        sexoId,
        fechaIngreso,
        fechaEgreso,
        salario,
        estadoId
    FROM Empleado
    ORDER BY id DESC;
END;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA DEPARTAMENTO
-- =======================================================
CREATE OR ALTER PROCEDURE dbo.sp_departamento_insertar
    @codigo CHAR(2),
    @nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Departamento (codigo, nombre)
        VALUES (@codigo, @nombre);

        SELECT SCOPE_IDENTITY() AS idDepartamentoGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_departamento_actualizar
    @id INT,
    @codigo CHAR(2),
    @nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Departamento
        SET codigo = @codigo,
            nombre = @nombre
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_departamento_eliminar
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Departamento
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_departamento_obtener
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT d.id,
           d.codigo,
           d.nombre
    FROM dbo.Departamento d
    WHERE d.id = @id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_departamento_listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT d.id,
           d.codigo,
           d.nombre
    FROM dbo.Departamento d
    ORDER BY d.id ASC;
END;
GO

-- =======================================================
-- PROCEDIMIENTOS PARA MUNICIPIO
-- =======================================================
CREATE OR ALTER PROCEDURE dbo.sp_municipio_insertar
    @id INT,
    @codigo CHAR(2),
    @departamentoId INT,
    @nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Municipio (id, codigo, departamentoId, nombre)
        VALUES (@id, @codigo, @departamentoId, @nombre);

        SELECT @id AS idMunicipioGenerado;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_municipio_actualizar
    @id INT,
    @codigo CHAR(2),
    @departamentoId INT,
    @nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Municipio
        SET codigo = @codigo,
            departamentoId = @departamentoId,
            nombre = @nombre
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_municipio_eliminar
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Municipio
        WHERE id = @id;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS MensajeError;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_municipio_obtener
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.id,
           m.codigo,
           m.departamentoId,
           d.nombre AS departamento,
           m.nombre
    FROM dbo.Municipio m
    INNER JOIN dbo.Departamento d
        ON m.departamentoId = d.id
    WHERE m.id = @id;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_municipio_listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.id,
           m.codigo,
           m.nombre,
           d.nombre AS departamento
    FROM dbo.Municipio m
    INNER JOIN dbo.Departamento d
        ON m.departamentoId = d.id
    ORDER BY m.id ASC;
END;
GO

