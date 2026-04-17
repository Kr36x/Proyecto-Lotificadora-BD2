using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LotificadoraApp.Empleado
{
    public partial class frmCrearEditarEmpleado : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idEmpleado;

        public frmCrearEditarEmpleado()
        {
            InitializeComponent();

            _modoEdicion = false;
            _idEmpleado = 0;

            ConectarEventos();
        }

        public frmCrearEditarEmpleado(int idEmpleado)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idEmpleado = idEmpleado;

            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmCrearEditarEmpleado_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            ckbSinFechaDeEgreso.CheckedChanged += ckbSinFechaDeEgreso_CheckedChanged;
        }

        private void frmCrearEditarEmpleado_Load(object? sender, EventArgs e)
        {
            ConfigurarDatePickers();
            CargarSexos();
            CargarEstados();

            if (_modoEdicion)
            {
                lblTitulo.Text = "EDITAR EMPLEADO";
                btnCrearEditar.Text = "EDITAR";
                CargarEmpleado();
            }
            else
            {
                lblTitulo.Text = "CREAR EMPLEADO";
                btnCrearEditar.Text = "CREAR";
                ckbSinFechaDeEgreso.Checked = true;
            }
        }

        private void ConfigurarDatePickers()
        {
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            guna2DateTimePicker1.Format = DateTimePickerFormat.Short;
            guna2DateTimePicker2.Format = DateTimePickerFormat.Short;

            dtpFechaNacimiento.FillColor = Color.White;
            guna2DateTimePicker1.FillColor = Color.White;
            guna2DateTimePicker2.FillColor = Color.White;

            dtpFechaNacimiento.ForeColor = Color.Black;
            guna2DateTimePicker1.ForeColor = Color.Black;
            guna2DateTimePicker2.ForeColor = Color.Black;

            dtpFechaNacimiento.MaxDate = DateTime.Today;
            dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);

            guna2DateTimePicker1.Value = DateTime.Today;
            guna2DateTimePicker2.Value = DateTime.Today;
        }

        private void CargarSexos()
        {
            DataTable dt = new();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("nombre", typeof(string));

            dt.Rows.Add("M", "M");
            dt.Rows.Add("F", "F");

            cmbSexo.DataSource = dt;
            cmbSexo.DisplayMember = "nombre";
            cmbSexo.ValueMember = "id";
            cmbSexo.SelectedIndex = -1;
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_estado_listar");
                cbEstado.DataSource = dt;
                cbEstado.DisplayMember = "nombre";
                cbEstado.ValueMember = "id";
                cbEstado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar estados: " + ex.Message);
            }
        }

        private void CargarEmpleado()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_empleado_obtener",
                    new SqlParameter("@id", _idEmpleado)
                );

                if (dt.Rows.Count == 0)
                {
                    MostrarMensajeError("No se encontró el empleado.");
                    Close();
                    return;
                }

                DataRow row = dt.Rows[0];

                txtIdentidad.Text = row["identidad"]?.ToString() ?? "";
                txtNombres.Text = row["nombres"]?.ToString() ?? "";
                txtApellidos.Text = row["apellidos"]?.ToString() ?? "";
                txtTelefono.Text = row["telefono"]?.ToString() ?? "";
                txtRTN.Text = row["salario"] == DBNull.Value ? "" : Convert.ToDecimal(row["salario"]).ToString("N2");

                if (row["fechaNacimiento"] != DBNull.Value)
                    dtpFechaNacimiento.Value = Convert.ToDateTime(row["fechaNacimiento"]);

                if (row["fechaIngreso"] != DBNull.Value)
                    guna2DateTimePicker1.Value = Convert.ToDateTime(row["fechaIngreso"]);

                if (row["fechaEgreso"] != DBNull.Value)
                {
                    ckbSinFechaDeEgreso.Checked = false;
                    guna2DateTimePicker2.Value = Convert.ToDateTime(row["fechaEgreso"]);
                    guna2DateTimePicker2.Enabled = true;
                }
                else
                {
                    ckbSinFechaDeEgreso.Checked = true;
                    guna2DateTimePicker2.Enabled = false;
                }

                if (row["sexoId"] != DBNull.Value)
                    cmbSexo.SelectedValue = row["sexoId"].ToString();

                if (row["estadoId"] != DBNull.Value)
                    cbEstado.SelectedValue = Convert.ToInt32(row["estadoId"]);
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar empleado: " + ex.Message);
            }
        }

        private void btnCrearEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario(out decimal salario))
                    return;

                object fechaEgreso = ckbSinFechaDeEgreso.Checked
                    ? DBNull.Value
                    : guna2DateTimePicker2.Value;

                DataTable dt;

                if (_modoEdicion)
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_empleado_actualizar",
                        new SqlParameter("@id", _idEmpleado),
                        new SqlParameter("@nombres", txtNombres.Text.Trim()),
                        new SqlParameter("@apellidos", txtApellidos.Text.Trim()),
                        new SqlParameter("@identidad", txtIdentidad.Text.Trim()),
                        new SqlParameter("@fechaNacimiento", dtpFechaNacimiento.Value.Date),
                        new SqlParameter("@telefono", string.IsNullOrWhiteSpace(txtTelefono.Text) ? DBNull.Value : txtTelefono.Text.Trim()),
                        new SqlParameter("@sexoId", cmbSexo.SelectedValue.ToString()),
                        new SqlParameter("@fechaIngreso", guna2DateTimePicker1.Value),
                        new SqlParameter("@fechaEgreso", fechaEgreso),
                        new SqlParameter("@salario", salario),
                        new SqlParameter("@estadoId", Convert.ToInt32(cbEstado.SelectedValue))
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_empleado_insertar",
                        new SqlParameter("@nombres", txtNombres.Text.Trim()),
                        new SqlParameter("@apellidos", txtApellidos.Text.Trim()),
                        new SqlParameter("@identidad", txtIdentidad.Text.Trim()),
                        new SqlParameter("@fechaNacimiento", dtpFechaNacimiento.Value.Date),
                        new SqlParameter("@telefono", string.IsNullOrWhiteSpace(txtTelefono.Text) ? DBNull.Value : txtTelefono.Text.Trim()),
                        new SqlParameter("@sexoId", cmbSexo.SelectedValue.ToString()),
                        new SqlParameter("@fechaIngreso", guna2DateTimePicker1.Value),
                        new SqlParameter("@fechaEgreso", fechaEgreso),
                        new SqlParameter("@salario", salario),
                        new SqlParameter("@estadoId", Convert.ToInt32(cbEstado.SelectedValue))
                    );
                }

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    _modoEdicion ? "Empleado actualizado correctamente." : "Empleado creado correctamente.",
                    "Operación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Ocurrió un error al guardar: " + ex.Message);
            }
        }

        private bool ValidarFormulario(out decimal salario)
        {
            salario = 0;

            if (string.IsNullOrWhiteSpace(txtIdentidad.Text))
            {
                MostrarWarning("La identidad es requerida.");
                txtIdentidad.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                MostrarWarning("Los nombres son requeridos.");
                txtNombres.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                MostrarWarning("Los apellidos son requeridos.");
                txtApellidos.Focus();
                return false;
            }

            if (cmbSexo.SelectedValue == null || cmbSexo.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione el sexo.");
                cmbSexo.Focus();
                return false;
            }

            if (!TryParseDecimal(txtRTN.Text, out salario) || salario < 0)
            {
                MostrarWarning("El salario debe ser un valor numérico válido.");
                txtRTN.Focus();
                return false;
            }

            if (cbEstado.SelectedValue == null || cbEstado.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione el estado.");
                cbEstado.Focus();
                return false;
            }

            if (dtpFechaNacimiento.Value.Date > DateTime.Today)
            {
                MostrarWarning("fechaNacimiento no puede ser mayor que hoy.");
                return false;
            }

            if (guna2DateTimePicker1.Value.Date < dtpFechaNacimiento.Value.Date)
            {
                MostrarWarning("fechaIngreso no puede ser menor que fechaNacimiento.");
                return false;
            }

            if (!ckbSinFechaDeEgreso.Checked &&
                guna2DateTimePicker2.Value.Date < guna2DateTimePicker1.Value.Date)
            {
                MostrarWarning("fechaEgreso no puede ser menor que fechaIngreso.");
                return false;
            }

            return true;
        }

        private static bool TryParseDecimal(string valor, out decimal resultado)
        {
            valor = valor.Trim();

            if (decimal.TryParse(valor, NumberStyles.Any, new CultureInfo("es-HN"), out resultado))
                return true;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out resultado))
                return true;

            resultado = 0;
            return false;
        }

        private void ckbSinFechaDeEgreso_CheckedChanged(object? sender, EventArgs e)
        {
            guna2DateTimePicker2.Enabled = !ckbSinFechaDeEgreso.Checked;
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            if (_modoEdicion)
            {
                CargarEmpleado();
                return;
            }

            txtIdentidad.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtRTN.Clear();

            cmbSexo.SelectedIndex = -1;
            cbEstado.SelectedIndex = -1;

            dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);
            guna2DateTimePicker1.Value = DateTime.Today;
            guna2DateTimePicker2.Value = DateTime.Today;

            ckbSinFechaDeEgreso.Checked = true;
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

        private static void MostrarWarning(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }
}