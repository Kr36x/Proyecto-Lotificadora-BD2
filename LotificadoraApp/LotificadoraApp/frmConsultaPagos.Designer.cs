namespace LotificadoraApp
{
    partial class frmConsultaPagos
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
            panel2 = new Panel();
            cbVenta = new ComboBox();
            label9 = new Label();
            label15 = new Label();
            label16 = new Label();
            label1 = new Label();
            txtIDCliente = new TextBox();
            txtCliente = new TextBox();
            chkTodasLasVentas = new CheckBox();
            btnCerrar = new Button();
            btnLimpiar = new Button();
            btnBuscar = new Button();
            panel1 = new Panel();
            txtCantidadPagos = new TextBox();
            txtTotalPagado = new TextBox();
            label2 = new Label();
            label4 = new Label();
            dgvPagos = new DataGridView();
            label3 = new Label();
            label5 = new Label();
            dtpFechaInicio = new DateTimePicker();
            dtpFechaFin = new DateTimePicker();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dtpFechaFin);
            panel2.Controls.Add(dtpFechaInicio);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(btnCerrar);
            panel2.Controls.Add(btnLimpiar);
            panel2.Controls.Add(btnBuscar);
            panel2.Controls.Add(chkTodasLasVentas);
            panel2.Controls.Add(txtCliente);
            panel2.Controls.Add(txtIDCliente);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(cbVenta);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(label15);
            panel2.Location = new Point(12, 27);
            panel2.Name = "panel2";
            panel2.Size = new Size(1028, 175);
            panel2.TabIndex = 17;
            // 
            // cbVenta
            // 
            cbVenta.FormattingEnabled = true;
            cbVenta.Location = new Point(93, 51);
            cbVenta.Name = "cbVenta";
            cbVenta.Size = new Size(304, 23);
            cbVenta.TabIndex = 3;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 54);
            label9.Name = "label9";
            label9.Size = new Size(39, 15);
            label9.TabIndex = 2;
            label9.Text = "Venta:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(12, 13);
            label15.Name = "label15";
            label15.Size = new Size(60, 15);
            label15.TabIndex = 0;
            label15.Text = "Id Cliente:";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(12, 9);
            label16.Name = "label16";
            label16.Size = new Size(51, 15);
            label16.TabIndex = 16;
            label16.Text = "CLIENTE";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(231, 13);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 8;
            label1.Text = "Cliente: ";
            // 
            // txtIDCliente
            // 
            txtIDCliente.Location = new Point(93, 10);
            txtIDCliente.Name = "txtIDCliente";
            txtIDCliente.Size = new Size(100, 23);
            txtIDCliente.TabIndex = 9;
            // 
            // txtCliente
            // 
            txtCliente.Location = new Point(287, 10);
            txtCliente.Name = "txtCliente";
            txtCliente.Size = new Size(100, 23);
            txtCliente.TabIndex = 10;
            // 
            // chkTodasLasVentas
            // 
            chkTodasLasVentas.AutoSize = true;
            chkTodasLasVentas.Location = new Point(93, 90);
            chkTodasLasVentas.Name = "chkTodasLasVentas";
            chkTodasLasVentas.Size = new Size(167, 19);
            chkTodasLasVentas.TabIndex = 11;
            chkTodasLasVentas.Text = "Todas las ventas del cliente";
            chkTodasLasVentas.UseVisualStyleBackColor = true;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(869, 51);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 29;
            btnCerrar.Text = "CERRAR";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(748, 51);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 28;
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(565, 51);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(135, 23);
            btnBuscar.TabIndex = 27;
            btnBuscar.Text = "BUSCAR";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dgvPagos);
            panel1.Controls.Add(txtCantidadPagos);
            panel1.Controls.Add(txtTotalPagado);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(12, 217);
            panel1.Name = "panel1";
            panel1.Size = new Size(1028, 438);
            panel1.TabIndex = 30;
            // 
            // txtCantidadPagos
            // 
            txtCantidadPagos.Location = new Point(297, 11);
            txtCantidadPagos.Name = "txtCantidadPagos";
            txtCantidadPagos.Size = new Size(100, 23);
            txtCantidadPagos.TabIndex = 10;
            // 
            // txtTotalPagado
            // 
            txtTotalPagado.Location = new Point(93, 11);
            txtTotalPagado.Name = "txtTotalPagado";
            txtTotalPagado.Size = new Size(100, 23);
            txtTotalPagado.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(231, 14);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 8;
            label2.Text = "Cantidad:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 14);
            label4.Name = "label4";
            label4.Size = new Size(78, 15);
            label4.TabIndex = 0;
            label4.Text = "Total Pagado:";
            // 
            // dgvPagos
            // 
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPagos.Location = new Point(12, 40);
            dgvPagos.Name = "dgvPagos";
            dgvPagos.Size = new Size(1002, 380);
            dgvPagos.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 141);
            label3.Name = "label3";
            label3.Size = new Size(73, 15);
            label3.TabIndex = 30;
            label3.Text = "Fecha Inicio:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(277, 141);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 31;
            label5.Text = "Fecha Fin:";
            // 
            // dtpFechaInicio
            // 
            dtpFechaInicio.Location = new Point(112, 140);
            dtpFechaInicio.Name = "dtpFechaInicio";
            dtpFechaInicio.Size = new Size(148, 23);
            dtpFechaInicio.TabIndex = 32;
            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Location = new Point(343, 141);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.Size = new Size(148, 23);
            dtpFechaFin.TabIndex = 33;
            // 
            // frmConsultaPagos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 676);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(label16);
            Name = "frmConsultaPagos";
            Text = "frmConsultaPagos";
            Load += frmConsultaPagos_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel2;
        private Button btnNuevoBeneficiario;
        private Button btnNuevoAval;
        private ComboBox cbBeneficiario;
        private Label label10;
        private ComboBox cbVenta;
        private Label label9;
        private ComboBox cbClientes;
        private Label label15;
        private Label label16;
        private CheckBox chkTodasLasVentas;
        private TextBox txtCliente;
        private TextBox txtIDCliente;
        private Label label1;
        private Button btnCerrar;
        private Button btnLimpiar;
        private Button btnBuscar;
        private Panel panel1;
        private DataGridView dgvPagos;
        private TextBox txtCantidadPagos;
        private TextBox txtTotalPagado;
        private Label label2;
        private Label label4;
        private DateTimePicker dtpFechaFin;
        private DateTimePicker dtpFechaInicio;
        private Label label5;
        private Label label3;
    }
}