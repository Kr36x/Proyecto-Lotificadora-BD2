using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaVistaLotes : Form
    {

        private bool _modoDetalle = true;
        public frmConsultaVistaLotes()
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

            dgvVistaLotes.ThemeStyle.BackColor = Color.White;
            dgvVistaLotes.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvVistaLotes.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvVistaLotes.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvVistaLotes.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvVistaLotes.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvVistaLotes.ThemeStyle.HeaderStyle.Height = 52;
            dgvVistaLotes.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvVistaLotes.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvVistaLotes.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaLotes.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvVistaLotes.ThemeStyle.RowsStyle.Height = 32;
            dgvVistaLotes.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvVistaLotes.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvVistaLotes.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void ConsultarLotes()
        {
            try
            {
                DataTable table = new DataTable();

                using SqlConnection connection = new SqlConnection(Db.ConnectionString);

                if (_modoDetalle)
                {
                    string sql = @"SELECT * FROM dbo.vw_lotes_disponibles;";

                    using SqlCommand command = new SqlCommand(sql, connection);
                    using SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
                else
                {
                    using SqlCommand command = new SqlCommand("dbo.sp_listar_lotes_disponibles", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }

                dgvVistaLotes.DataSource = table;
                //ConfigurarColumnasSegunModo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar lotes disponibles:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


    }
}