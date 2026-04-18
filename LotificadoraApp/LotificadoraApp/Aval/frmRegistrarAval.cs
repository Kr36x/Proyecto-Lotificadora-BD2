using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarAval : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idAval;

        public frmRegistrarAval()
        {
            InitializeComponent();

            _modoEdicion = false;
            _idAval = 0;

            ConectarEventos();
        }

        public frmRegistrarAval(int idAval)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idAval = idAval;

            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarAval_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmRegistrarAval_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarParentescos();

                if (_modoEdicion)
                {
                    lblTitulo.Text = "EDITAR AVAL";
                    btnCrearEditar.Text = "EDITAR";
                    CargarAval();
                }
                else
                {
                    lblTitulo.Text = "CREAR AVAL";
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

        private void CargarAval()
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_aval_obtener",
                new SqlParameter("@idAval", _idAval)
            );

            if (dt.Rows.Count == 0)
                throw new Exception("No se encontró el aval.");

            DataRow row = dt.Rows[0];

            txtIdentidad.Text = row["identidad"]?.ToString() ?? "";
            txtNombres.Text = row["nombres"]?.ToString() ?? "";
            txtApellidos.Text = row["apellidos"]?.ToString() ?? "";
            txtTelefono.Text = row["telefono"]?.ToString() ?? "";
            txtDireccion.Text = row["direccion"]?.ToString() ?? "";
            txtLugarTrabajo.Text = row["lugarTrabajo"]?.ToString() ?? "";
            txtIngresoMensual.Text = row["ingresoMensual"] == DBNull.Value
                ? ""
                : Convert.ToDecimal(row["ingresoMensual"]).ToString("N2");

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
                        "sp_aval_actualizar",
                        new SqlParameter("@idAval", _idAval),
                        new SqlParameter("@identidad", txtIdentidad.Text.Trim()),
                        new SqlParameter("@nombres", txtNombres.Text.Trim()),
                        new SqlParameter("@apellidos", txtApellidos.Text.Trim()),
                        new SqlParameter("@telefono", ValorDb(txtTelefono.Text)),
                        new SqlParameter("@direccion", ValorDb(txtDireccion.Text)),
                        new SqlParameter("@lugarTrabajo", ValorDb(txtLugarTrabajo.Text)),
                        new SqlParameter("@ingresoMensual", ValorDbDecimal(txtIngresoMensual.Text)),
                        new SqlParameter("@parentescoId", ValorDbCombo(cmbParentesco))
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_aval_insertar",
                        new SqlParameter("@identidad", txtIdentidad.Text.Trim()),
                        new SqlParameter("@nombres", txtNombres.Text.Trim()),
                        new SqlParameter("@apellidos", txtApellidos.Text.Trim()),
                        new SqlParameter("@telefono", ValorDb(txtTelefono.Text)),
                        new SqlParameter("@direccion", ValorDb(txtDireccion.Text)),
                        new SqlParameter("@lugarTrabajo", ValorDb(txtLugarTrabajo.Text)),
                        new SqlParameter("@ingresoMensual", ValorDbDecimal(txtIngresoMensual.Text)),
                        new SqlParameter("@parentescoId", ValorDbCombo(cmbParentesco))
                    );
                }

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    throw new Exception(dt.Rows[0]["MensajeError"].ToString());
                }

                MessageBox.Show(
                    _modoEdicion ? "Aval actualizado correctamente." : "Aval registrado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al guardar aval: " + ex.Message);
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

            if (!string.IsNullOrWhiteSpace(txtIngresoMensual.Text) &&
                ParseDecimal(txtIngresoMensual.Text) < 0)
                throw new Exception("El ingreso mensual no puede ser negativo.");
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

        private object ValorDbDecimal(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : ParseDecimal(valor);
        }

        private decimal ParseDecimal(string valor)
        {
            valor = valor.Replace(",", "");

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal r))
                return r;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.CurrentCulture, out r))
                return r;

            throw new Exception($"El valor '{valor}' no es un número válido.");
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            if (_modoEdicion)
            {
                CargarAval();
                return;
            }

            txtIdentidad.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtLugarTrabajo.Clear();
            txtIngresoMensual.Clear();
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