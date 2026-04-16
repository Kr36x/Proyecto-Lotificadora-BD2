namespace LotificadoraApp
{
    partial class frmMenuPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlEsqueleto = new Panel();
            pnlContenedor = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            panel1 = new Panel();
            CONSULTAS = new Label();
            panel2 = new Panel();
            btnConsultarPagos = new Button();
            btnConsultaLotesAptosCliente = new Button();
            btnConsultarPlanPago = new Button();
            btnRecaudacionEtapa = new Button();
            btnEstadoDeCuenta = new Button();
            btnCreditosActivos = new Button();
            btnLotesDisponibles = new Button();
            label2 = new Label();
            panel3 = new Panel();
            btnEstado = new Button();
            btnBanco = new Button();
            btnBloque = new Button();
            btnProyecto = new Button();
            btnEtapa = new Button();
            btnRegistrarLote = new Button();
            btnGestionClientes = new Button();
            btnRegistrarPago = new Button();
            btnRegistrarVentaCredito = new Button();
            btnCuentaBancaria = new Button();
            btnGasto = new Button();
            pnlEsqueleto.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // pnlEsqueleto
            // 
            pnlEsqueleto.Controls.Add(pnlContenedor);
            pnlEsqueleto.Controls.Add(flowLayoutPanel1);
            pnlEsqueleto.Dock = DockStyle.Fill;
            pnlEsqueleto.Location = new Point(0, 0);
            pnlEsqueleto.Name = "pnlEsqueleto";
            pnlEsqueleto.Size = new Size(1584, 861);
            pnlEsqueleto.TabIndex = 0;
            // 
            // pnlContenedor
            // 
            pnlContenedor.BackColor = SystemColors.ControlDark;
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.Location = new Point(200, 0);
            pnlContenedor.Name = "pnlContenedor";
            pnlContenedor.Size = new Size(1384, 861);
            pnlContenedor.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.Olive;
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(CONSULTAS);
            flowLayoutPanel1.Controls.Add(panel2);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(panel3);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(200, 861);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(193, 21);
            label1.TabIndex = 0;
            label1.Text = "SISTEMA LOTIFICADORA";
            // 
            // panel1
            // 
            panel1.Location = new Point(3, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 41);
            panel1.TabIndex = 1;
            // 
            // CONSULTAS
            // 
            CONSULTAS.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CONSULTAS.ForeColor = Color.WhiteSmoke;
            CONSULTAS.Location = new Point(3, 68);
            CONSULTAS.Name = "CONSULTAS";
            CONSULTAS.Size = new Size(193, 15);
            CONSULTAS.TabIndex = 2;
            CONSULTAS.Text = "CONSULTAS";
            // 
            // panel2
            // 
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.Controls.Add(btnConsultarPagos);
            panel2.Controls.Add(btnConsultaLotesAptosCliente);
            panel2.Controls.Add(btnConsultarPlanPago);
            panel2.Controls.Add(btnRecaudacionEtapa);
            panel2.Controls.Add(btnEstadoDeCuenta);
            panel2.Controls.Add(btnCreditosActivos);
            panel2.Controls.Add(btnLotesDisponibles);
            panel2.Location = new Point(3, 86);
            panel2.Name = "panel2";
            panel2.Size = new Size(193, 272);
            panel2.TabIndex = 3;
            // 
            // btnConsultarPagos
            // 
            btnConsultarPagos.BackColor = Color.Transparent;
            btnConsultarPagos.Dock = DockStyle.Top;
            btnConsultarPagos.Location = new Point(0, 222);
            btnConsultarPagos.Name = "btnConsultarPagos";
            btnConsultarPagos.Size = new Size(193, 37);
            btnConsultarPagos.TabIndex = 6;
            btnConsultarPagos.Text = "Consultar pagos";
            btnConsultarPagos.UseVisualStyleBackColor = false;
            // 
            // btnConsultaLotesAptosCliente
            // 
            btnConsultaLotesAptosCliente.BackColor = Color.Transparent;
            btnConsultaLotesAptosCliente.Dock = DockStyle.Top;
            btnConsultaLotesAptosCliente.Location = new Point(0, 185);
            btnConsultaLotesAptosCliente.Name = "btnConsultaLotesAptosCliente";
            btnConsultaLotesAptosCliente.Size = new Size(193, 37);
            btnConsultaLotesAptosCliente.TabIndex = 5;
            btnConsultaLotesAptosCliente.Text = "Consulta Lotes Aptos Cliente  ";
            btnConsultaLotesAptosCliente.UseVisualStyleBackColor = false;
            // 
            // btnConsultarPlanPago
            // 
            btnConsultarPlanPago.BackColor = Color.Transparent;
            btnConsultarPlanPago.Dock = DockStyle.Top;
            btnConsultarPlanPago.Location = new Point(0, 148);
            btnConsultarPlanPago.Name = "btnConsultarPlanPago";
            btnConsultarPlanPago.Size = new Size(193, 37);
            btnConsultarPlanPago.TabIndex = 4;
            btnConsultarPlanPago.Text = "Consultar Plan Pago";
            btnConsultarPlanPago.UseVisualStyleBackColor = false;
            // 
            // btnRecaudacionEtapa
            // 
            btnRecaudacionEtapa.BackColor = Color.Transparent;
            btnRecaudacionEtapa.Dock = DockStyle.Top;
            btnRecaudacionEtapa.Location = new Point(0, 111);
            btnRecaudacionEtapa.Name = "btnRecaudacionEtapa";
            btnRecaudacionEtapa.Size = new Size(193, 37);
            btnRecaudacionEtapa.TabIndex = 3;
            btnRecaudacionEtapa.Text = "Recaudación Etapa";
            btnRecaudacionEtapa.UseVisualStyleBackColor = false;
            // 
            // btnEstadoDeCuenta
            // 
            btnEstadoDeCuenta.BackColor = Color.Transparent;
            btnEstadoDeCuenta.Dock = DockStyle.Top;
            btnEstadoDeCuenta.Location = new Point(0, 74);
            btnEstadoDeCuenta.Name = "btnEstadoDeCuenta";
            btnEstadoDeCuenta.Size = new Size(193, 37);
            btnEstadoDeCuenta.TabIndex = 2;
            btnEstadoDeCuenta.Text = "Estado de Cuenta";
            btnEstadoDeCuenta.UseVisualStyleBackColor = false;
            // 
            // btnCreditosActivos
            // 
            btnCreditosActivos.BackColor = Color.Transparent;
            btnCreditosActivos.Dock = DockStyle.Top;
            btnCreditosActivos.Location = new Point(0, 37);
            btnCreditosActivos.Name = "btnCreditosActivos";
            btnCreditosActivos.Size = new Size(193, 37);
            btnCreditosActivos.TabIndex = 1;
            btnCreditosActivos.Text = "Créditos Activos";
            btnCreditosActivos.UseVisualStyleBackColor = false;
            // 
            // btnLotesDisponibles
            // 
            btnLotesDisponibles.BackColor = Color.Transparent;
            btnLotesDisponibles.Dock = DockStyle.Top;
            btnLotesDisponibles.Location = new Point(0, 0);
            btnLotesDisponibles.Name = "btnLotesDisponibles";
            btnLotesDisponibles.Size = new Size(193, 37);
            btnLotesDisponibles.TabIndex = 0;
            btnLotesDisponibles.Text = "Lotes Disponibles";
            btnLotesDisponibles.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(3, 361);
            label2.Name = "label2";
            label2.Size = new Size(193, 15);
            label2.TabIndex = 5;
            label2.Text = "OPERACIONES";
            // 
            // panel3
            // 
            panel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel3.Controls.Add(btnGasto);
            panel3.Controls.Add(btnCuentaBancaria);
            panel3.Controls.Add(btnEstado);
            panel3.Controls.Add(btnBanco);
            panel3.Controls.Add(btnBloque);
            panel3.Controls.Add(btnProyecto);
            panel3.Controls.Add(btnEtapa);
            panel3.Controls.Add(btnRegistrarLote);
            panel3.Controls.Add(btnGestionClientes);
            panel3.Controls.Add(btnRegistrarPago);
            panel3.Controls.Add(btnRegistrarVentaCredito);
            panel3.Location = new Point(3, 379);
            panel3.Name = "panel3";
            panel3.Size = new Size(193, 317);
            panel3.TabIndex = 6;
            // 
            // btnEstado
            // 
            btnEstado.Dock = DockStyle.Top;
            btnEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEstado.Location = new Point(0, 227);
            btnEstado.Name = "btnEstado";
            btnEstado.Size = new Size(193, 28);
            btnEstado.TabIndex = 8;
            btnEstado.Text = "Estado";
            btnEstado.UseVisualStyleBackColor = true;
            // 
            // btnBanco
            // 
            btnBanco.Dock = DockStyle.Top;
            btnBanco.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBanco.Location = new Point(0, 196);
            btnBanco.Name = "btnBanco";
            btnBanco.Size = new Size(193, 31);
            btnBanco.TabIndex = 7;
            btnBanco.Text = "Banco";
            btnBanco.UseVisualStyleBackColor = true;
            // 
            // btnBloque
            // 
            btnBloque.Dock = DockStyle.Top;
            btnBloque.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBloque.Location = new Point(0, 168);
            btnBloque.Name = "btnBloque";
            btnBloque.Size = new Size(193, 28);
            btnBloque.TabIndex = 6;
            btnBloque.Text = "Bloque";
            btnBloque.UseVisualStyleBackColor = true;
            // 
            // btnProyecto
            // 
            btnProyecto.Dock = DockStyle.Top;
            btnProyecto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnProyecto.Location = new Point(0, 140);
            btnProyecto.Name = "btnProyecto";
            btnProyecto.Size = new Size(193, 28);
            btnProyecto.TabIndex = 5;
            btnProyecto.Text = "Proyecto";
            btnProyecto.UseVisualStyleBackColor = true;
            // 
            // btnEtapa
            // 
            btnEtapa.Dock = DockStyle.Top;
            btnEtapa.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEtapa.Location = new Point(0, 112);
            btnEtapa.Name = "btnEtapa";
            btnEtapa.Size = new Size(193, 28);
            btnEtapa.TabIndex = 0;
            btnEtapa.Text = "Etapa";
            btnEtapa.UseVisualStyleBackColor = true;
            // 
            // btnRegistrarLote
            // 
            btnRegistrarLote.Dock = DockStyle.Top;
            btnRegistrarLote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegistrarLote.Location = new Point(0, 84);
            btnRegistrarLote.Name = "btnRegistrarLote";
            btnRegistrarLote.Size = new Size(193, 28);
            btnRegistrarLote.TabIndex = 4;
            btnRegistrarLote.Text = "Lote";
            btnRegistrarLote.UseVisualStyleBackColor = true;
            // 
            // btnGestionClientes
            // 
            btnGestionClientes.BackColor = Color.Transparent;
            btnGestionClientes.Dock = DockStyle.Top;
            btnGestionClientes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGestionClientes.Location = new Point(0, 56);
            btnGestionClientes.Name = "btnGestionClientes";
            btnGestionClientes.Size = new Size(193, 28);
            btnGestionClientes.TabIndex = 3;
            btnGestionClientes.Text = "Cliente";
            btnGestionClientes.UseVisualStyleBackColor = false;
            // 
            // btnRegistrarPago
            // 
            btnRegistrarPago.BackColor = Color.Transparent;
            btnRegistrarPago.Dock = DockStyle.Top;
            btnRegistrarPago.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegistrarPago.Location = new Point(0, 28);
            btnRegistrarPago.Name = "btnRegistrarPago";
            btnRegistrarPago.Size = new Size(193, 28);
            btnRegistrarPago.TabIndex = 1;
            btnRegistrarPago.Text = "Registrar Pago";
            btnRegistrarPago.UseVisualStyleBackColor = false;
            // 
            // btnRegistrarVentaCredito
            // 
            btnRegistrarVentaCredito.BackColor = Color.Transparent;
            btnRegistrarVentaCredito.Dock = DockStyle.Top;
            btnRegistrarVentaCredito.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegistrarVentaCredito.Location = new Point(0, 0);
            btnRegistrarVentaCredito.Name = "btnRegistrarVentaCredito";
            btnRegistrarVentaCredito.Size = new Size(193, 28);
            btnRegistrarVentaCredito.TabIndex = 0;
            btnRegistrarVentaCredito.Text = "Registrar Venta Crédito";
            btnRegistrarVentaCredito.UseVisualStyleBackColor = false;
            // 
            // btnCuentaBancaria
            // 
            btnCuentaBancaria.Dock = DockStyle.Top;
            btnCuentaBancaria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCuentaBancaria.Location = new Point(0, 255);
            btnCuentaBancaria.Name = "btnCuentaBancaria";
            btnCuentaBancaria.Size = new Size(193, 28);
            btnCuentaBancaria.TabIndex = 9;
            btnCuentaBancaria.Text = "Cuenta Bancaria";
            btnCuentaBancaria.UseVisualStyleBackColor = true;
            // 
            // btnGasto
            // 
            btnGasto.Dock = DockStyle.Top;
            btnGasto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGasto.Location = new Point(0, 283);
            btnGasto.Name = "btnGasto";
            btnGasto.Size = new Size(193, 28);
            btnGasto.TabIndex = 10;
            btnGasto.Text = "Gasto";
            btnGasto.UseVisualStyleBackColor = true;
            // 
            // frmMenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1584, 861);
            Controls.Add(pnlEsqueleto);
            Name = "frmMenuPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmMenuPrincipal";
            pnlEsqueleto.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlEsqueleto;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private Panel panel1;
        private Label CONSULTAS;
        private Panel panel2;
        private Button btnLotesDisponibles;
        private Button btnConsultarPlanPago;
        private Button btnRecaudacionEtapa;
        private Button btnEstadoDeCuenta;
        private Button btnCreditosActivos;
        private Label label2;
        private Panel panel3;
        private Button btnRegistrarPago;
        private Button btnRegistrarVentaCredito;
        private Panel pnlContenedor;
        private Button btnConsultaLotesAptosCliente;
        private Button btnGestionClientes;
        private Button btnRegistrarLote;
        private Button btnConsultarPagos;
        private Button btnEtapa;
        private Button btnProyecto;
        private Button btnBloque;
        private Button btnBanco;
        private Button btnEstado;
        private Button btnGasto;
        private Button btnCuentaBancaria;
    }
}