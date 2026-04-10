using System.Data;

namespace LotificadoraApp;

public class ConsultaSpEstadoCuentaForm : ConsultaBaseForm
{
    private readonly TextBox _txtIdCliente;

    public ConsultaSpEstadoCuentaForm() : base("Consulta por SP - Estado de Cuenta Cliente")
    {
        _txtIdCliente = CreateTextBox();

        FiltrosPanel.Controls.Add(CreateLabel("Id Cliente:"));
        FiltrosPanel.Controls.Add(_txtIdCliente);
    }

    protected override DataTable Consultar()
    {
        var idCliente = ParseRequiredInt(_txtIdCliente, "Id Cliente");

        return Db.ExecuteStoredProcedure(
            "dbo.sp_consulta_sp_estado_cuenta_cliente",
            Db.P("@idCliente", idCliente)
        );
    }

    protected override void LimpiarFiltros()
    {
        _txtIdCliente.Clear();
    }
}
