using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Banco
{
    public partial class frmBanco : Form
    {
        private int _idBancoSeleccionado = 0;
        private string _nombreBancoSeleccionado = string.Empty;
        private string _estadoSeleccionado = string.Empty;

        public frmBanco()
        {
            InitializeComponent();

            Load += frmBanco_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvBanco.CellClick += dgvBanco_CellClick;
            dgvBanco.SelectionChanged += dgvBanco_SelectionChanged;
        }

        private void frmBanco_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            ObtenerBancos();
        }

        private void ConfigurarGrid()
        {
            dgvBanco.AutoGenerateColumns = true;
            dgvBanco.ReadOnly = true;
            dgvBanco.MultiSelect = false;
            dgvBanco.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBanco.AllowUserToAddRows = false;
            dgvBanco.AllowUserToDeleteRows = false;
            dgvBanco.AllowUserToResizeRows = false;
            dgvBanco.RowHeadersVisible = false;
            dgvBanco.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBanco.ColumnHeadersHeight = 38;
            dgvBanco.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBanco.EnableHeadersVisualStyles = false;
            dgvBanco.BackgroundColor = Color.White;
            dgvBanco.BorderStyle = BorderStyle.None;
            dgvBanco.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBanco.GridColor = Color.FromArgb(210, 210, 210);

            dgvBanco.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvBanco.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBanco.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvBanco.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvBanco.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvBanco.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBanco.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvBanco.DefaultCellStyle.BackColor = Color.White;
            dgvBanco.DefaultCellStyle.ForeColor = Color.Black;
            dgvBanco.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBanco.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvBanco.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvBanco.DefaultCellStyle.Padding = new Padding(3);

            dgvBanco.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvBanco.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvBanco.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBanco.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvBanco.RowTemplate.Height = 32;

            dgvBanco.ThemeStyle.BackColor = Color.White;
            dgvBanco.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvBanco.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvBanco.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvBanco.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvBanco.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvBanco.ThemeStyle.HeaderStyle.Height = 38;

            dgvBanco.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvBanco.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvBanco.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBanco.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvBanco.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBanco.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgvBanco.ThemeStyle.RowsStyle.Height = 32;

            dgvBanco.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvBanco.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Black;
            dgvBanco.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBanco.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    BancoQueries.QR001,
                    new SqlParameter("@Ids", "1,2")
                );

                cmbEstado.DataSource = dt;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedIndex = -1;
            }
            catch
            {
                MostrarMensajeError("Error al cargar los estados.");
            }
        }

        private void ObtenerBancos()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_banco_listar");
                dgvBanco.DataSource = dt;
                ConfigurarEncabezados();
                LimpiarSeleccion();
            }
            catch
            {
                MostrarMensajeError("Error al obtener los bancos.");
            }
        }

        private void BuscarBancos(int estadoId)
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_banco_obtener",
                    new SqlParameter("@estadoId", estadoId)
                );

                dgvBanco.DataSource = dt;
                ConfigurarEncabezados();
                LimpiarSeleccion();
            }
            catch
            {
                MostrarMensajeError("Error al buscar los bancos.");
            }
        }

        private void ConfigurarEncabezados()
        {
            if (dgvBanco.Columns.Contains("id"))
            {
                //dgvBanco.Columns["id"].HeaderText = "ID";
                dgvBanco.Columns["id"].FillWeight = 15;
                dgvBanco.Columns["id"].MinimumWidth = 60;
                dgvBanco.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvBanco.Columns.Contains("nombreBanco"))
            {
                //dgvBanco.Columns["nombreBanco"].HeaderText = "Nombre del banco";
                dgvBanco.Columns["nombreBanco"].FillWeight = 55;
                dgvBanco.Columns["nombreBanco"].MinimumWidth = 180;
            }

            if (dgvBanco.Columns.Contains("estado"))
            {
                //dgvBanco.Columns["estado"].HeaderText = "Estado";
                dgvBanco.Columns["estado"].FillWeight = 30;
                dgvBanco.Columns["estado"].MinimumWidth = 100;
                dgvBanco.Columns["estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (cmbEstado.SelectedValue == null || cmbEstado.SelectedIndex < 0)
            {
                MostrarWarning("El filtro estado es requerido.");
                return;
            }

            int estadoId = Convert.ToInt32(cmbEstado.SelectedValue);

            if (estadoId <= 0)
            {
                MostrarWarning("El filtro estado es requerido.");
                return;
            }

            BuscarBancos(estadoId);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFiltro();
        }

        private void LimpiarFiltro()
        {
            cmbEstado.SelectedIndex = -1;
            ObtenerBancos();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using frmRegistrarBanco frm = new frmRegistrarBanco();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ObtenerBancos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idBancoSeleccionado == 0)
            {
                MostrarWarning("Debes seleccionar un banco para editar.");
                return;
            }

            using frmRegistrarBanco frm = new frmRegistrarBanco(_idBancoSeleccionado, _nombreBancoSeleccionado, _estadoSeleccionado);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ObtenerBancos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idBancoSeleccionado == 0)
            {
                MostrarWarning("Debes seleccionar un banco para eliminar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Deseas eliminar el banco '{_nombreBancoSeleccionado}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_banco_eliminar",
                    new SqlParameter("@idBanco", _idBancoSeleccionado)
                );

                ValidarRespuestaError(dt);

                MessageBox.Show(
                    "Banco eliminado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerBancos();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar el banco: " + ex.Message);
            }
        }

        private void dgvBanco_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvBanco_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvBanco.CurrentRow == null || dgvBanco.CurrentRow.Index < 0)
                return;

            DataGridViewRow fila = dgvBanco.CurrentRow;

            _idBancoSeleccionado = fila.Cells["id"].Value == DBNull.Value
                ? 0
                : Convert.ToInt32(fila.Cells["id"].Value);

            _nombreBancoSeleccionado = fila.Cells["nombreBanco"].Value?.ToString() ?? string.Empty;
            _estadoSeleccionado = fila.Cells["estado"].Value?.ToString() ?? string.Empty;
        }

        private void LimpiarSeleccion()
        {
            _idBancoSeleccionado = 0;
            _nombreBancoSeleccionado = string.Empty;
            _estadoSeleccionado = string.Empty;
        }

        private void ValidarRespuestaError(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0)
                return;

            if (dt.Columns.Contains("MensajeError") &&
                dt.Rows[0]["MensajeError"] != DBNull.Value)
            {
                throw new Exception(dt.Rows[0]["MensajeError"].ToString());
            }
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

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ObtenerBancos();
        }
    }
}