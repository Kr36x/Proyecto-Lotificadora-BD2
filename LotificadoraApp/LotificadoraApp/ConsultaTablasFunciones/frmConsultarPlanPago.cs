using System.Data;
using System.Drawing;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultarPlanPago : Form
    {
        private bool _modoDetalle = true;

        public frmConsultarPlanPago()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmConsultarPlanPago_Load;
            btnConsultar.Click += btnConsultar_Click;
            btnDetalle.Click += btnDetalle_Click;
            btnResumen.Click += btnResumen_Click;
            btnGenerarCuota.Click += btnGenerarCuota_Click;
        }

        private void frmConsultarPlanPago_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarVentasCredito();
            LimpiarVista();
        }

        private void CargarVentasCredito()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("dbo.sp_listar_ventas_credito_activas");

                cmbVentaCredito.DataSource = dt;
                cmbVentaCredito.DisplayMember = "descripcion";
                cmbVentaCredito.ValueMember = "idVentaCredito";
                cmbVentaCredito.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar ventas crédito:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnConsultar_Click(object? sender, EventArgs e)
        {
            ConsultarPlanPago();
        }

        private void btnDetalle_Click(object? sender, EventArgs e)
        {
            _modoDetalle = true;
            ConsultarPlanPago();
        }

        private void btnResumen_Click(object? sender, EventArgs e)
        {
            _modoDetalle = false;
            ConsultarPlanPago();
        }

        private void btnGenerarCuota_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cmbVentaCredito.SelectedIndex < 0 || cmbVentaCredito.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Seleccione una venta al crédito.",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                int idVentaCredito = Convert.ToInt32(cmbVentaCredito.SelectedValue);

                DialogResult r = MessageBox.Show(
                    "¿Desea generar las cuotas para este crédito?",
                    "Confirmar generación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (r != DialogResult.Yes)
                    return;

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_generar_cuotas_plan_cursor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idVentaCredito", idVentaCredito);
                cmd.Parameters.AddWithValue("@fechaPrimerVencimiento", DBNull.Value);

                cn.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show(
                    "Cuotas generadas correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ConsultarPlanPago();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al generar cuotas:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConsultarPlanPago()
        {
            try
            {
                if (cmbVentaCredito.SelectedIndex < 0 || cmbVentaCredito.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Seleccione una venta al crédito.",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                int idVentaCredito = Convert.ToInt32(cmbVentaCredito.SelectedValue);

                CargarResumenCredito(idVentaCredito);

                if (_modoDetalle)
                    CargarDetalleCuotas(idVentaCredito);
                else
                    CargarResumenCuotas(idVentaCredito);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar plan de pago:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarResumenCredito(int idVentaCredito)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_obtener_resumen_credito", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idVentaCredito", idVentaCredito);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                LimpiarResumen();
                dgvCuotaCredito.DataSource = null;
                lblRegistros.Text = "Registros: 0";
                //btnGenerarCuota.Enabled = false;
                throw new Exception("No se encontró información para ese crédito.");
            }

            lblCliente.Text = "Cliente: " + dr["cliente"].ToString();
            lblTotalPlan.Text = "Total plan: " + Convert.ToDecimal(dr["totalPlan"]).ToString("N2");
            lblSaldoPendiente.Text = "Saldo pendiente: " + Convert.ToDecimal(dr["saldoPendiente"]).ToString("N2");
            lblEstadoCredito.Text = "Estado credito: " + dr["estadoCredito"].ToString();
        }

        private void CargarDetalleCuotas(int idVentaCredito)
        {
            const string sql = @"
        SELECT * FROM dbo.fn_tvf_plan_pago_por_credito(@idVentaCredito)";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@idVentaCredito", idVentaCredito);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvCuotaCredito.DataSource = dt;
            lblRegistros.Text = $"Registros: {dt.Rows.Count}";
            //btnGenerarCuota.Enabled = dt.Rows.Count == 0;

          //  ConfigurarColumnasSegunModo();
        }

        private void CargarResumenCuotas(int idVentaCredito)
        {
            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand("dbo.sp_resumen_plan_pago_por_credito", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idVentaCredito", idVentaCredito);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvCuotaCredito.DataSource = dt;
            lblRegistros.Text = $"Registros: {dt.Rows.Count}";
            //btnGenerarCuota.Enabled = dt.Rows.Count == 0;

           // ConfigurarColumnasSegunModo();
        }

        private void LimpiarVista()
        {
            cmbVentaCredito.SelectedIndex = -1;
            LimpiarResumen();
            dgvCuotaCredito.DataSource = null;
            lblRegistros.Text = "Registros: 0";
            //btnGenerarCuota.Enabled = false;
        }

        private void LimpiarResumen()
        {
            lblCliente.Text = "Cliente:";
            lblTotalPlan.Text = "Total plan:";
            lblSaldoPendiente.Text = "Saldo pendiente:";
            lblEstadoCredito.Text = "Estado credito:";
        }

        private void ConfigurarGrid()
        {
            dgvCuotaCredito.AutoGenerateColumns = true;
            dgvCuotaCredito.ReadOnly = true;
            dgvCuotaCredito.MultiSelect = false;
            dgvCuotaCredito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCuotaCredito.AllowUserToAddRows = false;
            dgvCuotaCredito.AllowUserToDeleteRows = false;
            dgvCuotaCredito.AllowUserToResizeRows = false;
            dgvCuotaCredito.RowHeadersVisible = false;
            dgvCuotaCredito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCuotaCredito.ColumnHeadersHeight = 52;
            dgvCuotaCredito.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCuotaCredito.EnableHeadersVisualStyles = false;
            dgvCuotaCredito.BackgroundColor = Color.White;
            dgvCuotaCredito.BorderStyle = BorderStyle.None;
            dgvCuotaCredito.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCuotaCredito.GridColor = Color.FromArgb(210, 210, 210);

            dgvCuotaCredito.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvCuotaCredito.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCuotaCredito.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvCuotaCredito.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvCuotaCredito.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvCuotaCredito.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCuotaCredito.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvCuotaCredito.DefaultCellStyle.BackColor = Color.White;
            dgvCuotaCredito.DefaultCellStyle.ForeColor = Color.Black;
            dgvCuotaCredito.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCuotaCredito.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCuotaCredito.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvCuotaCredito.DefaultCellStyle.Padding = new Padding(3);

            dgvCuotaCredito.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvCuotaCredito.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvCuotaCredito.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCuotaCredito.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvCuotaCredito.RowTemplate.Height = 32;
        }

        private void ConfigurarColumnasSegunModo()
        {
            if (_modoDetalle)
            {
                ConfigurarColumna("idPlanPago", 8, 80, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("idCuota", 7, 70, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("numeroCuota", 8, 80, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("fechaVencimiento", 10, 100, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumnaMoneda("saldoInicial", 10, 100);
                ConfigurarColumnaMoneda("capitalProgramado", 10, 110);
                ConfigurarColumnaMoneda("interesProgramado", 10, 110);
                ConfigurarColumnaMoneda("montoCuota", 10, 100);
                ConfigurarColumnaMoneda("saldoFinal", 10, 100);
                ConfigurarColumnaMoneda("totalPagado", 10, 100);
                ConfigurarColumnaMoneda("saldoPendiente", 10, 110);
                ConfigurarColumna("estadoCuota", 10, 100, DataGridViewContentAlignment.MiddleCenter);
            }
            else
            {
                ConfigurarColumna("numeroCuota", 10, 90, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumna("fechaVencimiento", 14, 110, DataGridViewContentAlignment.MiddleCenter);
                ConfigurarColumnaMoneda("montoCuota", 14, 110);
                ConfigurarColumnaMoneda("totalPagado", 14, 110);
                ConfigurarColumnaMoneda("saldoPendiente", 14, 120);
                ConfigurarColumna("estadoCuota", 14, 110, DataGridViewContentAlignment.MiddleCenter);
            }
        }

        private void ConfigurarColumna(string nombre, float fillWeight, int minWidth, DataGridViewContentAlignment align)
        {
            if (!dgvCuotaCredito.Columns.Contains(nombre))
                return;

            dgvCuotaCredito.Columns[nombre].FillWeight = fillWeight;
            dgvCuotaCredito.Columns[nombre].MinimumWidth = minWidth;
            dgvCuotaCredito.Columns[nombre].DefaultCellStyle.Alignment = align;
        }

        private void ConfigurarColumnaMoneda(string nombre, float fillWeight, int minWidth)
        {
            if (!dgvCuotaCredito.Columns.Contains(nombre))
                return;

            dgvCuotaCredito.Columns[nombre].FillWeight = fillWeight;
            dgvCuotaCredito.Columns[nombre].MinimumWidth = minWidth;
            dgvCuotaCredito.Columns[nombre].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCuotaCredito.Columns[nombre].DefaultCellStyle.Format = "N2";
        }
    }
}