using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace LotificadoraApp.Empleado
{
    public partial class FrmEmpleado : Form
    {
        private int _idEmpleadoSeleccionado = 0;

        public FrmEmpleado()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += FrmEmpleado_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvCliente.CellClick += dgvCliente_CellClick;
            dgvCliente.SelectionChanged += dgvCliente_SelectionChanged;
        }

        private void FrmEmpleado_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEmpleados();
        }

        private void ConfigurarGrid()
        {
            dgvCliente.AutoGenerateColumns = true;
            dgvCliente.ReadOnly = true;
            dgvCliente.MultiSelect = false;
            dgvCliente.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCliente.AllowUserToAddRows = false;
            dgvCliente.AllowUserToDeleteRows = false;
            dgvCliente.AllowUserToResizeRows = false;
            dgvCliente.RowHeadersVisible = false;
            dgvCliente.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCliente.ColumnHeadersHeight = 40;
            dgvCliente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCliente.EnableHeadersVisualStyles = false;
            dgvCliente.BackgroundColor = Color.White;
            dgvCliente.BorderStyle = BorderStyle.None;
            dgvCliente.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCliente.GridColor = Color.FromArgb(210, 210, 210);

            dgvCliente.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvCliente.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCliente.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvCliente.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvCliente.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCliente.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvCliente.DefaultCellStyle.BackColor = Color.White;
            dgvCliente.DefaultCellStyle.ForeColor = Color.Black;
            dgvCliente.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCliente.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCliente.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvCliente.DefaultCellStyle.Padding = new Padding(3);

            dgvCliente.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvCliente.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvCliente.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCliente.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvCliente.RowTemplate.Height = 32;

            dgvCliente.ThemeStyle.BackColor = Color.White;
            dgvCliente.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvCliente.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvCliente.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvCliente.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCliente.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCliente.ThemeStyle.HeaderStyle.Height = 40;
            dgvCliente.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvCliente.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvCliente.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCliente.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvCliente.ThemeStyle.RowsStyle.Height = 32;
            dgvCliente.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvCliente.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCliente.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void CargarEmpleados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_empleado_listar");
                dgvCliente.DataSource = dt;
                _idEmpleadoSeleccionado = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al obtener empleados: " + ex.Message);
            }
        }

        private void BuscarEmpleados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_empleado_listar");

                string nombre = txtNombre.Text.Trim().Replace("'", "''");

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    DataView view = dt.DefaultView;
                    view.RowFilter = $"nombres LIKE '%{nombre}%' OR apellidos LIKE '%{nombre}%'";
                    dgvCliente.DataSource = view.ToTable();
                }
                else
                {
                    dgvCliente.DataSource = dt;
                }

                _idEmpleadoSeleccionado = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al buscar empleados: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            BuscarEmpleados();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtNombre.Clear();
            CargarEmpleados();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarEmpleados();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            using frmCrearEditarEmpleado frm = new frmCrearEditarEmpleado();

            if (frm.ShowDialog() == DialogResult.OK)
                CargarEmpleados();
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            if (_idEmpleadoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un empleado para editar.");
                return;
            }

            using frmCrearEditarEmpleado frm = new frmCrearEditarEmpleado(_idEmpleadoSeleccionado);

            if (frm.ShowDialog() == DialogResult.OK)
                CargarEmpleados();
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idEmpleadoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un empleado para eliminar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar el empleado seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_empleado_eliminar",
                    new SqlParameter("@id", _idEmpleadoSeleccionado)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Empleado eliminado correctamente.",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarEmpleados();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al eliminar empleado: " + ex.Message);
            }
        }

        private void dgvCliente_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvCliente_SelectionChanged(object? sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvCliente.CurrentRow == null || dgvCliente.CurrentRow.Index < 0)
                return;

            if (dgvCliente.CurrentRow.Cells["id"].Value == null ||
                dgvCliente.CurrentRow.Cells["id"].Value == DBNull.Value)
            {
                _idEmpleadoSeleccionado = 0;
                return;
            }

            _idEmpleadoSeleccionado = Convert.ToInt32(dgvCliente.CurrentRow.Cells["id"].Value);
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