using LotificadoraApp.Proyecto;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Banco
{
    public partial class frmRegistrarBanco : Form
    {
        public frmRegistrarBanco()
        {
            InitializeComponent();
            CargarEstados();
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dtEstados = Db.ExecuteStoredProcedure(BancoQueries.QR001,
                    new SqlParameter("@Ids", "1,2"));

                cmbEstado.DataSource = dtEstados;
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

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                bool esValido = ValidarBanco();

                if (!esValido)
                {
                    return;
                }

                DataTable dataTable = Db.ExecuteStoredProcedure(BancoQueries.QR004,
                    new SqlParameter("@nombreBanco", txtNombre.Text),
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
            txtNombre.Clear();
            cmbEstado.SelectedValue = -1;
        }

        private bool ValidarBanco()
        {
            string nombreBanco = txtNombre.Text;
            if (string.IsNullOrWhiteSpace(nombreBanco))
            {
                MostrarWarning("El nombre del banco es requerido");
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
