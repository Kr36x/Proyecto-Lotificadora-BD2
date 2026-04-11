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
            SELECT idCliente,	
                cliente,
                idVenta,
                idVentaCredito,	
                plazoAnios,	
                tasaInteresAnual,	
                estadoId,	
                idPlanPago,	
                totalPlan
            FROM dbo.vw_creditos_activos_cliente
            WHERE (@idCliente IS NULL OR idCliente = @idCliente)
            ORDER BY idCliente, idVentaCredito;";

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
