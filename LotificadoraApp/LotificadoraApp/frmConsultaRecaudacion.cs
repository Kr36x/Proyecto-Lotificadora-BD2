using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaRecaudacion : Form
    {
        public frmConsultaRecaudacion()
        {
            InitializeComponent();

            pkrFechaInicio.Format = DateTimePickerFormat.Custom;
            pkrFechaInicio.CustomFormat = "yyyy-MM-dd";
            pkrFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            pkrFechaFin.Format = DateTimePickerFormat.Custom;
            pkrFechaFin.CustomFormat = "yyyy-MM-dd";
            pkrFechaFin.Value = DateTime.Today;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEtapa.Text))
                {
                    MessageBox.Show(
                    "El campo id etapa es requerido",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                    return;
                }

                if (pkrFechaFin.Value.Date < pkrFechaInicio.Value.Date)
                {
                    MessageBox.Show(
                    "La fecha fin no puede ser mayor a la fecha de inicio",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                    return;
                }

                var datasource = Db.ExecuteStoredProcedure(
                    "dbo.sp_consulta_sp_recaudacion_etapa",
                    new SqlParameter("@idEtapa", Convert.ToInt32(txtEtapa.Text)),
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
