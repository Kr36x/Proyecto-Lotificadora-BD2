using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmGestionClientes : Form
    {
        public frmGestionClientes()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cbEstadoFiltro.Items.Clear();
            cbEstadoFiltro.Items.Add("Todos");
            cbEstadoFiltro.Items.Add("activo");
            cbEstadoFiltro.Items.Add("inactivo");
            cbEstadoFiltro.SelectedIndex = 0;

            lblRegistros.Text = "Registros: 0";
        }

        private void ConectarEventos()
        {
            Load += frmGestionClientes_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;

            btnNuevo.Click += btnNuevo_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnCerrar.Click += btnCerrar_Click;

            dgvClientes.CellDoubleClick += dgvClientes_CellDoubleClick;
        }

        private void frmGestionClientes_Load(object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtBuscar.Clear();
            cbEstadoFiltro.SelectedIndex = 0;
            CargarClientes();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnNuevo_Click(object? sender, EventArgs e)
        {
            using frmRegistrarCliente frm = new frmRegistrarCliente();
            frm.ShowDialog();
            CargarClientes();
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            EditarClienteSeleccionado();
        }

        private void dgvClientes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditarClienteSeleccionado();
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            try
            {
                int? idCliente = ObtenerIdClienteSeleccionado();

                if (!idCliente.HasValue)
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

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_cliente_eliminar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", idCliente.Value);

                DataTable dt = new DataTable();
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Columns.Contains("MensajeError"))
                {
                    string mensajeError = dt.Rows[0]["MensajeError"]?.ToString() ?? "No se pudo eliminar el cliente.";
                    MessageBox.Show(
                        mensajeError,
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

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void CargarClientes()
        {
            try
            {
                DataTable dt;

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_cliente_listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                using SqlDataAdapter da = new SqlDataAdapter(cmd);

                dt = new DataTable();
                da.Fill(dt);

                string texto = txtBuscar.Text.Trim().ToLower();
                string estado = cbEstadoFiltro.SelectedItem?.ToString() ?? "Todos";

                DataView view = dt.DefaultView;
                string filtro = "";

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    filtro = $"Convert(idCliente, 'System.String') LIKE '%{texto.Replace("'", "''")}%' " +
                             $"OR identidad LIKE '%{texto.Replace("'", "''")}%' " +
                             $"OR nombres LIKE '%{texto.Replace("'", "''")}%' " +
                             $"OR apellidos LIKE '%{texto.Replace("'", "''")}%'";
                }

                if (estado != "Todos")
                {
                    if (!string.IsNullOrWhiteSpace(filtro))
                        filtro += " AND ";

                    filtro += $"estado = '{estado.Replace("'", "''")}'";
                }

                view.RowFilter = filtro;

                dgvClientes.DataSource = null;
                dgvClientes.DataSource = view.ToTable();

                lblRegistros.Text = $"Registros: {((DataTable)dgvClientes.DataSource).Rows.Count}";
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

        private void EditarClienteSeleccionado()
        {
            try
            {
                int? idCliente = ObtenerIdClienteSeleccionado();

                if (!idCliente.HasValue)
                {
                    MessageBox.Show(
                        "Seleccione un cliente.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                using frmRegistrarCliente frm = new frmRegistrarCliente(idCliente.Value);
                frm.ShowDialog();

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

        private int? ObtenerIdClienteSeleccionado()
        {
            if (dgvClientes.CurrentRow == null)
                return null;

            if (dgvClientes.CurrentRow.Cells["idCliente"].Value == null)
                return null;

            return Convert.ToInt32(dgvClientes.CurrentRow.Cells["idCliente"].Value);
        }
    }
}