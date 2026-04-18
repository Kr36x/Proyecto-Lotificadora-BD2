using LotificadoraApp.RegistrarVentas;

namespace LotificadoraApp.RegistrarVentas
{
    public partial class frmVenta : Form
    {
        public frmVenta()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmVenta_Load;
            btnCredito.Click += btnCredito_Click;
            btnContado.Click += btnContado_Click;
        }

        private void frmVenta_Load(object? sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new frmRegistrarVentaCredito());
        }

        private void btnCredito_Click(object? sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new frmRegistrarVentaCredito());
        }

        private void btnContado_Click(object? sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new frmRegistrarVentaContado());
        }

        private void AbrirFormularioEnPanel(Form formulario)
        {
            pnlGrid.Controls.Clear();

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            pnlGrid.Controls.Add(formulario);
            pnlGrid.Tag = formulario;
            formulario.Show();
        }
    }
}