using System.Data;

namespace LotificadoraApp;

public class ConsultaVistaLotesForm : ConsultaBaseForm
{
    private readonly TextBox _txtIdProyecto;
    private readonly TextBox _txtIdEtapa;

    public ConsultaVistaLotesForm() : base("Consulta por Vista - Lotes Disponibles")
    {
        _txtIdProyecto = CreateTextBox();
        _txtIdEtapa = CreateTextBox();

        FiltrosPanel.Controls.Add(CreateLabel("Id Proyecto:"));
        FiltrosPanel.Controls.Add(_txtIdProyecto);
        FiltrosPanel.Controls.Add(CreateLabel("Id Etapa:"));
        FiltrosPanel.Controls.Add(_txtIdEtapa);
    }

    protected override DataTable Consultar()
    {
        const string sql = @"
            SELECT *
            FROM dbo.vw_lotes_disponibles
            WHERE (@idProyecto IS NULL OR idProyecto = @idProyecto)
              AND (@idEtapa IS NULL OR idEtapa = @idEtapa)
            ORDER BY idProyecto, idEtapa, idBloque, numeroLote;";

        var idProyecto = ParseNullableInt(_txtIdProyecto);
        var idEtapa = ParseNullableInt(_txtIdEtapa);

        return Db.ExecuteQuery(
            sql,
            Db.P("@idProyecto", idProyecto),
            Db.P("@idEtapa", idEtapa)
        );
    }

    protected override void LimpiarFiltros()
    {
        _txtIdProyecto.Clear();
        _txtIdEtapa.Clear();
    }
}
