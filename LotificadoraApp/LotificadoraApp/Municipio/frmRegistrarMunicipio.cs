using LotificadoraApp.Departamento;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Municipio
{
    public partial class frmRegistrarMunicipio : Form
    {
        private readonly bool _esEditar;
        private readonly int _municipioId;

        public frmRegistrarMunicipio()
        {
            InitializeComponent();
            _esEditar = false;
            ObtenerDepartamentos();
        }

        public frmRegistrarMunicipio(int municipioId)
        {
            InitializeComponent();
            _esEditar = true;
            _municipioId = municipioId;
            ObtenerMunicipio(municipioId);
        }

        private void frmRegistrarMunicipio_Load(object sender, EventArgs e)
        {

        }

        private void ObtenerMunicipio(int municipioId)
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    MunicipioQueries.QR006,
                    new SqlParameter("@id", municipioId)
                    );

                txtCodigo.Text = Convert.ToString(dataTable.Rows[0]["codigo"]);
                txtNombre.Text = Convert.ToString(dataTable.Rows[0]["nombre"]);

                ObtenerDepartamento(Convert.ToInt32(dataTable.Rows[0]["departamentoId"]));
            }
            catch
            {
                MostrarMensajeError("Error al obtener el departamento seleccionado");
            }
        }

        private void ObtenerDepartamento(int departamentoId)
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    DepartamentoQueries.QR005,
                    new SqlParameter("@id", departamentoId)
                    );

                cmbDepartamento.DataSource = dataTable;
                cmbDepartamento.DisplayMember = "nombre";
                cmbDepartamento.ValueMember = "id";
                cmbDepartamento.SelectedValue = Convert.ToInt32(dataTable.Rows[0]["id"]);
            }
            catch
            {
                MostrarMensajeError("Error al obtener los departamentos");
            }
        }

        private void ObtenerDepartamentos()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    DepartamentoQueries.QR001
                );

                cmbDepartamento.DataSource = dataTable;
                cmbDepartamento.DisplayMember = "nombre";
                cmbDepartamento.ValueMember = "id";
                cmbDepartamento.SelectedValue = 0;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los departamentos");
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

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarMunicipio())
                    return;

                DataTable dt;

                if (_esEditar)
                {
                    dt = Db.ExecuteStoredProcedure(
                       MunicipioQueries.QR005,
                       new SqlParameter("@id", _municipioId),
                       new SqlParameter("@codigo", txtCodigo.Text.Trim()),
                       new SqlParameter("@departamentoId", cmbDepartamento.SelectedValue),
                       new SqlParameter("@nombre", txtNombre.Text.Trim())
                   );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                         MunicipioQueries.QR004,
                         new SqlParameter("@codigo", txtCodigo.Text.Trim()),
                         new SqlParameter("@departamentoId", cmbDepartamento.SelectedValue),
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
                    "Municipio actualizado con éxito." :
                    "Municipio guardado con éxito.",
                    "Operación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                MostrarMensajeError("Ocurrió un error al guardar.");
            }
        }

        private bool ValidarMunicipio()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text.Trim()))
            {
                MostrarWarning("El código del municipio es requerido.");
                txtCodigo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text.Trim()))
            {
                MostrarWarning("El nombre del municipio es requerido.");
                txtNombre.Focus();
                return false;
            }

            return true;
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
