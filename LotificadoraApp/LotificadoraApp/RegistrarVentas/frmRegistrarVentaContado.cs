using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp.RegistrarVentas
{
    public partial class frmRegistrarVentaContado : Form
    {
        private int _idLoteSeleccionado = 0;
        private decimal _precioLoteActual = 0m;

        public frmRegistrarVentaContado()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            guna2DateTimePicker1.Value = DateTime.Today; // fecha venta
            dtpFechaInicio.Value = DateTime.Today;       // fecha pago

            CargarFormaPago();
            HabilitarControlesSegunFormaPago();
            LimpiarDetalleLote();
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarVentaContado_Load;

            cmbLotesDisponibles.SelectedIndexChanged += cmbLotesDisponibles_SelectedIndexChanged;
            cmbFormaDePago.SelectedIndexChanged += cmbFormaDePago_SelectedIndexChanged;

            btnCalcular.Click += btnCalcular_Click;
            btnRegistrarVenta.Click += btnRegistrarVenta_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            guna2Button2.Click += guna2Button2_Click;
        }

        private void frmRegistrarVentaContado_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarLotesDisponibles();
                CargarClientes();
                CargarEmpleados();
                LimpiarFormulario();
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
            DataTable dt = Db.ExecuteStoredProcedure("sp_cliente_listar_activo");

            cmbCliente.DataSource = dt;
            cmbCliente.DisplayMember = "nombreCompleto";
            cmbCliente.ValueMember = "idCliente";
            cmbCliente.SelectedIndex = -1;
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
                throw new Exception("Error al cargar empleados: " + ex.Message);
            }
        }

        private void CargarCuentasBancarias(int idLote)
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "dbo.sp_cuenta_bancaria_listar_por_lote",
                new SqlParameter("@idLote", idLote)
            );

            cmbCuentaBancaria.DataSource = dt;
            cmbCuentaBancaria.DisplayMember = "descripcion";
            cmbCuentaBancaria.ValueMember = "idCuentaBancaria";
            cmbCuentaBancaria.SelectedIndex = -1;
        }

        private void CargarFormaPago()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("valor");
            dt.Columns.Add("texto");

            dt.Rows.Add("efectivo", "efectivo");
            dt.Rows.Add("deposito", "deposito");

            cmbFormaDePago.DataSource = dt;
            cmbFormaDePago.DisplayMember = "texto";
            cmbFormaDePago.ValueMember = "valor";
            cmbFormaDePago.SelectedIndex = -1;
        }

        private void cmbLotesDisponibles_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbLotesDisponibles.SelectedIndex < 0 || cmbLotesDisponibles.SelectedValue == null)
            {
                LimpiarDetalleLote();
                cmbCuentaBancaria.DataSource = null;
                return;
            }

            try
            {
                _idLoteSeleccionado = Convert.ToInt32(cmbLotesDisponibles.SelectedValue);
                CargarDetalleLote(_idLoteSeleccionado);
                CargarCuentasBancarias(_idLoteSeleccionado);
            }
            catch
            {
            }
        }

        private void CargarDetalleLote(int idLote)
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "sp_obtener_detalle_lote_disponible",
                new SqlParameter("@idLote", idLote)
            );

            if (dt.Rows.Count == 0)
                throw new Exception("No se encontró el detalle del lote.");

            DataRow row = dt.Rows[0];

            _idLoteSeleccionado = Convert.ToInt32(row["idLote"]);
            _precioLoteActual = Convert.ToDecimal(row["precioFinalCalculado"]);

            lblIDLote.Text = "ID Lote: " + row["idLote"].ToString();
            lblProyecto.Text = "Proyecto: " + row["nombreProyecto"].ToString();
            lblEtapa.Text = "Etapa: " + row["nombreEtapa"].ToString();
            lblBloque.Text = "Bloque: " + row["nombreBloque"].ToString();
            lblNumeroLote.Text = "Numero Lote: " + row["numeroLote"].ToString();
            lblPrecioLote.Text = "Precio Lote: " + _precioLoteActual.ToString("N2");
        }

        private void LimpiarDetalleLote()
        {
            _idLoteSeleccionado = 0;
            _precioLoteActual = 0m;

            lblIDLote.Text = "ID Lote:";
            lblProyecto.Text = "Proyecto:";
            lblEtapa.Text = "Etapa:";
            lblBloque.Text = "Bloque:";
            lblNumeroLote.Text = "Numero Lote:";
            lblPrecioLote.Text = "Precio Lote:";
        }

        private void cmbFormaDePago_SelectedIndexChanged(object? sender, EventArgs e)
        {
            HabilitarControlesSegunFormaPago();
        }

        private void HabilitarControlesSegunFormaPago()
        {
            string formaPago = cmbFormaDePago.SelectedValue?.ToString() ?? string.Empty;

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

        private void btnCalcular_Click(object? sender, EventArgs e)
        {
            try
            {
                decimal totalVenta = CalcularTotalVenta();

                MessageBox.Show(
                    $"Total de la venta: {totalVenta:N2}",
                    "Cálculo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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

        private decimal CalcularTotalVenta()
        {
            decimal descuento = ParseDecimalOrZero(txtDescuento.Text);
            decimal recargo = ParseDecimalOrZero(txtRecargo.Text);

            decimal totalVenta = _precioLoteActual - descuento + recargo;

            if (totalVenta <= 0)
                throw new Exception("El total de la venta debe ser mayor que cero.");

            return totalVenta;
        }

        private void btnRegistrarVenta_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                int idCliente = Convert.ToInt32(cmbCliente.SelectedValue);
                DateTime fechaVenta = guna2DateTimePicker1.Value.Date;
                DateTime fechaPago = dtpFechaInicio.Value.Date;

                decimal descuento = ParseDecimalOrZero(txtDescuento.Text);
                decimal recargo = ParseDecimalOrZero(txtRecargo.Text);
                decimal totalVenta = CalcularTotalVenta();

                string formaPago = cmbFormaDePago.SelectedValue!.ToString()!;
                object idEmpleado = formaPago == "efectivo"
                    ? Convert.ToInt32(cmbEmpleado.SelectedValue)
                    : DBNull.Value;

                object idCuentaBancaria = formaPago == "deposito"
                    ? Convert.ToInt32(cmbCuentaBancaria.SelectedValue)
                    : DBNull.Value;

                object numeroReferencia = formaPago == "deposito"
                    ? txtReferencia.Text.Trim()
                    : DBNull.Value;

                DataTable dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_registrar_venta_contado_transaccional",
                    new SqlParameter("@idLote", _idLoteSeleccionado),
                    new SqlParameter("@idCliente", idCliente),
                    new SqlParameter("@fechaVenta", fechaVenta),
                    new SqlParameter("@precioLote", _precioLoteActual),
                    new SqlParameter("@descuento", descuento),
                    new SqlParameter("@recargo", recargo),
                    new SqlParameter("@fechaPago", fechaPago),
                    new SqlParameter("@formaPago", formaPago),
                    new SqlParameter("@idCuentaBancaria", idCuentaBancaria),
                    new SqlParameter("@numeroReferencia", numeroReferencia),
                    new SqlParameter("@idEmpleado", idEmpleado),
                    new SqlParameter("@observacion",
                        string.IsNullOrWhiteSpace(txtObservacion.Text)
                            ? DBNull.Value
                            : txtObservacion.Text.Trim())
                );

                if (dt.Rows.Count > 0 && dt.Columns.Contains("MensajeError"))
                {
                    string mensajeError = dt.Rows[0]["MensajeError"]?.ToString() ?? "Error al registrar la venta.";
                    throw new Exception(mensajeError);
                }

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    MessageBox.Show(
                        "Venta registrada correctamente.\n\n" +
                        $"Id Venta: {row["idVenta"]}\n" +
                        $"Id Pago: {row["idPago"]}\n" +
                        $"Id Factura: {row["idFactura"]}\n" +
                        $"Total Venta: {Convert.ToDecimal(row["totalVenta"]):N2}",
                        "Venta al contado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "La venta se registró correctamente.",
                        "Venta al contado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                CargarLotesDisponibles();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar la venta:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ValidarFormulario()
        {
            if (cmbLotesDisponibles.SelectedIndex < 0 || _idLoteSeleccionado <= 0)
                throw new Exception("Seleccione un lote disponible.");

            if (cmbCliente.SelectedIndex < 0 || cmbCliente.SelectedValue == null)
                throw new Exception("Seleccione un cliente.");

            if (cmbFormaDePago.SelectedIndex < 0 || cmbFormaDePago.SelectedValue == null)
                throw new Exception("Seleccione una forma de pago.");

            string formaPago = cmbFormaDePago.SelectedValue.ToString()!;

            if (formaPago == "efectivo")
            {
                if (cmbEmpleado.SelectedIndex < 0 || cmbEmpleado.SelectedValue == null)
                    throw new Exception("Seleccione el empleado que recibe el efectivo.");
            }

            if (formaPago == "deposito")
            {
                if (cmbCuentaBancaria.SelectedIndex < 0 || cmbCuentaBancaria.SelectedValue == null)
                    throw new Exception("Seleccione la cuenta bancaria.");

                if (string.IsNullOrWhiteSpace(txtReferencia.Text))
                    throw new Exception("Ingrese la referencia del depósito.");
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
            cmbFormaDePago.SelectedIndex = -1;
            cmbEmpleado.SelectedIndex = -1;
            cmbCuentaBancaria.SelectedIndex = -1;

            txtDescuento.Clear();
            txtRecargo.Clear();
            txtReferencia.Clear();
            txtObservacion.Clear();

            guna2DateTimePicker1.Value = DateTime.Today;
            dtpFechaInicio.Value = DateTime.Today;

            LimpiarDetalleLote();
            HabilitarControlesSegunFormaPago();
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

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void guna2Panel8_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}