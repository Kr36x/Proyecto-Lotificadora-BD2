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
        private int? _idVentaActual = null;
        private int? _idCuotaActual = null;

        public frmRegistrarPago()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            LimpiarCamposVenta();
            LimpiarCamposCuota();

            CargarFormaPago();
            ActualizarControlesFormaPago();
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarPago_Load;

            cmbLotesDisponibles.SelectedIndexChanged += cmbLotesDisponibles_SelectedIndexChanged;
            cbmCuotaPendiente.SelectedIndexChanged += cbmCuotaPendiente_SelectedIndexChanged;
            cbmFormaPago.SelectedIndexChanged += cbmFormaPago_SelectedIndexChanged;

            btnRegistrarPago.Click += btnRegistrarPago_Click;
            btnLimpiar.Click += btnLimpiar_Click;
        }

        private void frmRegistrarPago_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarVentasCredito();
                CargarEmpleados();
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

            cmbLotesDisponibles.DataSource = dt;
            cmbLotesDisponibles.DisplayMember = "descripcion";
            cmbLotesDisponibles.ValueMember = "idVenta";
            cmbLotesDisponibles.SelectedIndex = -1;
        }

        private void CargarFormaPago()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("valor");
            dt.Columns.Add("texto");

            dt.Rows.Add("efectivo", "efectivo");
            dt.Rows.Add("deposito", "deposito");

            cbmFormaPago.DataSource = dt;
            cbmFormaPago.DisplayMember = "texto";
            cbmFormaPago.ValueMember = "valor";
            cbmFormaPago.SelectedIndex = -1;
        }

        private void CargarEmpleados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_empleado_listar");

                DataView view = dt.DefaultView;
                view.RowFilter = "estadoId = 1";

                DataTable dtFiltrado = view.ToTable();
                dtFiltrado.Columns.Add(
                    "descripcion",
                    typeof(string),
                    "Convert(id, 'System.String') + ' - ' + nombres + ' ' + apellidos"
                );

                cmbEmpleado.DataSource = dtFiltrado;
                cmbEmpleado.DisplayMember = "descripcion";
                cmbEmpleado.ValueMember = "id";
                cmbEmpleado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar empleados:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void cmbLotesDisponibles_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbLotesDisponibles.SelectedIndex < 0 || cmbLotesDisponibles.SelectedValue == null)
            {
                LimpiarCamposVenta();
                LimpiarCamposCuota();
                cbmCuotaPendiente.DataSource = null;
                cmbCuentaBancaria.DataSource = null;
                _idEtapaActual = null;
                _idVentaActual = null;
                return;
            }

            try
            {
                int idVenta = Convert.ToInt32(cmbLotesDisponibles.SelectedValue);
                _idVentaActual = idVenta;

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

            lblIDVenta.Text = "ID venta: " + dr["idVenta"].ToString();
            lblCliente.Text = "Cliente: " + dr["cliente"].ToString();
            lblLote.Text = "Lote: " + dr["lote"].ToString();
            lblEtapa.Text = "Etapa: " + dr["nombreEtapa"].ToString();

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

            cbmCuotaPendiente.DataSource = dt;
            cbmCuotaPendiente.DisplayMember = "descripcion";
            cbmCuotaPendiente.ValueMember = "idCuota";
            cbmCuotaPendiente.SelectedIndex = -1;

            LimpiarCamposCuota();
        }

        private void cbmCuotaPendiente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbmCuotaPendiente.SelectedIndex < 0 || cbmCuotaPendiente.SelectedItem == null)
            {
                LimpiarCamposCuota();
                _idCuotaActual = null;
                return;
            }

            try
            {
                DataRowView row = (DataRowView)cbmCuotaPendiente.SelectedItem;

                _idCuotaActual = Convert.ToInt32(row["idCuota"]);

                lblIDCuota.Text = "ID cuota: " + row["idCuota"].ToString();
                lblMontoCuota.Text = "Monto cuota: " + Convert.ToDecimal(row["montoCuota"]).ToString("N2");
                lblSaldoPendiente.Text = "Saldo pendiente: " + Convert.ToDecimal(row["saldoPendiente"]).ToString("N2");
                lblEstadoCuota.Text = "Estado cuota: " + row["estadoCuota"].ToString();

                txtMontoPagar.Text = Convert.ToDecimal(row["saldoPendiente"]).ToString("N2");
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

            cmbCuentaBancaria.DataSource = dt;
            cmbCuentaBancaria.DisplayMember = "descripcion";
            cmbCuentaBancaria.ValueMember = "idCuentaBancaria";
            cmbCuentaBancaria.SelectedIndex = -1;
        }

        private void cbmFormaPago_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarControlesFormaPago();
        }

        private void ActualizarControlesFormaPago()
        {
            string formaPago = cbmFormaPago.SelectedValue?.ToString() ?? string.Empty;

            bool esEfectivo = formaPago == "efectivo";
            bool esDeposito = formaPago == "deposito";

            cmbEmpleado.Enabled = esEfectivo;

            cmbCuentaBancaria.Enabled = esDeposito;
            txtReferencia.Enabled = esDeposito;

            if (!esEfectivo)
                cmbEmpleado.SelectedIndex = -1;

            if (!esDeposito)
            {
                cmbCuentaBancaria.SelectedIndex = -1;
                txtReferencia.Clear();
            }
        }

        private void btnRegistrarPago_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                int idVenta = _idVentaActual!.Value;
                int idCuota = _idCuotaActual!.Value;
                string formaPago = cbmFormaPago.SelectedValue!.ToString()!;
                decimal montoTotal = ParseDecimalOrZero(txtMontoPagar.Text);

                object idCuentaBancaria = DBNull.Value;
                object numeroReferencia = DBNull.Value;
                object idEmpleado = DBNull.Value;

                if (formaPago == "deposito")
                {
                    idCuentaBancaria = Convert.ToInt32(cmbCuentaBancaria.SelectedValue);
                    numeroReferencia = string.IsNullOrWhiteSpace(txtReferencia.Text)
                        ? DBNull.Value
                        : txtReferencia.Text.Trim();
                }

                if (formaPago == "efectivo")
                {
                    idEmpleado = Convert.ToInt32(cmbEmpleado.SelectedValue);
                }

                object observacion = string.IsNullOrWhiteSpace(txtObservacion.Text)
                    ? DBNull.Value
                    : txtObservacion.Text.Trim();

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
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                DataTable dt = new DataTable();
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Columns.Contains("MensajeError"))
                {
                    string mensajeError = dt.Rows[0]["MensajeError"]?.ToString() ?? "Error al registrar el pago.";
                    throw new Exception(mensajeError);
                }

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

                int idVentaActual = _idVentaActual.Value;
                CargarCuotasPendientes(idVentaActual);

                txtMontoPagar.Clear();
                txtReferencia.Clear();
                txtObservacion.Clear();

                if (cbmFormaPago.SelectedValue?.ToString() == "deposito")
                    cmbCuentaBancaria.SelectedIndex = -1;

                if (cbmFormaPago.SelectedValue?.ToString() == "efectivo")
                    cmbEmpleado.SelectedIndex = -1;
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
            if (cmbLotesDisponibles.SelectedIndex < 0 || _idVentaActual == null)
                throw new Exception("Seleccione una venta a crédito.");

            if (cbmCuotaPendiente.SelectedIndex < 0 || _idCuotaActual == null)
                throw new Exception("Seleccione una cuota pendiente.");

            if (cbmFormaPago.SelectedIndex < 0 || cbmFormaPago.SelectedValue == null)
                throw new Exception("Seleccione la forma de pago.");

            if (string.IsNullOrWhiteSpace(txtMontoPagar.Text))
                throw new Exception("Ingrese el monto a pagar.");

            decimal montoAPagar = ParseDecimalOrZero(txtMontoPagar.Text);
            if (montoAPagar <= 0)
                throw new Exception("El monto a pagar debe ser mayor que cero.");

            decimal saldoPendiente = ObtenerSaldoPendienteDesdeLabel();
            if (montoAPagar > saldoPendiente)
                throw new Exception("El monto a pagar no puede ser mayor al saldo pendiente.");

            string formaPago = cbmFormaPago.SelectedValue.ToString()!;

            if (formaPago == "deposito")
            {
                if (cmbCuentaBancaria.SelectedIndex < 0 || cmbCuentaBancaria.SelectedValue == null)
                    throw new Exception("Seleccione una cuenta bancaria.");

                if (string.IsNullOrWhiteSpace(txtReferencia.Text))
                    throw new Exception("Ingrese el número de referencia.");
            }

            if (formaPago == "efectivo")
            {
                if (cmbEmpleado.SelectedIndex < 0 || cmbEmpleado.SelectedValue == null)
                    throw new Exception("Seleccione el empleado que recibe el efectivo.");
            }
        }

        private decimal ObtenerSaldoPendienteDesdeLabel()
        {
            string texto = lblSaldoPendiente.Text.Replace("Saldo pendiente:", "").Trim();
            return ParseDecimalOrZero(texto);
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            cmbLotesDisponibles.SelectedIndex = -1;
            cbmCuotaPendiente.DataSource = null;
            cmbCuentaBancaria.DataSource = null;

            cbmFormaPago.SelectedIndex = -1;
            cmbEmpleado.SelectedIndex = -1;

            txtMontoPagar.Clear();
            txtReferencia.Clear();
            txtObservacion.Clear();

            LimpiarCamposVenta();
            LimpiarCamposCuota();

            _idEtapaActual = null;
            _idVentaActual = null;
            _idCuotaActual = null;

            ActualizarControlesFormaPago();
        }

        private void LimpiarCamposVenta()
        {
            lblIDVenta.Text = "ID venta:";
            lblCliente.Text = "Cliente:";
            lblEtapa.Text = "Etapa:";
            lblLote.Text = "Lote:";
        }

        private void LimpiarCamposCuota()
        {
            lblIDCuota.Text = "ID cuota:";
            lblMontoCuota.Text = "Monto cuota:";
            lblSaldoPendiente.Text = "Saldo pendiente:";
            lblEstadoCuota.Text = "Estado cuota:";
            txtMontoPagar.Clear();
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
    }
}