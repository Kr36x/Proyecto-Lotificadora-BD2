using LotificadoraApp.Banco;
using LotificadoraApp.Estado;
using LotificadoraApp.Lote;
using LotificadoraApp.Proyecto;
using LotificadoraApp.Bloque;
using LotificadoraApp.Etapa;
using LotificadoraApp.CuentaBancaria;
using LotificadoraApp.Gasto;
using LotificadoraApp.GestionGastos;
using LotificadoraApp.Empleado;
using Guna.UI2.WinForms;
using LotificadoraApp.Aval;
using LotificadoraApp.Beneficiario;
using LotificadoraApp.RegistrarVentas;

namespace LotificadoraApp
{
    public partial class frmMenuPrincipal : Form
    {
        private Form? _formActivo;

        public frmMenuPrincipal()
        {
            InitializeComponent();
            ConfigurarMenu();
            ConectarEventos();
        }

        private void ConfigurarMenu()
        {
            Text = "Sistema Lotificadora";
            StartPosition = FormStartPosition.CenterScreen;

            // Colores base
            Color fondoMenu = Color.FromArgb(108, 142, 35);      // verde olivo
            Color botonNormal = Color.FromArgb(245, 245, 245);   // gris claro
            Color botonHover = Color.FromArgb(222, 235, 200);    // verde claro
            Color botonActivo = Color.FromArgb(198, 219, 174);   // un poco más fuerte
            Color textoBoton = Color.FromArgb(35, 35, 35);

            BackColor = Color.White;

            // Panel contenedor principal
            pnlContenedor.BorderStyle = BorderStyle.None;

            // Si tienes un panel lateral, esto mejora el look
            foreach (Control c in Controls)
            {
                if (c is Panel p && p != pnlContenedor)
                {
                    p.BackColor = fondoMenu;
                }
            }

            // Estilizar todos los botones del formulario
            var botones = ObtenerBotones(this);

            foreach (Button btn in botones)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = fondoMenu;
                btn.BackColor = botonNormal;
                btn.ForeColor = textoBoton;
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                btn.Cursor = Cursors.Hand;

                btn.MouseEnter += (_, _) =>
                {
                    if (btn.Tag?.ToString() != "ACTIVO")
                        btn.BackColor = botonHover;
                };

                btn.MouseLeave += (_, _) =>
                {
                    if (btn.Tag?.ToString() != "ACTIVO")
                        btn.BackColor = botonNormal;
                };
            }
        }

        private void ConectarEventos()
        {
            // Ajusta estos nombres a los nombres reales de tus botones
            btnLotesDisponibles.Click += (_, _) =>
            {
                //MarcarBotonActivo(btnLotesDisponibles);
                AbrirEnContenedor(new frmConsultaVistaLotes());
            };

            btnCreditosActivos.Click += (_, _) =>
            {
               // MarcarBotonActivo(btnCreditosActivos);
                AbrirEnContenedor(new frmConsultaVistaCredito());
            };

            btnEstadoDeCuenta.Click += (_, _) =>
            {
                //MarcarBotonActivo(btnEstadoDeCuenta);
                AbrirEnContenedor(new frmConsultaEstadoCuenta());
            };

            btnRecaudacionEtapa.Click += (_, _) =>
            {
                //MarcarBotonActivo(btnRecaudacionEtapa);
                AbrirEnContenedor(new frmConsultaRecaudacion());
            };

            btnConsultarPlanPago.Click += (_, _) =>
            {
                //MarcarBotonActivo(btnConsultarPlanPago);
                AbrirEnContenedor(new frmConsultarPlanPago());
            };

            btnRegistrarVentaCredito.Click += (_, _) =>
            {
                MarcarBotonActivo(btnRegistrarVentaCredito);
                AbrirEnContenedor(new frmVenta());
            };

            btnRegistrarPago.Click += (_, _) =>
            {
                MarcarBotonActivo(btnRegistrarPago);
                AbrirEnContenedor(new frmRegistrarPago());
            };

            btnConsultaLotesAptosCliente.Click += (_, _) =>
            {
               // MarcarBotonActivo(btnConsultaLotesAptosCliente);
                AbrirEnContenedor(new frmConsultaLotesAptosCliente());
            };

            btnCliente.Click += (_, _) =>
            {
                MarcarBotonActivo(btnCliente);
                AbrirEnContenedor(new frmGestionClientes());
            };

            btnLote.Click += (_, _) =>
            {
                MarcarBotonActivo(btnLote);
                AbrirEnContenedor(new frmLote());
            };

            btnConsultarPagos.Click += (_, _) =>
            {
                //MarcarBotonActivo(btnConsultarPagos);
                AbrirEnContenedor(new frmConsultaPagos());
            };

            btnProyecto.Click += (_, _) =>
            {
                MarcarBotonActivo(btnProyecto);
                AbrirEnContenedor(new frmProyecto());
            };

            btnBanco.Click += (_, _) =>
            {
                MarcarBotonActivo(btnBanco);
                AbrirEnContenedor(new frmBanco());
            };

            btnEstado.Click += (_, _) =>
            {
                MarcarBotonActivo(btnEstado);
                AbrirEnContenedor(new frmEstado());
            };

            btnBloque.Click += (_, _) =>
            {
                MarcarBotonActivo(btnBloque);
                AbrirEnContenedor(new frmBloque());
            };

            btnEtapa.Click += (_, _) =>
            {
                MarcarBotonActivo(btnEtapa);
                AbrirEnContenedor(new frmEtapa());
            };

            btnCuentaBancaria.Click += (_, _) =>
            {
                MarcarBotonActivo(btnCuentaBancaria);
                AbrirEnContenedor(new frmCuentaBancaria());
            };

            btnTipGasto.Click += (_, _) =>
            {
                MarcarBotonActivo(btnTipGasto);
                AbrirEnContenedor(new frmTipoGasto());
            };

            btnGestionGastos.Click += (_, _) =>
            {
                MarcarBotonActivo(btnGestionGastos);
                AbrirEnContenedor(new frmGestionGasto());
            };

            btnEmpleado.Click += (_, _) =>
            {
                MarcarBotonActivo(btnEmpleado);
                AbrirEnContenedor(new FrmEmpleado());
            };

            btnAval.Click += (_, _) =>
            {
                MarcarBotonActivo(btnAval);
                AbrirEnContenedor(new frmAval());
            };
            
            btnBeneficiario .Click += (_, _) =>
            {
                MarcarBotonActivo(btnBeneficiario);
                AbrirEnContenedor(new frmBeneficiario());
            };  
        }

        private void AbrirEnContenedor(Form formularioHijo)
        {
            if (_formActivo != null)
            {
                _formActivo.Close();
            }

            _formActivo = formularioHijo;

            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;

            pnlContenedor.Controls.Clear();
            pnlContenedor.Controls.Add(formularioHijo);
            pnlContenedor.Tag = formularioHijo;

            formularioHijo.BringToFront();
            formularioHijo.Show();
        }

        private void MarcarBotonActivo(Guna2Button botonActivo)
        {
            Color botonNormal = Color.FromArgb(245, 245, 245);
            Color botonSeleccionado = Color.FromArgb(198, 219, 174);

            var botones = ObtenerBotones(this);

            foreach (Button btn in botones)
            {
                btn.Tag = null;
                btn.BackColor = botonNormal;
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            }

            botonActivo.Tag = "ACTIVO";
            botonActivo.BackColor = botonSeleccionado;
            botonActivo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        private Button[] ObtenerBotones(Control contenedor)
        {
            return contenedor.Controls
                .Cast<Control>()
                .SelectMany(c =>
                {
                    if (c is Button b)
                        return new[] { b };

                    return ObtenerBotones(c);
                })
                .ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}