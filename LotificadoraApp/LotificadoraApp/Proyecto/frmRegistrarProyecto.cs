using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LotificadoraApp.Proyecto
{
    public partial class frmRegistrarProyecto : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idProyecto;

        private string _estadoInicialNombre = string.Empty;
        private int _departamentoInicialId = 0;
        private int _municipioInicialId = 0;
        private string _coloniaInicial = string.Empty;
        private string _direccionDetalleInicial = string.Empty;
        private string _claveCatastralInicial = string.Empty;
        private string _observacionLegalInicial = string.Empty;

        private bool _cargandoCombos = false;
        private DataTable _dtMunicipios = new DataTable();

        public frmRegistrarProyecto()
        {
            InitializeComponent();

            _modoEdicion = false;
            _idProyecto = 0;

            ConfigurarDateTimePicker();
            ConectarEventos();
        }

        public frmRegistrarProyecto(
            int idProyecto,
            string nombreProyecto,
            string descripcion,
            DateTime fechaInicio,
            DateTime? fechaFinEstimada,
            decimal areaTotalV2,
            int maxAniosFinanciamiento,
            string estadoNombre,
            int departamentoId,
            int municipioId,
            string aldeaColonia,
            string direccionDetalle,
            string claveCatastral,
            string observacionLegal)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idProyecto = idProyecto;

            _estadoInicialNombre = estadoNombre ?? string.Empty;
            _departamentoInicialId = departamentoId;
            _municipioInicialId = municipioId;
            _coloniaInicial = aldeaColonia ?? string.Empty;
            _direccionDetalleInicial = direccionDetalle ?? string.Empty;
            _claveCatastralInicial = claveCatastral ?? string.Empty;
            _observacionLegalInicial = observacionLegal ?? string.Empty;

            ConfigurarDateTimePicker();
            ConectarEventos();

            txtNombreProyecto.Text = nombreProyecto;
            txtDescripcion.Text = descripcion;
            dtpFechaInicio.Value = fechaInicio;

            if (fechaFinEstimada.HasValue)
                dtpFechaFinEstimada.Value = fechaFinEstimada.Value;

            txtAreaTotal.Text = areaTotalV2.ToString("N2");
            txtAnioFinanciamiento.Text = maxAniosFinanciamiento.ToString();
            txtColonia.Text = _coloniaInicial;
            txtDireccionDetalle.Text = _direccionDetalleInicial;
            txtClaveCatastral.Text = _claveCatastralInicial;
            txtObsLegal.Text = _observacionLegalInicial;
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarProyecto_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
            cmbDepartamento.SelectedIndexChanged += cmbDepartamento_SelectedIndexChanged;
        }

        private void ConfigurarDateTimePicker()
        {
            dtpFechaInicio.Format = DateTimePickerFormat.Custom;
            dtpFechaInicio.CustomFormat = "yyyy-MM-dd";
            dtpFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpFechaInicio.FillColor = Color.White;
            dtpFechaInicio.ForeColor = Color.Black;

            dtpFechaFinEstimada.Format = DateTimePickerFormat.Custom;
            dtpFechaFinEstimada.CustomFormat = "yyyy-MM-dd";
            dtpFechaFinEstimada.Value = DateTime.Today;
            dtpFechaFinEstimada.FillColor = Color.White;
            dtpFechaFinEstimada.ForeColor = Color.Black;
        }

        private void frmRegistrarProyecto_Load(object sender, EventArgs e)
        {
            _cargandoCombos = true;

            CargarEstados();
            CargarDepartamentos();
            CargarMunicipios();

            if (_modoEdicion)
            {
                lblBloque.Text = "EDITAR PROYECTO";
                btnCrearEditar.Text = "EDITAR";

                SeleccionarEstadoPorNombre(_estadoInicialNombre);

                if (_departamentoInicialId > 0)
                {
                    cmbDepartamento.SelectedValue = _departamentoInicialId;
                    FiltrarMunicipiosPorDepartamento();
                }

                if (_municipioInicialId > 0)
                {
                    cmbMunicipio.SelectedValue = _municipioInicialId;
                }

                txtColonia.Text = _coloniaInicial;
            }
            else
            {
                lblBloque.Text = "CREAR PROYECTO";
                btnCrearEditar.Text = "CREAR";
                cmbEstado.SelectedIndex = -1;
                cmbDepartamento.SelectedIndex = -1;
                cmbMunicipio.DataSource = null;
            }

            _cargandoCombos = false;
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    ProyectoQueries.QR001,
                    new SqlParameter("@Ids", "1,2,3")
                );

                cmbEstado.DataSource = dataTable;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los estados");
            }
        }

        private void CargarDepartamentos()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure("sp_departamento_listar");

                cmbDepartamento.DataSource = dataTable;
                cmbDepartamento.DisplayMember = "nombre";
                cmbDepartamento.ValueMember = "id";
                cmbDepartamento.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los departamentos");
            }
        }

        private void CargarMunicipios()
        {
            try
            {
                _dtMunicipios = Db.ExecuteStoredProcedure("sp_municipio_listar");
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los municipios");
            }
        }

        private void FiltrarMunicipiosPorDepartamento()
        {
            try
            {
                if (cmbDepartamento.SelectedItem is not DataRowView departamentoSeleccionado)
                {
                    cmbMunicipio.DataSource = null;
                    return;
                }

                string nombreDepartamento = departamentoSeleccionado["nombre"]?.ToString() ?? "";

                if (_dtMunicipios == null || _dtMunicipios.Rows.Count == 0)
                {
                    cmbMunicipio.DataSource = null;
                    return;
                }

                DataView view = _dtMunicipios.DefaultView;
                view.RowFilter = $"departamento = '{nombreDepartamento.Replace("'", "''")}'";

                cmbMunicipio.DataSource = view.ToTable();
                cmbMunicipio.DisplayMember = "nombre";
                cmbMunicipio.ValueMember = "id";
                cmbMunicipio.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al filtrar los municipios");
            }
        }

        private void SeleccionarEstadoPorNombre(string estadoNombre)
        {
            if (string.IsNullOrWhiteSpace(estadoNombre))
                return;

            for (int i = 0; i < cmbEstado.Items.Count; i++)
            {
                if (cmbEstado.Items[i] is DataRowView item)
                {
                    string nombre = item["nombre"]?.ToString() ?? "";

                    if (string.Equals(nombre, estadoNombre, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbEstado.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void cmbDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            FiltrarMunicipiosPorDepartamento();
        }

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarProyecto(out decimal areaTotal, out int aniosFinanciamiento))
                    return;

                DataTable dataTable;

                if (_modoEdicion)
                {
                    dataTable = Db.ExecuteStoredProcedure(
                        "sp_proyecto_actualizar",
                        new SqlParameter("@idProyecto", _idProyecto),
                        new SqlParameter("@nombreProyecto", txtNombreProyecto.Text.Trim()),
                        new SqlParameter("@descripcion",
                            string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : txtDescripcion.Text.Trim()),
                        new SqlParameter("@fechaInicio", dtpFechaInicio.Value.Date),
                        new SqlParameter("@fechaFinEstimada", dtpFechaFinEstimada.Value.Date),
                        new SqlParameter("@areaTotalV2", areaTotal),
                        new SqlParameter("@maxAniosFinanciamiento", aniosFinanciamiento),
                        new SqlParameter("@estado", Convert.ToInt32(cmbEstado.SelectedValue)),
                        new SqlParameter("@departamentoId", Convert.ToInt32(cmbDepartamento.SelectedValue)),
                        new SqlParameter("@municipioId", Convert.ToInt32(cmbMunicipio.SelectedValue)),
                        new SqlParameter("@aldeaColonia",
                            string.IsNullOrWhiteSpace(txtColonia.Text) ? DBNull.Value : txtColonia.Text.Trim()),
                        new SqlParameter("@direccionDetalle",
                            string.IsNullOrWhiteSpace(txtDireccionDetalle.Text) ? DBNull.Value : txtDireccionDetalle.Text.Trim()),
                        new SqlParameter("@claveCatastral",
                            string.IsNullOrWhiteSpace(txtClaveCatastral.Text) ? DBNull.Value : txtClaveCatastral.Text.Trim()),
                        new SqlParameter("@observacionLegal",
                            string.IsNullOrWhiteSpace(txtObsLegal.Text) ? DBNull.Value : txtObsLegal.Text.Trim())
                    );
                }
                else
                {
                    dataTable = Db.ExecuteStoredProcedure(
                        "sp_proyecto_insertar",
                        new SqlParameter("@nombreProyecto", txtNombreProyecto.Text.Trim()),
                        new SqlParameter("@descripcion",
                            string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : txtDescripcion.Text.Trim()),
                        new SqlParameter("@fechaInicio", dtpFechaInicio.Value.Date),
                        new SqlParameter("@fechaFinEstimada", dtpFechaFinEstimada.Value.Date),
                        new SqlParameter("@areaTotalV2", areaTotal),
                        new SqlParameter("@maxAniosFinanciamiento", aniosFinanciamiento),
                        new SqlParameter("@estado", Convert.ToInt32(cmbEstado.SelectedValue)),
                        new SqlParameter("@departamentoId", Convert.ToInt32(cmbDepartamento.SelectedValue)),
                        new SqlParameter("@municipioId", Convert.ToInt32(cmbMunicipio.SelectedValue)),
                        new SqlParameter("@aldeaColonia",
                            string.IsNullOrWhiteSpace(txtColonia.Text) ? DBNull.Value : txtColonia.Text.Trim()),
                        new SqlParameter("@direccionDetalle",
                            string.IsNullOrWhiteSpace(txtDireccionDetalle.Text) ? DBNull.Value : txtDireccionDetalle.Text.Trim()),
                        new SqlParameter("@claveCatastral",
                            string.IsNullOrWhiteSpace(txtClaveCatastral.Text) ? DBNull.Value : txtClaveCatastral.Text.Trim()),
                        new SqlParameter("@observacionLegal",
                            string.IsNullOrWhiteSpace(txtObsLegal.Text) ? DBNull.Value : txtObsLegal.Text.Trim())
                    );
                }

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    _modoEdicion ? "Proyecto actualizado con éxito" : "Registro guardado con éxito",
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

        private bool ValidarProyecto(out decimal areaTotal, out int aniosFinanciamiento)
        {
            areaTotal = 0;
            aniosFinanciamiento = 0;

            if (string.IsNullOrWhiteSpace(txtNombreProyecto.Text))
            {
                MostrarWarning("El nombre del proyecto es requerido");
                txtNombreProyecto.Focus();
                return false;
            }

            if (cmbDepartamento.SelectedValue == null ||
                !int.TryParse(cmbDepartamento.SelectedValue.ToString(), out int departamentoId) ||
                departamentoId <= 0)
            {
                MostrarWarning("El departamento es requerido");
                cmbDepartamento.Focus();
                return false;
            }

            if (cmbMunicipio.SelectedValue == null ||
                !int.TryParse(cmbMunicipio.SelectedValue.ToString(), out int municipioId) ||
                municipioId <= 0)
            {
                MostrarWarning("El municipio es requerido");
                cmbMunicipio.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtColonia.Text))
            {
                MostrarWarning("La colonia es requerida");
                txtColonia.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAreaTotal.Text))
            {
                MostrarWarning("El área total es requerida");
                txtAreaTotal.Focus();
                return false;
            }

            if (!TryParseDecimal(txtAreaTotal.Text, out areaTotal) || areaTotal <= 0)
            {
                MostrarWarning("El área total debe ser un número mayor que 0");
                txtAreaTotal.Focus();
                return false;
            }

            if (dtpFechaInicio.Value.Date > dtpFechaFinEstimada.Value.Date)
            {
                MostrarWarning("La fecha inicio no puede ser mayor que la fecha fin estimada");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAnioFinanciamiento.Text))
            {
                MostrarWarning("El máximo de años de financiamiento es requerido");
                txtAnioFinanciamiento.Focus();
                return false;
            }

            if (!int.TryParse(txtAnioFinanciamiento.Text.Trim(), out aniosFinanciamiento) || aniosFinanciamiento <= 0)
            {
                MostrarWarning("El máximo de años de financiamiento debe ser mayor que 0");
                txtAnioFinanciamiento.Focus();
                return false;
            }

            if (cmbEstado.SelectedValue == null ||
                cmbEstado.SelectedIndex < 0 ||
                Convert.ToInt32(cmbEstado.SelectedValue) <= 0)
            {
                MostrarWarning("El estado es requerido");
                cmbEstado.Focus();
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

        private void btnCancelar_Click(object sender, EventArgs e)
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

        private void btnCancelar_Click_1(object sender, EventArgs e) { }
        private void txtAreaTotal_TextChanged(object sender, EventArgs e) { }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void btnCrearEditar_Click_1(object sender, EventArgs e) { }
        private void guna2HtmlLabel10_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel8_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel9_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel12_Click(object sender, EventArgs e) { }
    }
}