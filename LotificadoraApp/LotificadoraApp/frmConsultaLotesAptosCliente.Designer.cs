namespace LotificadoraApp
{
    partial class frmConsultaLotesAptosCliente
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
            lblRegistros = new Label();
            txtCapacidadMaxima = new TextBox();
            label5 = new Label();
            txtIngresoMensual = new TextBox();
            label4 = new Label();
            dgvLotesAptos = new DataGridView();
            label7 = new Label();
            txtIdCliente = new TextBox();
            label8 = new Label();
            txtCliente = new TextBox();
            label1 = new Label();
            label3 = new Label();
            label9 = new Label();
            panel2 = new Panel();
            panel1 = new Panel();
            txtPrima = new TextBox();
            label2 = new Label();
            txtPlazoAnios = new TextBox();
            label6 = new Label();
            txtPorcentajeMaxIngreso = new TextBox();
            label10 = new Label();
            btnConsultar = new Button();
            btnLimpiar = new Button();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLotesAptos).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblRegistros
            // 
            lblRegistros.AutoSize = true;
            lblRegistros.Location = new Point(12, 646);
            lblRegistros.Name = "lblRegistros";
            lblRegistros.Size = new Size(55, 15);
            lblRegistros.TabIndex = 17;
            lblRegistros.Text = "Registros";
            // 
            // txtCapacidadMaxima
            // 
            txtCapacidadMaxima.Location = new Point(131, 72);
            txtCapacidadMaxima.Name = "txtCapacidadMaxima";
            txtCapacidadMaxima.Size = new Size(178, 23);
            txtCapacidadMaxima.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 75);
            label5.Name = "label5";
            label5.Size = new Size(112, 15);
            label5.TabIndex = 6;
            label5.Text = "Capacidad Máxima:";
            // 
            // txtIngresoMensual
            // 
            txtIngresoMensual.Location = new Point(131, 43);
            txtIngresoMensual.Name = "txtIngresoMensual";
            txtIngresoMensual.Size = new Size(178, 23);
            txtIngresoMensual.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 46);
            label4.Name = "label4";
            label4.Size = new Size(97, 15);
            label4.TabIndex = 4;
            label4.Text = "Ingreso Mensual:";
            // 
            // dgvLotesAptos
            // 
            dgvLotesAptos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLotesAptos.Location = new Point(12, 315);
            dgvLotesAptos.Name = "dgvLotesAptos";
            dgvLotesAptos.Size = new Size(1043, 328);
            dgvLotesAptos.TabIndex = 15;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 15);
            label7.Name = "label7";
            label7.Size = new Size(49, 15);
            label7.TabIndex = 14;
            label7.Text = "FILTROS";
            // 
            // txtIdCliente
            // 
            txtIdCliente.Location = new Point(81, 16);
            txtIdCliente.Name = "txtIdCliente";
            txtIdCliente.Size = new Size(75, 23);
            txtIdCliente.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 19);
            label8.Name = "label8";
            label8.RightToLeft = RightToLeft.No;
            label8.Size = new Size(60, 15);
            label8.TabIndex = 12;
            label8.Text = "Id Cliente:";
            // 
            // txtCliente
            // 
            txtCliente.Location = new Point(131, 14);
            txtCliente.Name = "txtCliente";
            txtCliente.Size = new Size(178, 23);
            txtCliente.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 154);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 11;
            label1.Text = "RESUMEN";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 17);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.No;
            label3.Size = new Size(47, 15);
            label3.TabIndex = 2;
            label3.Text = "Cliente:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 297);
            label9.Name = "label9";
            label9.Size = new Size(115, 15);
            label9.TabIndex = 16;
            label9.Text = "DETALLE DE CUOTAS";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnCerrar);
            panel2.Controls.Add(btnLimpiar);
            panel2.Controls.Add(btnConsultar);
            panel2.Controls.Add(txtPorcentajeMaxIngreso);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(txtPlazoAnios);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(txtPrima);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(txtIdCliente);
            panel2.Controls.Add(label8);
            panel2.Location = new Point(12, 33);
            panel2.Name = "panel2";
            panel2.Size = new Size(1043, 118);
            panel2.TabIndex = 13;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(txtCapacidadMaxima);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtIngresoMensual);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtCliente);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(12, 172);
            panel1.Name = "panel1";
            panel1.Size = new Size(1043, 113);
            panel1.TabIndex = 12;
            // 
            // txtPrima
            // 
            txtPrima.Location = new Point(217, 16);
            txtPrima.Name = "txtPrima";
            txtPrima.Size = new Size(178, 23);
            txtPrima.TabIndex = 15;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(170, 19);
            label2.Name = "label2";
            label2.RightToLeft = RightToLeft.No;
            label2.Size = new Size(41, 15);
            label2.TabIndex = 14;
            label2.Text = "Prima:";
            // 
            // txtPlazoAnios
            // 
            txtPlazoAnios.Location = new Point(503, 16);
            txtPlazoAnios.Name = "txtPlazoAnios";
            txtPlazoAnios.Size = new Size(178, 23);
            txtPlazoAnios.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(420, 19);
            label6.Name = "label6";
            label6.RightToLeft = RightToLeft.No;
            label6.Size = new Size(68, 15);
            label6.TabIndex = 16;
            label6.Text = "Plazo Años:";
            // 
            // txtPorcentajeMaxIngreso
            // 
            txtPorcentajeMaxIngreso.Location = new Point(841, 16);
            txtPorcentajeMaxIngreso.Name = "txtPorcentajeMaxIngreso";
            txtPorcentajeMaxIngreso.Size = new Size(178, 23);
            txtPorcentajeMaxIngreso.TabIndex = 19;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(689, 19);
            label10.Name = "label10";
            label10.RightToLeft = RightToLeft.No;
            label10.Size = new Size(146, 15);
            label10.TabIndex = 18;
            label10.Text = "% Máx. Ingreso permitido:";
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(744, 76);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(75, 23);
            btnConsultar.TabIndex = 20;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(845, 76);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 21;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(944, 76);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 22;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // frmConsultaLotesAptosCliente
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 676);
            Controls.Add(lblRegistros);
            Controls.Add(dgvLotesAptos);
            Controls.Add(label7);
            Controls.Add(label1);
            Controls.Add(label9);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "frmConsultaLotesAptosCliente";
            Text = "frmConsultaLotesAptosCliente";
            ((System.ComponentModel.ISupportInitialize)dgvLotesAptos).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRegistros;
        private TextBox txtCapacidadMaxima;
        private Label label5;
        private TextBox txtIngresoMensual;
        private Label label4;
        private DataGridView dgvLotesAptos;
        private Label label7;
        private TextBox txtIdCliente;
        private Label label8;
        private TextBox txtCliente;
        private Label label1;
        private Label label3;
        private Label label9;
        private Panel panel2;
        private TextBox txtPorcentajeMaxIngreso;
        private Label label10;
        private TextBox txtPlazoAnios;
        private Label label6;
        private TextBox txtPrima;
        private Label label2;
        private Panel panel1;
        private Button btnConsultar;
        private Button btnCerrar;
        private Button btnLimpiar;
    }
}