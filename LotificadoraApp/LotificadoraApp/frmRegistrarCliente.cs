using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarCliente : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idCliente;
        private int? _idDatosLaborales;

        private sealed class EstadoCivilItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = "";
            public override string ToString() => Nombre;
        }

        public frmRegistrarCliente()
        {
            InitializeComponent();
            _modoEdicion = false;
            _idCliente = 0;

            ConfigurarFormulario();
            ConectarEventos();
        }

        public frmRegistrarCliente(int idCliente)
        {
            InitializeComponent();
            _modoEdicion = true;
            _idCliente = idCliente;

            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtIdClienteGenerado.ReadOnly = true;
            txtIdClienteGenerado.TabStop = false;

            CargarEstadoCivil();

            cbEstado.Items.Clear();
            cbEstado.Items.Add("activo");
            cbEstado.Items.Add("inactivo");
            cbEstado.SelectedItem = "activo";

            chkRegistrarDatosLaborales.Checked = false;
            HabilitarDatosLaborales(false);

            if (_modoEdicion)
            {
                Text = "Editar Cliente";
                btnGuardarCliente.Text = "GUARDAR CAMBIOS";
            }
            else
            {
                Text = "Registrar Cliente";
                btnGuardarCliente.Text = "GUARDAR CLIENTE";
            }
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarCliente_Load;
            chkRegistrarDatosLaborales.CheckedChanged += chkRegistrarDatosLaborales_CheckedChanged;
            btnGuardarCliente.Click += btnGuardarCliente_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void frmRegistrarCliente_Load(object? sender, EventArgs e)
        {
            if (_modoEdicion)
            {
                CargarCliente();
                CargarDatosLaborales();
            }
        }

        private void CargarEstadoCivil()
        {
            const string sql = @"exec dbo.sp_estado_civil_listar";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cbEstadoCivil.DataSource = dt;
            cbEstadoCivil.DisplayMember = "descripcion";
            cbEstadoCivil.ValueMember = "id";
            cbEstadoCivil.SelectedIndex = -1;
        }

        private void CargarEstado()
        {
            cbEstado.Items.Clear();
            cbEstado.Items.Add("activo");
            cbEstado.Items.Add("inactivo");
            cbEstado.SelectedItem = "activo";
        }

        private void chkRegistrarDatosLaborales_CheckedChanged(object? sender, EventArgs e)
        {
            HabilitarDatosLaborales(chkRegistrarDatosLaborales.Checked);
        }

        private void HabilitarDatosLaborales(bool habilitar)
        {
            txtEmpresa.Enabled = habilitar;
            txtCargo.Enabled = habilitar;
            txtIngresoMensual.Enabled = habilitar;
            txtAntiguedadLaboral.Enabled = habilitar;
            txtTelefonoTrabajo.Enabled = habilitar;
            txtDireccionTrabajo.Enabled = habilitar;

            if (!habilitar && !_modoEdicion)
            {
                txtEmpresa.Clear();
                txtCargo.Clear();
                txtIngresoMensual.Clear();
                txtAntiguedadLaboral.Clear();
                txtTelefonoTrabajo.Clear();
                txtDireccionTrabajo.Clear();
            }
        }

        private void btnGuardarCliente_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                int idClienteFinal = _modoEdicion ? _idCliente : GuardarClienteInsertar();

                if (_modoEdicion)
                    GuardarClienteActualizar();

                txtIdClienteGenerado.Text = idClienteFinal.ToString();

                if (chkRegistrarDatosLaborales.Checked)
                {
                    if (_idDatosLaborales.HasValue)
                        ActualizarDatosLaborales(idClienteFinal, _idDatosLaborales.Value);
                    else
                        InsertarDatosLaborales(idClienteFinal);
                }

                MessageBox.Show(
                    _modoEdicion
                        ? "Cliente actualizado correctamente."
                        : "Cliente registrado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                if (!_modoEdicion)
                    LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al guardar cliente:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private int GuardarClienteInsertar()
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_cliente_insertar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@identidad", txtIdentidad.Text.Trim());
            cmd.Parameters.AddWithValue("@nombres", txtNombres.Text.Trim());
            cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text.Trim());
            cmd.Parameters.AddWithValue("@fechaNacimiento", dtpFechaNacimiento.Value.Date);
            cmd.Parameters.AddWithValue("@telefono", ValorDb(txtTelefono.Text));
            cmd.Parameters.AddWithValue("@correo", ValorDb(txtCorreo.Text));
            cmd.Parameters.AddWithValue("@direccion", ValorDb(txtDireccion.Text));
            cmd.Parameters.AddWithValue("@estadoCivilId", Convert.ToInt32(cbEstadoCivil.SelectedValue));
            cmd.Parameters.AddWithValue("@rtn", ValorDb(txtRTN.Text));
            cmd.Parameters.AddWithValue("@estado", cbEstado.SelectedItem?.ToString() ?? "activo");

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count == 0)
                throw new Exception("No se devolvió el id del cliente generado.");

            if (dt.Columns.Contains("MensajeError") && dt.Rows[0]["MensajeError"] != DBNull.Value)
                throw new Exception(dt.Rows[0]["MensajeError"].ToString());

            return Convert.ToInt32(dt.Rows[0]["idClienteGenerado"]);
        }

        private void GuardarClienteActualizar()
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_cliente_actualizar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idCliente", _idCliente);
            cmd.Parameters.AddWithValue("@identidad", txtIdentidad.Text.Trim());
            cmd.Parameters.AddWithValue("@nombres", txtNombres.Text.Trim());
            cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text.Trim());
            cmd.Parameters.AddWithValue("@fechaNacimiento", dtpFechaNacimiento.Value.Date);
            cmd.Parameters.AddWithValue("@telefono", ValorDb(txtTelefono.Text));
            cmd.Parameters.AddWithValue("@correo", ValorDb(txtCorreo.Text));
            cmd.Parameters.AddWithValue("@direccion", ValorDb(txtDireccion.Text));
            cmd.Parameters.AddWithValue("@estadoCivilId", Convert.ToInt32(cbEstadoCivil.SelectedValue));
            cmd.Parameters.AddWithValue("@rtn", ValorDb(txtRTN.Text));
            cmd.Parameters.AddWithValue("@estado", cbEstado.SelectedItem?.ToString() ?? "activo");

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0 && dt.Columns.Contains("MensajeError") && dt.Rows[0]["MensajeError"] != DBNull.Value)
                throw new Exception(dt.Rows[0]["MensajeError"].ToString());
        }

        private void InsertarDatosLaborales(int idCliente)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_datos_laborales_insertar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idCliente", idCliente);
            cmd.Parameters.AddWithValue("@empresa", txtEmpresa.Text.Trim());
            cmd.Parameters.AddWithValue("@cargo", txtCargo.Text.Trim());
            cmd.Parameters.AddWithValue("@ingresoMensual", ParseDecimal(txtIngresoMensual.Text));
            cmd.Parameters.AddWithValue("@antiguedadLaboral", ValorDbInt(txtAntiguedadLaboral.Text));
            cmd.Parameters.AddWithValue("@telefonoTrabajo", ValorDb(txtTelefonoTrabajo.Text));
            cmd.Parameters.AddWithValue("@direccionTrabajo", ValorDb(txtDireccionTrabajo.Text));

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0 && dt.Columns.Contains("idDatosLaboralesGenerado"))
                _idDatosLaborales = Convert.ToInt32(dt.Rows[0]["idDatosLaboralesGenerado"]);
        }

        private void ActualizarDatosLaborales(int idCliente, int idDatosLaborales)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_datos_laborales_actualizar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idDatosLaborales", idDatosLaborales);
            cmd.Parameters.AddWithValue("@idCliente", idCliente);
            cmd.Parameters.AddWithValue("@empresa", txtEmpresa.Text.Trim());
            cmd.Parameters.AddWithValue("@cargo", txtCargo.Text.Trim());
            cmd.Parameters.AddWithValue("@ingresoMensual", ParseDecimal(txtIngresoMensual.Text));
            cmd.Parameters.AddWithValue("@antiguedadLaboral", ValorDbInt(txtAntiguedadLaboral.Text));
            cmd.Parameters.AddWithValue("@telefonoTrabajo", ValorDb(txtTelefonoTrabajo.Text));
            cmd.Parameters.AddWithValue("@direccionTrabajo", ValorDb(txtDireccionTrabajo.Text));

            cn.Open();
            cmd.ExecuteNonQuery();
        }



        private void CargarCliente()
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_cliente_obtener", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idCliente", _idCliente);

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count == 0)
                throw new Exception("No se encontró el cliente.");

            DataRow row = dt.Rows[0];

            txtIdClienteGenerado.Text = row["idCliente"].ToString();
            txtIdentidad.Text = row["identidad"]?.ToString() ?? "";
            txtNombres.Text = row["nombres"]?.ToString() ?? "";
            txtApellidos.Text = row["apellidos"]?.ToString() ?? "";
            txtTelefono.Text = row["telefono"]?.ToString() ?? "";
            txtCorreo.Text = row["correo"]?.ToString() ?? "";
            txtDireccion.Text = row["direccion"]?.ToString() ?? "";
            txtRTN.Text = row["rtn"]?.ToString() ?? "";

            if (row["fechaNacimiento"] != DBNull.Value)
                dtpFechaNacimiento.Value = Convert.ToDateTime(row["fechaNacimiento"]);

            if (row["estado"] != DBNull.Value)
                cbEstado.SelectedItem = row["estado"].ToString();

            if (row["estadoCivilId"] != DBNull.Value)
            {
                cbEstadoCivil.SelectedValue = Convert.ToInt32(row["estadoCivilId"]);
            }
            else
            {
                cbEstadoCivil.SelectedIndex = -1;
            }
        }

        private void CargarDatosLaborales()
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_datos_laborales_obtener_por_cliente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idCliente", _idCliente);

            DataTable dt = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                _idDatosLaborales = null;
                chkRegistrarDatosLaborales.Checked = false;
                return;
            }

            DataRow row = dt.Rows[0];
            _idDatosLaborales = Convert.ToInt32(row["idDatosLaborales"]);

            chkRegistrarDatosLaborales.Checked = true;

            txtEmpresa.Text = row["empresa"]?.ToString() ?? "";
            txtCargo.Text = row["cargo"]?.ToString() ?? "";
            txtIngresoMensual.Text = row["ingresoMensual"] == DBNull.Value ? "" : Convert.ToDecimal(row["ingresoMensual"]).ToString("N2");
            txtAntiguedadLaboral.Text = row["antiguedadLaboral"]?.ToString() ?? "";
            txtTelefonoTrabajo.Text = row["telefonoTrabajo"]?.ToString() ?? "";
            txtDireccionTrabajo.Text = row["direccionTrabajo"]?.ToString() ?? "";
        }

        private int ObtenerEstadoCivilId()
        {
            if (cbEstadoCivil.SelectedItem is not EstadoCivilItem item)
                throw new Exception("Seleccione el estado civil.");

            return item.Id;
        }

        private void ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtIdentidad.Text))
                throw new Exception("La identidad es obligatoria.");

            if (string.IsNullOrWhiteSpace(txtNombres.Text))
                throw new Exception("Los nombres son obligatorios.");

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
                throw new Exception("Los apellidos son obligatorios.");

            if (cbEstadoCivil.SelectedIndex < 0)
                throw new Exception("Seleccione el estado civil.");

            if (cbEstado.SelectedIndex < 0)
                throw new Exception("Seleccione el estado.");

            if (chkRegistrarDatosLaborales.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtEmpresa.Text))
                    throw new Exception("La empresa es obligatoria.");

                if (string.IsNullOrWhiteSpace(txtCargo.Text))
                    throw new Exception("El cargo es obligatorio.");

                if (string.IsNullOrWhiteSpace(txtIngresoMensual.Text))
                    throw new Exception("El ingreso mensual es obligatorio.");

                if (ParseDecimal(txtIngresoMensual.Text) < 0)
                    throw new Exception("El ingreso mensual no puede ser negativo.");

                if (!string.IsNullOrWhiteSpace(txtAntiguedadLaboral.Text) && ParseInt(txtAntiguedadLaboral.Text) < 0)
                    throw new Exception("La antigüedad laboral no puede ser negativa.");
            }
        }

        private object ValorDb(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : valor.Trim();
        }

        private object ValorDbInt(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : ParseInt(valor);
        }

        private decimal ParseDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new Exception("Valor decimal vacío.");

            valor = valor.Replace(",", "");

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal r))
                return r;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.CurrentCulture, out r))
                return r;

            throw new Exception($"El valor '{valor}' no es un número válido.");
        }

        private int ParseInt(string valor)
        {
            if (int.TryParse(valor.Trim(), out int r))
                return r;

            throw new Exception($"El valor '{valor}' no es un entero válido.");
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            if (_modoEdicion)
            {
                CargarCliente();
                CargarDatosLaborales();
                return;
            }

            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtIdentidad.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            dtpFechaNacimiento.Value = DateTime.Today;
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            txtRTN.Clear();

            cbEstadoCivil.SelectedIndex = -1;
            cbEstado.SelectedItem = "activo";

            chkRegistrarDatosLaborales.Checked = false;
            txtIdClienteGenerado.Clear();
            _idDatosLaborales = null;
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}