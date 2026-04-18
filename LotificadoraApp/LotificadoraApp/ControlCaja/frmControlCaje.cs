using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp.ControlCaja
{
    public partial class frmControlCaje : Form
    {
        private bool _modoDetalle = true;

        public frmControlCaje()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today;
            dtpFechaOperacion.Value = DateTime.Today;

            ConfigurarGrid();
            LimpiarResumenCaja();
            LimpiarResumenDeposito();
        }

        private void ConectarEventos()
        {
            Load += frmControlCaje_Load;

            btnConsultar.Click += btnConsultar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnDetalle.Click += btnDetalle_Click;
            btnResumen.Click += btnResumen_Click;

            btnConsultarPendiente.Click += btnConsultarPendiente_Click;
            btnRegistrarDeposito.Click += btnRegistrarDeposito_Click;
        }

        private void frmControlCaje_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarEmpleados();
                CargarCuentasBancarias();
                CargarResumenCaja();
                CargarMovimientosCaja();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar control de caja:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarEmpleados()
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

        private void CargarCuentasBancarias()
        {
            DataTable dt = Db.ExecuteStoredProcedure("dbo.sp_cuenta_bancaria_listar_activas_combo");

            cmbCuentaBancaria.DataSource = dt;
            cmbCuentaBancaria.DisplayMember = "descripcion";
            cmbCuentaBancaria.ValueMember = "idCuentaBancaria";
            cmbCuentaBancaria.SelectedIndex = -1;
        }

        private void btnConsultar_Click(object? sender, EventArgs e)
        {
            CargarResumenCaja();
            CargarMovimientosCaja();
        }

        private void btnDetalle_Click(object? sender, EventArgs e)
        {
            _modoDetalle = true;
            CargarMovimientosCaja();
        }

        private void btnResumen_Click(object? sender, EventArgs e)
        {
            _modoDetalle = false;
            CargarMovimientosCaja();
        }

        private void CargarResumenCaja()
        {
            DataTable dt = Db.ExecuteStoredProcedure(
                "dbo.sp_control_caja_obtener_resumen",
                new SqlParameter("@fechaInicio", dtpFechaInicio.Value.Date),
                new SqlParameter("@fechaFin", dtpFechaFin.Value.Date)
            );

            if (dt.Rows.Count == 0)
            {
                LimpiarResumenCaja();
                return;
            }

            DataRow row = dt.Rows[0];

            lblTotalRecibido.Text = "Total recibido: L. " + Convert.ToDecimal(row["totalRecibido"]).ToString("N2");
            lblTotalDepositado.Text = "Total depositado: L. " + Convert.ToDecimal(row["totalDepositado"]).ToString("N2");
            lblSaldoCaja.Text = "Saldo en caja: L. " + Convert.ToDecimal(row["saldoCaja"]).ToString("N2");
            lblMovimientos.Text = "Movimientos: " + row["totalMovimientos"].ToString();
        }

        private void CargarMovimientosCaja()
        {
            DataTable dt;

            if (_modoDetalle)
            {
                dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_control_caja_listar",
                    new SqlParameter("@fechaInicio", dtpFechaInicio.Value.Date),
                    new SqlParameter("@fechaFin", dtpFechaFin.Value.Date)
                );
            }
            else
            {
                dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_control_caja_resumen_movimientos",
                    new SqlParameter("@fechaInicio", dtpFechaInicio.Value.Date),
                    new SqlParameter("@fechaFin", dtpFechaFin.Value.Date)
                );
            }

            dgvControlCaja.DataSource = dt;
            ConfigurarColumnasSegunModo();
        }

        private void btnConsultarPendiente_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cmbCuentaBancaria.SelectedIndex < 0 || cmbCuentaBancaria.SelectedValue == null)
                    throw new Exception("Seleccione una cuenta bancaria.");

                int idCuentaBancaria = Convert.ToInt32(cmbCuentaBancaria.SelectedValue);

                DataTable dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_control_caja_consultar_pendiente_deposito",
                    new SqlParameter("@idCuentaBancaria", idCuentaBancaria),
                    new SqlParameter("@fechaOperacion", dtpFechaOperacion.Value.Date)
                );

                if (dt.Rows.Count == 0)
                {
                    LimpiarResumenDeposito();
                    return;
                }

                DataRow row = dt.Rows[0];

                lblCantidadPagosPendientes.Text =
                    "Cantidad pagos pendientes: " + row["cantidadPagos"].ToString();

                lblTotalDepositar.Text =
                    "Total a depositar: L. " + Convert.ToDecimal(row["totalPendiente"]).ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar pendiente:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRegistrarDeposito_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarDeposito();

                int idCuentaBancaria = Convert.ToInt32(cmbCuentaBancaria.SelectedValue);
                int idEmpleado = Convert.ToInt32(cmbEmpleado.SelectedValue);

                object observacion = string.IsNullOrWhiteSpace(txtObservacionDeposito.Text)
                    ? DBNull.Value
                    : txtObservacionDeposito.Text.Trim();

                DataTable dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_procesar_deposito_caja_transaccional",
                    new SqlParameter("@idCuentaBancaria", idCuentaBancaria),
                    new SqlParameter("@idEmpleado", idEmpleado),
                    new SqlParameter("@fechaOperacion", dtpFechaOperacion.Value.Date),
                    new SqlParameter("@observacion", observacion)
                );

                if (dt.Rows.Count > 0 && dt.Columns.Contains("MensajeError"))
                {
                    string mensajeError = dt.Rows[0]["MensajeError"]?.ToString() ?? "Error al procesar depósito.";
                    throw new Exception(mensajeError);
                }

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    MessageBox.Show(
                        "Depósito procesado correctamente.\n\n" +
                        $"Id Depósito: {row["idDepositoCaja"]}\n" +
                        $"Cantidad pagos: {row["cantidadPagos"]}\n" +
                        $"Total depositado: L. {Convert.ToDecimal(row["totalDepositado"]).ToString("N2")}",
                        "Depósito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                txtObservacionDeposito.Clear();
                LimpiarResumenDeposito();

                CargarResumenCaja();
                CargarMovimientosCaja();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar depósito:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ValidarDeposito()
        {
            if (cmbCuentaBancaria.SelectedIndex < 0 || cmbCuentaBancaria.SelectedValue == null)
                throw new Exception("Seleccione una cuenta bancaria.");

            if (cmbEmpleado.SelectedIndex < 0 || cmbEmpleado.SelectedValue == null)
                throw new Exception("Seleccione un empleado.");

            decimal totalPendiente = ObtenerTotalDepositarDesdeLabel();
            if (totalPendiente <= 0)
                throw new Exception("No hay efectivo pendiente para depositar. Consulte primero.");
        }

        private decimal ObtenerTotalDepositarDesdeLabel()
        {
            string texto = lblTotalDepositar.Text.Replace("Total a depositar: L.", "").Trim();

            if (string.IsNullOrWhiteSpace(texto) || texto == "0.00" || texto == "0")
                return 0m;

            return ParseDecimalOrZero(texto);
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

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today;
            dtpFechaOperacion.Value = DateTime.Today;

            cmbCuentaBancaria.SelectedIndex = -1;
            cmbEmpleado.SelectedIndex = -1;
            txtObservacionDeposito.Clear();

            LimpiarResumenCaja();
            LimpiarResumenDeposito();

            CargarResumenCaja();
            CargarMovimientosCaja();
        }

        private void LimpiarResumenCaja()
        {
            lblTotalRecibido.Text = "Total recibido:";
            lblTotalDepositado.Text = "Total depositado:";
            lblSaldoCaja.Text = "Saldo en caja:";
            lblMovimientos.Text = "Movimientos:";
        }

        private void LimpiarResumenDeposito()
        {
            lblCantidadPagosPendientes.Text = "Cantidad pagos pendientes:";
            lblTotalDepositar.Text = "Total a depositar:";
        }

        private void ConfigurarGrid()
        {
            dgvControlCaja.AutoGenerateColumns = true;
            dgvControlCaja.ReadOnly = true;
            dgvControlCaja.MultiSelect = false;
            dgvControlCaja.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvControlCaja.AllowUserToAddRows = false;
            dgvControlCaja.AllowUserToDeleteRows = false;
            dgvControlCaja.AllowUserToResizeRows = false;
            dgvControlCaja.RowHeadersVisible = false;
            dgvControlCaja.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvControlCaja.ColumnHeadersHeight = 52;
            dgvControlCaja.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvControlCaja.EnableHeadersVisualStyles = false;
            dgvControlCaja.BackgroundColor = Color.White;
            dgvControlCaja.BorderStyle = BorderStyle.None;
            dgvControlCaja.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvControlCaja.GridColor = Color.FromArgb(210, 210, 210);

            dgvControlCaja.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvControlCaja.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvControlCaja.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvControlCaja.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvControlCaja.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvControlCaja.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvControlCaja.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvControlCaja.DefaultCellStyle.BackColor = Color.White;
            dgvControlCaja.DefaultCellStyle.ForeColor = Color.Black;
            dgvControlCaja.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvControlCaja.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvControlCaja.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvControlCaja.DefaultCellStyle.Padding = new Padding(3);

            dgvControlCaja.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvControlCaja.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvControlCaja.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvControlCaja.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvControlCaja.RowTemplate.Height = 32;
        }

        private void ConfigurarColumnasSegunModo()
        {
            if (_modoDetalle)
            {
                ConfigurarColumna("idControlCaja", 8, 90, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("idPago", 8, 80, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("idDepositoCaja", 10, 100, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("idEmpleado", 8, 80, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("nombres", 14, 120, DataGridViewContentAlignment.MiddleLeft);
                ConfigurarColumna("apellidos", 14, 120, DataGridViewContentAlignment.MiddleLeft);
                ConfigurarColumna("fechaMovimiento", 12, 120, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("tipoMovimiento", 14, 130, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumnaMoneda("monto", 10, 100);
                ConfigurarColumna("observacion", 22, 180, DataGridViewContentAlignment.MiddleLeft);
            }
            else
            {
                ConfigurarColumna("fechaMovimiento", 18, 120, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("tipoMovimiento", 18, 130, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumnaMoneda("monto", 14, 100);
                ConfigurarColumna("empleado", 20, 180, DataGridViewContentAlignment.MiddleLeft);
                ConfigurarColumna("observacion", 30, 220, DataGridViewContentAlignment.MiddleLeft);
            }
        }

        private void ConfigurarColumna(string nombre, float fillWeight, int minWidth, DataGridViewContentAlignment align)
        {
            if (!dgvControlCaja.Columns.Contains(nombre))
                return;

            dgvControlCaja.Columns[nombre].FillWeight = fillWeight;
            dgvControlCaja.Columns[nombre].MinimumWidth = minWidth;
            dgvControlCaja.Columns[nombre].DefaultCellStyle.Alignment = align;
        }

        private void ConfigurarColumnaMoneda(string nombre, float fillWeight, int minWidth)
        {
            if (!dgvControlCaja.Columns.Contains(nombre))
                return;

            dgvControlCaja.Columns[nombre].FillWeight = fillWeight;
            dgvControlCaja.Columns[nombre].MinimumWidth = minWidth;
            dgvControlCaja.Columns[nombre].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvControlCaja.Columns[nombre].DefaultCellStyle.Format = "N2";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
        }

        private void guna2Panel8_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}