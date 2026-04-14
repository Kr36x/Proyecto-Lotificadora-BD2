using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Lote
{
    public partial class frmLote : Form
    {
        public frmLote()
        {
            InitializeComponent();
        }

        private void frmLote_Load(object sender, EventArgs e)
        {
            ObtenerLotes();
            CargarBloques();
        }

        private void ObtenerLotes()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(LoteQueries.QR001);
                dgvLote.DataSource = dt;
            }
            catch (Exception)
            {
                MostrarMensajeError("Ocurrió un error al obtener los lotes");
            }
        }

        private void CargarBloques()
        {
            try
            {
                DataTable dtBloques = Db.ExecuteStoredProcedure(LoteQueries.QR003);

                cmbBloque.DataSource = dtBloques;
                cmbBloque.DisplayMember = "nombreBloque";
                cmbBloque.ValueMember = "idBloque";
                cmbBloque.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MostrarMensajeError("Ocurrió un error al cargar los bloques");
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

        private static void MostrarWarning(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void Limpiar()
        {
            txtNumeroLote.Clear();
            cmbBloque.SelectedIndex = -1;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string numeroLote = txtNumeroLote.Text;
                int bloqueId = Convert.ToInt32(cmbBloque.SelectedValue);

                if (string.IsNullOrWhiteSpace(numeroLote) &&
                    bloqueId <= 0)
                {
                    MostrarWarning("Los filtros son requeridos");
                    return;
                }

                DataTable dt = Db.ExecuteStoredProcedure(LoteQueries.QR002,
                    new SqlParameter("@bloqueId", bloqueId),
                    new SqlParameter("@numeroLote", numeroLote));

                dgvLote.DataSource = dt;
            }
            catch (Exception)
            {
                MostrarMensajeError("Ocurrió un error al consultar los lotes");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
            ObtenerLotes();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            frmRegistrarLote frmRegistrarLote = new();
            frmRegistrarLote.ShowDialog();
        }
    }
}
