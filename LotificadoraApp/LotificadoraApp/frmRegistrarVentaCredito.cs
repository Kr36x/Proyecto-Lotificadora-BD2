using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarVentaCredito : Form
    {
        private decimal _tasaInteresActual = 0m;

        public frmRegistrarVentaCredito()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtIdLote.ReadOnly = true;
            txtProyecto.ReadOnly = true;
            txtEtapa.ReadOnly = true;
            txtBloque.ReadOnly = true;
            txtNumeroLote.ReadOnly = true;
            txtPrecioLote.ReadOnly = true;
            txtTasaInteres.ReadOnly = true;
            txtTotalVenta.ReadOnly = true;
            txtMontoFinanciado.ReadOnly = true;
            txtCuotaEstimada.ReadOnly = true;

            txtIdLote.TabStop = false;
            txtProyecto.TabStop = false;
            txtEtapa.TabStop = false;
            txtBloque.TabStop = false;
            txtNumeroLote.TabStop = false;
            txtPrecioLote.TabStop = false;
            txtTasaInteres.TabStop = false;
            txtTotalVenta.TabStop = false;
            txtMontoFinanciado.TabStop = false;
            txtCuotaEstimada.TabStop = false;

            cbFinanciarTotal.Checked = false;
            dtpFechaVenta.Value = DateTime.Today;
            dtpInicioPago.Value = DateTime.Today.AddMonths(1);
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarVentaCredito_Load;
            cmbLoteDisponible.SelectedIndexChanged += cmbLoteDisponible_SelectedIndexChanged;
            cbFinanciarTotal.CheckedChanged += cbFinanciarTotal_CheckedChanged;

            btnCalcular.Click += btnCalcular_Click;
            btnRegistrarVenta.Click += btnRegistrarVenta_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCerrar.Click += btnCerrar_Click;

            btnNuevoBeneficiario.Click += btnNuevoBeneficiario_Click;
            btnNuevoAval.Click += btnNuevoAval_Click;
        }

        private void frmRegistrarVentaCredito_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarLotesDisponibles();
                CargarClientes();
                CargarAvales();
                CargarBeneficiarios();
                LimpiarCamposDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar datos iniciales:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarLotesDisponibles()
        {
            const string sql = @"exec dbo.sp_listar_lotes_disponibles;";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbLoteDisponible.DataSource = dt;
            cmbLoteDisponible.DisplayMember = "numeroLote";
            cmbLoteDisponible.ValueMember = "idLote";
            cmbLoteDisponible.SelectedIndex = -1;
        }

        private void CargarClientes()
        {
            const string sql = @"exec dbo.sp_cliente_listar_activo;";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cbClientes.DataSource = dt;
            cbClientes.DisplayMember = "nombreCompleto";
            cbClientes.ValueMember = "idCliente";
            cbClientes.SelectedIndex = -1;
        }

        private void CargarAvales()
        {
            const string sql = @"exec dbo.sp_aval_listar_comboBox;";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cbAval.DataSource = dt;
            cbAval.DisplayMember = "nombreCompleto";
            cbAval.ValueMember = "idAval";
            cbAval.SelectedIndex = -1;
        }

        private void CargarBeneficiarios()
        {
            const string sql = @"exec dbo.sp_beneficiario_listar_comboBox;";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cbBeneficiario.DataSource = dt;
            cbBeneficiario.DisplayMember = "nombreCompleto";
            cbBeneficiario.ValueMember = "idBeneficiario";
            cbBeneficiario.SelectedIndex = -1;
        }

        private void cmbLoteDisponible_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbLoteDisponible.SelectedIndex < 0)
            {
                LimpiarCamposDetalle();
                return;
            }

            try
            {
                int idLote = Convert.ToInt32(cmbLoteDisponible.SelectedValue);
                CargarDetalleLote(idLote);
                CalcularResumen();
            }
            catch
            {
                // Evita errores de binding al iniciar
            }
        }

        private void CargarDetalleLote(int idLote)
        {
            const string sql = @"exec dbo.sp_obtener_detalle_lote_disponible @idLote = @idLote;";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@idLote", idLote);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
                throw new Exception("No se encontró información del lote seleccionado.");

            txtIdLote.Text = dr["idLote"].ToString();
            txtProyecto.Text = dr["nombreProyecto"].ToString();
            txtEtapa.Text = dr["nombreEtapa"].ToString();
            txtBloque.Text = dr["nombreBloque"].ToString();
            txtNumeroLote.Text = dr["numeroLote"].ToString();

            decimal precio = Convert.ToDecimal(dr["precioFinalCalculado"]);
            txtPrecioLote.Text = precio.ToString("N2");

            _tasaInteresActual = Convert.ToDecimal(dr["tasaInteresAnual"]);
            txtTasaInteres.Text = _tasaInteresActual.ToString("N2");
        }

        private void cbFinanciarTotal_CheckedChanged(object? sender, EventArgs e)
        {
            txtPrima.Enabled = !cbFinanciarTotal.Checked;

            if (cbFinanciarTotal.Checked)
                txtPrima.Text = "0";

            CalcularResumen();
        }

        private void btnCalcular_Click(object? sender, EventArgs e)
        {
            try
            {
                CalcularResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al calcular:\n" + ex.Message,
                    "Cálculo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void CalcularResumen()
        {
            decimal precioLote = ParseDecimalOrZero(txtPrecioLote.Text);
            decimal descuento = ParseDecimalOrZero(txtDescuento.Text);
            decimal recargo = ParseDecimalOrZero(txtRecargo.Text);
            decimal prima = ParseDecimalOrZero(txtPrima.Text);
            int plazoAnios = ParseIntOrZero(txtPlazoAños.Text);

            decimal totalVenta = precioLote - descuento + recargo;
            if (totalVenta < 0)
                totalVenta = 0;

            decimal montoFinanciado;

            if (cbFinanciarTotal.Checked)
            {
                prima = 0;
                montoFinanciado = totalVenta;
                txtPrima.Text = "0";
            }
            else
            {
                montoFinanciado = totalVenta - prima;
                if (montoFinanciado < 0)
                    montoFinanciado = 0;
            }

            decimal cuotaEstimada = 0;

            if (plazoAnios > 0 && montoFinanciado > 0)
            {
                int numeroCuotas = plazoAnios * 12;
                decimal tasaMensual = _tasaInteresActual / 12m / 100m;

                if (tasaMensual == 0)
                {
                    cuotaEstimada = montoFinanciado / numeroCuotas;
                }
                else
                {
                    double tm = (double)tasaMensual;
                    double mf = (double)montoFinanciado;
                    double pot = Math.Pow(1 + tm, numeroCuotas);

                    double cuota = mf * ((tm * pot) / (pot - 1));
                    cuotaEstimada = (decimal)cuota;
                }
            }

            txtTotalVenta.Text = totalVenta.ToString("N2");
            txtMontoFinanciado.Text = montoFinanciado.ToString("N2");
            txtCuotaEstimada.Text = cuotaEstimada.ToString("N2");
        }
        private void btnNuevoAval_Click(object? sender, EventArgs e)
        {
            using frmRegistrarAval frm = new frmRegistrarAval();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarAvales();

                if (frm.IdAvalGenerado.HasValue)
                    cbAval.SelectedValue = frm.IdAvalGenerado.Value;
            }
        }
        private void btnNuevoBeneficiario_Click(object? sender, EventArgs e)
        {
            using frmRegistrarBeneficiario frm = new frmRegistrarBeneficiario();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarBeneficiarios();

                if (frm.IdBeneficiarioGenerado.HasValue)
                    cbBeneficiario.SelectedValue = frm.IdBeneficiarioGenerado.Value;
            }
        }
        private void btnRegistrarVenta_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();
                CalcularResumen();

                int idLote = Convert.ToInt32(txtIdLote.Text);
                int idCliente = Convert.ToInt32(cbClientes.SelectedValue);
                int idAval = Convert.ToInt32(cbAval.SelectedValue);
                int idBeneficiario = Convert.ToInt32(cbBeneficiario.SelectedValue);

                DateTime fechaVenta = dtpFechaVenta.Value.Date;
                DateTime fechaInicioPago = dtpInicioPago.Value.Date;

                decimal precioLote = ParseDecimalOrZero(txtPrecioLote.Text);
                decimal descuento = ParseDecimalOrZero(txtDescuento.Text);
                decimal recargo = ParseDecimalOrZero(txtRecargo.Text);
                decimal prima = ParseDecimalOrZero(txtPrima.Text);
                int financiaTotal = cbFinanciarTotal.Checked ? 1 : 0;
                int plazoAnios = int.Parse(txtPlazoAños.Text.Trim());

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_registrar_venta_credito", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idLote", idLote);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.Parameters.AddWithValue("@idAval", idAval);
                cmd.Parameters.AddWithValue("@idBeneficiario", idBeneficiario);
                cmd.Parameters.AddWithValue("@fechaVenta", fechaVenta);
                cmd.Parameters.AddWithValue("@precioLote", precioLote);
                cmd.Parameters.AddWithValue("@descuento", descuento);
                cmd.Parameters.AddWithValue("@recargo", recargo);
                cmd.Parameters.AddWithValue("@prima", prima);
                cmd.Parameters.AddWithValue("@financiaTotal", financiaTotal);
                cmd.Parameters.AddWithValue("@plazoAnios", plazoAnios);
                cmd.Parameters.AddWithValue("@fechaInicioPago", fechaInicioPago);

                DataTable dt = new DataTable();
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    int idVentaCreditoGenerada = Convert.ToInt32(row["idVentaCredito"]);

                    string mensaje =
                        "Venta registrada correctamente.\n\n" +
                        $"Id Venta: {row["idVenta"]}\n" +
                        $"Id Venta Crédito: {row["idVentaCredito"]}\n" +
                        $"Total Venta: {Convert.ToDecimal(row["totalVenta"]):N2}\n" +
                        $"Monto Financiado: {Convert.ToDecimal(row["montoFinanciado"]):N2}\n" +
                        $"Tasa Interés: {Convert.ToDecimal(row["tasaInteresAnual"]):N2}\n" +
                        $"Número de Cuotas: {row["numeroCuotas"]}\n" +
                        $"Cuota Mensual Estimada: {Convert.ToDecimal(row["cuotaMensualEstimada"]):N2}\n" +
                        $"Total Interés: {Convert.ToDecimal(row["totalInteres"]):N2}\n" +
                        $"Total Plan: {Convert.ToDecimal(row["totalPlan"]):N2}\n" +
                        $"Ingreso Mensual Cliente: {Convert.ToDecimal(row["ingresoMensual"]):N2}\n" +
                        $"Capacidad Máxima (30%): {Convert.ToDecimal(row["capacidadMaxima"]):N2}";

                    MessageBox.Show(
                        mensaje,
                        "Venta registrada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    DialogResult respuesta = MessageBox.Show(
                        "¿Desea generar las cuotas ahora?",
                        "Generar cuotas",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (respuesta == DialogResult.Yes)
                    {
                        GenerarCuotas(idVentaCreditoGenerada, dtpInicioPago.Value.Date);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "La venta se ejecutó, pero no se devolvió resumen.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                CargarLotesDisponibles();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

                if (mensaje.ToLower().Contains("30% del ingreso mensual"))
                {
                    mensaje = "No es posible registrar la venta porque la cuota estimada supera la capacidad de pago permitida del cliente.";
                }
                else if (mensaje.ToLower().Contains("datos laborales"))
                {
                    mensaje = "No es posible registrar la venta porque el cliente no tiene datos laborales registrados.";
                }

                MessageBox.Show(
                    "Error al registrar la venta:\n" + mensaje,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void GenerarCuotas(int idVentaCredito, DateTime fechaPrimerVencimiento)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_generar_cuotas_plan_cursor", cn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idVentaCredito", idVentaCredito);
            cmd.Parameters.AddWithValue("@fechaPrimerVencimiento", fechaPrimerVencimiento);

            cn.Open();
            cmd.ExecuteNonQuery();

            MessageBox.Show(
                "Las cuotas fueron generadas correctamente.",
                "Cuotas generadas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        private void ValidarFormulario()
        {
            if (cmbLoteDisponible.SelectedIndex < 0)
                throw new Exception("Seleccione un lote disponible.");

            if (cbClientes.SelectedIndex < 0)
                throw new Exception("Seleccione un cliente.");

            if (cbAval.SelectedIndex < 0)
                throw new Exception("Seleccione un aval.");

            if (cbBeneficiario.SelectedIndex < 0)
                throw new Exception("Seleccione un beneficiario.");

            if (string.IsNullOrWhiteSpace(txtPlazoAños.Text))
                throw new Exception("Ingrese el plazo en años.");

            if (!int.TryParse(txtPlazoAños.Text.Trim(), out int plazo) || plazo <= 0)
                throw new Exception("El plazo en años debe ser un entero mayor que cero.");

            if (!cbFinanciarTotal.Checked)
            {
                decimal prima = ParseDecimalOrZero(txtPrima.Text);
                if (prima <= 0)
                    throw new Exception("Si no financia el total, la prima debe ser mayor que cero.");
            }
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void LimpiarFormulario()
        {
            cmbLoteDisponible.SelectedIndex = -1;
            cbClientes.SelectedIndex = -1;
            cbAval.SelectedIndex = -1;
            cbBeneficiario.SelectedIndex = -1;

            dtpFechaVenta.Value = DateTime.Today;
            dtpInicioPago.Value = DateTime.Today.AddMonths(1);

            txtDescuento.Clear();
            txtRecargo.Clear();
            txtPrima.Clear();
            txtPlazoAños.Clear();

            cbFinanciarTotal.Checked = false;

            LimpiarCamposDetalle();
        }

        private void LimpiarCamposDetalle()
        {
            txtIdLote.Clear();
            txtProyecto.Clear();
            txtEtapa.Clear();
            txtBloque.Clear();
            txtNumeroLote.Clear();
            txtPrecioLote.Clear();
            txtTasaInteres.Clear();
            txtTotalVenta.Clear();
            txtMontoFinanciado.Clear();
            txtCuotaEstimada.Clear();

            _tasaInteresActual = 0m;
        }

        private decimal ParseDecimalOrZero(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return 0m;

            valor = valor.Replace(",", "");

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal r))
                return r;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.CurrentCulture, out r))
                return r;

            throw new Exception($"El valor '{valor}' no es un número válido.");
        }

        private int ParseIntOrZero(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return 0;

            if (int.TryParse(valor.Trim(), out int r))
                return r;

            throw new Exception($"El valor '{valor}' no es un entero válido.");
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            // vacío a propósito
        }
    }
}