using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaVistaCredito : Form
    {

        private bool _modoDetalle = true;
        public frmConsultaVistaCredito()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmConsultaVistaLotes_Load;
            btnRecargar.Click += btnConsultar_Click;
            btnDetalle.Click += btnDetalle_Click;
            btnResumen.Click += btnResumen_Click;
        }

        private void frmConsultaVistaLotes_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ConsultarLotes();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            ConsultarLotes();
        }
        private void btnDetalle_Click(object? sender, EventArgs e)
        {
            _modoDetalle = true;
            ConsultarLotes();
        }

        private void btnResumen_Click(object? sender, EventArgs e)
        {
            _modoDetalle = false;
            ConsultarLotes();
        }
        private void ConfigurarGrid()
        {
            dgvVistaCreditosActivos.AutoGenerateColumns = true;
            dgvVistaCreditosActivos.ReadOnly = true;
            dgvVistaCreditosActivos.MultiSelect = false;
            dgvVistaCreditosActivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVistaCreditosActivos.AllowUserToAddRows = false;
            dgvVistaCreditosActivos.AllowUserToDeleteRows = false;
            dgvVistaCreditosActivos.AllowUserToResizeRows = false;
            dgvVistaCreditosActivos.RowHeadersVisible = false;
            dgvVistaCreditosActivos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVistaCreditosActivos.ColumnHeadersHeight = 52;
            dgvVistaCreditosActivos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvVistaCreditosActivos.EnableHeadersVisualStyles = false;
            dgvVistaCreditosActivos.BackgroundColor = Color.White;
            dgvVistaCreditosActivos.BorderStyle = BorderStyle.None;
            dgvVistaCreditosActivos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVistaCreditosActivos.GridColor = Color.FromArgb(210, 210, 210);

            dgvVistaCreditosActivos.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvVistaCreditosActivos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVistaCreditosActivos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvVistaCreditosActivos.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvVistaCreditosActivos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvVistaCreditosActivos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVistaCreditosActivos.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvVistaCreditosActivos.DefaultCellStyle.BackColor = Color.White;
            dgvVistaCreditosActivos.DefaultCellStyle.ForeColor = Color.Black;
            dgvVistaCreditosActivos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaCreditosActivos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvVistaCreditosActivos.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvVistaCreditosActivos.DefaultCellStyle.Padding = new Padding(3);

            dgvVistaCreditosActivos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvVistaCreditosActivos.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvVistaCreditosActivos.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaCreditosActivos.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvVistaCreditosActivos.RowTemplate.Height = 32;

            dgvVistaCreditosActivos.ThemeStyle.BackColor = Color.White;
            dgvVistaCreditosActivos.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvVistaCreditosActivos.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvVistaCreditosActivos.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvVistaCreditosActivos.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvVistaCreditosActivos.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvVistaCreditosActivos.ThemeStyle.HeaderStyle.Height = 52;
            dgvVistaCreditosActivos.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvVistaCreditosActivos.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvVistaCreditosActivos.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaCreditosActivos.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvVistaCreditosActivos.ThemeStyle.RowsStyle.Height = 32;
            dgvVistaCreditosActivos.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvVistaCreditosActivos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaCreditosActivos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void ConsultarLotes()
        {
            try
            {
  
                DataTable table = new DataTable();

                using SqlConnection connection = new SqlConnection(Db.ConnectionString);

                if (_modoDetalle)
                {
                    string sql = @"SELECT * FROM dbo.vw_creditos_activos_cliente;";

                    using SqlCommand command = new SqlCommand(sql, connection);
                    using SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
                else
                {
                    using SqlCommand command = new SqlCommand("dbo.sp_resumen_creditos_activos", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }

                dgvVistaCreditosActivos.DataSource = table;
                //ConfigurarColumnasSegunModo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar créditos activos disponibles:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


    }
}