using System.Data;

namespace LotificadoraApp;

public class frmConsultaVistaCreditos : ConsultaBaseForm
{
    private readonly TextBox _txtIdCliente;

    public frmConsultaVistaCreditos() : base("Consulta por Vista - Creditos Activos")
    {
        _txtIdCliente = CreateTextBox();

        FiltrosPanel.Controls.Add(CreateLabel("Id Cliente:"));
        FiltrosPanel.Controls.Add(_txtIdCliente);
    }

    protected override DataTable Consultar()
    {
        const string sql = @"
        SELECT 
            v.idCliente,
            v.cliente,
            v.idVenta,
            v.idVentaCredito,
            v.plazoAnios,
            v.tasaInteresAnual,
            e.nombre AS estado,
            v.idPlanPago,
            v.totalPlan,
            v.saldoPendiente
        FROM dbo.vw_creditos_activos_cliente v
        INNER JOIN dbo.Estado e
            ON v.estadoId = e.id
        WHERE (@idCliente IS NULL OR v.idCliente = @idCliente)
        ORDER BY v.idCliente, v.idVentaCredito;";

        var idCliente = ParseNullableInt(_txtIdCliente);

        return Db.ExecuteQuery(
            sql,
            Db.Parameter("@idCliente", idCliente)
        );
    }

    protected override void LimpiarFiltros()
    {
        _txtIdCliente.Clear();
    }
}
