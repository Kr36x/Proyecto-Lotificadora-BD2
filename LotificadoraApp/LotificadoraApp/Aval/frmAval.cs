using System.Data;
using System.Drawing;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp.Aval
{
    public partial class frmAval : Form
    {
        private int _idAvalSeleccionado = 0;

        public frmAval()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmAval_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvAval.CellClick += dgvAval_CellClick;
            dgvAval.SelectionChanged += dgvAval_SelectionChanged;
        }

        private void frmAval_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarAvales();
        }

        private void ConfigurarGrid()
        {
            dgvAval.AutoGenerateColumns = true;
            dgvAval.ReadOnly = true;
            dgvAval.MultiSelect = false;
            dgvAval.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAval.AllowUserToAddRows = false;
            dgvAval.AllowUserToDeleteRows = false;
            dgvAval.AllowUserToResizeRows = false;
            dgvAval.RowHeadersVisible = false;
            dgvAval.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAval.ColumnHeadersHeight = 40;
            dgvAval.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvAval.EnableHeadersVisualStyles = false;
            dgvAval.BackgroundColor = Color.White;
            dgvAval.BorderStyle = BorderStyle.None;
            dgvAval.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAval.GridColor = Color.FromArgb(210, 210, 210);

            dgvAval.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvAval.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAval.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvAval.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvAval.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvAval.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvAval.DefaultCellStyle.BackColor = Color.White;
            dgvAval.DefaultCellStyle.ForeColor = Color.Black;
            dgvAval.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvAval.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvAval.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvAval.DefaultCellStyle.Padding = new Padding(3);

            dgvAval.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvAval.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvAval.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvAval.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvAval.RowTemplate.Height = 32;

            dgvAval.ThemeStyle.BackColor = Color.White;
            dgvAval.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvAval.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvAval.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvAval.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAval.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvAval.ThemeStyle.HeaderStyle.Height = 40;
            dgvAval.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvAval.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvAval.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvAval.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvAval.ThemeStyle.RowsStyle.Height = 32;
            dgvAval.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvAval.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvAval.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void CargarAvales()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_aval_listar");
                dgvAval.DataSource = dt;
                _idAvalSeleccionado = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al obtener avales: " + ex.Message);
            }
        }

        private void BuscarAvales()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_aval_listar");
                string texto = txtNombre.Text.Trim().Replace("'", "''");

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    DataView view = dt.DefaultView;
                    view.RowFilter =
                        $"identidad LIKE '%{texto}%' OR " +
                        $"nombres LIKE '%{texto}%' OR " +
                        $"apellidos LIKE '%{texto}%'";
                    dgvAval.DataSource = view.ToTable();
                }
                else
                {
                    dgvAval.DataSource = dt;
                }

                _idAvalSeleccionado = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al buscar avales: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            BuscarAvales();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtNombre.Clear();
            CargarAvales();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarAvales();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            using frmRegistrarAval frm = new frmRegistrarAval();

            if (frm.ShowDialog() == DialogResult.OK)
                CargarAvales();
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            if (_idAvalSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un aval para editar.");
                return;
            }

            using frmRegistrarAval frm = new frmRegistrarAval(_idAvalSeleccionado);

            if (frm.ShowDialog() == DialogResult.OK)
                CargarAvales();
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idAvalSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un aval para eliminar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar el aval seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_aval_eliminar",
                    new SqlParameter("@idAval", _idAvalSeleccionado)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString()!);
                    return;
                }

                MessageBox.Show(
                    "Aval eliminado correctamente.",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarAvales();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar aval: " + ex.Message);
            }
        }

        private void dgvAval_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvAval_SelectionChanged(object? sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvAval.CurrentRow == null || dgvAval.CurrentRow.Index < 0)
                return;

            if (dgvAval.CurrentRow.Cells["idAval"].Value == null ||
                dgvAval.CurrentRow.Cells["idAval"].Value == DBNull.Value)
            {
                _idAvalSeleccionado = 0;
                return;
            }

            _idAvalSeleccionado = Convert.ToInt32(dgvAval.CurrentRow.Cells["idAval"].Value);
        }

        private static void MostrarMensajeError(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private static void MostrarWarning(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }
}