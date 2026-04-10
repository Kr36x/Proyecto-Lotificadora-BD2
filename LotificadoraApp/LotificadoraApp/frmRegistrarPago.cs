using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarPago : Form
    {
        private int? _idEtapaActual = null;

        public frmRegistrarPago()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtIdVenta.ReadOnly = true;
            txtCliente.ReadOnly = true;
            txtLote.ReadOnly = true;
            txtEtapa.ReadOnly = true;

            txtIDcuota.ReadOnly = true;
            txtMontoCuota.ReadOnly = true;
            txtSaldoPendiente.ReadOnly = true;
            txtEstadoCuota.ReadOnly = true;

            txtIdVenta.TabStop = false;
            txtCliente.TabStop = false;
            txtLote.TabStop = false;
            txtEtapa.TabStop = false;
            txtIDcuota.TabStop = false;
            txtMontoCuota.TabStop = false;
            txtSaldoPendiente.TabStop = false;
            txtEstadoCuota.TabStop = false;

            rbEfectivo.Checked = true;
            ActualizarControlesFormaPago();
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarPago_Load;

            cmbVentaCredito.SelectedIndexChanged += cmbVentaCredito_SelectedIndexChanged;
            cbCuotaPendiente.SelectedIndexChanged += cbCuotaPendiente_SelectedIndexChanged;

            rbEfectivo.CheckedChanged += rbFormaPago_CheckedChanged;
            rbDeposito.CheckedChanged += rbFormaPago_CheckedChanged;

            btnRegistrarPago.Click += btnRegistrarPago_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void frmRegistrarPago_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarVentasCredito();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar el formulario:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarVentasCredito()
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_listar_ventas_credito_activas", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbVentaCredito.DataSource = dt;
            cmbVentaCredito.DisplayMember = "descripcion";
            cmbVentaCredito.ValueMember = "idVenta";
            cmbVentaCredito.SelectedIndex = -1;
        }

        private void cmbVentaCredito_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbVentaCredito.SelectedIndex < 0)
            {
                LimpiarCamposVenta();
                LimpiarCamposCuota();
                cbCuotaPendiente.DataSource = null;
                cbCuentaBancaria.DataSource = null;
                _idEtapaActual = null;
                return;
            }

            try
            {
                int idVenta = Convert.ToInt32(cmbVentaCredito.SelectedValue);
                CargarDetalleVenta(idVenta);
                CargarCuotasPendientes(idVenta);

                if (_idEtapaActual.HasValue)
                    CargarCuentasBancariasPorEtapa(_idEtapaActual.Value);
            }
            catch
            {
            }
        }

        private void CargarDetalleVenta(int idVenta)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_obtener_detalle_venta_credito", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idVenta", idVenta);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
                throw new Exception("No se encontró la venta seleccionada.");

            txtIdVenta.Text = dr["idVenta"].ToString();
            txtCliente.Text = dr["cliente"].ToString();
            txtLote.Text = dr["lote"].ToString();
            txtEtapa.Text = dr["nombreEtapa"].ToString();

            _idEtapaActual = Convert.ToInt32(dr["idEtapa"]);
        }

        private void CargarCuotasPendientes(int idVenta)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_listar_cuotas_pendientes_por_venta", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idVenta", idVenta);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cbCuotaPendiente.DataSource = dt;
            cbCuotaPendiente.DisplayMember = "descripcion";
            cbCuotaPendiente.ValueMember = "idCuota";
            cbCuotaPendiente.SelectedIndex = -1;

            LimpiarCamposCuota();
        }

        private void cbCuotaPendiente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbCuotaPendiente.SelectedIndex < 0)
            {
                LimpiarCamposCuota();
                return;
            }

            try
            {
                DataRowView row = (DataRowView)cbCuotaPendiente.SelectedItem;

                txtIDcuota.Text = row["idCuota"].ToString();
                txtMontoCuota.Text = Convert.ToDecimal(row["montoCuota"]).ToString("N2");
                txtSaldoPendiente.Text = Convert.ToDecimal(row["saldoPendiente"]).ToString("N2");
                txtEstadoCuota.Text = row["estadoCuota"].ToString();
                txtMontoAPagar.Text = Convert.ToDecimal(row["saldoPendiente"]).ToString("N2");
            }
            catch
            {
            }
        }

        private void CargarCuentasBancariasPorEtapa(int idEtapa)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_listar_cuentas_bancarias_por_etapa", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idEtapa", idEtapa);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cbCuentaBancaria.DataSource = dt;
            cbCuentaBancaria.DisplayMember = "descripcion";
            cbCuentaBancaria.ValueMember = "idCuentaBancaria";
            cbCuentaBancaria.SelectedIndex = -1;
        }

        private void rbFormaPago_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarControlesFormaPago();
        }

        private void ActualizarControlesFormaPago()
        {
            bool esDeposito = rbDeposito.Checked;

            cbCuentaBancaria.Enabled = esDeposito;
            txtNoReferencia.Enabled = esDeposito;

            if (!esDeposito)
            {
                cbCuentaBancaria.SelectedIndex = -1;
                txtNoReferencia.Clear();
            }
        }

        private void btnRegistrarPago_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                int idVenta = int.Parse(txtIdVenta.Text.Trim());
                int idCuota = int.Parse(txtIDcuota.Text.Trim());
                string formaPago = rbEfectivo.Checked ? "efectivo" : "deposito";
                decimal montoTotal = ParseDecimalOrZero(txtMontoAPagar.Text);

                object idCuentaBancaria = DBNull.Value;
                object numeroReferencia = DBNull.Value;

                if (formaPago == "deposito")
                {
                    idCuentaBancaria = Convert.ToInt32(cbCuentaBancaria.SelectedValue);
                    numeroReferencia = string.IsNullOrWhiteSpace(txtNoReferencia.Text)
                        ? DBNull.Value
                        : txtNoReferencia.Text.Trim();
                }

                object observacion = string.IsNullOrWhiteSpace(textBox1.Text)
                    ? DBNull.Value
                    : textBox1.Text.Trim();

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_registrar_pago", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cmd.Parameters.AddWithValue("@idCuota", idCuota);
                cmd.Parameters.AddWithValue("@formaPago", formaPago);
                cmd.Parameters.AddWithValue("@montoTotal", montoTotal);
                cmd.Parameters.AddWithValue("@idCuentaBancaria", idCuentaBancaria);
                cmd.Parameters.AddWithValue("@numeroReferencia", numeroReferencia);
                cmd.Parameters.AddWithValue("@observacion", observacion);

                DataTable dt = new DataTable();
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    string mensaje =
                        "Pago registrado correctamente.\n\n" +
                        $"Id Pago: {row["idPago"]}\n" +
                        $"Id Factura: {row["idFactura"]}\n" +
                        $"Monto Pagado: {Convert.ToDecimal(row["montoPagado"]):N2}\n" +
                        $"Monto Capital: {Convert.ToDecimal(row["montoCapital"]):N2}\n" +
                        $"Monto Interés: {Convert.ToDecimal(row["montoInteres"]):N2}\n" +
                        $"Número Factura: {row["numeroFactura"]}";

                    MessageBox.Show(
                        mensaje,
                        "Pago registrado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "El pago se ejecutó, pero no se devolvió resumen.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                int idVentaActual = int.Parse(txtIdVenta.Text);
                CargarCuotasPendientes(idVentaActual);

                txtMontoAPagar.Clear();
                txtNoReferencia.Clear();
                textBox1.Clear();

                if (rbDeposito.Checked)
                    cbCuentaBancaria.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar pago:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ValidarFormulario()
        {
            if (cmbVentaCredito.SelectedIndex < 0)
                throw new Exception("Seleccione una venta a crédito.");

            if (cbCuotaPendiente.SelectedIndex < 0)
                throw new Exception("Seleccione una cuota pendiente.");

            if (!rbEfectivo.Checked && !rbDeposito.Checked)
                throw new Exception("Seleccione la forma de pago.");

            if (string.IsNullOrWhiteSpace(txtMontoAPagar.Text))
                throw new Exception("Ingrese el monto a pagar.");

            decimal montoAPagar = ParseDecimalOrZero(txtMontoAPagar.Text);
            if (montoAPagar <= 0)
                throw new Exception("El monto a pagar debe ser mayor que cero.");

            decimal saldoPendiente = ParseDecimalOrZero(txtSaldoPendiente.Text);
            if (montoAPagar > saldoPendiente)
                throw new Exception("El monto a pagar no puede ser mayor al saldo pendiente.");

            if (rbDeposito.Checked)
            {
                if (cbCuentaBancaria.SelectedIndex < 0)
                    throw new Exception("Seleccione una cuenta bancaria.");

                if (string.IsNullOrWhiteSpace(txtNoReferencia.Text))
                    throw new Exception("Ingrese el número de referencia.");
            }
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            cmbVentaCredito.SelectedIndex = -1;
            cbCuotaPendiente.DataSource = null;
            cbCuentaBancaria.DataSource = null;

            rbEfectivo.Checked = true;
            rbDeposito.Checked = false;

            txtMontoAPagar.Clear();
            txtNoReferencia.Clear();
            textBox1.Clear();

            LimpiarCamposVenta();
            LimpiarCamposCuota();

            _idEtapaActual = null;
            ActualizarControlesFormaPago();
        }

        private void LimpiarCamposVenta()
        {
            txtIdVenta.Clear();
            txtCliente.Clear();
            txtLote.Clear();
            txtEtapa.Clear();
        }

        private void LimpiarCamposCuota()
        {
            txtIDcuota.Clear();
            txtMontoCuota.Clear();
            txtSaldoPendiente.Clear();
            txtEstadoCuota.Clear();
            txtMontoAPagar.Clear();
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

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}