using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace LotificadoraApp
{
    public partial class frmConsultaEstadoCuenta : Form
    {
        private bool _modoDetalle = true;

        public frmConsultaEstadoCuenta()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmConsultaEstadoCuenta_Load;
            btnConsultar.Click += btnConsultar_Click;
            btnDetalle.Click += btnDetalle_Click;
            btnResumen.Click += btnResumen_Click;
        }

        private void frmConsultaEstadoCuenta_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarClientes();
        }

        private void CargarClientes()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_cliente_listar_activo");

                cmbCliente.DataSource = dt;
                cmbCliente.DisplayMember = "nombreCompleto";
                cmbCliente.ValueMember = "idCliente";
                cmbCliente.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar clientes:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnConsultar_Click(object? sender, EventArgs e)
        {
            ConsultarEstadoCuenta();
        }

        private void btnDetalle_Click(object? sender, EventArgs e)
        {
            _modoDetalle = true;
            ConsultarEstadoCuenta();
        }

        private void btnResumen_Click(object? sender, EventArgs e)
        {
            _modoDetalle = false;
            ConsultarEstadoCuenta();
        }

        private void ConsultarEstadoCuenta()
        {
            try
            {
                if (cmbCliente.SelectedIndex < 0 || cmbCliente.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Seleccione un cliente.",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                int idCliente = Convert.ToInt32(cmbCliente.SelectedValue);
                DataTable dt;

                if (_modoDetalle)
                {
                    dt = Db.ExecuteQuery(
                        @"SELECT *
                          FROM dbo.fn_tvf_estado_cuenta_cliente(@idCliente);",
                        new SqlParameter("@idCliente", idCliente)
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_resumen_estado_cuenta_cliente",
                        new SqlParameter("@idCliente", idCliente)
                    );
                }

                dgvVistaLotes.DataSource = dt;
                //ConfigurarColumnasSegunModo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar estado de cuenta:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarGrid()
        {
            dgvVistaLotes.AutoGenerateColumns = true;
            dgvVistaLotes.ReadOnly = true;
            dgvVistaLotes.MultiSelect = false;
            dgvVistaLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVistaLotes.AllowUserToAddRows = false;
            dgvVistaLotes.AllowUserToDeleteRows = false;
            dgvVistaLotes.AllowUserToResizeRows = false;
            dgvVistaLotes.RowHeadersVisible = false;
            dgvVistaLotes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVistaLotes.ColumnHeadersHeight = 52;
            dgvVistaLotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvVistaLotes.EnableHeadersVisualStyles = false;
            dgvVistaLotes.BackgroundColor = Color.White;
            dgvVistaLotes.BorderStyle = BorderStyle.None;
            dgvVistaLotes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVistaLotes.GridColor = Color.FromArgb(210, 210, 210);

            dgvVistaLotes.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvVistaLotes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVistaLotes.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvVistaLotes.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvVistaLotes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvVistaLotes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVistaLotes.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvVistaLotes.DefaultCellStyle.BackColor = Color.White;
            dgvVistaLotes.DefaultCellStyle.ForeColor = Color.Black;
            dgvVistaLotes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaLotes.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvVistaLotes.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvVistaLotes.DefaultCellStyle.Padding = new Padding(3);

            dgvVistaLotes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvVistaLotes.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvVistaLotes.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaLotes.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvVistaLotes.RowTemplate.Height = 32;
        }

        private void ConfigurarColumnasSegunModo()
        {
            if (_modoDetalle)
            {
                if (dgvVistaLotes.Columns.Contains("idCliente"))
                    dgvVistaLotes.Columns["idCliente"].FillWeight = 7;

                if (dgvVistaLotes.Columns.Contains("cliente"))
                    dgvVistaLotes.Columns["cliente"].FillWeight = 18;

                if (dgvVistaLotes.Columns.Contains("idVenta"))
                    dgvVistaLotes.Columns["idVenta"].FillWeight = 7;

                if (dgvVistaLotes.Columns.Contains("idVentaCredito"))
                    dgvVistaLotes.Columns["idVentaCredito"].FillWeight = 8;

                if (dgvVistaLotes.Columns.Contains("idPlanPago"))
                    dgvVistaLotes.Columns["idPlanPago"].FillWeight = 8;

                if (dgvVistaLotes.Columns.Contains("idCuota"))
                    dgvVistaLotes.Columns["idCuota"].FillWeight = 7;

                if (dgvVistaLotes.Columns.Contains("numeroCuota"))
                    dgvVistaLotes.Columns["numeroCuota"].FillWeight = 8;

                if (dgvVistaLotes.Columns.Contains("fechaVencimiento"))
                    dgvVistaLotes.Columns["fechaVencimiento"].FillWeight = 10;

                if (dgvVistaLotes.Columns.Contains("montoCuota"))
                {
                    dgvVistaLotes.Columns["montoCuota"].FillWeight = 10;
                    dgvVistaLotes.Columns["montoCuota"].DefaultCellStyle.Format = "N2";
                    dgvVistaLotes.Columns["montoCuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvVistaLotes.Columns.Contains("totalPagado"))
                {
                    dgvVistaLotes.Columns["totalPagado"].FillWeight = 10;
                    dgvVistaLotes.Columns["totalPagado"].DefaultCellStyle.Format = "N2";
                    dgvVistaLotes.Columns["totalPagado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvVistaLotes.Columns.Contains("saldoPendiente"))
                {
                    dgvVistaLotes.Columns["saldoPendiente"].FillWeight = 10;
                    dgvVistaLotes.Columns["saldoPendiente"].DefaultCellStyle.Format = "N2";
                    dgvVistaLotes.Columns["saldoPendiente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvVistaLotes.Columns.Contains("estadoId"))
                    dgvVistaLotes.Columns["estadoId"].FillWeight = 7;
            }
            else
            {
                if (dgvVistaLotes.Columns.Contains("idCliente"))
                    dgvVistaLotes.Columns["idCliente"].FillWeight = 8;

                if (dgvVistaLotes.Columns.Contains("cliente"))
                    dgvVistaLotes.Columns["cliente"].FillWeight = 22;

                if (dgvVistaLotes.Columns.Contains("idCuota"))
                    dgvVistaLotes.Columns["idCuota"].FillWeight = 8;

                if (dgvVistaLotes.Columns.Contains("numeroCuota"))
                    dgvVistaLotes.Columns["numeroCuota"].FillWeight = 8;

                if (dgvVistaLotes.Columns.Contains("fechaVencimiento"))
                    dgvVistaLotes.Columns["fechaVencimiento"].FillWeight = 12;

                if (dgvVistaLotes.Columns.Contains("montoCuota"))
                {
                    dgvVistaLotes.Columns["montoCuota"].FillWeight = 12;
                    dgvVistaLotes.Columns["montoCuota"].DefaultCellStyle.Format = "N2";
                    dgvVistaLotes.Columns["montoCuota"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvVistaLotes.Columns.Contains("saldoPendiente"))
                {
                    dgvVistaLotes.Columns["saldoPendiente"].FillWeight = 12;
                    dgvVistaLotes.Columns["saldoPendiente"].DefaultCellStyle.Format = "N2";
                    dgvVistaLotes.Columns["saldoPendiente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvVistaLotes.Columns.Contains("estado"))
                    dgvVistaLotes.Columns["estado"].FillWeight = 10;
            }
        }
    }
}