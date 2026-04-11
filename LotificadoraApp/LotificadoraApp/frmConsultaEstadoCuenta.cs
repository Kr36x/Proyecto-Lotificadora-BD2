using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaEstadoCuenta : Form
    {
        public frmConsultaEstadoCuenta()
        {
            InitializeComponent();
        }

        private void txtConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCliente.Text))
                {
                    MessageBox.Show(
                        "El campo id del cliente es requerido",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var datasource = Db.ExecuteQuery(
                    @"SELECT 
                ec.idCliente,
                ec.cliente,
                ec.idCuota,
                ec.numeroCuota,
                ec.fechaVencimiento,
                ec.montoCuota,
                ec.saldoPendiente,
                e.nombre AS estado
              FROM dbo.fn_tvf_estado_cuenta_cliente(@idCliente) ec
              INNER JOIN dbo.Estado e
                  ON ec.estadoId = e.id",
                    new SqlParameter("@idCliente", Convert.ToInt32(txtCliente.Text))
                );

                grdEstadoCuenta.DataSource = datasource;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Error al consultar estado cuenta",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCliente.Clear();
        }
    }
}
