use DB20192002534
/*
====================================================
  SP PARA LOTES EN FORMULARIOS CRUD
====================================================
  */
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
    ORDER BY b.nombreBloque;
END;
GO
--2. Obtener lote por id para editar
CREATE OR ALTER PROCEDURE sp_lote_obtener_por_id
    @idLote INT
AS
BEGIN
    SELECT
        l.idLote,
        l.idBloque,
        l.numeroLote,
        l.areaV2,
        l.esEsquina,
        l.cercaParque,
        l.calleCerrada,
        l.precioBase,
        l.recargoTotal,
        l.precioFinal,
        l.estadoId
    FROM Lote l
    WHERE l.idLote = @idLote;
END;
GO
--3. Listar lotes para gestión
CREATE OR ALTER PROCEDURE sp_lote_listar_gestion
AS
BEGIN
    SELECT
        l.idLote,
        l.idBloque,
        l.estadoId,
        l.numeroLote AS NumeroLote,
        l.areaV2 AS AreaV2,
        b.nombreBloque AS Bloque,
        CASE l.esEsquina 
            WHEN 1 THEN 'SI'
            ELSE 'NO'
        END AS [¿Es Esquina?],
        CASE l.cercaParque 
            WHEN 1 THEN 'SI'
            ELSE 'NO'
        END AS [¿Está cerca del parque?],
        CASE l.calleCerrada 
            WHEN 1 THEN 'SI'
            ELSE 'NO'
        END AS [¿Es calle cerrada?],
        l.precioBase AS PrecioBase,
        l.recargoTotal AS RecargoTotal,
        l.precioFinal AS PrecioFinal,
        e.nombre AS Estado
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
        l.estadoId,
        l.numeroLote AS NumeroLote,
        l.areaV2 AS AreaV2,
        b.nombreBloque AS Bloque,
        CASE l.esEsquina 
            WHEN 1 THEN 'SI'
            ELSE 'NO'
        END AS [¿Es Esquina?],
        CASE l.cercaParque 
            WHEN 1 THEN 'SI'
            ELSE 'NO'
        END AS [¿Está cerca del parque?],
        CASE l.calleCerrada 
            WHEN 1 THEN 'SI'
            ELSE 'NO'
        END AS [¿Es calle cerrada?],
        l.precioBase AS PrecioBase,
        l.recargoTotal AS RecargoTotal,
        l.precioFinal AS PrecioFinal,
        e.nombre AS Estado
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
