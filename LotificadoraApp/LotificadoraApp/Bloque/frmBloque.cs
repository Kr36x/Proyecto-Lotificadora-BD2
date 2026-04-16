using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LotificadoraApp.Bloque
{
    public partial class frmBloque : Form
    {
        private int _idBloqueSeleccionado = 0;
        private int _idEtapaSeleccionada = 0;
        private string _nombreBloqueSeleccionado = string.Empty;
        private string _descripcionSeleccionada = string.Empty;

        public frmBloque()
        {
            InitializeComponent();

            Load += frmBloque_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvMain.CellClick += dgvMain_CellClick;
            dgvMain.SelectionChanged += dgvMain_SelectionChanged;

            txtBuscador.KeyDown += txtBuscador_KeyDown;
        }

        private void frmBloque_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarBloques();
        }

        private void ConfigurarGrid()
        {
            dgvMain.AutoGenerateColumns = true;
            dgvMain.ReadOnly = true;
            dgvMain.MultiSelect = false;
            dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMain.AllowUserToAddRows = false;
            dgvMain.AllowUserToDeleteRows = false;
            dgvMain.AllowUserToResizeRows = false;
            dgvMain.RowHeadersVisible = false;
            dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMain.ColumnHeadersHeight = 38;
            dgvMain.BorderStyle = BorderStyle.None;
            dgvMain.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.BackgroundColor = Color.White;
            dgvMain.GridColor = Color.FromArgb(210, 210, 210);

            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMain.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvMain.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvMain.DefaultCellStyle.BackColor = Color.White;
            dgvMain.DefaultCellStyle.ForeColor = Color.Black;
            dgvMain.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMain.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvMain.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvMain.DefaultCellStyle.Padding = new Padding(3);

            dgvMain.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvMain.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvMain.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMain.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvMain.RowsDefaultCellStyle.BackColor = Color.White;
            dgvMain.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvMain.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMain.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvMain.RowTemplate.Height = 32;

            dgvMain.ThemeStyle.BackColor = Color.White;
            dgvMain.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);

            dgvMain.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvMain.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvMain.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvMain.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvMain.ThemeStyle.HeaderStyle.Height = 38;

            dgvMain.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvMain.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvMain.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMain.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvMain.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMain.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvMain.ThemeStyle.RowsStyle.Height = 32;

            dgvMain.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvMain.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Black;
            dgvMain.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMain.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }
        private void CargarBloques(string filtro = "")
        {
            try
            {
                var dt = Db.ExecuteStoredProcedure("sp_bloque_listar");

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string texto = filtro.Trim().ToLower();

                    var filas = dt.AsEnumerable().Where(r =>
                        r["idBloque"].ToString().ToLower().Contains(texto) ||
                        r["idEtapa"].ToString().ToLower().Contains(texto) ||
                        (r["nombreBloque"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["descripcion"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["precioVaraCuadrada"]?.ToString() ?? "").ToLower().Contains(texto)
                    );

                    if (filas.Any())
                        dgvMain.DataSource = filas.CopyToDataTable();
                    else
                        dgvMain.DataSource = dt.Clone();
                }
                else
                {
                    dgvMain.DataSource = dt;
                }

                ConfigurarEncabezados();
                LimpiarSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar bloques: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarEncabezados()
        {
            if (dgvMain.Columns.Contains("idBloque"))
            {
                dgvMain.Columns["idBloque"].HeaderText = "ID Bloque";
                dgvMain.Columns["idBloque"].FillWeight = 10;
                dgvMain.Columns["idBloque"].MinimumWidth = 20;
                dgvMain.Columns["idBloque"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvMain.Columns.Contains("idEtapa"))
            {
                dgvMain.Columns["idEtapa"].HeaderText = "ID Etapa";
                dgvMain.Columns["idEtapa"].FillWeight = 10;
                dgvMain.Columns["idEtapa"].MinimumWidth = 20;
                dgvMain.Columns["idEtapa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvMain.Columns.Contains("nombreBloque"))
            {
                dgvMain.Columns["nombreBloque"].HeaderText = "Nombre del bloque";
                dgvMain.Columns["nombreBloque"].FillWeight = 20;
                dgvMain.Columns["nombreBloque"].MinimumWidth = 140;
            }

            if (dgvMain.Columns.Contains("descripcion"))
            {
                dgvMain.Columns["descripcion"].HeaderText = "Descripción";
                dgvMain.Columns["descripcion"].FillWeight = 35;
                dgvMain.Columns["descripcion"].MinimumWidth = 180;
            }

            if (dgvMain.Columns.Contains("precioVaraCuadrada"))
            {
                dgvMain.Columns["precioVaraCuadrada"].HeaderText = "Precio V²";
                dgvMain.Columns["precioVaraCuadrada"].FillWeight = 15;
                dgvMain.Columns["precioVaraCuadrada"].MinimumWidth = 50;
                dgvMain.Columns["precioVaraCuadrada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["precioVaraCuadrada"].DefaultCellStyle.Format = "N2";
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarBloques(txtBuscador.Text);
        }

        private void txtBuscador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CargarBloques(txtBuscador.Text);
                e.SuppressKeyPress = true;
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCrearEditarBloque())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    CargarBloques(txtBuscador.Text.Trim());
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idBloqueSeleccionado == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar un bloque para editar.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            using (var frm = new frmCrearEditarBloque(
                _idBloqueSeleccionado,
                _idEtapaSeleccionada,
                _nombreBloqueSeleccionado,
                _descripcionSeleccionada))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    CargarBloques(txtBuscador.Text.Trim());
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idBloqueSeleccionado == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar un bloque para eliminar.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Deseas eliminar el bloque '{_nombreBloqueSeleccionado}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                var dt = Db.ExecuteStoredProcedure(
                    "sp_bloque_eliminar",
                    new SqlParameter("@idBloque", _idBloqueSeleccionado)
                );

                ValidarRespuestaError(dt);

                MessageBox.Show(
                    "Bloque eliminado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarBloques(txtBuscador.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar bloque: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerDatosSeleccionados();
        }

        private void dgvMain_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerDatosSeleccionados();
        }

        private void ObtenerDatosSeleccionados()
        {
            if (dgvMain.CurrentRow == null || dgvMain.CurrentRow.Index < 0)
                return;

            DataGridViewRow fila = dgvMain.CurrentRow;

            _idBloqueSeleccionado = fila.Cells["idBloque"].Value == DBNull.Value
                ? 0
                : Convert.ToInt32(fila.Cells["idBloque"].Value);

            _idEtapaSeleccionada = fila.Cells["idEtapa"].Value == DBNull.Value
                ? 0
                : Convert.ToInt32(fila.Cells["idEtapa"].Value);

            _nombreBloqueSeleccionado = fila.Cells["nombreBloque"].Value?.ToString() ?? string.Empty;
            _descripcionSeleccionada = fila.Cells["descripcion"].Value?.ToString() ?? string.Empty;
        }

        private void LimpiarSeleccion()
        {
            _idBloqueSeleccionado = 0;
            _idEtapaSeleccionada = 0;
            _nombreBloqueSeleccionado = string.Empty;
            _descripcionSeleccionada = string.Empty;
        }

        private void ValidarRespuestaError(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0)
                return;

            bool tieneMensajeError = dt.Columns
                .Cast<DataColumn>()
                .Any(c => c.ColumnName.Equals("MensajeError", StringComparison.OrdinalIgnoreCase));

            if (tieneMensajeError)
            {
                string mensaje = dt.Rows[0]["MensajeError"]?.ToString() ?? "Ocurrió un error.";
                throw new Exception(mensaje);
            }
        }

        private void pnlCRUD_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}