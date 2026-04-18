using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmFacturas : Form
    {
        public frmFacturas()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            ConfigurarGrid();
        }

        private void ConectarEventos()
        {
            Load += frmFacturas_Load;
            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnCrear.Click += btnCrear_Click;
        }

        private void frmFacturas_Load(object? sender, EventArgs e)
        {
            ObtenerFacturas();
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            BuscarFacturas();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtCliente.Clear();
            ObtenerFacturas();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            ObtenerFacturas();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            VerDetalleFacturaSeleccionada();
        }

        private void ObtenerFacturas()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_factura_listar",
                    new SqlParameter("@numeroFactura", DBNull.Value),
                    new SqlParameter("@fechaInicio", DBNull.Value),
                    new SqlParameter("@fechaFin", DBNull.Value),
                    new SqlParameter("@nombreCliente", DBNull.Value)
                );

                dgvFactura.DataSource = dt;
                ConfigurarColumnasFactura();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al obtener facturas:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BuscarFacturas()
        {
            try
            {
                object nombreCliente = string.IsNullOrWhiteSpace(txtCliente.Text)
                    ? DBNull.Value
                    : txtCliente.Text.Trim();

                DataTable dt = Db.ExecuteStoredProcedure(
                    "dbo.sp_factura_listar",
                    new SqlParameter("@numeroFactura", DBNull.Value),
                    new SqlParameter("@fechaInicio", DBNull.Value),
                    new SqlParameter("@fechaFin", DBNull.Value),
                    new SqlParameter("@nombreCliente", nombreCliente)
                );

                dgvFactura.DataSource = dt;
                ConfigurarColumnasFactura();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al buscar facturas:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void VerDetalleFacturaSeleccionada()
        {
            try
            {
                if (dgvFactura.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Seleccione una factura.",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                if (!dgvFactura.Columns.Contains("idFactura"))
                {
                    MessageBox.Show(
                        "No se encontró la columna idFactura en el grid.",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                int idFactura = Convert.ToInt32(dgvFactura.CurrentRow.Cells["idFactura"].Value);

                DataTable dtFactura = Db.ExecuteStoredProcedure(
                    "dbo.sp_factura_obtener",
                    new SqlParameter("@idFactura", idFactura)
                );

                DataTable dtDetalle = Db.ExecuteStoredProcedure(
                    "dbo.sp_factura_detalle_listar",
                    new SqlParameter("@idFactura", idFactura)
                );

                if (dtFactura.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontró la factura seleccionada.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                MostrarDialogoFactura(dtFactura.Rows[0], dtDetalle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al mostrar detalle de factura:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void MostrarDialogoFactura(DataRow factura, DataTable detalle)
        {
            Form dlg = new Form();
            dlg.Text = "Detalle de Factura";
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.Size = new Size(950, 680);
            dlg.MinimizeBox = false;
            dlg.MaximizeBox = false;
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlg.BackColor = Color.White;

            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 170,
                BackColor = Color.White,
                Padding = new Padding(25, 15, 25, 10)
            };

            Label lblTitulo = new Label
            {
                Text = "FACTURA",
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblNumeroFactura = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(30, 60),
                Text = "Número factura: " + factura["numeroFactura"].ToString()
            };

            Label lblFechaFactura = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(30, 92),
                Text = "Fecha factura: " + Convert.ToDateTime(factura["fechaFactura"]).ToString("dd/MM/yyyy HH:mm")
            };

            Label lblCliente = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(30, 124),
                Text = "Cliente: " + factura["nombreCliente"].ToString()
            };

            Label lblRTN = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(500, 60),
                Text = "RTN: " + factura["rtnCliente"].ToString()
            };

            Label lblIdPago = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(500, 92),
                Text = "ID Pago: " + factura["idPago"].ToString()
            };

            Label lblTotal = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(500, 124),
                Text = "Total factura: L. " + Convert.ToDecimal(factura["totalFactura"]).ToString("N2")
            };

            pnlTop.Controls.Add(lblTitulo);
            pnlTop.Controls.Add(lblNumeroFactura);
            pnlTop.Controls.Add(lblFechaFactura);
            pnlTop.Controls.Add(lblCliente);
            pnlTop.Controls.Add(lblRTN);
            pnlTop.Controls.Add(lblIdPago);
            pnlTop.Controls.Add(lblTotal);

            Panel pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(25, 10, 25, 10)
            };

            DataGridView dgvDetalle = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(220, 220, 220),
                DataSource = detalle
            };

            dgvDetalle.EnableHeadersVisualStyles = false;
            dgvDetalle.ColumnHeadersHeight = 44;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDetalle.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dgvDetalle.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvDetalle.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetalle.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvDetalle.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalle.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvDetalle.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvDetalle.DefaultCellStyle.BackColor = Color.White;
            dgvDetalle.DefaultCellStyle.ForeColor = Color.Black;
            dgvDetalle.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvDetalle.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDetalle.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);

            dgvDetalle.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvDetalle.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvDetalle.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvDetalle.RowTemplate.Height = 34;

            if (dgvDetalle.Columns.Contains("idDetalleFactura"))
                dgvDetalle.Columns["idDetalleFactura"].Visible = false;

            if (dgvDetalle.Columns.Contains("idFactura"))
                dgvDetalle.Columns["idFactura"].Visible = false;

            if (dgvDetalle.Columns.Contains("descripcion"))
            {
                dgvDetalle.Columns["descripcion"].FillWeight = 35;
                dgvDetalle.Columns["descripcion"].MinimumWidth = 180;
            }

            if (dgvDetalle.Columns.Contains("capital"))
            {
                dgvDetalle.Columns["capital"].FillWeight = 20;
                dgvDetalle.Columns["capital"].MinimumWidth = 110;
                dgvDetalle.Columns["capital"].DefaultCellStyle.Format = "N2";
                dgvDetalle.Columns["capital"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvDetalle.Columns.Contains("interes"))
            {
                dgvDetalle.Columns["interes"].FillWeight = 18;
                dgvDetalle.Columns["interes"].MinimumWidth = 100;
                dgvDetalle.Columns["interes"].DefaultCellStyle.Format = "N2";
                dgvDetalle.Columns["interes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvDetalle.Columns.Contains("subtotal"))
            {
                dgvDetalle.Columns["subtotal"].FillWeight = 20;
                dgvDetalle.Columns["subtotal"].MinimumWidth = 120;
                dgvDetalle.Columns["subtotal"].DefaultCellStyle.Format = "N2";
                dgvDetalle.Columns["subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            pnlGridContainer.Controls.Add(dgvDetalle);

            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(25, 10, 25, 10)
            };

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 100,
                Height = 34,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            btnCerrar.Left = pnlBottom.Width - btnCerrar.Width - 10;
            btnCerrar.Top = 10;
            btnCerrar.Click += (s, e) => dlg.Close();

            pnlBottom.Resize += (s, e) =>
            {
                btnCerrar.Left = pnlBottom.Width - btnCerrar.Width - 10;
            };

            pnlBottom.Controls.Add(btnCerrar);

            dlg.Controls.Add(pnlGridContainer);
            dlg.Controls.Add(pnlBottom);
            dlg.Controls.Add(pnlTop);

            dlg.ShowDialog(this);
        }
        private void ConfigurarGrid()
        {
            dgvFactura.AutoGenerateColumns = true;
            dgvFactura.ReadOnly = true;
            dgvFactura.MultiSelect = false;
            dgvFactura.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFactura.AllowUserToAddRows = false;
            dgvFactura.AllowUserToDeleteRows = false;
            dgvFactura.AllowUserToResizeRows = false;
            dgvFactura.RowHeadersVisible = false;
            dgvFactura.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFactura.ColumnHeadersHeight = 52;
            dgvFactura.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvFactura.EnableHeadersVisualStyles = false;
            dgvFactura.BackgroundColor = Color.White;
            dgvFactura.BorderStyle = BorderStyle.None;
            dgvFactura.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvFactura.GridColor = Color.FromArgb(210, 210, 210);

            dgvFactura.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvFactura.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFactura.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvFactura.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvFactura.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvFactura.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFactura.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvFactura.DefaultCellStyle.BackColor = Color.White;
            dgvFactura.DefaultCellStyle.ForeColor = Color.Black;
            dgvFactura.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvFactura.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvFactura.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvFactura.DefaultCellStyle.Padding = new Padding(3);

            dgvFactura.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvFactura.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvFactura.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvFactura.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvFactura.RowTemplate.Height = 32;
        }

        private void ConfigurarColumnasFactura()
        {
            if (dgvFactura.Columns.Contains("idFactura"))
                dgvFactura.Columns["idFactura"].FillWeight = 8;

            if (dgvFactura.Columns.Contains("idPago"))
                dgvFactura.Columns["idPago"].FillWeight = 8;

            if (dgvFactura.Columns.Contains("numeroFactura"))
                dgvFactura.Columns["numeroFactura"].FillWeight = 14;

            if (dgvFactura.Columns.Contains("fechaFactura"))
                dgvFactura.Columns["fechaFactura"].FillWeight = 14;

            if (dgvFactura.Columns.Contains("nombreCliente"))
                dgvFactura.Columns["nombreCliente"].FillWeight = 24;

            if (dgvFactura.Columns.Contains("rtnCliente"))
                dgvFactura.Columns["rtnCliente"].FillWeight = 14;

            if (dgvFactura.Columns.Contains("totalFactura"))
            {
                dgvFactura.Columns["totalFactura"].FillWeight = 12;
                dgvFactura.Columns["totalFactura"].DefaultCellStyle.Format = "N2";
                dgvFactura.Columns["totalFactura"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
    }
}