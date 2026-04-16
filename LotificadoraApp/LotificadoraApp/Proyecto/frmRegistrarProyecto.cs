using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LotificadoraApp.Proyecto
{
    public partial class frmRegistrarProyecto : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idProyecto;

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
            string estadoNombre)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idProyecto = idProyecto;

            ConfigurarDateTimePicker();
            ConectarEventos();

            txtNombreProyecto.Text = nombreProyecto;
            txtDescripcion.Text = descripcion;
            dtpFechaInicio.Value = fechaInicio;

            if (fechaFinEstimada.HasValue)
                dtpFechaFinEstimada.Value = fechaFinEstimada.Value;

            txtAreaTotal.Text = areaTotalV2.ToString("N2");
            txtAnioFinanciamiento.Text = maxAniosFinanciamiento.ToString();

            _estadoInicialNombre = estadoNombre ?? string.Empty;
        }

        private string _estadoInicialNombre = string.Empty;

        private void ConectarEventos()
        {
            Load += frmRegistrarProyecto_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
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
            CargarEstados();

            if (_modoEdicion)
            {
                lblBloque.Text = "EDITAR PROYECTO";
                btnCrearEditar.Text = "EDITAR";

                if (!string.IsNullOrWhiteSpace(_estadoInicialNombre))
                {
                    for (int i = 0; i < cmbEstado.Items.Count; i++)
                    {
                        if (cmbEstado.Items[i] is DataRowView item)
                        {
                            string nombre = item["nombre"]?.ToString() ?? "";
                            if (string.Equals(nombre, _estadoInicialNombre, StringComparison.OrdinalIgnoreCase))
                            {
                                cmbEstado.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                lblBloque.Text = "CREAR PROYECTO";
                btnCrearEditar.Text = "CREAR";
            }
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
                cmbEstado.SelectedIndex = _modoEdicion ? cmbEstado.SelectedIndex : -1;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los estados");
            }
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
                        new SqlParameter("@estado", Convert.ToInt32(cmbEstado.SelectedValue))
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
                        new SqlParameter("@estado", Convert.ToInt32(cmbEstado.SelectedValue))
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
            catch
            {
                MostrarMensajeError("Ocurrió un error al guardar");
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
                MostrarWarning("La fechaInicio no puede ser mayor que fechaFinEstimada");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAnioFinanciamiento.Text))
            {
                MostrarWarning("maxAniosFinanciamiento es requerido");
                txtAnioFinanciamiento.Focus();
                return false;
            }

            if (!int.TryParse(txtAnioFinanciamiento.Text.Trim(), out aniosFinanciamiento) || aniosFinanciamiento <= 0)
            {
                MostrarWarning("maxAniosFinanciamiento debe ser mayor que 0");
                txtAnioFinanciamiento.Focus();
                return false;
            }

            if (cmbEstado.SelectedValue == null || cmbEstado.SelectedIndex < 0 || Convert.ToInt32(cmbEstado.SelectedValue) <= 0)
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
    }
}