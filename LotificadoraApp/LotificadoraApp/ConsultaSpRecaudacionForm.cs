using System.Data;

namespace LotificadoraApp;

public class ConsultaSpRecaudacionForm : ConsultaBaseForm
{
    private readonly TextBox _txtIdEtapa;
    private readonly DateTimePicker _dtpInicio;
    private readonly DateTimePicker _dtpFin;

    public ConsultaSpRecaudacionForm() : base("Consulta por SP - Recaudacion por Etapa")
    {
        _txtIdEtapa = CreateTextBox();
        _dtpInicio = new DateTimePicker
        {
            Width = 130,
            Format = DateTimePickerFormat.Short,
            Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
            Margin = new Padding(0, 4, 14, 0)
        };
        _dtpFin = new DateTimePicker
        {
            Width = 130,
            Format = DateTimePickerFormat.Short,
            Value = DateTime.Today,
            Margin = new Padding(0, 4, 14, 0)
        };

        FiltrosPanel.Controls.Add(CreateLabel("Id Etapa:"));
        FiltrosPanel.Controls.Add(_txtIdEtapa);
        FiltrosPanel.Controls.Add(CreateLabel("Fecha Inicio:"));
        FiltrosPanel.Controls.Add(_dtpInicio);
        FiltrosPanel.Controls.Add(CreateLabel("Fecha Fin:"));
        FiltrosPanel.Controls.Add(_dtpFin);
    }

    protected override DataTable Consultar()
    {
        var idEtapa = ParseRequiredInt(_txtIdEtapa, "Id Etapa");

        if (_dtpFin.Value.Date < _dtpInicio.Value.Date)
        {
            throw new InvalidOperationException("Fecha Fin no puede ser menor que Fecha Inicio.");
        }

        return Db.ExecuteStoredProcedure(
            "dbo.sp_consulta_sp_recaudacion_etapa",
            Db.P("@idEtapa", idEtapa),
            Db.P("@fechaInicio", _dtpInicio.Value.Date),
            Db.P("@fechaFin", _dtpFin.Value.Date)
        );
    }

    protected override void LimpiarFiltros()
    {
        _txtIdEtapa.Clear();
        _dtpInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        _dtpFin.Value = DateTime.Today;
    }
}
