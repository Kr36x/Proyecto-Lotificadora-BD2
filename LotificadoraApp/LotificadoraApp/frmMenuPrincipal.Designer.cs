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
            btnRegistrarLote = new Button();
            btnGestionClientes = new Button();
            btnRegistrarCliente = new Button();
            btnRegistrarPago = new Button();
            btnRegistrarVentaCredito = new Button();
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
            pnlEsqueleto.Margin = new Padding(4, 5, 4, 5);
            pnlEsqueleto.Name = "pnlEsqueleto";
            pnlEsqueleto.Size = new Size(1816, 1127);
            pnlEsqueleto.TabIndex = 0;
            // 
            // pnlContenedor
            // 
            pnlContenedor.BackColor = SystemColors.ControlDark;
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.Location = new Point(286, 0);
            pnlContenedor.Margin = new Padding(4, 5, 4, 5);
            pnlContenedor.Name = "pnlContenedor";
            pnlContenedor.Size = new Size(1530, 1127);
            pnlContenedor.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.OliveDrab;
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(CONSULTAS);
            flowLayoutPanel1.Controls.Add(panel2);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(panel3);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(286, 1127);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(185, 64);
            label1.TabIndex = 0;
            label1.Text = "SISTEMA LOTIFICADORA";
            // 
            // panel1
            // 
            panel1.Location = new Point(4, 69);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(286, 68);
            panel1.TabIndex = 1;
            // 
            // CONSULTAS
            // 
            CONSULTAS.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CONSULTAS.ForeColor = Color.WhiteSmoke;
            CONSULTAS.Location = new Point(4, 142);
            CONSULTAS.Margin = new Padding(4, 0, 4, 0);
            CONSULTAS.Name = "CONSULTAS";
            CONSULTAS.Size = new Size(276, 25);
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
            panel2.Location = new Point(4, 172);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(276, 453);
            panel2.TabIndex = 3;
            // 
            // btnConsultarPagos
            // 
            btnConsultarPagos.BackColor = Color.Transparent;
            btnConsultarPagos.Dock = DockStyle.Top;
            btnConsultarPagos.Location = new Point(0, 372);
            btnConsultarPagos.Margin = new Padding(4, 5, 4, 5);
            btnConsultarPagos.Name = "btnConsultarPagos";
            btnConsultarPagos.Size = new Size(276, 62);
            btnConsultarPagos.TabIndex = 6;
            btnConsultarPagos.Text = "Consultar pagos";
            btnConsultarPagos.UseVisualStyleBackColor = false;
            // 
            // btnConsultaLotesAptosCliente
            // 
            btnConsultaLotesAptosCliente.BackColor = Color.Transparent;
            btnConsultaLotesAptosCliente.Dock = DockStyle.Top;
            btnConsultaLotesAptosCliente.Location = new Point(0, 310);
            btnConsultaLotesAptosCliente.Margin = new Padding(4, 5, 4, 5);
            btnConsultaLotesAptosCliente.Name = "btnConsultaLotesAptosCliente";
            btnConsultaLotesAptosCliente.Size = new Size(276, 62);
            btnConsultaLotesAptosCliente.TabIndex = 5;
            btnConsultaLotesAptosCliente.Text = "Consulta Lotes Aptos Cliente  ";
            btnConsultaLotesAptosCliente.UseVisualStyleBackColor = false;
            // 
            // btnConsultarPlanPago
            // 
            btnConsultarPlanPago.BackColor = Color.Transparent;
            btnConsultarPlanPago.Dock = DockStyle.Top;
            btnConsultarPlanPago.Location = new Point(0, 248);
            btnConsultarPlanPago.Margin = new Padding(4, 5, 4, 5);
            btnConsultarPlanPago.Name = "btnConsultarPlanPago";
            btnConsultarPlanPago.Size = new Size(276, 62);
            btnConsultarPlanPago.TabIndex = 4;
            btnConsultarPlanPago.Text = "Consultar Plan Pago";
            btnConsultarPlanPago.UseVisualStyleBackColor = false;
            // 
            // btnRecaudacionEtapa
            // 
            btnRecaudacionEtapa.BackColor = Color.Transparent;
            btnRecaudacionEtapa.Dock = DockStyle.Top;
            btnRecaudacionEtapa.Location = new Point(0, 186);
            btnRecaudacionEtapa.Margin = new Padding(4, 5, 4, 5);
            btnRecaudacionEtapa.Name = "btnRecaudacionEtapa";
            btnRecaudacionEtapa.Size = new Size(276, 62);
            btnRecaudacionEtapa.TabIndex = 3;
            btnRecaudacionEtapa.Text = "Recaudación Etapa";
            btnRecaudacionEtapa.UseVisualStyleBackColor = false;
            // 
            // btnEstadoDeCuenta
            // 
            btnEstadoDeCuenta.BackColor = Color.Transparent;
            btnEstadoDeCuenta.Dock = DockStyle.Top;
            btnEstadoDeCuenta.Location = new Point(0, 124);
            btnEstadoDeCuenta.Margin = new Padding(4, 5, 4, 5);
            btnEstadoDeCuenta.Name = "btnEstadoDeCuenta";
            btnEstadoDeCuenta.Size = new Size(276, 62);
            btnEstadoDeCuenta.TabIndex = 2;
            btnEstadoDeCuenta.Text = "Estado de Cuenta";
            btnEstadoDeCuenta.UseVisualStyleBackColor = false;
            // 
            // btnCreditosActivos
            // 
            btnCreditosActivos.BackColor = Color.Transparent;
            btnCreditosActivos.Dock = DockStyle.Top;
            btnCreditosActivos.Location = new Point(0, 62);
            btnCreditosActivos.Margin = new Padding(4, 5, 4, 5);
            btnCreditosActivos.Name = "btnCreditosActivos";
            btnCreditosActivos.Size = new Size(276, 62);
            btnCreditosActivos.TabIndex = 1;
            btnCreditosActivos.Text = "Créditos Activos";
            btnCreditosActivos.UseVisualStyleBackColor = false;
            // 
            // btnLotesDisponibles
            // 
            btnLotesDisponibles.BackColor = Color.Transparent;
            btnLotesDisponibles.Dock = DockStyle.Top;
            btnLotesDisponibles.Location = new Point(0, 0);
            btnLotesDisponibles.Margin = new Padding(4, 5, 4, 5);
            btnLotesDisponibles.Name = "btnLotesDisponibles";
            btnLotesDisponibles.Size = new Size(276, 62);
            btnLotesDisponibles.TabIndex = 0;
            btnLotesDisponibles.Text = "Lotes Disponibles";
            btnLotesDisponibles.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(4, 630);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(276, 25);
            label2.TabIndex = 5;
            label2.Text = "OPERACIONES";
            // 
            // panel3
            // 
            panel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel3.Controls.Add(btnRegistrarLote);
            panel3.Controls.Add(btnGestionClientes);
            panel3.Controls.Add(btnRegistrarCliente);
            panel3.Controls.Add(btnRegistrarPago);
            panel3.Controls.Add(btnRegistrarVentaCredito);
            panel3.Location = new Point(4, 660);
            panel3.Margin = new Padding(4, 5, 4, 5);
            panel3.Name = "panel3";
            panel3.Size = new Size(276, 312);
            panel3.TabIndex = 6;
            // 
            // btnRegistrarLote
            // 
            btnRegistrarLote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegistrarLote.Location = new Point(0, 243);
            btnRegistrarLote.Name = "btnRegistrarLote";
            btnRegistrarLote.Size = new Size(276, 63);
            btnRegistrarLote.TabIndex = 4;
            btnRegistrarLote.Text = "Lote";
            btnRegistrarLote.UseVisualStyleBackColor = true;
            // 
            // btnGestionClientes
            // 
            btnGestionClientes.BackColor = Color.Transparent;
            btnGestionClientes.Dock = DockStyle.Top;
            btnGestionClientes.Location = new Point(0, 186);
            btnGestionClientes.Margin = new Padding(4, 5, 4, 5);
            btnGestionClientes.Name = "btnGestionClientes";
            btnGestionClientes.Size = new Size(276, 62);
            btnGestionClientes.TabIndex = 3;
            btnGestionClientes.Text = "Gestionar Clientes";
            btnGestionClientes.UseVisualStyleBackColor = false;
            // 
            // btnRegistrarCliente
            // 
            btnRegistrarCliente.BackColor = Color.Transparent;
            btnRegistrarCliente.Dock = DockStyle.Top;
            btnRegistrarCliente.Location = new Point(0, 124);
            btnRegistrarCliente.Margin = new Padding(4, 5, 4, 5);
            btnRegistrarCliente.Name = "btnRegistrarCliente";
            btnRegistrarCliente.Size = new Size(276, 62);
            btnRegistrarCliente.TabIndex = 2;
            btnRegistrarCliente.Text = "Registrar Cliente";
            btnRegistrarCliente.UseVisualStyleBackColor = false;
            // 
            // btnRegistrarPago
            // 
            btnRegistrarPago.BackColor = Color.Transparent;
            btnRegistrarPago.Dock = DockStyle.Top;
            btnRegistrarPago.Location = new Point(0, 62);
            btnRegistrarPago.Margin = new Padding(4, 5, 4, 5);
            btnRegistrarPago.Name = "btnRegistrarPago";
            btnRegistrarPago.Size = new Size(276, 62);
            btnRegistrarPago.TabIndex = 1;
            btnRegistrarPago.Text = "Registrar Pago";
            btnRegistrarPago.UseVisualStyleBackColor = false;
            // 
            // btnRegistrarVentaCredito
            // 
            btnRegistrarVentaCredito.BackColor = Color.Transparent;
            btnRegistrarVentaCredito.Dock = DockStyle.Top;
            btnRegistrarVentaCredito.Location = new Point(0, 0);
            btnRegistrarVentaCredito.Margin = new Padding(4, 5, 4, 5);
            btnRegistrarVentaCredito.Name = "btnRegistrarVentaCredito";
            btnRegistrarVentaCredito.Size = new Size(276, 62);
            btnRegistrarVentaCredito.TabIndex = 0;
            btnRegistrarVentaCredito.Text = "Registrar Venta Crédito";
            btnRegistrarVentaCredito.UseVisualStyleBackColor = false;
            // 
            // frmMenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1816, 1127);
            Controls.Add(pnlEsqueleto);
            Margin = new Padding(4, 5, 4, 5);
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
        private Button btnRegistrarCliente;
        private Button btnConsultaLotesAptosCliente;
        private Button btnGestionClientes;
        private Button btnRegistrarLote;
        private Button btnConsultarPagos;
    }
}