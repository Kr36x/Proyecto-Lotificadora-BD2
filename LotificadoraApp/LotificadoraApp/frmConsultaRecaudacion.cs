using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaRecaudacion : Form
    {
        public frmConsultaRecaudacion()
        {
            InitializeComponent();
            pkrFechaInicio = new DateTimePicker
            {
                Width = 130,
                Format = DateTimePickerFormat.Short,
                Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                Margin = new Padding(0, 4, 14, 0)
            };

            pkrFechaFin = new DateTimePicker
            {
                Width = 130,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                Margin = new Padding(0, 4, 14, 0)
            };
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (pkrFechaFin.Value.Date < pkrFechaInicio.Value.Date)
                {
                    MessageBox.Show(
                    "La fecha fin no puede ser mayor a la fecha de inicio",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                    return;
                }

                var datasource = Db.ExecuteStoredProcedure(
                    "dbo.sp_consulta_sp_recaudacion_etapa",
                    new SqlParameter("@idEtapa", Convert.ToInt32(txtEtapa)),
                    new SqlParameter("@fechaInicio", pkrFechaInicio.Value.Date),
                    new SqlParameter("@fechaFin", pkrFechaFin.Value.Date)
                );

                grdRecaudacion.DataSource = datasource;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Error al consultar recaudación por etapa",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtEtapa.Clear();
            pkrFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            pkrFechaFin.Value = DateTime.Today;
        }
    }
}
