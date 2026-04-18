using System.Data;
using System.Drawing;

namespace LotificadoraApp.Departamento
{
    public partial class frmDepartamento : Form
    {
        private int _departamentoIdSeleccionado = 0;

        public frmDepartamento()
        {
            InitializeComponent();
        }

        private void frmDepartamento_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ObtenerDepartamentos();
        }

        private void ObtenerDepartamentos()
        {
            try
            {
                LimpiarSeleccion();

                DataTable dataTable = Db.ExecuteStoredProcedure(
                    DepartamentoQueries.QR001);

                dgvDepartamento.DataSource = dataTable;
                //ConfigurarColumnas();
            }
            catch
            {
                MostrarMensajeError("Error al obtener los departamentos");
            }
        }

        private void ConfigurarGrid()
        {
            dgvDepartamento.AutoGenerateColumns = true;
            dgvDepartamento.ReadOnly = true;
            dgvDepartamento.MultiSelect = false;
            dgvDepartamento.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDepartamento.AllowUserToAddRows = false;
            dgvDepartamento.AllowUserToDeleteRows = false;
            dgvDepartamento.AllowUserToResizeRows = false;
            dgvDepartamento.RowHeadersVisible = false;
            dgvDepartamento.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDepartamento.ColumnHeadersHeight = 52;
            dgvDepartamento.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDepartamento.EnableHeadersVisualStyles = false;
            dgvDepartamento.BackgroundColor = Color.White;
            dgvDepartamento.BorderStyle = BorderStyle.None;
            dgvDepartamento.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDepartamento.GridColor = Color.FromArgb(210, 210, 210);

            dgvDepartamento.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvDepartamento.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDepartamento.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvDepartamento.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvDepartamento.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvDepartamento.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDepartamento.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvDepartamento.DefaultCellStyle.BackColor = Color.White;
            dgvDepartamento.DefaultCellStyle.ForeColor = Color.Black;
            dgvDepartamento.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvDepartamento.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDepartamento.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvDepartamento.DefaultCellStyle.Padding = new Padding(3);

            dgvDepartamento.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvDepartamento.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvDepartamento.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvDepartamento.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvDepartamento.RowTemplate.Height = 32;
        }

        private void ConfigurarColumnas()
        {
            if (dgvDepartamento.Columns.Contains("id"))
            {
                dgvDepartamento.Columns["id"].FillWeight = 15;
                dgvDepartamento.Columns["id"].MinimumWidth = 70;
                dgvDepartamento.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvDepartamento.Columns.Contains("nombre"))
            {
                dgvDepartamento.Columns["nombre"].FillWeight = 45;
                dgvDepartamento.Columns["nombre"].MinimumWidth = 180;
                dgvDepartamento.Columns["nombre"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvDepartamento.Columns.Contains("estado"))
            {
                dgvDepartamento.Columns["estado"].FillWeight = 20;
                dgvDepartamento.Columns["estado"].MinimumWidth = 120;
                dgvDepartamento.Columns["estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvDepartamento.Columns.Contains("idPais"))
            {
                dgvDepartamento.Columns["idPais"].FillWeight = 20;
                dgvDepartamento.Columns["idPais"].MinimumWidth = 90;
                dgvDepartamento.Columns["idPais"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult confirmacion = MessageBox.Show(
               "¿Desea eliminar el departamento seleccionado?",
               "Confirmar eliminación",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
           );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                  DepartamentoQueries.QR002,
                  Db.Parameter("@id", _departamentoIdSeleccionado));

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Departamento eliminado correctamente",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerDepartamentos();
            }
            catch (Exception)
            {
                MostrarMensajeError("Ocurrió un error al eliminar");
            }
        }

        private void dgvDepartamento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvDepartamento_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvDepartamento.CurrentRow == null || dgvDepartamento.CurrentRow.Index < 0)
                return;

            if (dgvDepartamento.CurrentRow.Cells["id"].Value == null ||
                dgvDepartamento.CurrentRow.Cells["id"].Value == DBNull.Value)
            {
                LimpiarSeleccion();
                return;
            }

            _departamentoIdSeleccionado = Convert.ToInt32(dgvDepartamento.CurrentRow.Cells["id"].Value);
        }

        private void LimpiarSeleccion()
        {
            _departamentoIdSeleccionado = 0;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using frmRegistrarDepartamento frmRegistrarDepartamento = new();

            frmRegistrarDepartamento.FormClosed += (_, _) => ObtenerDepartamentos();
            frmRegistrarDepartamento.ShowDialog();
            ObtenerDepartamentos();
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_departamentoIdSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un departamento para editar");
                return;
            }

            using frmRegistrarDepartamento frmRegistrarDepartamento =
                new(_departamentoIdSeleccionado);

            frmRegistrarDepartamento.FormClosed += (_, _) => ObtenerDepartamentos();
            frmRegistrarDepartamento.ShowDialog();
        }
    }
}