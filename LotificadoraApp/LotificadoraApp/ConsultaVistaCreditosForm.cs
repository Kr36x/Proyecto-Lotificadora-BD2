using System.Data;

namespace LotificadoraApp;

public class ConsultaVistaCreditosForm : ConsultaBaseForm
{
    private readonly TextBox _txtIdCliente;

    public ConsultaVistaCreditosForm() : base("Consulta por Vista - Creditos Activos")
    {
        _txtIdCliente = CreateTextBox();

        FiltrosPanel.Controls.Add(CreateLabel("Id Cliente:"));
        FiltrosPanel.Controls.Add(_txtIdCliente);
    }

    protected override DataTable Consultar()
    {
        const string sql = @"
            SELECT *
            FROM dbo.vw_creditos_activos_cliente
            WHERE (@idCliente IS NULL OR idCliente = @idCliente)
            ORDER BY idCliente, idVentaCredito;";

        var idCliente = ParseNullableInt(_txtIdCliente);

        return Db.ExecuteQuery(
            sql,
            Db.P("@idCliente", idCliente)
        );
    }

    protected override void LimpiarFiltros()
    {
        _txtIdCliente.Clear();
    }
}
