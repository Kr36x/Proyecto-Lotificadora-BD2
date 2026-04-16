using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarCliente : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idCliente;
        private int? _idDatosLaborales;

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
            CargarEstadoCivil();
            CargarEstados();

            ConfigurarDatePicker();

            if (_modoEdicion)
            {
                lblTitulo.Text = "EDITAR CLIENTE";
                btnCrearEditar.Text = "EDITAR";
            }
            else
            {
                lblTitulo.Text = "CREAR CLIENTE";
                btnCrearEditar.Text = "CREAR";
            }
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarCliente_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmRegistrarCliente_Load(object? sender, EventArgs e)
        {
            if (_modoEdicion)
            {
                CargarCliente();
                CargarDatosLaborales();
            }
        }

        private void ConfigurarDatePicker()
        {
            dtpFechaNacimiento.Format = DateTimePickerFormat.Custom;
            dtpFechaNacimiento.CustomFormat = "dd/MM/yyyy";
            dtpFechaNacimiento.FillColor = Color.White;
            dtpFechaNacimiento.ForeColor = Color.Black;
            dtpFechaNacimiento.BorderColor = Color.FromArgb(213, 218, 223);
            dtpFechaNacimiento.BorderThickness = 2;
            dtpFechaNacimiento.BorderRadius = 5;
            dtpFechaNacimiento.MaxDate = DateTime.Today;
            dtpFechaNacimiento.Value = DateTime.Today;
        }

        private void CargarEstadoCivil()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_estado_civil_listar");

                cbEstadoCivil.DataSource = dt;
                cbEstadoCivil.DisplayMember = "descripcion";
                cbEstadoCivil.ValueMember = "id";
                cbEstadoCivil.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar estado civil: " + ex.Message);
            }
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_estado_obtener_por_ids",
                    Db.Parameter("@Ids", "1,2")
                );

                if (dt.Rows.Count == 0)
                {
                    dt = new DataTable();
                    dt.Columns.Add("id", typeof(int));
                    dt.Columns.Add("nombre", typeof(string));
                    dt.Rows.Add(1, "activo");
                    dt.Rows.Add(2, "inactivo");
                }

                cbEstado.DataSource = dt;
                cbEstado.DisplayMember = "nombre";
                cbEstado.ValueMember = "id";
                cbEstado.SelectedValue = 1;
            }
            catch
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("nombre", typeof(string));
                dt.Rows.Add(1, "activo");
                dt.Rows.Add(2, "inactivo");

                cbEstado.DataSource = dt;
                cbEstado.DisplayMember = "nombre";
                cbEstado.ValueMember = "id";
                cbEstado.SelectedValue = 1;
            }
        }

        private void btnCrearEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                int idClienteFinal;

                if (_modoEdicion)
                {
                    ActualizarCliente();
                    idClienteFinal = _idCliente;
                }
                else
                {
                    idClienteFinal = InsertarCliente();
                }

                if (_idDatosLaborales.HasValue)
                    ActualizarDatosLaborales(idClienteFinal, _idDatosLaborales.Value);
                else
                    InsertarDatosLaborales(idClienteFinal);

                MessageBox.Show(
                    _modoEdicion
                        ? "Cliente actualizado correctamente."
                        : "Cliente registrado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al guardar cliente: " + ex.Message);
            }
        }

        private int InsertarCliente()
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_cliente_insertar",
                Db.Parameter("@identidad", txtIdentidad.Text.Trim()),
                Db.Parameter("@nombres", txtNombres.Text.Trim()),
                Db.Parameter("@apellidos", txtApellidos.Text.Trim()),
                Db.Parameter("@fechaNacimiento", dtpFechaNacimiento.Value.Date),
                Db.Parameter("@telefono", ValorDb(txtTelefono.Text)),
                Db.Parameter("@correo", ValorDb(txtCorreo.Text)),
                Db.Parameter("@direccion", ValorDb(txtDireccion.Text)),
                Db.Parameter("@estadoCivilId", cbEstadoCivil.SelectedValue),
                Db.Parameter("@rtn", ValorDb(txtRTN.Text)),
                Db.Parameter("@estado", cbEstado.SelectedValue)
            );

            ValidarRespuestaError(dt);

            if (dt.Rows.Count == 0 || !dt.Columns.Contains("idClienteGenerado"))
                throw new Exception("No se devolvió el id del cliente generado.");

            return Convert.ToInt32(dt.Rows[0]["idClienteGenerado"]);
        }

        private void ActualizarCliente()
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_cliente_actualizar",
                Db.Parameter("@idCliente", _idCliente),
                Db.Parameter("@identidad", txtIdentidad.Text.Trim()),
                Db.Parameter("@nombres", txtNombres.Text.Trim()),
                Db.Parameter("@apellidos", txtApellidos.Text.Trim()),
                Db.Parameter("@fechaNacimiento", dtpFechaNacimiento.Value.Date),
                Db.Parameter("@telefono", ValorDb(txtTelefono.Text)),
                Db.Parameter("@correo", ValorDb(txtCorreo.Text)),
                Db.Parameter("@direccion", ValorDb(txtDireccion.Text)),
                Db.Parameter("@estadoCivilId", cbEstadoCivil.SelectedValue),
                Db.Parameter("@rtn", ValorDb(txtRTN.Text)),
                Db.Parameter("@estado", cbEstado.SelectedValue)
            );

            ValidarRespuestaError(dt);
        }

        private void InsertarDatosLaborales(int idCliente)
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_datos_laborales_insertar",
                Db.Parameter("@idCliente", idCliente),
                Db.Parameter("@empresa", txtEmpresa.Text.Trim()),
                Db.Parameter("@cargo", txtCargo.Text.Trim()),
                Db.Parameter("@ingresoMensual", ParseDecimal(txtIngresoMensual.Text)),
                Db.Parameter("@antiguedadLaboral", ParseInt(txtAntiguedadLaboral.Text)),
                Db.Parameter("@telefonoTrabajo", ValorDb(txtTelefonoTrabajo.Text)),
                Db.Parameter("@direccionTrabajo", ValorDb(txtDireccionTrabajo.Text))
            );

            ValidarRespuestaError(dt);

            if (dt.Rows.Count > 0 && dt.Columns.Contains("idDatosLaboralesGenerado"))
                _idDatosLaborales = Convert.ToInt32(dt.Rows[0]["idDatosLaboralesGenerado"]);
        }

        private void ActualizarDatosLaborales(int idCliente, int idDatosLaborales)
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_datos_laborales_actualizar",
                Db.Parameter("@idDatosLaborales", idDatosLaborales),
                Db.Parameter("@idCliente", idCliente),
                Db.Parameter("@empresa", txtEmpresa.Text.Trim()),
                Db.Parameter("@cargo", txtCargo.Text.Trim()),
                Db.Parameter("@ingresoMensual", ParseDecimal(txtIngresoMensual.Text)),
                Db.Parameter("@antiguedadLaboral", ParseInt(txtAntiguedadLaboral.Text)),
                Db.Parameter("@telefonoTrabajo", ValorDb(txtTelefonoTrabajo.Text)),
                Db.Parameter("@direccionTrabajo", ValorDb(txtDireccionTrabajo.Text))
            );

            ValidarRespuestaError(dt);
        }

        private void CargarCliente()
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_cliente_obtener",
                Db.Parameter("@idCliente", _idCliente)
            );

            if (dt.Rows.Count == 0)
                throw new Exception("No se encontró el cliente.");

            DataRow row = dt.Rows[0];

            txtIdentidad.Text = row["identidad"]?.ToString() ?? "";
            txtNombres.Text = row["nombres"]?.ToString() ?? "";
            txtApellidos.Text = row["apellidos"]?.ToString() ?? "";
            txtTelefono.Text = row["telefono"]?.ToString() ?? "";
            txtCorreo.Text = row["correo"]?.ToString() ?? "";
            txtDireccion.Text = row["direccion"]?.ToString() ?? "";
            txtRTN.Text = row["rtn"]?.ToString() ?? "";

            if (row["fechaNacimiento"] != DBNull.Value)
                dtpFechaNacimiento.Value = Convert.ToDateTime(row["fechaNacimiento"]);

            if (row["estadoCivilId"] != DBNull.Value)
                cbEstadoCivil.SelectedValue = Convert.ToInt32(row["estadoCivilId"]);
            else
                cbEstadoCivil.SelectedIndex = -1;

            if (row["estadoId"] != DBNull.Value)
                cbEstado.SelectedValue = Convert.ToInt32(row["estadoId"]);
        }

        private void CargarDatosLaborales()
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_datos_laborales_obtener_por_cliente",
                Db.Parameter("@idCliente", _idCliente)
            );

            if (dt.Rows.Count == 0)
            {
                _idDatosLaborales = null;
                return;
            }

            DataRow row = dt.Rows[0];

            _idDatosLaborales = Convert.ToInt32(row["idDatosLaborales"]);
            txtEmpresa.Text = row["empresa"]?.ToString() ?? "";
            txtCargo.Text = row["cargo"]?.ToString() ?? "";
            txtIngresoMensual.Text = row["ingresoMensual"] == DBNull.Value ? "" : Convert.ToDecimal(row["ingresoMensual"]).ToString("N2");
            txtAntiguedadLaboral.Text = row["antiguedadLaboral"]?.ToString() ?? "";
            txtTelefonoTrabajo.Text = row["telefonoTrabajo"]?.ToString() ?? "";
            txtDireccionTrabajo.Text = row["direccionTrabajo"]?.ToString() ?? "";
        }

        private void ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtIdentidad.Text))
                throw new Exception("La identidad es obligatoria.");

            if (string.IsNullOrWhiteSpace(txtNombres.Text))
                throw new Exception("Los nombres son obligatorios.");

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
                throw new Exception("Los apellidos son obligatorios.");

            if (cbEstadoCivil.SelectedIndex < 0 || cbEstadoCivil.SelectedValue == null)
                throw new Exception("Seleccione el estado civil.");

            if (cbEstado.SelectedIndex < 0 || cbEstado.SelectedValue == null)
                throw new Exception("Seleccione el estado.");

            if (string.IsNullOrWhiteSpace(txtEmpresa.Text))
                throw new Exception("La empresa es obligatoria.");

            if (string.IsNullOrWhiteSpace(txtCargo.Text))
                throw new Exception("El cargo es obligatorio.");

            if (string.IsNullOrWhiteSpace(txtIngresoMensual.Text))
                throw new Exception("El ingreso mensual es obligatorio.");

            if (ParseDecimal(txtIngresoMensual.Text) < 0)
                throw new Exception("El ingreso mensual no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(txtAntiguedadLaboral.Text))
                throw new Exception("La antigüedad laboral es obligatoria.");

            if (ParseInt(txtAntiguedadLaboral.Text) < 0)
                throw new Exception("La antigüedad laboral no puede ser negativa.");
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
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            txtRTN.Clear();

            txtEmpresa.Clear();
            txtCargo.Clear();
            txtIngresoMensual.Clear();
            txtAntiguedadLaboral.Clear();
            txtTelefonoTrabajo.Clear();
            txtDireccionTrabajo.Clear();

            dtpFechaNacimiento.Value = DateTime.Today;
            cbEstadoCivil.SelectedIndex = -1;
            cbEstado.SelectedValue = 1;
            _idDatosLaborales = null;
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ValidarRespuestaError(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0)
                return;

            if (dt.Columns.Contains("MensajeError") &&
                dt.Rows[0]["MensajeError"] != DBNull.Value)
            {
                throw new Exception(dt.Rows[0]["MensajeError"].ToString());
            }
        }

        private object ValorDb(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : valor.Trim();
        }

        private decimal ParseDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new Exception("Valor decimal vacío.");

            valor = valor.Trim();

            if (decimal.TryParse(valor, NumberStyles.Any, new CultureInfo("es-HN"), out decimal r))
                return r;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out r))
                return r;

            throw new Exception($"El valor '{valor}' no es un número válido.");
        }

        private int ParseInt(string valor)
        {
            if (int.TryParse(valor.Trim(), out int r))
                return r;

            throw new Exception($"El valor '{valor}' no es un entero válido.");
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