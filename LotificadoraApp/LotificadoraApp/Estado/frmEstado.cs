using System.Data;
using System.Drawing;

namespace LotificadoraApp.Estado
{
    public partial class frmEstado : Form
    {
        private int _idEstadoSeleccionado = 0;
        private string _nombreEstadoSeleccionado = string.Empty;

        public frmEstado()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmEstado_Load;

            btnActualizar.Click += btnActualizar_Click;
            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvEstado.CellClick += dgvEstado_CellClick;
            dgvEstado.SelectionChanged += dgvEstado_SelectionChanged;
        }

        private void frmEstado_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ObtenerEstados();
        }

        private void ConfigurarGrid()
        {
            dgvEstado.AutoGenerateColumns = true;
            dgvEstado.ReadOnly = true;
            dgvEstado.MultiSelect = false;
            dgvEstado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstado.AllowUserToAddRows = false;
            dgvEstado.AllowUserToDeleteRows = false;
            dgvEstado.AllowUserToResizeRows = false;
            dgvEstado.RowHeadersVisible = false;
            dgvEstado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstado.ColumnHeadersHeight = 40;
            dgvEstado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvEstado.EnableHeadersVisualStyles = false;
            dgvEstado.BackgroundColor = Color.White;
            dgvEstado.BorderStyle = BorderStyle.None;
            dgvEstado.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEstado.GridColor = Color.FromArgb(210, 210, 210);

            dgvEstado.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvEstado.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEstado.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvEstado.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvEstado.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvEstado.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvEstado.DefaultCellStyle.BackColor = Color.White;
            dgvEstado.DefaultCellStyle.ForeColor = Color.Black;
            dgvEstado.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvEstado.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvEstado.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvEstado.DefaultCellStyle.Padding = new Padding(3);

            dgvEstado.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvEstado.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvEstado.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvEstado.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvEstado.RowTemplate.Height = 32;

            dgvEstado.ThemeStyle.BackColor = Color.White;
            dgvEstado.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvEstado.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvEstado.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvEstado.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvEstado.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvEstado.ThemeStyle.HeaderStyle.Height = 40;
            dgvEstado.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvEstado.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvEstado.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvEstado.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvEstado.ThemeStyle.RowsStyle.Height = 32;
            dgvEstado.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvEstado.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvEstado.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void ObtenerEstados()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure("sp_estado_listar");
                dgvEstado.DataSource = dataTable;
                LimpiarSeleccion();
            }
            catch
            {
                MostrarMensajeError("Error al obtener los estados");
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ObtenerEstados();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using frmRegistrarEstado frm = new frmRegistrarEstado();

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerEstados();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idEstadoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un estado para editar");
                return;
            }

            using frmRegistrarEstado frm = new frmRegistrarEstado(
                _idEstadoSeleccionado,
                _nombreEstadoSeleccionado
            );

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerEstados();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idEstadoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un estado para eliminar");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar el estado seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    "sp_estado_eliminar",
                    Db.Parameter("@id", _idEstadoSeleccionado)
                );

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Estado eliminado correctamente",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerEstados();
            }
            catch
            {
                MostrarMensajeError("Ocurrió un error al eliminar");
            }
        }

        private void dgvEstado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvEstado_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvEstado.CurrentRow == null || dgvEstado.CurrentRow.Index < 0)
                return;

            if (dgvEstado.CurrentRow.Cells["id"].Value == null ||
                dgvEstado.CurrentRow.Cells["id"].Value == DBNull.Value)
            {
                LimpiarSeleccion();
                return;
            }

            _idEstadoSeleccionado = Convert.ToInt32(dgvEstado.CurrentRow.Cells["id"].Value);
            _nombreEstadoSeleccionado = dgvEstado.CurrentRow.Cells["nombre"].Value?.ToString() ?? string.Empty;
        }

        private void LimpiarSeleccion()
        {
            _idEstadoSeleccionado = 0;
            _nombreEstadoSeleccionado = string.Empty;
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