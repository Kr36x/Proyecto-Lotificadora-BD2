using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace LotificadoraApp.Proyecto
{
    public partial class frmProyecto : Form
    {
        public frmProyecto()
        {
            InitializeComponent();
        }

        private void frmProyecto_Load(object sender, EventArgs e)
        {
            CargarEstados();
            ObtenerProyectos();
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dtEstados = Db.ExecuteStoredProcedure(ProyectoQueries.QR001,
                    new SqlParameter("@Ids", "1,2,3"));

                cmbEstado.DataSource = dtEstados;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = -1;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los estados");
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

        private void BuscarProyectos(int estadoId)
        {
            try
            {
                DataTable dtEstados = Db.ExecuteStoredProcedure(
                    ProyectoQueries.QR003,
                    new SqlParameter("@estadoId", estadoId));

                dgvProyecto.DataSource = dtEstados;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al buscar los proyectos");
            }
        }

        private void ObtenerProyectos()
        {
            try
            {
                DataTable dtEstados = Db.ExecuteStoredProcedure(ProyectoQueries.QR002);
                dgvProyecto.DataSource = dtEstados;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al obtener los proyectos");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int estadoId = Convert.ToInt32(cmbEstado.SelectedValue);
            if (estadoId <= 0)
            {
                MostrarWarning("El filtro estado es requerido");
                return;
            }

            BuscarProyectos(estadoId);
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

        private void btnCrear_Click(object sender, EventArgs e)
        {
            frmRegistrarProyecto frmRegistrarProyecto = new();
            frmRegistrarProyecto.ShowDialog();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbEstado.SelectedIndex = -1;
            ObtenerProyectos();
        }
    }
}
