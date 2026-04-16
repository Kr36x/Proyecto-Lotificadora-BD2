using LotificadoraApp.Banco;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Estado
{
    public partial class frmRegistrarEstado : Form
    {
        public frmRegistrarEstado()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                bool esValido = ValidarEstado();

                if (!esValido)
                {
                    return;
                }

                DataTable dataTable = Db.ExecuteStoredProcedure(EstadoQueries.QR002,
                    new SqlParameter("@nombre", txtNombre.Text)
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

        private static void MostrarMensajeError(string mensaje)
        {
            MessageBox.Show(
                    mensaje,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
        }

        private void LimpiarFormularios()
        {
            txtNombre.Clear();
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

        private bool ValidarEstado()
        {
            string nombre = txtNombre.Text;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarWarning("El nombre del estado es requerido");
                return false;
            }

            return true;
        }
    }
}
