using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;

namespace LotificadoraApp.Lote
{
    public partial class frmLote : Form
    {
        private int _idLoteSeleccionado = 0;

        public frmLote()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmLote_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvLote.CellClick += dgvLote_CellClick;
            dgvLote.SelectionChanged += dgvLote_SelectionChanged;
            txtNumeroLote.KeyDown += txtNumeroLote_KeyDown;
        }

        private void frmLote_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            ObtenerLotes();
        }

        private void ConfigurarGrid()
        {
            dgvLote.AutoGenerateColumns = true;
            dgvLote.ReadOnly = true;
            dgvLote.MultiSelect = false;
            dgvLote.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLote.AllowUserToAddRows = false;
            dgvLote.AllowUserToDeleteRows = false;
            dgvLote.AllowUserToResizeRows = false;
            dgvLote.RowHeadersVisible = false;
            dgvLote.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLote.ColumnHeadersHeight = 52;
            dgvLote.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLote.EnableHeadersVisualStyles = false;
            dgvLote.BackgroundColor = Color.White;
            dgvLote.BorderStyle = BorderStyle.None;
            dgvLote.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLote.GridColor = Color.FromArgb(210, 210, 210);

            dgvLote.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvLote.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLote.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvLote.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvLote.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvLote.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLote.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvLote.DefaultCellStyle.BackColor = Color.White;
            dgvLote.DefaultCellStyle.ForeColor = Color.Black;
            dgvLote.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvLote.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLote.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvLote.DefaultCellStyle.Padding = new Padding(3);

            dgvLote.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvLote.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvLote.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvLote.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvLote.RowTemplate.Height = 32;

            dgvLote.ThemeStyle.BackColor = Color.White;
            dgvLote.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvLote.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvLote.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvLote.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLote.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvLote.ThemeStyle.HeaderStyle.Height = 52;
            dgvLote.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvLote.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvLote.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvLote.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvLote.ThemeStyle.RowsStyle.Height = 32;
            dgvLote.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvLote.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvLote.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteQuery(@"
                    SELECT id, nombre
                    FROM Estado
                    WHERE id IN (7, 8, 9)
                    ORDER BY id;
                ");

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
                MostrarMensajeError("Ocurrió un error al cargar los estados: " + ex.Message);
            }
        }

        private void ObtenerLotes()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_lote_listar_gestion");

                dgvLote.DataSource = dt;
                ConfigurarEncabezados();
                LimpiarSeleccion();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Ocurrió un error al obtener los lotes: " + ex.Message);
            }
        }

        private void BuscarLotes()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_lote_buscar",
                    Db.Parameter("@numeroLote", txtNumeroLote.Text.Trim()),
                    Db.Parameter("@estadoId", cmbEstado.SelectedValue == null ? 0 : Convert.ToInt32(cmbEstado.SelectedValue))
                );

                dgvLote.DataSource = dt;
                ConfigurarEncabezados();
                LimpiarSeleccion();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Ocurrió un error al consultar los lotes: " + ex.Message);
            }
        }

        private void ConfigurarEncabezados()
        {
            if (dgvLote.Columns.Contains("idLote"))
                dgvLote.Columns["idLote"].Width = 50;
                 dgvLote.Columns["idLote"].Visible = true;

            if (dgvLote.Columns.Contains("idBloque"))
                dgvLote.Columns["idBloque"].Width = 50;
            dgvLote.Columns["idBloque"].Visible = true;

            if (dgvLote.Columns.Contains("estadoId"))
                dgvLote.Columns["estadoId"].Width = 50;
            dgvLote.Columns["estadoId"].Visible = true;

            if (dgvLote.Columns.Contains("NumeroLote"))
            {
                //dgvLote.Columns["NumeroLote"].HeaderText = "N° lote";
                dgvLote.Columns["NumeroLote"].FillWeight = 8;
                dgvLote.Columns["NumeroLote"].MinimumWidth = 80;
                dgvLote.Columns["NumeroLote"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvLote.Columns.Contains("AreaV2"))
            {
                //dgvLote.Columns["AreaV2"].HeaderText = "Área\nV2";
                dgvLote.Columns["AreaV2"].FillWeight = 8;
                dgvLote.Columns["AreaV2"].MinimumWidth = 85;
                dgvLote.Columns["AreaV2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvLote.Columns["AreaV2"].DefaultCellStyle.Format = "N2";
            }

            if (dgvLote.Columns.Contains("Bloque"))
            {
                //dgvLote.Columns["Bloque"].HeaderText = "Bloque";
                dgvLote.Columns["Bloque"].FillWeight = 12;
                dgvLote.Columns["Bloque"].MinimumWidth = 100;
            }

            if (dgvLote.Columns.Contains("¿Es Esquina?"))
            {
                dgvLote.Columns["¿Es Esquina?"].FillWeight = 8;
                dgvLote.Columns["¿Es Esquina?"].MinimumWidth = 80;
                dgvLote.Columns["¿Es Esquina?"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvLote.Columns.Contains("¿Está cerca del parque?"))
            {
                dgvLote.Columns["¿Está cerca del parque?"].FillWeight = 11;
                dgvLote.Columns["¿Está cerca del parque?"].MinimumWidth = 110;
                dgvLote.Columns["¿Está cerca del parque?"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvLote.Columns.Contains("¿Es calle cerrada?"))
            {
                dgvLote.Columns["¿Es calle cerrada?"].FillWeight = 10;
                dgvLote.Columns["¿Es calle cerrada?"].MinimumWidth = 100;
                dgvLote.Columns["¿Es calle cerrada?"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvLote.Columns.Contains("PrecioBase"))
            {
                //dgvLote.Columns["PrecioBase"].HeaderText = "Precio\nbase";
                dgvLote.Columns["PrecioBase"].FillWeight = 9;
                dgvLote.Columns["PrecioBase"].MinimumWidth = 95;
                dgvLote.Columns["PrecioBase"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvLote.Columns["PrecioBase"].DefaultCellStyle.Format = "N2";
            }

            if (dgvLote.Columns.Contains("RecargoTotal"))
            {
                //dgvLote.Columns["RecargoTotal"].HeaderText = "Recargo\ntotal";
                dgvLote.Columns["RecargoTotal"].FillWeight = 9;
                dgvLote.Columns["RecargoTotal"].MinimumWidth = 95;
                dgvLote.Columns["RecargoTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvLote.Columns["RecargoTotal"].DefaultCellStyle.Format = "N2";
            }

            if (dgvLote.Columns.Contains("PrecioFinal"))
            {
               // dgvLote.Columns["PrecioFinal"].HeaderText = "Precio\nfinal";
                dgvLote.Columns["PrecioFinal"].FillWeight = 9;
                dgvLote.Columns["PrecioFinal"].MinimumWidth = 95;
                dgvLote.Columns["PrecioFinal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvLote.Columns["PrecioFinal"].DefaultCellStyle.Format = "N2";
            }

            if (dgvLote.Columns.Contains("Estado"))
            {
               // dgvLote.Columns["Estado"].HeaderText = "Estado";
                dgvLote.Columns["Estado"].FillWeight = 7;
                dgvLote.Columns["Estado"].MinimumWidth = 80;
                dgvLote.Columns["Estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarLotes();
        }

        private void txtNumeroLote_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarLotes();
                e.SuppressKeyPress = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNumeroLote.Clear();
            cmbEstado.SelectedValue = 0;
            ObtenerLotes();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ObtenerLotes();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using frmRegistrarLote frm = new frmRegistrarLote();

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerLotes();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idLoteSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un lote para editar.");
                return;
            }

            using frmRegistrarLote frm = new frmRegistrarLote(_idLoteSeleccionado);

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerLotes();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idLoteSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un lote para eliminar.");
                return;
            }

            DialogResult r = MessageBox.Show(
                "¿Desea eliminar el lote seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (r != DialogResult.Yes)
                return;

            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_lote_eliminar",
                    Db.Parameter("@idLote", _idLoteSeleccionado)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Lote eliminado correctamente.",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerLotes();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Ocurrió un error al eliminar el lote: " + ex.Message);
            }
        }

        private void dgvLote_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvLote_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvLote.CurrentRow == null || dgvLote.CurrentRow.Index < 0)
                return;

            if (dgvLote.CurrentRow.Cells["idLote"].Value == null ||
                dgvLote.CurrentRow.Cells["idLote"].Value == DBNull.Value)
            {
                _idLoteSeleccionado = 0;
                return;
            }

            _idLoteSeleccionado = Convert.ToInt32(dgvLote.CurrentRow.Cells["idLote"].Value);
        }

        private void LimpiarSeleccion()
        {
            _idLoteSeleccionado = 0;
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