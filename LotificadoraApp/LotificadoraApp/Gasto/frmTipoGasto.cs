using System.Data;
using System.Drawing;

namespace LotificadoraApp.Gasto
{
    public partial class frmTipoGasto : Form
    {
        private int _idTipoGastoSeleccionado = 0;
        private string _nombreTipoGastoSeleccionado = string.Empty;
        private int _estadoIdSeleccionado = 0;

        public frmTipoGasto()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmTipoGasto_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvGasto.CellClick += dgvGasto_CellClick;
            dgvGasto.SelectionChanged += dgvGasto_SelectionChanged;
        }

        private void frmTipoGasto_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            ObtenerTiposGasto();
        }

        private void ConfigurarGrid()
        {
            dgvGasto.AutoGenerateColumns = true;
            dgvGasto.ReadOnly = true;
            dgvGasto.MultiSelect = false;
            dgvGasto.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGasto.AllowUserToAddRows = false;
            dgvGasto.AllowUserToDeleteRows = false;
            dgvGasto.AllowUserToResizeRows = false;
            dgvGasto.RowHeadersVisible = false;
            dgvGasto.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGasto.ColumnHeadersHeight = 40;
            dgvGasto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvGasto.EnableHeadersVisualStyles = false;
            dgvGasto.BackgroundColor = Color.White;
            dgvGasto.BorderStyle = BorderStyle.None;
            dgvGasto.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvGasto.GridColor = Color.FromArgb(210, 210, 210);

            dgvGasto.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvGasto.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGasto.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvGasto.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvGasto.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvGasto.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGasto.DefaultCellStyle.BackColor = Color.White;
            dgvGasto.DefaultCellStyle.ForeColor = Color.Black;
            dgvGasto.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGasto.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGasto.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvGasto.DefaultCellStyle.Padding = new Padding(3);

            dgvGasto.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvGasto.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvGasto.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGasto.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvGasto.RowTemplate.Height = 32;

            dgvGasto.ThemeStyle.BackColor = Color.White;
            dgvGasto.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvGasto.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvGasto.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvGasto.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvGasto.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvGasto.ThemeStyle.HeaderStyle.Height = 40;
            dgvGasto.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvGasto.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvGasto.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGasto.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvGasto.ThemeStyle.RowsStyle.Height = 32;
            dgvGasto.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvGasto.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGasto.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_estado_listar");

                DataRow filaTodos = dt.NewRow();
                filaTodos["id"] = 0;
                filaTodos["nombre"] = "Todos";
                dt.Rows.InsertAt(filaTodos, 0);

                cmbEstado.DataSource = dt;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar estados: " + ex.Message);
            }
        }

        private void ObtenerTiposGasto()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_tipo_gasto_listar");
                dgvGasto.DataSource = dt;
                LimpiarSeleccion();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al obtener tipos de gasto: " + ex.Message);
            }
        }

        private void BuscarTiposGasto()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_tipo_gasto_listar");

                if (cmbEstado.SelectedValue != null &&
                    int.TryParse(cmbEstado.SelectedValue.ToString(), out int estadoId) &&
                    estadoId > 0)
                {
                    DataView view = dt.DefaultView;
                    view.RowFilter = $"estadoId = {estadoId}";
                    dgvGasto.DataSource = view.ToTable();
                }
                else
                {
                    dgvGasto.DataSource = dt;
                }

                LimpiarSeleccion();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al buscar tipos de gasto: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            BuscarTiposGasto();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            cmbEstado.SelectedValue = 0;
            ObtenerTiposGasto();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            ObtenerTiposGasto();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            using frmCrearEditarTipoGasto frm = new frmCrearEditarTipoGasto();

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerTiposGasto();
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            if (_idTipoGastoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un tipo de gasto para editar.");
                return;
            }

            using frmCrearEditarTipoGasto frm = new frmCrearEditarTipoGasto(
                _idTipoGastoSeleccionado,
                _nombreTipoGastoSeleccionado,
                _estadoIdSeleccionado
            );

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerTiposGasto();
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idTipoGastoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un tipo de gasto para eliminar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar el tipo de gasto seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_tipo_gasto_eliminar",
                    Db.Parameter("@idTipoGasto", _idTipoGastoSeleccionado)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Tipo de gasto eliminado correctamente.",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerTiposGasto();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar tipo de gasto: " + ex.Message);
            }
        }

        private void dgvGasto_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvGasto_SelectionChanged(object? sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvGasto.CurrentRow == null || dgvGasto.CurrentRow.Index < 0)
                return;

            if (dgvGasto.CurrentRow.Cells["idTipoGasto"].Value == null ||
                dgvGasto.CurrentRow.Cells["idTipoGasto"].Value == DBNull.Value)
            {
                LimpiarSeleccion();
                return;
            }

            _idTipoGastoSeleccionado = Convert.ToInt32(dgvGasto.CurrentRow.Cells["idTipoGasto"].Value);
            _nombreTipoGastoSeleccionado = dgvGasto.CurrentRow.Cells["nombreTipoGasto"].Value?.ToString() ?? string.Empty;
            _estadoIdSeleccionado = dgvGasto.CurrentRow.Cells["estadoId"].Value == DBNull.Value
                ? 0
                : Convert.ToInt32(dgvGasto.CurrentRow.Cells["estadoId"].Value);
        }

        private void LimpiarSeleccion()
        {
            _idTipoGastoSeleccionado = 0;
            _nombreTipoGastoSeleccionado = string.Empty;
            _estadoIdSeleccionado = 0;
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