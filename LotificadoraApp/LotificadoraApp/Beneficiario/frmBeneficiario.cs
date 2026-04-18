using System.Data;
using System.Drawing;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp.Beneficiario
{
    public partial class frmBeneficiario : Form
    {
        private int _idBeneficiarioSeleccionado = 0;

        public frmBeneficiario()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmBeneficiario_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvBeneficiario.CellClick += dgvBeneficiario_CellClick;
            dgvBeneficiario.SelectionChanged += dgvBeneficiario_SelectionChanged;
        }

        private void frmBeneficiario_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarBeneficiarios();
        }

        private void ConfigurarGrid()
        {
            dgvBeneficiario.AutoGenerateColumns = true;
            dgvBeneficiario.ReadOnly = true;
            dgvBeneficiario.MultiSelect = false;
            dgvBeneficiario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBeneficiario.AllowUserToAddRows = false;
            dgvBeneficiario.AllowUserToDeleteRows = false;
            dgvBeneficiario.AllowUserToResizeRows = false;
            dgvBeneficiario.RowHeadersVisible = false;
            dgvBeneficiario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBeneficiario.ColumnHeadersHeight = 40;
            dgvBeneficiario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBeneficiario.EnableHeadersVisualStyles = false;
            dgvBeneficiario.BackgroundColor = Color.White;
            dgvBeneficiario.BorderStyle = BorderStyle.None;
            dgvBeneficiario.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBeneficiario.GridColor = Color.FromArgb(210, 210, 210);

            dgvBeneficiario.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvBeneficiario.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBeneficiario.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvBeneficiario.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvBeneficiario.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvBeneficiario.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvBeneficiario.DefaultCellStyle.BackColor = Color.White;
            dgvBeneficiario.DefaultCellStyle.ForeColor = Color.Black;
            dgvBeneficiario.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBeneficiario.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvBeneficiario.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvBeneficiario.DefaultCellStyle.Padding = new Padding(3);

            dgvBeneficiario.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvBeneficiario.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvBeneficiario.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBeneficiario.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvBeneficiario.RowTemplate.Height = 32;

            dgvBeneficiario.ThemeStyle.BackColor = Color.White;
            dgvBeneficiario.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvBeneficiario.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvBeneficiario.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvBeneficiario.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvBeneficiario.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvBeneficiario.ThemeStyle.HeaderStyle.Height = 40;
            dgvBeneficiario.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvBeneficiario.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvBeneficiario.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBeneficiario.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvBeneficiario.ThemeStyle.RowsStyle.Height = 32;
            dgvBeneficiario.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvBeneficiario.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvBeneficiario.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void CargarBeneficiarios()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_beneficiario_listar");
                dgvBeneficiario.DataSource = dt;
                _idBeneficiarioSeleccionado = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al obtener beneficiarios: " + ex.Message);
            }
        }

        private void BuscarBeneficiarios()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_beneficiario_listar");
                string texto = txtNombre.Text.Trim().Replace("'", "''");

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    DataView view = dt.DefaultView;
                    view.RowFilter =
                        $"identidad LIKE '%{texto}%' OR " +
                        $"nombres LIKE '%{texto}%' OR " +
                        $"apellidos LIKE '%{texto}%'";
                    dgvBeneficiario.DataSource = view.ToTable();
                }
                else
                {
                    dgvBeneficiario.DataSource = dt;
                }

                _idBeneficiarioSeleccionado = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al buscar beneficiarios: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            BuscarBeneficiarios();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtNombre.Clear();
            CargarBeneficiarios();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarBeneficiarios();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            using frmRegistrarBeneficiario frm = new frmRegistrarBeneficiario();

            if (frm.ShowDialog() == DialogResult.OK)
                CargarBeneficiarios();
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            if (_idBeneficiarioSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un beneficiario para editar.");
                return;
            }

            using frmRegistrarBeneficiario frm = new frmRegistrarBeneficiario(_idBeneficiarioSeleccionado);

            if (frm.ShowDialog() == DialogResult.OK)
                CargarBeneficiarios();
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idBeneficiarioSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un beneficiario para eliminar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar el beneficiario seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_beneficiario_eliminar",
                    new SqlParameter("@idBeneficiario", _idBeneficiarioSeleccionado)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString()!);
                    return;
                }

                MessageBox.Show(
                    "Beneficiario eliminado correctamente.",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarBeneficiarios();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar beneficiario: " + ex.Message);
            }
        }

        private void dgvBeneficiario_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvBeneficiario_SelectionChanged(object? sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvBeneficiario.CurrentRow == null || dgvBeneficiario.CurrentRow.Index < 0)
                return;

            if (dgvBeneficiario.CurrentRow.Cells["idBeneficiario"].Value == null ||
                dgvBeneficiario.CurrentRow.Cells["idBeneficiario"].Value == DBNull.Value)
            {
                _idBeneficiarioSeleccionado = 0;
                return;
            }

            _idBeneficiarioSeleccionado = Convert.ToInt32(dgvBeneficiario.CurrentRow.Cells["idBeneficiario"].Value);
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