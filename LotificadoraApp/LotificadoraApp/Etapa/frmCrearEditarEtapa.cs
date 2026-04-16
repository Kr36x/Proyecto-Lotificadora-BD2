using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace LotificadoraApp.Etapa
{
    public partial class frmRegistrarEtapa : Form
    {
        private readonly bool _esEdicion;
        private readonly int _idEtapa;

        public bool OperacionExitosa { get; private set; } = false;

        public frmRegistrarEtapa()
        {
            InitializeComponent();

            _esEdicion = false;
            _idEtapa = 0;

            ConfigurarFormulario();
        }

        public frmRegistrarEtapa(int idEtapa)
        {
            InitializeComponent();

            _esEdicion = true;
            _idEtapa = idEtapa;

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            Load += frmCrearEditarEtapa_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
            
 
            AcceptButton = btnCrearEditar;
            CancelButton = btnCancelar;
        }
        private void ConfigurarDatePickers()
        {
            EstilizarDatePicker(dtpFechaInicio);
            EstilizarDatePicker(dtpFechaFinEstimada);

            // fecha fin opcional
            dtpFechaFinEstimada.Checked = false;
            dtpFechaFinEstimada.ShowCheckBox = true;
        }

        private void EstilizarDatePicker(Guna.UI2.WinForms.Guna2DateTimePicker dtp)
        {
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "dd/MM/yyyy";

            dtp.FillColor = Color.White;
            dtp.ForeColor = Color.Black;
            dtp.BorderColor = Color.FromArgb(213, 218, 223);
            dtp.BorderThickness = 2;
            dtp.BorderRadius = 5;

            dtp.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // parte desplegable
            dtp.CheckedState.FillColor = Color.White;
            dtp.CheckedState.ForeColor = Color.Black;
            dtp.HoverState.BorderColor = Color.Olive;
            dtp.FocusedColor = Color.Olive;
            dtp.ShadowDecoration.Enabled = false;
        }
        private void frmCrearEditarEtapa_Load(object sender, EventArgs e)
        {
            try
            {
                CargarProyectos();
                CargarEstados();

                lblAreaTotal.Text = "Área total (vr²)";
                lblPrecioV2.Text = "Precio por Vr²";

                dtpFechaInicio.Format = DateTimePickerFormat.Short;
                dtpFechaFinEstimada.Format = DateTimePickerFormat.Short;
                //dtpFechaFinEstimada.ShowCheckBox = true;
                //dtpFechaFinEstimada.Checked = false;

                dtpFechaInicio.CheckedState.Parent = dtpFechaInicio;
                dtpFechaInicio.HoverState.Parent = dtpFechaInicio;


                dtpFechaFinEstimada.CheckedState.Parent = dtpFechaFinEstimada;
                dtpFechaFinEstimada.HoverState.Parent = dtpFechaFinEstimada;
                ConfigurarDatePickers();

                if (_esEdicion)
                {
                    lblTitulo.Text = "EDITAR ETAPA";
                    btnCrearEditar.Text = "EDITAR";
                    CargarDatosEtapa();
                }
                else
                {
                    lblTitulo.Text = "CREAR ETAPA";
                    btnCrearEditar.Text = "CREAR";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar el formulario: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarProyectos()
        {
            var dt = Db.ExecuteQuery(@"
                SELECT idProyecto, nombreProyecto
                FROM Proyecto
                ORDER BY nombreProyecto;
            ");

            cmbProyecto.DataSource = dt;
            cmbProyecto.DisplayMember = "nombreProyecto";
            cmbProyecto.ValueMember = "idProyecto";
            cmbProyecto.SelectedIndex = -1;
        }

        private void CargarEstados()
        {
            var dt = Db.ExecuteQuery(@"
                SELECT id, nombre
                FROM Estado
                WHERE id IN (4, 5, 6)
                ORDER BY nombre;
            ");

            cmbEstado.DataSource = dt;
            cmbEstado.DisplayMember = "nombre";
            cmbEstado.ValueMember = "id";
            cmbEstado.SelectedIndex = -1;
        }

        private void CargarDatosEtapa()
        {
            var dt = Db.ExecuteStoredProcedure(
                "sp_etapa_obtener",
                Db.Parameter("@idEtapa", _idEtapa)
            );

            if (dt.Rows.Count == 0)
                throw new Exception("No se encontró la etapa seleccionada.");

            DataRow row = dt.Rows[0];

            cmbProyecto.SelectedValue = Convert.ToInt32(row["idProyecto"]);
            txtNombreEtapa.Text = row["nombreEtapa"]?.ToString() ?? string.Empty;

            dtpFechaInicio.Value = Convert.ToDateTime(row["fechaInicio"]);

            if (row["fechaFinEstimada"] == DBNull.Value)
            {
                dtpFechaFinEstimada.Checked = false;
            }
            else
            {
                dtpFechaFinEstimada.Checked = true;
                dtpFechaFinEstimada.Value = Convert.ToDateTime(row["fechaFinEstimada"]);
            }

            txtAreaTotalV2.Text = Convert.ToDecimal(row["areaTotalV2"]).ToString("N2");
            txtPorcentajeAreaVerde.Text = Convert.ToDecimal(row["porcentajeAreaVerde"]).ToString("N2");
            txtPorcentajeAreaComun.Text = Convert.ToDecimal(row["porcentajeAreaComun"]).ToString("N2");
            txtPorcentajeAreaLotes.Text = Convert.ToDecimal(row["porcentajeAreaLotes"]).ToString("N2");
            txtPrecioVaraCuadrada.Text = Convert.ToDecimal(row["precioVaraCuadrada"]).ToString("N2");
            txtTasaInteresAnual.Text = Convert.ToDecimal(row["tasaInteresAnual"]).ToString("N2");
            cmbEstado.SelectedValue = Convert.ToInt32(row["estadoId"]);
        }

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario())
                return;

            try
            {
                int idProyecto = Convert.ToInt32(cmbProyecto.SelectedValue);
                string nombreEtapa = txtNombreEtapa.Text.Trim();
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                object fechaFinEstimada = dtpFechaFinEstimada.Checked
                    ? dtpFechaFinEstimada.Value.Date
                    : DBNull.Value;

                decimal areaTotalV2 = ParseDecimal(txtAreaTotalV2.Text);
                decimal porcentajeAreaVerde = ParseDecimal(txtPorcentajeAreaVerde.Text);
                decimal porcentajeAreaComun = ParseDecimal(txtPorcentajeAreaComun.Text);
                decimal porcentajeAreaLotes = ParseDecimal(txtPorcentajeAreaLotes.Text);
                decimal precioVaraCuadrada = ParseDecimal(txtPrecioVaraCuadrada.Text);
                decimal tasaInteresAnual = ParseDecimal(txtTasaInteresAnual.Text);
                int estado = Convert.ToInt32(cmbEstado.SelectedValue);

                if (_esEdicion)
                {
                    var dt = Db.ExecuteStoredProcedure(
                        "sp_etapa_actualizar",
                        Db.Parameter("@idEtapa", _idEtapa),
                        Db.Parameter("@idProyecto", idProyecto),
                        Db.Parameter("@nombreEtapa", nombreEtapa),
                        Db.Parameter("@fechaInicio", fechaInicio),
                        Db.Parameter("@fechaFinEstimada", fechaFinEstimada),
                        Db.Parameter("@areaTotalV2", areaTotalV2),
                        Db.Parameter("@porcentajeAreaVerde", porcentajeAreaVerde),
                        Db.Parameter("@porcentajeAreaComun", porcentajeAreaComun),
                        Db.Parameter("@porcentajeAreaLotes", porcentajeAreaLotes),
                        Db.Parameter("@precioVaraCuadrada", precioVaraCuadrada),
                        Db.Parameter("@tasaInteresAnual", tasaInteresAnual),
                        Db.Parameter("@estado", estado)
                    );

                    ValidarRespuestaError(dt);

                    MessageBox.Show(
                        "Etapa actualizada correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    var dt = Db.ExecuteStoredProcedure(
                        "sp_etapa_insertar",
                        Db.Parameter("@idProyecto", idProyecto),
                        Db.Parameter("@nombreEtapa", nombreEtapa),
                        Db.Parameter("@fechaInicio", fechaInicio),
                        Db.Parameter("@fechaFinEstimada", fechaFinEstimada),
                        Db.Parameter("@areaTotalV2", areaTotalV2),
                        Db.Parameter("@porcentajeAreaVerde", porcentajeAreaVerde),
                        Db.Parameter("@porcentajeAreaComun", porcentajeAreaComun),
                        Db.Parameter("@porcentajeAreaLotes", porcentajeAreaLotes),
                        Db.Parameter("@precioVaraCuadrada", precioVaraCuadrada),
                        Db.Parameter("@tasaInteresAnual", tasaInteresAnual),
                        Db.Parameter("@estado", estado)
                    );

                    ValidarRespuestaError(dt);

                    MessageBox.Show(
                        "Etapa creada correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                OperacionExitosa = true;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al guardar la etapa: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool ValidarFormulario()
        {
            if (cmbProyecto.SelectedIndex < 0 || cmbProyecto.SelectedValue == null)
            {
                MessageBox.Show("Debes seleccionar un proyecto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProyecto.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreEtapa.Text))
            {
                MessageBox.Show("El nombre de la etapa es requerido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreEtapa.Focus();
                return false;
            }

            if (cmbEstado.SelectedIndex < 0 || cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Debes seleccionar un estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEstado.Focus();
                return false;
            }

            try
            {
                decimal areaTotalV2 = ParseDecimal(txtAreaTotalV2.Text);
                decimal porcentajeAreaVerde = ParseDecimal(txtPorcentajeAreaVerde.Text);
                decimal porcentajeAreaComun = ParseDecimal(txtPorcentajeAreaComun.Text);
                decimal porcentajeAreaLotes = ParseDecimal(txtPorcentajeAreaLotes.Text);
                decimal precioVaraCuadrada = ParseDecimal(txtPrecioVaraCuadrada.Text);
                decimal tasaInteresAnual = ParseDecimal(txtTasaInteresAnual.Text);

                if (areaTotalV2 <= 0)
                    throw new Exception("El área total debe ser mayor que cero.");

                if (precioVaraCuadrada <= 0)
                    throw new Exception("El precio por vara cuadrada debe ser mayor que cero.");

                if (porcentajeAreaVerde < 0 || porcentajeAreaComun < 0 || porcentajeAreaLotes < 0)
                    throw new Exception("Los porcentajes no pueden ser negativos.");

                decimal suma = porcentajeAreaVerde + porcentajeAreaComun + porcentajeAreaLotes;
                if (suma != 100)
                    throw new Exception("La suma de porcentaje área verde, área común y área lotes debe ser igual a 100.");

                if (tasaInteresAnual < 0)
                    throw new Exception("La tasa de interés anual no puede ser negativa.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private decimal ParseDecimal(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return 0;

            texto = texto.Trim();

            if (decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorInvariante))
                return valorInvariante;

            if (decimal.TryParse(texto, NumberStyles.Any, new CultureInfo("es-HN"), out decimal valorEs))
                return valorEs;

            throw new Exception($"Valor numérico inválido: {texto}");
        }

        private void ValidarRespuestaError(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0)
                return;

            bool tieneMensajeError = dt.Columns
                .Cast<DataColumn>()
                .Any(c => c.ColumnName.Equals("MensajeError", StringComparison.OrdinalIgnoreCase));

            if (tieneMensajeError)
            {
                string mensaje = dt.Rows[0]["MensajeError"]?.ToString() ?? "Ocurrió un error.";
                throw new Exception(mensaje);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}