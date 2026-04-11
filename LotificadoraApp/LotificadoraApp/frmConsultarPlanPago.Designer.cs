namespace LotificadoraApp
{
    partial class frmConsultarPlanPago
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
            panel1 = new Panel();
            txtEstadoCredito = new TextBox();
            label2 = new Label();
            txtSaldoPendiente = new TextBox();
            label6 = new Label();
            txtTotalPlan = new TextBox();
            label5 = new Label();
            txtIdPlanPago = new TextBox();
            label4 = new Label();
            txtCliente = new TextBox();
            label3 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            txtIDVentaCredito = new TextBox();
            label8 = new Label();
            label7 = new Label();
            dgvDetallesCuotas = new DataGridView();
            label9 = new Label();
            lblRegistros = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetallesCuotas).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(txtEstadoCredito);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtSaldoPendiente);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtTotalPlan);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtIdPlanPago);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtCliente);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(12, 120);
            panel1.Name = "panel1";
            panel1.Size = new Size(1043, 165);
            panel1.TabIndex = 5;
            // 
            // txtEstadoCredito
            // 
            txtEstadoCredito.Location = new Point(131, 128);
            txtEstadoCredito.Name = "txtEstadoCredito";
            txtEstadoCredito.Size = new Size(178, 23);
            txtEstadoCredito.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 131);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 10;
            label2.Text = "Estado crédito:";
            // 
            // txtSaldoPendiente
            // 
            txtSaldoPendiente.Location = new Point(131, 99);
            txtSaldoPendiente.Name = "txtSaldoPendiente";
            txtSaldoPendiente.Size = new Size(178, 23);
            txtSaldoPendiente.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(15, 102);
            label6.Name = "label6";
            label6.Size = new Size(95, 15);
            label6.TabIndex = 8;
            label6.Text = "Saldo pendiente:";
            // 
            // txtTotalPlan
            // 
            txtTotalPlan.Location = new Point(131, 70);
            txtTotalPlan.Name = "txtTotalPlan";
            txtTotalPlan.Size = new Size(178, 23);
            txtTotalPlan.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 73);
            label5.Name = "label5";
            label5.Size = new Size(61, 15);
            label5.TabIndex = 6;
            label5.Text = "Total plan:";
            // 
            // txtIdPlanPago
            // 
            txtIdPlanPago.Location = new Point(131, 41);
            txtIdPlanPago.Name = "txtIdPlanPago";
            txtIdPlanPago.Size = new Size(178, 23);
            txtIdPlanPago.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 44);
            label4.Name = "label4";
            label4.Size = new Size(77, 15);
            label4.TabIndex = 4;
            label4.Text = "ID plan pago:";
            // 
            // txtCliente
            // 
            txtCliente.Location = new Point(131, 12);
            txtCliente.Name = "txtCliente";
            txtCliente.Size = new Size(178, 23);
            txtCliente.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 15);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.No;
            label3.Size = new Size(47, 15);
            label3.TabIndex = 2;
            label3.Text = "Cliente:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 102);
            label1.Name = "label1";
            label1.Size = new Size(132, 15);
            label1.TabIndex = 4;
            label1.Text = "RESUMEN DEL CREDITO";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(txtIDVentaCredito);
            panel2.Controls.Add(label8);
            panel2.Location = new Point(12, 36);
            panel2.Name = "panel2";
            panel2.Size = new Size(1043, 55);
            panel2.TabIndex = 6;
            // 
            // txtIDVentaCredito
            // 
            txtIDVentaCredito.Location = new Point(131, 16);
            txtIDVentaCredito.Name = "txtIDVentaCredito";
            txtIDVentaCredito.Size = new Size(178, 23);
            txtIDVentaCredito.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 19);
            label8.Name = "label8";
            label8.RightToLeft = RightToLeft.No;
            label8.Size = new Size(93, 15);
            label8.TabIndex = 12;
            label8.Text = "ID venta crédito:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 18);
            label7.Name = "label7";
            label7.Size = new Size(49, 15);
            label7.TabIndex = 7;
            label7.Text = "FILTROS";
            // 
            // dgvDetallesCuotas
            // 
            dgvDetallesCuotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetallesCuotas.Location = new Point(12, 318);
            dgvDetallesCuotas.Name = "dgvDetallesCuotas";
            dgvDetallesCuotas.Size = new Size(1043, 328);
            dgvDetallesCuotas.TabIndex = 8;
            dgvDetallesCuotas.CellContentClick += dgvDetallesCuotas_CellContentClick;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 300);
            label9.Name = "label9";
            label9.Size = new Size(115, 15);
            label9.TabIndex = 9;
            label9.Text = "DETALLE DE CUOTAS";
            // 
            // lblRegistros
            // 
            lblRegistros.AutoSize = true;
            lblRegistros.Location = new Point(12, 649);
            lblRegistros.Name = "lblRegistros";
            lblRegistros.Size = new Size(55, 15);
            lblRegistros.TabIndex = 10;
            lblRegistros.Text = "Registros";
            // 
            // frmConsultarPlanPago
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 676);
            Controls.Add(lblRegistros);
            Controls.Add(label9);
            Controls.Add(dgvDetallesCuotas);
            Controls.Add(label7);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "frmConsultarPlanPago";
            Text = "frmConsultarPlanPago";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetallesCuotas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private TextBox txtEstadoCredito;
        private Label label2;
        private TextBox txtSaldoPendiente;
        private Label label6;
        private TextBox txtTotalPlan;
        private Label label5;
        private TextBox txtIdPlanPago;
        private Label label4;
        private TextBox txtCliente;
        private Label label3;
        private Label label1;
        private Panel panel2;
        private TextBox txtIDVentaCredito;
        private Label label8;
        private Label label7;
        private DataGridView dgvDetallesCuotas;
        private Label label9;
        private Label lblRegistros;
    }
}