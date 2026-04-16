using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmGestionClientes : Form
    {
        private int _idClienteSeleccionado = 0;

        public frmGestionClientes()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            ConfigurarGrid();
            CargarEstadosFiltro();
        }

        private void ConectarEventos()
        {
            Load += frmGestionClientes_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvCliente.CellClick += dgvCliente_CellClick;
            dgvCliente.SelectionChanged += dgvCliente_SelectionChanged;
            dgvCliente.CellDoubleClick += dgvCliente_CellDoubleClick;
        }

        private void frmGestionClientes_Load(object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void ConfigurarGrid()
        {
            dgvCliente.AutoGenerateColumns = true;
            dgvCliente.ReadOnly = true;
            dgvCliente.AllowUserToAddRows = false;
            dgvCliente.AllowUserToDeleteRows = false;
            dgvCliente.AllowUserToResizeRows = false;
            dgvCliente.MultiSelect = false;
            dgvCliente.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCliente.RowHeadersVisible = false;
            dgvCliente.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCliente.ColumnHeadersHeight = 45;
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
            dgvCliente.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

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
            dgvCliente.ThemeStyle.HeaderStyle.Height = 45;
            dgvCliente.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvCliente.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvCliente.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCliente.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvCliente.ThemeStyle.RowsStyle.Height = 32;
            dgvCliente.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvCliente.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvCliente.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void CargarEstadosFiltro()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_estado_obtener_por_ids",
                    Db.Parameter("@Ids", "1,2")
                );

                if (dt.Rows.Count == 0)
                {
                    dt = new DataTable();
                    dt.Columns.Add("id", typeof(int));
                    dt.Columns.Add("nombre", typeof(string));
                    dt.Rows.Add(1, "activo");
                    dt.Rows.Add(2, "inactivo");
                }

                DataRow filaTodos = dt.NewRow();
                filaTodos["id"] = 0;
                filaTodos["nombre"] = "Todos";
                dt.Rows.InsertAt(filaTodos, 0);

                cmbEstado.DataSource = dt;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = 0;
            }
            catch
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("nombre", typeof(string));
                dt.Rows.Add(0, "Todos");
                dt.Rows.Add(1, "activo");
                dt.Rows.Add(2, "inactivo");

                cmbEstado.DataSource = dt;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = 0;
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            cmbEstado.SelectedValue = 0;
            CargarClientes();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            using frmRegistrarCliente frm = new frmRegistrarCliente();
            if (frm.ShowDialog() == DialogResult.OK)
                CargarClientes();
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            EditarClienteSeleccionado();
        }

        private void dgvCliente_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditarClienteSeleccionado();
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_idClienteSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un cliente.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                DialogResult r = MessageBox.Show(
                    "¿Desea eliminar el cliente seleccionado?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (r != DialogResult.Yes)
                    return;

                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_cliente_eliminar",
                    Db.Parameter("@idCliente", _idClienteSeleccionado)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MessageBox.Show(
                        dt.Rows[0]["MensajeError"].ToString(),
                        "No se pudo eliminar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                MessageBox.Show(
                    "Cliente eliminado correctamente.",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar cliente:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarClientes()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_cliente_listar");

                int estadoId = cmbEstado.SelectedValue == null ? 0 : Convert.ToInt32(cmbEstado.SelectedValue);

                if (estadoId > 0)
                {
                    var filas = dt.AsEnumerable()
                        .Where(r => r["estadoId"] != DBNull.Value && Convert.ToInt32(r["estadoId"]) == estadoId);

                    dt = filas.Any() ? filas.CopyToDataTable() : dt.Clone();
                }

                dgvCliente.DataSource = dt;
                ConfigurarEncabezados();
                _idClienteSeleccionado = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar clientes:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarEncabezados()
        {
            if (dgvCliente.Columns.Contains("idCliente"))
            {
                //dgvCliente.Columns["idCliente"].HeaderText = "ID";
                dgvCliente.Columns["idCliente"].FillWeight = 6;
                dgvCliente.Columns["idCliente"].MinimumWidth = 50;
                dgvCliente.Columns["idCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvCliente.Columns.Contains("identidad"))
            {
                //dgvCliente.Columns["identidad"].HeaderText = "Identidad";
                dgvCliente.Columns["identidad"].FillWeight = 12;
                dgvCliente.Columns["identidad"].MinimumWidth = 110;
            }

            if (dgvCliente.Columns.Contains("nombres"))
            {
                //dgvCliente.Columns["nombres"].HeaderText = "Nombres";
                dgvCliente.Columns["nombres"].FillWeight = 14;
                dgvCliente.Columns["nombres"].MinimumWidth = 120;
            }

            if (dgvCliente.Columns.Contains("apellidos"))
            {
                //dgvCliente.Columns["apellidos"].HeaderText = "Apellidos";
                dgvCliente.Columns["apellidos"].FillWeight = 14;
                dgvCliente.Columns["apellidos"].MinimumWidth = 120;
            }

            if (dgvCliente.Columns.Contains("fechaNacimiento"))
            {
                //dgvCliente.Columns["fechaNacimiento"].HeaderText = "Fecha\nnacimiento";
                dgvCliente.Columns["fechaNacimiento"].FillWeight = 10;
                dgvCliente.Columns["fechaNacimiento"].MinimumWidth = 95;
                dgvCliente.Columns["fechaNacimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvCliente.Columns["fechaNacimiento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvCliente.Columns.Contains("telefono"))
            {
                //dgvCliente.Columns["telefono"].HeaderText = "Teléfono";
                dgvCliente.Columns["telefono"].FillWeight = 10;
                dgvCliente.Columns["telefono"].MinimumWidth = 95;
            }

            if (dgvCliente.Columns.Contains("correo"))
            {
                //dgvCliente.Columns["correo"].HeaderText = "Correo";
                dgvCliente.Columns["correo"].FillWeight = 14;
                dgvCliente.Columns["correo"].MinimumWidth = 130;
            }

            if (dgvCliente.Columns.Contains("direccion"))
            {
                //dgvCliente.Columns["direccion"].HeaderText = "Dirección";
                dgvCliente.Columns["direccion"].FillWeight = 16;
                dgvCliente.Columns["direccion"].MinimumWidth = 140;
            }

            if (dgvCliente.Columns.Contains("estadoCivilId"))
                dgvCliente.Columns["estadoCivilId"].Visible = true;

            if (dgvCliente.Columns.Contains("estadoCivil"))
            {
                //dgvCliente.Columns["estadoCivil"].HeaderText = "Estado\ncivil";
                dgvCliente.Columns["estadoCivil"].FillWeight = 10;
                dgvCliente.Columns["estadoCivil"].MinimumWidth = 95;
            }

            if (dgvCliente.Columns.Contains("rtn"))
            {
                //dgvCliente.Columns["rtn"].HeaderText = "RTN";
                dgvCliente.Columns["rtn"].FillWeight = 10;
                dgvCliente.Columns["rtn"].MinimumWidth = 95;
            }

            if (dgvCliente.Columns.Contains("estadoId"))
                dgvCliente.Columns["estadoId"].Visible = true;

            if (dgvCliente.Columns.Contains("estado"))
            {
                //dgvCliente.Columns["estado"].HeaderText = "Estado";
                dgvCliente.Columns["estado"].FillWeight = 8;
                dgvCliente.Columns["estado"].MinimumWidth = 80;
                dgvCliente.Columns["estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void EditarClienteSeleccionado()
        {
            try
            {
                if (_idClienteSeleccionado <= 0)
                {
                    MessageBox.Show(
                        "Seleccione un cliente.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                using frmRegistrarCliente frm = new frmRegistrarCliente(_idClienteSeleccionado);
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al abrir edición:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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

            if (dgvCliente.CurrentRow.Cells["idCliente"].Value == null ||
                dgvCliente.CurrentRow.Cells["idCliente"].Value == DBNull.Value)
            {
                _idClienteSeleccionado = 0;
                return;
            }

            _idClienteSeleccionado = Convert.ToInt32(dgvCliente.CurrentRow.Cells["idCliente"].Value);
        }
    }
}