using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace LotificadoraApp.Proyecto
{
    public partial class frmProyecto : Form
    {
        private int _idProyectoSeleccionado = 0;
        private string _nombreProyectoSeleccionado = string.Empty;
        private string _descripcionSeleccionada = string.Empty;
        private DateTime _fechaInicioSeleccionada = DateTime.Today;
        private DateTime? _fechaFinEstimadaSeleccionada = null;
        private decimal _areaTotalSeleccionada = 0;
        private int _maxAniosFinanciamientoSeleccionado = 0;
        private string _estadoSeleccionado = string.Empty;

        public frmProyecto()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmProyecto_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvProyecto.CellClick += dgvProyecto_CellClick;
            dgvProyecto.SelectionChanged += dgvProyecto_SelectionChanged;
        }

        private void frmProyecto_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEstados();
            ObtenerProyectos();
        }

        private void ConfigurarGrid()
        {
           

            dgvProyecto.AutoGenerateColumns = true;
            dgvProyecto.ReadOnly = true;
            dgvProyecto.MultiSelect = false;
            dgvProyecto.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProyecto.AllowUserToAddRows = false;
            dgvProyecto.AllowUserToDeleteRows = false;
            dgvProyecto.AllowUserToResizeRows = false;
            dgvProyecto.RowHeadersVisible = false;
            dgvProyecto.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProyecto.ColumnHeadersHeight = 45;
            dgvProyecto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProyecto.EnableHeadersVisualStyles = false;
            dgvProyecto.BackgroundColor = Color.White;
            dgvProyecto.BorderStyle = BorderStyle.None;
            dgvProyecto.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProyecto.GridColor = Color.FromArgb(210, 210, 210);

            dgvProyecto.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvProyecto.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProyecto.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvProyecto.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvProyecto.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProyecto.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProyecto.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvProyecto.DefaultCellStyle.BackColor = Color.White;
            dgvProyecto.DefaultCellStyle.ForeColor = Color.Black;
            dgvProyecto.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvProyecto.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvProyecto.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvProyecto.DefaultCellStyle.Padding = new Padding(3);

            dgvProyecto.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvProyecto.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvProyecto.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvProyecto.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvProyecto.RowTemplate.Height = 32;

            dgvProyecto.ThemeStyle.BackColor = Color.White;
            dgvProyecto.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvProyecto.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvProyecto.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvProyecto.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProyecto.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProyecto.ThemeStyle.HeaderStyle.Height = 45;
            dgvProyecto.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvProyecto.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvProyecto.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvProyecto.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvProyecto.ThemeStyle.RowsStyle.Height = 32;
            dgvProyecto.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvProyecto.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvProyecto.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;

            if (dgvProyecto.Columns.Contains("idProyecto"))
            {
                //dgvCliente.Columns["idCliente"].HeaderText = "ID";
                dgvProyecto.Columns["idProyecto"].FillWeight = 6;
                dgvProyecto.Columns["idProyecto"].MinimumWidth = 60;
                dgvProyecto.Columns["idProyecto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvProyecto.Columns.Contains("estado"))
            {
                //dgvCliente.Columns["idCliente"].HeaderText = "ID";
                dgvProyecto.Columns["estado"].FillWeight = 6;
                dgvProyecto.Columns["estado"].MinimumWidth = 80;
                dgvProyecto.Columns["estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvProyecto.Columns.Contains("nombreProyecto"))
            {
                //dgvCliente.Columns["idCliente"].HeaderText = "ID";
                dgvProyecto.Columns["nombreProyecto"].FillWeight = 20;
                dgvProyecto.Columns["nombreProyecto"].MinimumWidth = 200;
                dgvProyecto.Columns["nombreProyecto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvProyecto.Columns.Contains("descripcion"))
            {
                //dgvCliente.Columns["idCliente"].HeaderText = "ID";
                dgvProyecto.Columns["descripcion"].FillWeight = 30;
                dgvProyecto.Columns["descripcion"].MinimumWidth = 300;
                dgvProyecto.Columns["descripcion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dtEstados = Db.ExecuteStoredProcedure(
                    ProyectoQueries.QR001,
                    new SqlParameter("@Ids", "1,2,3")
                );

                DataRow filaTodos = dtEstados.NewRow();
                filaTodos["id"] = 0;
                filaTodos["nombre"] = "Todos";
                dtEstados.Rows.InsertAt(filaTodos, 0);

                cmbEstado.DataSource = dtEstados;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = 0;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los estados");
            }
        }

        private void ObtenerProyectos()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure("sp_proyecto_listar");
                dgvProyecto.DataSource = dataTable;
                LimpiarSeleccion();
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al obtener los proyectos");
            }
        }

        private void BuscarProyectos()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure("sp_proyecto_listar");
                DataView view = dataTable.DefaultView;

                List<string> filtros = new List<string>();

                string nombreProyecto = txtNombreProyecto.Text.Trim().Replace("'", "''");
                if (!string.IsNullOrWhiteSpace(nombreProyecto))
                {
                    filtros.Add($"nombreProyecto LIKE '%{nombreProyecto}%'");
                }

                if (cmbEstado.SelectedValue != null &&
                    int.TryParse(cmbEstado.SelectedValue.ToString(), out int estadoId) &&
                    estadoId > 0)
                {
                    if (cmbEstado.SelectedItem is DataRowView item)
                    {
                        string estadoNombre = item["nombre"]?.ToString()?.Replace("'", "''") ?? "";
                        filtros.Add($"estado = '{estadoNombre}'");
                    }
                }

                view.RowFilter = string.Join(" AND ", filtros);
                dgvProyecto.DataSource = view.ToTable();
                LimpiarSeleccion();
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al buscar los proyectos");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarProyectos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombreProyecto.Clear();
            cmbEstado.SelectedValue = 0;
            ObtenerProyectos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ObtenerProyectos();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using frmRegistrarProyecto frm = new frmRegistrarProyecto();

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerProyectos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idProyectoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un proyecto para editar");
                return;
            }

            using frmRegistrarProyecto frm = new frmRegistrarProyecto(
                _idProyectoSeleccionado,
                _nombreProyectoSeleccionado,
                _descripcionSeleccionada,
                _fechaInicioSeleccionada,
                _fechaFinEstimadaSeleccionada,
                _areaTotalSeleccionada,
                _maxAniosFinanciamientoSeleccionado,
                _estadoSeleccionado
            );

            if (frm.ShowDialog() == DialogResult.OK)
                ObtenerProyectos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idProyectoSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un proyecto para eliminar");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar el proyecto seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    "sp_proyecto_eliminar",
                    new SqlParameter("@idProyecto", _idProyectoSeleccionado)
                );

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Proyecto eliminado correctamente",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerProyectos();
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al eliminar el proyecto");
            }
        }

        private void dgvProyecto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvProyecto_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvProyecto.CurrentRow == null || dgvProyecto.CurrentRow.Index < 0)
                return;

            DataGridViewRow fila = dgvProyecto.CurrentRow;

            if (fila.Cells["idProyecto"].Value == null || fila.Cells["idProyecto"].Value == DBNull.Value)
            {
                LimpiarSeleccion();
                return;
            }

            _idProyectoSeleccionado = Convert.ToInt32(fila.Cells["idProyecto"].Value);
            _nombreProyectoSeleccionado = fila.Cells["nombreProyecto"].Value?.ToString() ?? "";
            _descripcionSeleccionada = fila.Cells["descripcion"].Value?.ToString() ?? "";
            _fechaInicioSeleccionada = fila.Cells["fechaInicio"].Value == DBNull.Value
                ? DateTime.Today
                : Convert.ToDateTime(fila.Cells["fechaInicio"].Value);

            _fechaFinEstimadaSeleccionada = fila.Cells["fechaFinEstimada"].Value == DBNull.Value
                ? null
                : Convert.ToDateTime(fila.Cells["fechaFinEstimada"].Value);

            _areaTotalSeleccionada = fila.Cells["areaTotalV2"].Value == DBNull.Value
                ? 0
                : Convert.ToDecimal(fila.Cells["areaTotalV2"].Value);

            _maxAniosFinanciamientoSeleccionado = fila.Cells["maxAniosFinanciamiento"].Value == DBNull.Value
                ? 0
                : Convert.ToInt32(fila.Cells["maxAniosFinanciamiento"].Value);

            _estadoSeleccionado = fila.Cells["estado"].Value?.ToString() ?? "";
        }

        private void LimpiarSeleccion()
        {
            _idProyectoSeleccionado = 0;
            _nombreProyectoSeleccionado = string.Empty;
            _descripcionSeleccionada = string.Empty;
            _fechaInicioSeleccionada = DateTime.Today;
            _fechaFinEstimadaSeleccionada = null;
            _areaTotalSeleccionada = 0;
            _maxAniosFinanciamientoSeleccionado = 0;
            _estadoSeleccionado = string.Empty;
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