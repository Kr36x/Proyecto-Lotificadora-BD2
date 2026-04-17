using System.Data;
using System.Drawing;

namespace LotificadoraApp.CuentaBancaria
{
    public partial class frmCuentaBancaria : Form
    {
        private int _idCuentaBancariaSeleccionada = 0;

        public frmCuentaBancaria()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmCuentaBancaria_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvCuentaBancaria.CellClick += dgvCuentaBancaria_CellClick;
            dgvCuentaBancaria.SelectionChanged += dgvCuentaBancaria_SelectionChanged;
        }

        private void frmCuentaBancaria_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            ObtenerCuentasBancarias();
        }

        private void ConfigurarGrid()
        {
            dgvCuentaBancaria.AutoGenerateColumns = true;
            dgvCuentaBancaria.ReadOnly = true;
            dgvCuentaBancaria.MultiSelect = false;
            dgvCuentaBancaria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCuentaBancaria.AllowUserToAddRows = false;
            dgvCuentaBancaria.AllowUserToDeleteRows = false;
            dgvCuentaBancaria.AllowUserToResizeRows = false;
            dgvCuentaBancaria.RowHeadersVisible = false;
            dgvCuentaBancaria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCuentaBancaria.ColumnHeadersHeight = 40;
            dgvCuentaBancaria.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCuentaBancaria.EnableHeadersVisualStyles = false;
            dgvCuentaBancaria.BackgroundColor = Color.White;
            dgvCuentaBancaria.BorderStyle = BorderStyle.None;
            dgvCuentaBancaria.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCuentaBancaria.GridColor = Color.FromArgb(210, 210, 210);

            dgvCuentaBancaria.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvCuentaBancaria.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCuentaBancaria.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvCuentaBancaria.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvCuentaBancaria.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCuentaBancaria.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvCuentaBancaria.DefaultCellStyle.BackColor = Color.White;
            dgvCuentaBancaria.DefaultCellStyle.ForeColor = Color.Black;
            dgvCuentaBancaria.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCuentaBancaria.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCuentaBancaria.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvCuentaBancaria.DefaultCellStyle.Padding = new Padding(3);

            dgvCuentaBancaria.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvCuentaBancaria.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvCuentaBancaria.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCuentaBancaria.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvCuentaBancaria.RowTemplate.Height = 32;

            dgvCuentaBancaria.ThemeStyle.BackColor = Color.White;
            dgvCuentaBancaria.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvCuentaBancaria.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvCuentaBancaria.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvCuentaBancaria.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCuentaBancaria.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCuentaBancaria.ThemeStyle.HeaderStyle.Height = 40;
            dgvCuentaBancaria.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvCuentaBancaria.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvCuentaBancaria.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCuentaBancaria.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvCuentaBancaria.ThemeStyle.RowsStyle.Height = 32;
            dgvCuentaBancaria.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvCuentaBancaria.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCuentaBancaria.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
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

        private void ObtenerCuentasBancarias()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_cuenta_bancaria_listar");
                dgvCuentaBancaria.DataSource = dt;
                LimpiarSeleccion();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al obtener cuentas bancarias: " + ex.Message);
            }
        }

        private void BuscarCuentasBancarias()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_cuenta_bancaria_listar");

                if (cmbEstado.SelectedValue != null &&
                    int.TryParse(cmbEstado.SelectedValue.ToString(), out int estadoId) &&
                    estadoId > 0)
                {
                    DataView view = dt.DefaultView;
                    view.RowFilter = $"estadoId = {estadoId}";
                    dgvCuentaBancaria.DataSource = view.ToTable();
                }
                else
                {
                    dgvCuentaBancaria.DataSource = dt;
                }

                LimpiarSeleccion();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al buscar cuentas bancarias: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            BuscarCuentasBancarias();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            cmbEstado.SelectedValue = 0;
            ObtenerCuentasBancarias();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            ObtenerCuentasBancarias();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            using frmCrearEditarCuentaBancaria frm = new frmCrearEditarCuentaBancaria();

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerCuentasBancarias();
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            if (_idCuentaBancariaSeleccionada <= 0)
            {
                MostrarWarning("Seleccione una cuenta bancaria para editar.");
                return;
            }

            using frmCrearEditarCuentaBancaria frm = new frmCrearEditarCuentaBancaria(_idCuentaBancariaSeleccionada);

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerCuentasBancarias();
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idCuentaBancariaSeleccionada <= 0)
            {
                MostrarWarning("Seleccione una cuenta bancaria para eliminar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar la cuenta bancaria seleccionada?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_cuenta_bancaria_eliminar",
                    Db.Parameter("@idCuentaBancaria", _idCuentaBancariaSeleccionada)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Cuenta bancaria eliminada correctamente.",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerCuentasBancarias();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar cuenta bancaria: " + ex.Message);
            }
        }

        private void dgvCuentaBancaria_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvCuentaBancaria_SelectionChanged(object? sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvCuentaBancaria.CurrentRow == null || dgvCuentaBancaria.CurrentRow.Index < 0)
                return;

            if (dgvCuentaBancaria.CurrentRow.Cells["idCuentaBancaria"].Value == null ||
                dgvCuentaBancaria.CurrentRow.Cells["idCuentaBancaria"].Value == DBNull.Value)
            {
                LimpiarSeleccion();
                return;
            }

            _idCuentaBancariaSeleccionada = Convert.ToInt32(
                dgvCuentaBancaria.CurrentRow.Cells["idCuentaBancaria"].Value
            );
        }

        private void LimpiarSeleccion()
        {
            _idCuentaBancariaSeleccionada = 0;
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