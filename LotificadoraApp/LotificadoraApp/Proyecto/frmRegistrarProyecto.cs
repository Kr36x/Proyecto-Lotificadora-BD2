using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Proyecto
{
    public partial class frmRegistrarProyecto : Form
    {
        public frmRegistrarProyecto()
        {
            InitializeComponent();

            ConfigurarDateTimePicker();
        }

        private void ConfigurarDateTimePicker()
        {
            pkrFechaInicio.Format = DateTimePickerFormat.Custom;
            pkrFechaInicio.CustomFormat = "yyyy-MM-dd";
            pkrFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            pkrFechaFin.Format = DateTimePickerFormat.Custom;
            pkrFechaFin.CustomFormat = "yyyy-MM-dd";
            pkrFechaFin.Value = DateTime.Today;
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(ProyectoQueries.QR001,
                    new SqlParameter("@Ids", "1,2,3"));

                cmbEstado.DataSource = dataTable;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = -1;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los estados");
            }
        }

        private static void MostrarMensajeError(string mensaje)
        {
            MessageBox.Show(
                    mensaje,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
        }

        private static void MostrarWarning(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void frmRegistrarProyecto_Load(object sender, EventArgs e)
        {
            CargarEstados();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                bool esValido = ValidarProyecto();

                if (!esValido)
                {
                    return;
                }

                DataTable dataTable = Db.ExecuteStoredProcedure(ProyectoQueries.QR004,
                    new SqlParameter("@nombreProyecto", txtNombreProyecto.Text),
                    new SqlParameter("@descripcion", txtDescripcion.Text),
                    new SqlParameter("@fechaInicio", pkrFechaInicio.Value),
                    new SqlParameter("@fechaFinEstimada", pkrFechaFin.Value),
                    new SqlParameter("@areaTotalV2", Convert.ToDecimal(txtAreaTotal.Text)),
                    new SqlParameter("@maxAniosFinanciamiento",
                    Convert.ToInt32(txtAnioFinanciamiento.Text)),
                    new SqlParameter("@estado", Convert.ToInt32(cmbEstado.SelectedValue))
                    );

                if (dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    $"Registro guardado con éxito",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimpiarFormularios();
            }
            catch (Exception ex)
            {
                _ = ex;
                MostrarMensajeError("Ocurrió un error al guardar");
            }
        }

        private void LimpiarFormularios()
        {
            txtNombreProyecto.Clear();
            txtDescripcion.Clear();
            txtAreaTotal.Clear();
            txtAnioFinanciamiento.Clear();
            cmbEstado.SelectedValue = -1;
            ConfigurarDateTimePicker();
        }

        private bool ValidarProyecto()
        {
            string nombreProyecto = txtNombreProyecto.Text;
            if (string.IsNullOrWhiteSpace(nombreProyecto))
            {
                MostrarWarning("El nombre del proyecto es requerido");
                return false;
            }

            string descripcion = txtDescripcion.Text;
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MostrarWarning("La descripción es requerida");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAreaTotal.Text))
            {
                MostrarWarning("El área total es requerido");
                return false;
            }

            decimal areaTotal = Convert.ToDecimal(txtAreaTotal.Text);
            if (areaTotal <= 0)
            {
                MostrarWarning("El área total debe ser mayor que 0");
                return false;
            }

            DateTime fechaInicial = Convert.ToDateTime(pkrFechaInicio.Value);
            DateTime fechaFinal = Convert.ToDateTime(pkrFechaFin.Value);
            if (fechaInicial > fechaFinal)
            {
                MostrarWarning("La fecha inicial no puede ser mayor que la fecha final");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAnioFinanciamiento.Text))
            {
                MostrarWarning("El año de financiamiento es requerido");
                return false;
            }

            int aniosFinanciamiento = Convert.ToInt32(txtAnioFinanciamiento.Text);
            if (aniosFinanciamiento <= 0)
            {
                MostrarWarning("La cantidad de años de financiamiento debe ser mayor que 0");
                return false;
            }

            int estadoId = Convert.ToInt32(cmbEstado.SelectedValue);
            if (estadoId <= 0)
            {
                MostrarWarning("El estado es requerido");
                return false;
            }

            return true;
        }
    }
}
