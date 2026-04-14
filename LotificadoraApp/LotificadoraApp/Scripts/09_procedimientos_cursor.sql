-- -------------------------
-- 1
-- -------------------------
CREATE OR ALTER PROCEDURE dbo.sp_generar_cuotas_plan_cursor
    @idVentaCredito INT,
    @fechaPrimerVencimiento DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idPlanPago INT;
    DECLARE @montoFinanciado DECIMAL(18,2);
    DECLARE @plazoAnios INT;
    DECLARE @tasaInteresAnual DECIMAL(5,2);
    DECLARE @tasaMensual DECIMAL(18,8);
    DECLARE @cuotaEstimada DECIMAL(18,2);
    DECLARE @totalCuotas INT;

    DECLARE @numeroCuota INT;
    DECLARE @saldoActual DECIMAL(18,2);
    DECLARE @interes DECIMAL(18,2);
    DECLARE @capital DECIMAL(18,2);
    DECLARE @montoCuota DECIMAL(18,2);
    DECLARE @saldoFinal DECIMAL(18,2);
    DECLARE @fechaVencimiento DATE;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT
            @idPlanPago = pp.idPlanPago,
            @montoFinanciado = vc.montoFinanciado,
            @plazoAnios = vc.plazoAnios,
            @tasaInteresAnual = vc.tasaInteresAnual,
            @cuotaEstimada = pp.cuotaMensualEstimada
        FROM VentaCredito vc
        INNER JOIN PlanPago pp ON pp.idVentaCredito = vc.idVentaCredito
        WHERE vc.idVentaCredito = @idVentaCredito;

        IF @idPlanPago IS NULL
        BEGIN
            RAISERROR('No existe plan de pago para el credito indicado.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Cuota WHERE idPlanPago = @idPlanPago)
        BEGIN
            RAISERROR('El plan de pago ya tiene cuotas generadas.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        SET @totalCuotas = ISNULL(@plazoAnios, 0) * 12;
        IF @totalCuotas <= 0
        BEGIN
            RAISERROR('El plazo del credito no es valido.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @fechaPrimerVencimiento IS NULL
            SET @fechaPrimerVencimiento = EOMONTH(GETDATE());

        SET @tasaMensual = ISNULL(@tasaInteresAnual, 0) / 12.0 / 100.0;

        IF ISNULL(@cuotaEstimada, 0) <= 0
        BEGIN
            IF @tasaMensual = 0
                SET @cuotaEstimada = ROUND(@montoFinanciado / @totalCuotas, 2);
            ELSE
                SET @cuotaEstimada = ROUND(
                    @montoFinanciado *
                    (
                        @tasaMensual * POWER(1 + @tasaMensual, @totalCuotas)
                    ) /
                    (
                        POWER(1 + @tasaMensual, @totalCuotas) - 1
                    ),
                    2
                );
        END

        DECLARE @numeros TABLE (n INT PRIMARY KEY);
        INSERT INTO @numeros(n)
        SELECT TOP (@totalCuotas) ROW_NUMBER() OVER (ORDER BY (SELECT NULL))
        FROM sys.all_objects;

        SET @saldoActual = @montoFinanciado;

        DECLARE cur_cuotas CURSOR LOCAL FAST_FORWARD FOR
            SELECT n FROM @numeros ORDER BY n;

        OPEN cur_cuotas;
        FETCH NEXT FROM cur_cuotas INTO @numeroCuota;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @fechaVencimiento = EOMONTH(DATEADD(MONTH, @numeroCuota - 1, @fechaPrimerVencimiento));
            SET @interes = ROUND(CASE WHEN @tasaMensual = 0 THEN 0 ELSE (@saldoActual * @tasaMensual) END, 2);
            SET @capital = ROUND(@cuotaEstimada - @interes, 2);
            SET @montoCuota = @cuotaEstimada;
            SET @saldoFinal = ROUND(@saldoActual - @capital, 2);

            IF @numeroCuota = @totalCuotas
            BEGIN
                SET @capital = @saldoActual;
                SET @montoCuota = ROUND(@capital + @interes, 2);
                SET @saldoFinal = 0;
            END

            IF @capital < 0 SET @capital = 0;
            IF @saldoFinal < 0 SET @saldoFinal = 0;

            INSERT INTO Cuota
            (
                idPlanPago,
                numeroCuota,
                fechaVencimiento,
                saldoInicial,
                capitalProgramado,
                interesProgramado,
                montoCuota,
                saldoFinal,
                estadoId
            )
            VALUES
            (
                @idPlanPago,
                @numeroCuota,
                @fechaVencimiento,
                @saldoActual,
                @capital,
                @interes,
                @montoCuota,
                @saldoFinal, 13);

            SET @saldoActual = @saldoFinal;

            FETCH NEXT FROM cur_cuotas INTO @numeroCuota;
        END

        CLOSE cur_cuotas;
        DEALLOCATE cur_cuotas;

        UPDATE pp
        SET
            pp.totalCapital = ISNULL(t.totalCapital, 0),
            pp.totalInteres = ISNULL(t.totalInteres, 0),
            pp.totalPlan = ISNULL(t.totalPlan, 0),
            pp.cuotaMensualEstimada = ISNULL(t.cuotaPromedio, pp.cuotaMensualEstimada)
        FROM PlanPago pp
        INNER JOIN
        (
            SELECT
                idPlanPago,
                SUM(capitalProgramado) AS totalCapital,
                SUM(interesProgramado) AS totalInteres,
                SUM(montoCuota) AS totalPlan,
                AVG(montoCuota) AS cuotaPromedio
            FROM Cuota
            WHERE idPlanPago = @idPlanPago
            GROUP BY idPlanPago
        ) t ON t.idPlanPago = pp.idPlanPago
        WHERE pp.idPlanPago = @idPlanPago;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF CURSOR_STATUS('local', 'cur_cuotas') >= -1
        BEGIN
            CLOSE cur_cuotas;
            DEALLOCATE cur_cuotas;
        END

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
CREATE OR ALTER PROCEDURE dbo.sp_recalcular_precios_lotes_etapa_cursor
    @idEtapa INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idLote INT;
    DECLARE @precioBase DECIMAL(18,2);
    DECLARE @recargo DECIMAL(18,2);
    DECLARE @precioFinal DECIMAL(18,2);

    DECLARE cur_lotes CURSOR LOCAL FAST_FORWARD FOR
        SELECT l.idLote
        FROM Lote l
        INNER JOIN Bloque b ON b.idBloque = l.idBloque
        WHERE b.idEtapa = @idEtapa;

    OPEN cur_lotes;
    FETCH NEXT FROM cur_lotes INTO @idLote;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @precioBase = dbo.fn_lote_precio_base(@idLote);
        SET @recargo = dbo.fn_lote_recargo_total(@idLote);
        SET @precioFinal = @precioBase + @recargo;

        UPDATE Lote
        SET
            precioBase = @precioBase,
            recargoTotal = @recargo,
            precioFinal = @precioFinal
        WHERE idLote = @idLote;

        FETCH NEXT FROM cur_lotes INTO @idLote;
    END

    CLOSE cur_lotes;
    DEALLOCATE cur_lotes;
END;
GO


-- -------------------------
-- 3
-- -------------------------
CREATE OR ALTER PROCEDURE dbo.sp_actualizar_cuotas_vencidas_cursor
    @fechaCorte DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @fechaCorte IS NULL
        SET @fechaCorte = CAST(GETDATE() AS DATE);

    DECLARE @idCuota INT;
    DECLARE @saldoPendiente DECIMAL(18,2);

    DECLARE cur_cuotas_vencidas CURSOR LOCAL FAST_FORWARD FOR
        SELECT idCuota
        FROM Cuota
        WHERE fechaVencimiento <= @fechaCorte
          AND estadoId IN (13, 14, 16);

    OPEN cur_cuotas_vencidas;
    FETCH NEXT FROM cur_cuotas_vencidas INTO @idCuota;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @saldoPendiente = dbo.fn_cuota_saldo_pendiente(@idCuota);

        IF @saldoPendiente <= 0
        BEGIN
            UPDATE Cuota
            SET estadoId = 15
            WHERE idCuota = @idCuota;
        END
        ELSE
        BEGIN
            UPDATE Cuota
            SET estadoId = 16
            WHERE idCuota = @idCuota;
        END

        FETCH NEXT FROM cur_cuotas_vencidas INTO @idCuota;
    END

    CLOSE cur_cuotas_vencidas;
    DEALLOCATE cur_cuotas_vencidas;
END;
GO


