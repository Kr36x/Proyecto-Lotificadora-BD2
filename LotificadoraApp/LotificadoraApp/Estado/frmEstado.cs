using LotificadoraApp.Banco;
using System.Data;

namespace LotificadoraApp.Estado
{
    public partial class frmEstado : Form
    {
        public frmEstado()
        {
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            frmRegistrarEstado frmRegistrarEstado = new();
            frmRegistrarEstado.ShowDialog();
        }

        private void frmEstado_Load(object sender, EventArgs e)
        {
            ObtenerEstados();
        }

        private void ObtenerEstados()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(EstadoQueries.QR001);
                dgvEstado.DataSource = dataTable;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al obtener los estados");
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

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            ObtenerEstados();
        }
    }
}
