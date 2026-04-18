using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Departamento
{
    public partial class frmRegistrarDepartamento : Form
    {
        private readonly bool _esEditar;
        private readonly int _departamentoId;

        public frmRegistrarDepartamento()
        {
            InitializeComponent();
            _esEditar = false;
        }

        public frmRegistrarDepartamento(int departamentoId)
        {
            InitializeComponent();
            _esEditar = true;
            ObtenerDepartamento(departamentoId);
        }

        private void ObtenerDepartamento(int departamentoId)
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    DepartamentoQueries.QR001);

                txtCodigo.Text = Convert.ToString(dataTable.Rows[0]["id"]);
                txtNombre.Text = Convert.ToString(dataTable.Rows[0]["nombre"]);
            }
            catch
            {
                MostrarMensajeError("Error al obtener los departamentos");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarDepartamento())
                    return;

                DataTable dt;

                if (_esEditar)
                {
                    dt = Db.ExecuteStoredProcedure(
                        DepartamentoQueries.QR004,
                        new SqlParameter("@id", _departamentoId),
                        new SqlParameter("@codigo", txtCodigo.Text.Trim()),
                        new SqlParameter("@nombre", txtNombre.Text.Trim())
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                         DepartamentoQueries.QR003,
                         new SqlParameter("@codigo", txtCodigo.Text.Trim()),
                         new SqlParameter("@nombre", txtNombre.Text.Trim())
                    );
                }

                if (dt.Columns.Contains("MensajeError") &&
                    dt.Rows.Count > 0 &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    _esEditar ?
                    "Departamento actualizado con éxito." :
                    "Departamento guardado con éxito.",
                    "Operación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MostrarMensajeError("Ocurrió un error al guardar.");
            }
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

        private bool ValidarDepartamento()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text.Trim()))
            {
                MostrarWarning("El nombre del departamento es requerido.");
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCodigo.Text.Trim()))
            {
                MostrarWarning("El código del departamento es requerido.");
                txtCodigo.Focus();
                return false;
            }

            return true;
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
    }
}
