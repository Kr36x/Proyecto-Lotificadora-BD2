using System.Data;

namespace LotificadoraApp.Municipio
{
    public partial class frmMunicipio : Form
    {
        private int _municipioIdSeleccionado = 0;
        public frmMunicipio()
        {
            InitializeComponent();
        }

        private void frmMunicipio_Load(object sender, EventArgs e)
        {
            ObtenerMunicipios();
        }

        private void ObtenerMunicipios()
        {
            try
            {
                LimpiarSeleccion();

                DataTable dataTable = Db.ExecuteStoredProcedure(
                    MunicipioQueries.QR001);

                dgvMunicipio.DataSource = dataTable;
            }
            catch
            {
                MostrarMensajeError("Error al obtener los municipios");
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
                "¿Desea eliminar el municipio seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                  MunicipioQueries.QR002,
                  Db.Parameter("@id", _municipioIdSeleccionado));

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Municipio eliminado correctamente",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerMunicipios();
            }
            catch (Exception)
            {
                MostrarMensajeError("Ocurrió un error al eliminar");
            }
        }

        private void dgvMunicipio_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvMunicipio_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvMunicipio.CurrentRow == null || dgvMunicipio.CurrentRow.Index < 0)
                return;

            if (dgvMunicipio.CurrentRow.Cells["id"].Value == null ||
                dgvMunicipio.CurrentRow.Cells["id"].Value == DBNull.Value)
            {
                LimpiarSeleccion();
                return;
            }

            _municipioIdSeleccionado = Convert.ToInt32(dgvMunicipio.CurrentRow.Cells["id"].Value);
        }

        private void LimpiarSeleccion()
        {
            _municipioIdSeleccionado = 0;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using frmRegistrarMunicipio frmRegistrarMunicipio = new();

            frmRegistrarMunicipio.FormClosed += (_, _) => ObtenerMunicipios();
            frmRegistrarMunicipio.ShowDialog();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            if (_municipioIdSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un municipio para editar");
                return;
            }

            using frmRegistrarMunicipio frmRegistrarMunicipio =
                new(_municipioIdSeleccionado);

            frmRegistrarMunicipio.FormClosed += (_, _) => ObtenerMunicipios();
            frmRegistrarMunicipio.ShowDialog();
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
