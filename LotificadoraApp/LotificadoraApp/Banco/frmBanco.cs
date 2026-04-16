using LotificadoraApp.Proyecto;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Banco
{
    public partial class frmBanco : Form
    {
        public frmBanco()
        {
            InitializeComponent();
            CargarEstados();
            ObtenerBancos();
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(BancoQueries.QR001,
                    new SqlParameter("@Ids", "1,2"));

                cmbEstado.DataSource = dataTable;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedValue = -1;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al cargar los estados");
            }
        }

        private void ObtenerBancos()
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(BancoQueries.QR002);
                dgvBanco.DataSource = dataTable;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al obtener los bancos");
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbEstado.SelectedIndex = -1;
            ObtenerBancos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int estadoId = Convert.ToInt32(cmbEstado.SelectedValue);
            if (estadoId <= 0)
            {
                MostrarWarning("El filtro estado es requerido");
                return;
            }

            BuscarBancos(estadoId);
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


        private void BuscarBancos(int estadoId)
        {
            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                    BancoQueries.QR003,
                    new SqlParameter("@estadoId", estadoId));

                dgvBanco.DataSource = dataTable;
            }
            catch (Exception)
            {
                MostrarMensajeError("Error al buscar los bancos");
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            frmRegistrarBanco frmRegistrarBanco = new();
            frmRegistrarBanco.ShowDialog();
        }

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            ObtenerBancos();
        }
    }
}
