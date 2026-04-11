using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
                MarcarBotonActivo(btnLotesDisponibles);
                AbrirEnContenedor(new frmConsultaVistaLotes());
            };

            btnCreditosActivos.Click += (_, _) =>
            {
                MarcarBotonActivo(btnCreditosActivos);
                AbrirEnContenedor(new frmConsultaVistaCreditos());
            };

            btnEstadoDeCuenta.Click += (_, _) =>
            {
                MarcarBotonActivo(btnEstadoDeCuenta);
                AbrirEnContenedor(new frmConsultaEstadoCuenta());
            };

            btnRecaudacionEtapa.Click += (_, _) =>
            {
                MarcarBotonActivo(btnRecaudacionEtapa);
                AbrirEnContenedor(new frmConsultaRecaudacion());
            };

            btnConsultarPlanPago.Click += (_, _) =>
            {
                MarcarBotonActivo(btnConsultarPlanPago);
                AbrirEnContenedor(new frmConsultarPlanPago());
            };

            btnRegistrarVentaCredito.Click += (_, _) =>
            {
                MarcarBotonActivo(btnRegistrarVentaCredito);
                AbrirEnContenedor(new frmRegistrarVentaCredito());
            };

            btnRegistrarPago.Click += (_, _) =>
            {
                MarcarBotonActivo(btnRegistrarPago);
                AbrirEnContenedor(new frmRegistrarPago());
            };

            btnRegistrarCliente.Click += (_, _) =>
            {
                MarcarBotonActivo(btnRegistrarCliente);
                AbrirEnContenedor(new frmRegistrarCliente());
            };

            btnConsultaLotesAptosCliente.Click += (_, _) =>
            {
                MarcarBotonActivo(btnConsultaLotesAptosCliente);
                AbrirEnContenedor(new frmConsultaLotesAptosCliente());
            };

            btnGestionClientes.Click += (_, _) =>
            {
                MarcarBotonActivo(btnGestionClientes);
                AbrirEnContenedor(new frmGestionClientes());
            };

            btnRegistrarLote.Click += (_, _) =>
            {
                MarcarBotonActivo(btnRegistrarLote);
                AbrirEnContenedor(new frmRegistrarLote());
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

        private void MarcarBotonActivo(Button botonActivo)
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
    }
}