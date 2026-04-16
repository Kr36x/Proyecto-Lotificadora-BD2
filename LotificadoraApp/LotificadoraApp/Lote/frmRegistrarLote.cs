using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LotificadoraApp
{
    public partial class frmRegistrarLote : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idLote;

        public frmRegistrarLote()
        {
            InitializeComponent();
            _modoEdicion = false;
            _idLote = 0;

            ConectarEventosCalculo();
            ConectarEventosFormulario();
        }

        public frmRegistrarLote(int idLote)
        {
            InitializeComponent();
            _modoEdicion = true;
            _idLote = idLote;

            ConectarEventosCalculo();
            ConectarEventosFormulario();
        }

        private void ConectarEventosFormulario()
        {
            Load += frmRegistrarLote_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmRegistrarLote_Load(object sender, EventArgs e)
        {
            ConfigurarFormulario();
            CargarBloques();
            CargarEstados();

            if (_modoEdicion)
            {
                lblBloque.Text = "EDITAR LOTE";
                btnCrearEditar.Text = "EDITAR";
                CargarLote();
            }
            else
            {
                lblBloque.Text = "CREAR LOTE";
                btnCrearEditar.Text = "CREAR";
                CalcularPrecios();
            }
        }

        private void ConfigurarFormulario()
        {
            txtPrecioBase.ReadOnly = true;
            txtRecargoTotal.ReadOnly = true;
            txtPrecioFinal.ReadOnly = true;

            txtPrecioBase.TabStop = false;
            txtRecargoTotal.TabStop = false;
            txtPrecioFinal.TabStop = false;
        }

        private void ConectarEventosCalculo()
        {
            txtArea.TextChanged += (_, _) => CalcularPrecios();
            cmbBloque.SelectedIndexChanged += (_, _) => CalcularPrecios();
            chkEsquina.CheckedChanged += (_, _) => CalcularPrecios();
            chkParque.CheckedChanged += (_, _) => CalcularPrecios();
            chkCalleCerrada.CheckedChanged += (_, _) => CalcularPrecios();
        }

        private void CargarBloques()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_lote_cargar_bloques");

                cmbBloque.DataSource = dt;
                cmbBloque.DisplayMember = "nombreBloque";
                cmbBloque.ValueMember = "idBloque";
                cmbBloque.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar los bloques: " + ex.Message);
            }
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteQuery(@"
                    SELECT id, nombre
                    FROM Estado
                    WHERE id IN (7, 8, 9)
                    ORDER BY id;
                ");

                cmbEstado.DataSource = dt;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = 7;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar los estados: " + ex.Message);
            }
        }

        private void CargarLote()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_lote_obtener_por_id",
                    Db.Parameter("@idLote", _idLote)
                );

                if (dt.Rows.Count == 0)
                {
                    MostrarMensajeError("No se encontró el lote seleccionado.");
                    Close();
                    return;
                }

                DataRow row = dt.Rows[0];

                cmbBloque.SelectedValue = Convert.ToInt32(row["idBloque"]);
                txtLote.Text = row["numeroLote"]?.ToString() ?? "";
                txtArea.Text = Convert.ToDecimal(row["areaV2"]).ToString("N2");

                chkEsquina.Checked = row["esEsquina"] != DBNull.Value && Convert.ToBoolean(row["esEsquina"]);
                chkParque.Checked = row["cercaParque"] != DBNull.Value && Convert.ToBoolean(row["cercaParque"]);
                chkCalleCerrada.Checked = row["calleCerrada"] != DBNull.Value && Convert.ToBoolean(row["calleCerrada"]);

                txtPrecioBase.Text = row["precioBase"] == DBNull.Value ? "" : Convert.ToDecimal(row["precioBase"]).ToString("N2");
                txtRecargoTotal.Text = row["recargoTotal"] == DBNull.Value ? "" : Convert.ToDecimal(row["recargoTotal"]).ToString("N2");
                txtPrecioFinal.Text = row["precioFinal"] == DBNull.Value ? "" : Convert.ToDecimal(row["precioFinal"]).ToString("N2");

                cmbEstado.SelectedValue = Convert.ToInt32(row["estadoId"]);

                CalcularPrecios();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar el lote: " + ex.Message);
            }
        }

        private void CalcularPrecios()
        {
            try
            {
                txtPrecioBase.Clear();
                txtRecargoTotal.Clear();
                txtPrecioFinal.Clear();

                if (cmbBloque.SelectedIndex < 0)
                    return;

                if (!TryParseDecimal(txtArea.Text, out decimal areaV2) || areaV2 <= 0)
                    return;

                if (cmbBloque.SelectedItem is not DataRowView row)
                    return;

                if (!row.Row.Table.Columns.Contains("precioVaraCuadrada"))
                    return;

                decimal precioVaraCuadrada = Convert.ToDecimal(row["precioVaraCuadrada"]);
                decimal precioBase = areaV2 * precioVaraCuadrada;

                decimal porcentajeRecargo = 0m;

                if (chkEsquina.Checked)
                    porcentajeRecargo += 0.10m;

                if (chkParque.Checked)
                    porcentajeRecargo += 0.05m;

                if (chkCalleCerrada.Checked)
                    porcentajeRecargo += 0.03m;

                decimal recargoTotal = precioBase * porcentajeRecargo;
                decimal precioFinal = precioBase + recargoTotal;

                txtPrecioBase.Text = precioBase.ToString("N2");
                txtRecargoTotal.Text = recargoTotal.ToString("N2");
                txtPrecioFinal.Text = precioFinal.ToString("N2");
            }
            catch
            {
                txtPrecioBase.Clear();
                txtRecargoTotal.Clear();
                txtPrecioFinal.Clear();
            }
        }

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario(out decimal areaV2, out decimal precioBase, out decimal recargoTotal, out decimal precioFinal))
                    return;

                int idBloque = Convert.ToInt32(cmbBloque.SelectedValue);
                string numeroLote = txtLote.Text.Trim();
                bool esEsquina = chkEsquina.Checked;
                bool cercaParque = chkParque.Checked;
                bool calleCerrada = chkCalleCerrada.Checked;
                int estadoLote = Convert.ToInt32(cmbEstado.SelectedValue);

                DataTable dt;

                if (_modoEdicion)
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_lote_actualizar",
                        Db.Parameter("@idLote", _idLote),
                        Db.Parameter("@idBloque", idBloque),
                        Db.Parameter("@numeroLote", numeroLote),
                        Db.Parameter("@areaV2", areaV2),
                        Db.Parameter("@esEsquina", esEsquina),
                        Db.Parameter("@cercaParque", cercaParque),
                        Db.Parameter("@calleCerrada", calleCerrada),
                        Db.Parameter("@precioBase", precioBase),
                        Db.Parameter("@recargoTotal", recargoTotal),
                        Db.Parameter("@precioFinal", precioFinal),
                        Db.Parameter("@estadoLote", estadoLote)
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_lote_insertar",
                        Db.Parameter("@idBloque", idBloque),
                        Db.Parameter("@numeroLote", numeroLote),
                        Db.Parameter("@areaV2", areaV2),
                        Db.Parameter("@esEsquina", esEsquina),
                        Db.Parameter("@cercaParque", cercaParque),
                        Db.Parameter("@calleCerrada", calleCerrada),
                        Db.Parameter("@precioBase", precioBase),
                        Db.Parameter("@recargoTotal", recargoTotal),
                        Db.Parameter("@precioFinal", precioFinal),
                        Db.Parameter("@estadoLote", estadoLote)
                    );
                }

                ValidarRespuestaError(dt);

                MessageBox.Show(
                    _modoEdicion ? "Lote actualizado correctamente." : "Lote registrado correctamente.",
                    "Operación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al guardar lote: " + ex.Message);
            }
        }

        private bool ValidarFormulario(out decimal areaV2, out decimal precioBase, out decimal recargoTotal, out decimal precioFinal)
        {
            areaV2 = 0;
            precioBase = 0;
            recargoTotal = 0;
            precioFinal = 0;

            if (cmbBloque.SelectedIndex < 0 || cmbBloque.SelectedValue == null)
            {
                MostrarWarning("Seleccione un bloque.");
                cmbBloque.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLote.Text))
            {
                MostrarWarning("Ingrese el número de lote.");
                txtLote.Focus();
                return false;
            }

            if (cmbEstado.SelectedIndex < 0 || cmbEstado.SelectedValue == null)
            {
                MostrarWarning("Seleccione un estado.");
                cmbEstado.Focus();
                return false;
            }

            if (!TryParseDecimal(txtArea.Text, out areaV2))
            {
                MostrarWarning("Ingrese un valor numérico válido para el área.");
                txtArea.Focus();
                return false;
            }

            if (areaV2 <= 0)
            {
                MostrarWarning("El área debe ser mayor que cero.");
                txtArea.Focus();
                return false;
            }

            if (!TryParseDecimal(txtPrecioBase.Text, out precioBase) ||
                !TryParseDecimal(txtRecargoTotal.Text, out recargoTotal) ||
                !TryParseDecimal(txtPrecioFinal.Text, out precioFinal))
            {
                MostrarWarning("No se pudieron calcular los precios del lote.");
                return false;
            }

            if (precioBase < 0 || recargoTotal < 0 || precioFinal < 0)
            {
                MostrarWarning("Los precios calculados no pueden ser negativos.");
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
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

        private static bool TryParseDecimal(string valor, out decimal resultado)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                resultado = 0;
                return false;
            }

            valor = valor.Trim();

            if (decimal.TryParse(valor, NumberStyles.Any, new CultureInfo("es-HN"), out resultado))
                return true;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out resultado))
                return true;

            resultado = 0;
            return false;
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