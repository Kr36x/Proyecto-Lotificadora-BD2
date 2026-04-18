using System.Data;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarBeneficiario : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idBeneficiario;

        public frmRegistrarBeneficiario()
        {
            InitializeComponent();

            _modoEdicion = false;
            _idBeneficiario = 0;

            ConectarEventos();
        }

        public frmRegistrarBeneficiario(int idBeneficiario)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idBeneficiario = idBeneficiario;

            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarBeneficiario_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmRegistrarBeneficiario_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarParentescos();

                if (_modoEdicion)
                {
                    lblTitulo.Text = "EDITAR BENEFICIARIO";
                    btnCrearEditar.Text = "EDITAR";
                    CargarBeneficiario();
                }
                else
                {
                    lblTitulo.Text = "CREAR BENEFICIARIO";
                    btnCrearEditar.Text = "CREAR";
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar formulario: " + ex.Message);
            }
        }

        private void CargarParentescos()
        {
            DataTable dt = Db.ExecuteStoredProcedure("sp_parentesco_listar");

            cmbParentesco.DataSource = dt;
            cmbParentesco.DisplayMember = "descripcion";
            cmbParentesco.ValueMember = "id";
            cmbParentesco.SelectedIndex = -1;
        }

        private void CargarBeneficiario()
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_beneficiario_obtener",
                new SqlParameter("@idBeneficiario", _idBeneficiario)
            );

            if (dt.Rows.Count == 0)
                throw new Exception("No se encontró el beneficiario.");

            DataRow row = dt.Rows[0];

            txtIdentidad.Text = row["identidad"]?.ToString() ?? "";
            txtNombres.Text = row["nombres"]?.ToString() ?? "";
            txtApellidos.Text = row["apellidos"]?.ToString() ?? "";
            txtTelefono.Text = row["telefono"]?.ToString() ?? "";
            txtDireccion.Text = row["direccion"]?.ToString() ?? "";

            if (row["parentescoId"] != DBNull.Value)
                cmbParentesco.SelectedValue = Convert.ToInt32(row["parentescoId"]);
            else
                cmbParentesco.SelectedIndex = -1;
        }

        private void btnCrearEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                DataTable dt;

                if (_modoEdicion)
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_beneficiario_actualizar",
                        new SqlParameter("@idBeneficiario", _idBeneficiario),
                        new SqlParameter("@identidad", txtIdentidad.Text.Trim()),
                        new SqlParameter("@nombres", txtNombres.Text.Trim()),
                        new SqlParameter("@apellidos", txtApellidos.Text.Trim()),
                        new SqlParameter("@telefono", ValorDb(txtTelefono.Text)),
                        new SqlParameter("@parentescoId", ValorDbCombo(cmbParentesco)),
                        new SqlParameter("@direccion", ValorDb(txtDireccion.Text))
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_beneficiario_insertar",
                        new SqlParameter("@identidad", txtIdentidad.Text.Trim()),
                        new SqlParameter("@nombres", txtNombres.Text.Trim()),
                        new SqlParameter("@apellidos", txtApellidos.Text.Trim()),
                        new SqlParameter("@telefono", ValorDb(txtTelefono.Text)),
                        new SqlParameter("@parentescoId", ValorDbCombo(cmbParentesco)),
                        new SqlParameter("@direccion", ValorDb(txtDireccion.Text))
                    );
                }

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    throw new Exception(dt.Rows[0]["MensajeError"].ToString());
                }

                MessageBox.Show(
                    _modoEdicion ? "Beneficiario actualizado correctamente." : "Beneficiario registrado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al guardar beneficiario: " + ex.Message);
            }
        }

        private void ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtIdentidad.Text))
                throw new Exception("La identidad es obligatoria.");

            if (string.IsNullOrWhiteSpace(txtNombres.Text))
                throw new Exception("Los nombres son obligatorios.");

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
                throw new Exception("Los apellidos son obligatorios.");
        }

        private object ValorDb(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : valor.Trim();
        }

        private object ValorDbCombo(ComboBox combo)
        {
            return combo.SelectedIndex < 0 || combo.SelectedValue == null
                ? DBNull.Value
                : combo.SelectedValue;
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            if (_modoEdicion)
            {
                CargarBeneficiario();
                return;
            }

            txtIdentidad.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            cmbParentesco.SelectedIndex = -1;
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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