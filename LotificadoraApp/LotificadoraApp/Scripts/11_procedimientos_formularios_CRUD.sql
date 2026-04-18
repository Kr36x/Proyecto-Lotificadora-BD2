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
