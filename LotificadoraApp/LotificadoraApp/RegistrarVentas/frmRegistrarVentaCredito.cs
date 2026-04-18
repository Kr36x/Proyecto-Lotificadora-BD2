using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using LotificadoraApp.Aval;
using LotificadoraApp.Beneficiario;

namespace LotificadoraApp
{
    public partial class frmRegistrarVentaCredito : Form
    {
        private decimal _tasaInteresActual = 0m;
        private int _idLoteSeleccionado = 0;
        private decimal _precioLoteActual = 0m;

        public frmRegistrarVentaCredito()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtInteres.ReadOnly = true;
            txtTotalVenta.ReadOnly = true;
            txtMontoFinanciado.ReadOnly = true;
            txtCuotaEstimada.ReadOnly = true;

            txtInteres.TabStop = false;
            txtTotalVenta.TabStop = false;
            txtMontoFinanciado.TabStop = false;
            txtCuotaEstimada.TabStop = false;

            chkFinanciarTotal.Checked = false;
            dtpFechaVenta.Value = DateTime.Today;
            dtpFechaInicioPago.Value = DateTime.Today.AddMonths(1);

            LimpiarCamposDetalle();
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarVentaCredito_Load;

            cmbLotesDisponibles.SelectedIndexChanged += cmbLotesDisponibles_SelectedIndexChanged;
            chkFinanciarTotal.CheckedChanged += chkFinanciarTotal_CheckedChanged;

            btnCalcular.Click += btnCalcular_Click;
            btnRegistrarVenta.Click += btnRegistrarVenta_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            guna2Button2.Click += guna2Button2_Click;
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
            DataTable dt = Db.ExecuteStoredProcedure("dbo.sp_listar_lotes_disponibles_combo");

            cmbLotesDisponibles.DataSource = dt;
            cmbLotesDisponibles.DisplayMember = "descripcion";
            cmbLotesDisponibles.ValueMember = "idLote";
            cmbLotesDisponibles.SelectedIndex = -1;
        }

        private void CargarClientes()
        {
            DataTable dt = Db.ExecuteStoredProcedure("dbo.sp_cliente_listar_activo");

            cmbCliente.DataSource = dt;
            cmbCliente.DisplayMember = "nombreCompleto";
            cmbCliente.ValueMember = "idCliente";
            cmbCliente.SelectedIndex = -1;
        }

        private void CargarAvales()
        {
            DataTable dt = Db.ExecuteStoredProcedure("dbo.sp_aval_listar_comboBox");

            cmbAval.DataSource = dt;
            cmbAval.DisplayMember = "nombreCompleto";
            cmbAval.ValueMember = "idAval";
            cmbAval.SelectedIndex = -1;
        }

        private void CargarBeneficiarios()
        {
            DataTable dt = Db.ExecuteStoredProcedure("dbo.sp_beneficiario_listar_comboBox");

            cmbBenificiario.DataSource = dt;
            cmbBenificiario.DisplayMember = "nombreCompleto";
            cmbBenificiario.ValueMember = "idBeneficiario";
            cmbBenificiario.SelectedIndex = -1;
        }

        private void cmbLotesDisponibles_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbLotesDisponibles.SelectedIndex < 0 || cmbLotesDisponibles.SelectedValue == null)
            {
                LimpiarCamposDetalle();
                return;
            }

            try
            {
                _idLoteSeleccionado = Convert.ToInt32(cmbLotesDisponibles.SelectedValue);
                CargarDetalleLote(_idLoteSeleccionado);
                CalcularResumen();
            }
            catch
            {
            }
        }

        private void CargarDetalleLote(int idLote)
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "dbo.sp_obtener_detalle_lote_disponible",
                new SqlParameter("@idLote", idLote)
            );

            if (dt.Rows.Count == 0)
                throw new Exception("No se encontró información del lote seleccionado.");

            DataRow row = dt.Rows[0];

            _idLoteSeleccionado = Convert.ToInt32(row["idLote"]);
            _precioLoteActual = Convert.ToDecimal(row["precioFinalCalculado"]);
            _tasaInteresActual = Convert.ToDecimal(row["tasaInteresAnual"]);

            lblIDLote.Text = "ID Lote: " + row["idLote"].ToString();
            lblProyecto.Text = "Proyecto: " + row["nombreProyecto"].ToString();
            lblEtapa.Text = "Etapa: " + row["nombreEtapa"].ToString();
            lblBloque.Text = "Bloque: " + row["nombreBloque"].ToString();
            lblNumeroLote.Text = "Numero Lote: " + row["numeroLote"].ToString();
            lblPrecioLote.Text = "Precio Lote: " + _precioLoteActual.ToString("N2");

            txtInteres.Text = _tasaInteresActual.ToString("N2");
        }

        private void chkFinanciarTotal_CheckedChanged(object? sender, EventArgs e)
        {
            txtPrima.Enabled = !chkFinanciarTotal.Checked;

            if (chkFinanciarTotal.Checked)
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
            decimal descuento = ParseDecimalOrZero(txtDescuento.Text);
            decimal recargo = ParseDecimalOrZero(txtRecargo.Text);
            decimal prima = ParseDecimalOrZero(txtPrima.Text);
            int plazoAnios = ParseIntOrZero(txtPlazoAños.Text);

            decimal totalVenta = _precioLoteActual - descuento + recargo;
            if (totalVenta < 0)
                totalVenta = 0;

            decimal montoFinanciado;

            if (chkFinanciarTotal.Checked)
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

        private void btnRegistrarVenta_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();
                CalcularResumen();

                int idCliente = Convert.ToInt32(cmbCliente.SelectedValue);
                int idAval = Convert.ToInt32(cmbAval.SelectedValue);
                int idBeneficiario = Convert.ToInt32(cmbBenificiario.SelectedValue);

                DateTime fechaVenta = dtpFechaVenta.Value.Date;
                DateTime fechaInicioPago = dtpFechaInicioPago.Value.Date;

                decimal descuento = ParseDecimalOrZero(txtDescuento.Text);
                decimal recargo = ParseDecimalOrZero(txtRecargo.Text);
                decimal prima = ParseDecimalOrZero(txtPrima.Text);
                int financiaTotal = chkFinanciarTotal.Checked ? 1 : 0;
                int plazoAnios = int.Parse(txtPlazoAños.Text.Trim());

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_registrar_venta_credito", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idLote", _idLoteSeleccionado);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.Parameters.AddWithValue("@idAval", idAval);
                cmd.Parameters.AddWithValue("@idBeneficiario", idBeneficiario);
                cmd.Parameters.AddWithValue("@fechaVenta", fechaVenta);
                cmd.Parameters.AddWithValue("@precioLote", _precioLoteActual);
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
                        GenerarCuotas(idVentaCreditoGenerada, dtpFechaInicioPago.Value.Date);
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
                    mensaje = "No es posible registrar la venta porque la cuota estimada supera la capacidad de pago permitida del cliente.";
                else if (mensaje.ToLower().Contains("datos laborales"))
                    mensaje = "No es posible registrar la venta porque el cliente no tiene datos laborales registrados.";

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
            if (cmbLotesDisponibles.SelectedIndex < 0 || _idLoteSeleccionado <= 0)
                throw new Exception("Seleccione un lote disponible.");

            if (cmbCliente.SelectedIndex < 0 || cmbCliente.SelectedValue == null)
                throw new Exception("Seleccione un cliente.");

            if (cmbAval.SelectedIndex < 0 || cmbAval.SelectedValue == null)
                throw new Exception("Seleccione un aval.");

            if (cmbBenificiario.SelectedIndex < 0 || cmbBenificiario.SelectedValue == null)
                throw new Exception("Seleccione un beneficiario.");

            if (string.IsNullOrWhiteSpace(txtPlazoAños.Text))
                throw new Exception("Ingrese el plazo en años.");

            if (!int.TryParse(txtPlazoAños.Text.Trim(), out int plazo) || plazo <= 0)
                throw new Exception("El plazo en años debe ser un entero mayor que cero.");

            if (!chkFinanciarTotal.Checked)
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

        private void guna2Button2_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void LimpiarFormulario()
        {
            cmbLotesDisponibles.SelectedIndex = -1;
            cmbCliente.SelectedIndex = -1;
            cmbAval.SelectedIndex = -1;
            cmbBenificiario.SelectedIndex = -1;

            dtpFechaVenta.Value = DateTime.Today;
            dtpFechaInicioPago.Value = DateTime.Today.AddMonths(1);

            txtDescuento.Clear();
            txtRecargo.Clear();
            txtPrima.Clear();
            txtPlazoAños.Clear();

            chkFinanciarTotal.Checked = false;

            LimpiarCamposDetalle();
        }

        private void LimpiarCamposDetalle()
        {
            _idLoteSeleccionado = 0;
            _precioLoteActual = 0m;
            _tasaInteresActual = 0m;

            lblIDLote.Text = "ID Lote:";
            lblProyecto.Text = "Proyecto:";
            lblEtapa.Text = "Etapa:";
            lblBloque.Text = "Bloque:";
            lblNumeroLote.Text = "Numero Lote:";
            lblPrecioLote.Text = "Precio Lote:";

            txtInteres.Clear();
            txtTotalVenta.Clear();
            txtMontoFinanciado.Clear();
            txtCuotaEstimada.Clear();
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
        }
    }
}