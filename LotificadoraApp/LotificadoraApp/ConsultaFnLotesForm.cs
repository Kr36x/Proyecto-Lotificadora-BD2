using System.Data;

namespace LotificadoraApp;

public class ConsultaFnLotesForm : ConsultaBaseForm
{
    private readonly TextBox _txtIdEtapa;

    public ConsultaFnLotesForm() : base("Consulta por Funcion Tabla - Lotes Disponibles")
    {
        _txtIdEtapa = CreateTextBox();

        FiltrosPanel.Controls.Add(CreateLabel("Id Etapa:"));
        FiltrosPanel.Controls.Add(_txtIdEtapa);
    }

    protected override DataTable Consultar()
    {
        const string sql = @"
            SELECT *
            FROM dbo.fn_tvf_lotes_disponibles(@idEtapa)
            ORDER BY idProyecto, idEtapa, idBloque, numeroLote;";

        var idEtapa = ParseNullableInt(_txtIdEtapa);

        return Db.ExecuteQuery(
            sql,
            Db.P("@idEtapa", idEtapa)
        );
    }

    protected override void LimpiarFiltros()
    {
        _txtIdEtapa.Clear();
    }
}
