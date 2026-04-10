using System.Data;

namespace LotificadoraApp;

public class ConsultaFnEstadoCuentaForm : ConsultaBaseForm
{
    private readonly TextBox _txtIdCliente;

    public ConsultaFnEstadoCuentaForm() : base("Consulta por Funcion Tabla - Estado Cuenta Cliente")
    {
        _txtIdCliente = CreateTextBox();

        FiltrosPanel.Controls.Add(CreateLabel("Id Cliente:"));
        FiltrosPanel.Controls.Add(_txtIdCliente);
    }

    protected override DataTable Consultar()
    {
        const string sql = @"
            SELECT *
            FROM dbo.fn_tvf_estado_cuenta_cliente(@idCliente)
            ORDER BY numeroCuota;";

        var idCliente = ParseRequiredInt(_txtIdCliente, "Id Cliente");

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
