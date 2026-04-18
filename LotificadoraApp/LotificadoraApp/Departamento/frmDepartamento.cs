using System.Data;

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
            }
            catch
            {
                MostrarMensajeError("Error al obtener los departamentos");
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
