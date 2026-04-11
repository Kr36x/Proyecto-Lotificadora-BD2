using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace LotificadoraApp
{
    public partial class frmRegistrarLote : Form
    {
        public frmRegistrarLote()
        {
            InitializeComponent();
            ConectarEventosCalculo();
        }

        private void frmRegistrarLote_Load(object sender, EventArgs e)
        {
            ConfigurarFormulario();
            CargarBloques();
            CargarEstados();
            CalcularPrecios();
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
                DataTable dtBloques = Db.ExecuteStoredProcedure("sp_bloque_listar");

                cmbBloque.DataSource = dtBloques;
                cmbBloque.DisplayMember = "nombreBloque";
                cmbBloque.ValueMember = "idBloque";
                cmbBloque.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar bloques:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarEstados()
        {
            try
            {
                const string sql = @"
            SELECT id, nombre
            FROM dbo.Estado
            WHERE id IN (7, 8, 9)
            ORDER BY id;";

                DataTable dtEstados = Db.ExecuteQuery(sql);

                cmbEstado.DataSource = dtEstados;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = 7;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar estados:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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

        private void btnRegistrar_Click(object sender, EventArgs e)
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

                DataTable dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_lote_insertar",
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

                if (dt.Rows.Count == 0)
                    throw new Exception("No se devolvió resultado al registrar el lote.");

                if (dt.Columns.Contains("MensajeError") && dt.Rows[0]["MensajeError"] != DBNull.Value)
                    throw new Exception(dt.Rows[0]["MensajeError"].ToString());

                int idLoteGenerado = Convert.ToInt32(dt.Rows[0]["idLoteGenerado"]);

                MessageBox.Show(
                    $"Lote registrado correctamente. ID generado: {idLoteGenerado}",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar lote:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool ValidarFormulario(out decimal areaV2, out decimal precioBase, out decimal recargoTotal, out decimal precioFinal)
        {
            areaV2 = 0;
            precioBase = 0;
            recargoTotal = 0;
            precioFinal = 0;

            if (cmbBloque.SelectedIndex < 0)
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

            if (cmbEstado.SelectedIndex < 0)
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

        private bool TryParseDecimal(string valor, out decimal resultado)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                resultado = 0;
                return false;
            }

            valor = valor.Trim().Replace(",", "");

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out resultado))
                return true;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.CurrentCulture, out resultado))
                return true;

            resultado = 0;
            return false;
        }

        private void MostrarWarning(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void LimpiarFormulario()
        {
            txtLote.Clear();
            txtArea.Clear();

            chkEsquina.Checked = false;
            chkParque.Checked = false;
            chkCalleCerrada.Checked = false;

            txtPrecioBase.Clear();
            txtRecargoTotal.Clear();
            txtPrecioFinal.Clear();

            cmbBloque.SelectedIndex = -1;
            cmbEstado.SelectedValue = 7;
        }
    }
}