using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultarPlanPago : Form
    {
        public frmConsultarPlanPago()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtCliente.ReadOnly = true;
            txtIdPlanPago.ReadOnly = true;
            txtTotalPlan.ReadOnly = true;
            txtSaldoPendiente.ReadOnly = true;
            txtEstadoCredito.ReadOnly = true;

            txtCliente.TabStop = false;
            txtIdPlanPago.TabStop = false;
            txtTotalPlan.TabStop = false;
            txtSaldoPendiente.TabStop = false;
            txtEstadoCredito.TabStop = false;

            dgvDetallesCuotas.ReadOnly = true;
            dgvDetallesCuotas.AllowUserToAddRows = false;
            dgvDetallesCuotas.AllowUserToDeleteRows = false;
            dgvDetallesCuotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetallesCuotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblRegistros.Text = "Registros: 0";
        }

        private void ConectarEventos()
        {
            Load += frmConsultarPlanPago_Load;
            txtIDVentaCredito.KeyDown += txtIDVentaCredito_KeyDown;
        }

        private void frmConsultarPlanPago_Load(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void txtIDVentaCredito_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ConsultarPlanPago();
            }
        }

        private void ConsultarPlanPago()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIDVentaCredito.Text))
                    throw new Exception("Ingrese el ID de venta crédito.");

                if (!int.TryParse(txtIDVentaCredito.Text.Trim(), out int idVentaCredito) || idVentaCredito <= 0)
                    throw new Exception("El ID de venta crédito debe ser un entero válido.");

                CargarResumenCredito(idVentaCredito);
                CargarDetalleCuotas(idVentaCredito);
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
                dgvDetallesCuotas.DataSource = null;
                lblRegistros.Text = "Registros: 0";
                throw new Exception("No se encontró información para ese ID de venta crédito.");
            }

            txtCliente.Text = dr["cliente"].ToString();
            txtIdPlanPago.Text = dr["idPlanPago"].ToString();
            txtTotalPlan.Text = Convert.ToDecimal(dr["totalPlan"]).ToString("N2");
            txtSaldoPendiente.Text = Convert.ToDecimal(dr["saldoPendiente"]).ToString("N2");
            txtEstadoCredito.Text = dr["estadoCredito"].ToString();
        }

        private void CargarDetalleCuotas(int idVentaCredito)
        {
            const string sql = @"
            SELECT
                    f.idPlanPago,
                    f.idCuota,
                    f.numeroCuota,
                    f.fechaVencimiento,
                    f.saldoInicial,
                    f.capitalProgramado,
                    f.interesProgramado,
                    f.montoCuota,
                    f.saldoFinal,
                    f.totalPagado,
                    f.saldoPendiente,
                    e.nombre AS estadoCuota
                FROM dbo.fn_tvf_plan_pago_por_credito(@idVentaCredito) f
                INNER JOIN dbo.Estado e
                    ON f.estadoId = e.id
                ORDER BY f.numeroCuota";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@idVentaCredito", idVentaCredito);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvDetallesCuotas.DataSource = null;
            dgvDetallesCuotas.DataSource = dt;
            lblRegistros.Text = $"Registros: {dt.Rows.Count}";
        }

        private void LimpiarFormulario()
        {
            txtIDVentaCredito.Clear();
            LimpiarResumen();
            dgvDetallesCuotas.DataSource = null;
            lblRegistros.Text = "Registros: 0";
        }

        private void LimpiarResumen()
        {
            txtCliente.Clear();
            txtIdPlanPago.Clear();
            txtTotalPlan.Clear();
            txtSaldoPendiente.Clear();
            txtEstadoCredito.Clear();
        }

        private void dgvDetallesCuotas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}