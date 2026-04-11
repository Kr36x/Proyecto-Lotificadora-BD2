namespace LotificadoraApp
{
    partial class frmRegistrarPago
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
            txtEstadoCuota = new TextBox();
            label7 = new Label();
            txtSaldoPendiente = new TextBox();
            label8 = new Label();
            txtMontoCuota = new TextBox();
            label9 = new Label();
            txtIDcuota = new TextBox();
            label10 = new Label();
            cbCuotaPendiente = new ComboBox();
            label11 = new Label();
            txtEtapa = new TextBox();
            label6 = new Label();
            txtLote = new TextBox();
            label5 = new Label();
            txtCliente = new TextBox();
            label4 = new Label();
            txtIdVenta = new TextBox();
            label3 = new Label();
            cmbVentaCredito = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            textBox1 = new TextBox();
            label13 = new Label();
            rbDeposito = new RadioButton();
            rbEfectivo = new RadioButton();
            label12 = new Label();
            txtMontoAPagar = new TextBox();
            label19 = new Label();
            txtNoReferencia = new TextBox();
            label20 = new Label();
            cbCuentaBancaria = new ComboBox();
            label21 = new Label();
            label22 = new Label();
            btnCerrar = new Button();
            btnLimpiar = new Button();
            btnRegistrarPago = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(txtEstadoCuota);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(txtSaldoPendiente);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(txtMontoCuota);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(txtIDcuota);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(cbCuotaPendiente);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(txtEtapa);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtLote);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtCliente);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtIdVenta);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(cmbVentaCredito);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(23, 88);
            panel1.Name = "panel1";
            panel1.Size = new Size(488, 380);
            panel1.TabIndex = 3;
            // 
            // txtEstadoCuota
            // 
            txtEstadoCuota.Location = new Point(125, 315);
            txtEstadoCuota.Name = "txtEstadoCuota";
            txtEstadoCuota.Size = new Size(178, 23);
            txtEstadoCuota.TabIndex = 19;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 318);
            label7.Name = "label7";
            label7.Size = new Size(78, 15);
            label7.TabIndex = 18;
            label7.Text = "Estado cuota:";
            // 
            // txtSaldoPendiente
            // 
            txtSaldoPendiente.Location = new Point(125, 286);
            txtSaldoPendiente.Name = "txtSaldoPendiente";
            txtSaldoPendiente.Size = new Size(178, 23);
            txtSaldoPendiente.TabIndex = 17;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 289);
            label8.Name = "label8";
            label8.Size = new Size(95, 15);
            label8.TabIndex = 16;
            label8.Text = "Saldo pendiente:";
            // 
            // txtMontoCuota
            // 
            txtMontoCuota.Location = new Point(125, 257);
            txtMontoCuota.Name = "txtMontoCuota";
            txtMontoCuota.Size = new Size(178, 23);
            txtMontoCuota.TabIndex = 15;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 260);
            label9.Name = "label9";
            label9.Size = new Size(79, 15);
            label9.TabIndex = 14;
            label9.Text = "Monto cuota:";
            // 
            // txtIDcuota
            // 
            txtIDcuota.Location = new Point(125, 228);
            txtIDcuota.Name = "txtIDcuota";
            txtIDcuota.Size = new Size(178, 23);
            txtIDcuota.TabIndex = 13;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 231);
            label10.Name = "label10";
            label10.Size = new Size(54, 15);
            label10.TabIndex = 12;
            label10.Text = "ID cuota:";
            // 
            // cbCuotaPendiente
            // 
            cbCuotaPendiente.FormattingEnabled = true;
            cbCuotaPendiente.Location = new Point(125, 190);
            cbCuotaPendiente.Name = "cbCuotaPendiente";
            cbCuotaPendiente.Size = new Size(338, 23);
            cbCuotaPendiente.TabIndex = 11;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(12, 193);
            label11.Name = "label11";
            label11.Size = new Size(101, 15);
            label11.TabIndex = 10;
            label11.Text = "Cuota pendiente: ";
            // 
            // txtEtapa
            // 
            txtEtapa.Location = new Point(98, 135);
            txtEtapa.Name = "txtEtapa";
            txtEtapa.Size = new Size(178, 23);
            txtEtapa.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 138);
            label6.Name = "label6";
            label6.Size = new Size(39, 15);
            label6.TabIndex = 8;
            label6.Text = "Etapa:";
            // 
            // txtLote
            // 
            txtLote.Location = new Point(98, 106);
            txtLote.Name = "txtLote";
            txtLote.Size = new Size(178, 23);
            txtLote.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 109);
            label5.Name = "label5";
            label5.Size = new Size(33, 15);
            label5.TabIndex = 6;
            label5.Text = "Lote:";
            // 
            // txtCliente
            // 
            txtCliente.Location = new Point(98, 77);
            txtCliente.Name = "txtCliente";
            txtCliente.Size = new Size(178, 23);
            txtCliente.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 80);
            label4.Name = "label4";
            label4.Size = new Size(47, 15);
            label4.TabIndex = 4;
            label4.Text = "Cliente:";
            // 
            // txtIdVenta
            // 
            txtIdVenta.Location = new Point(98, 48);
            txtIdVenta.Name = "txtIdVenta";
            txtIdVenta.Size = new Size(178, 23);
            txtIdVenta.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 51);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 2;
            label3.Text = "ID venta:";
            // 
            // cmbVentaCredito
            // 
            cmbVentaCredito.FormattingEnabled = true;
            cmbVentaCredito.Location = new Point(98, 10);
            cmbVentaCredito.Name = "cmbVentaCredito";
            cmbVentaCredito.Size = new Size(338, 23);
            cmbVentaCredito.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 13);
            label2.Name = "label2";
            label2.Size = new Size(84, 15);
            label2.TabIndex = 0;
            label2.Text = "Venta Crédito: ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 70);
            label1.Name = "label1";
            label1.Size = new Size(123, 15);
            label1.TabIndex = 2;
            label1.Text = "SELECCIÓN DE VENTA";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(label13);
            panel2.Controls.Add(rbDeposito);
            panel2.Controls.Add(rbEfectivo);
            panel2.Controls.Add(label12);
            panel2.Controls.Add(txtMontoAPagar);
            panel2.Controls.Add(label19);
            panel2.Controls.Add(txtNoReferencia);
            panel2.Controls.Add(label20);
            panel2.Controls.Add(cbCuentaBancaria);
            panel2.Controls.Add(label21);
            panel2.Location = new Point(523, 88);
            panel2.Name = "panel2";
            panel2.Size = new Size(520, 380);
            panel2.TabIndex = 21;
            panel2.Paint += panel2_Paint;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(15, 252);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(482, 81);
            textBox1.TabIndex = 10;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(15, 226);
            label13.Name = "label13";
            label13.Size = new Size(76, 15);
            label13.TabIndex = 9;
            label13.Text = "Observación:";
            // 
            // rbDeposito
            // 
            rbDeposito.AutoSize = true;
            rbDeposito.Location = new Point(71, 55);
            rbDeposito.Name = "rbDeposito";
            rbDeposito.Size = new Size(72, 19);
            rbDeposito.TabIndex = 8;
            rbDeposito.TabStop = true;
            rbDeposito.Text = "Déposito";
            rbDeposito.UseVisualStyleBackColor = true;
            // 
            // rbEfectivo
            // 
            rbEfectivo.AutoSize = true;
            rbEfectivo.Location = new Point(71, 30);
            rbEfectivo.Name = "rbEfectivo";
            rbEfectivo.Size = new Size(67, 19);
            rbEfectivo.TabIndex = 7;
            rbEfectivo.TabStop = true;
            rbEfectivo.Text = "Efectivo";
            rbEfectivo.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(18, 10);
            label12.Name = "label12";
            label12.Size = new Size(90, 15);
            label12.TabIndex = 6;
            label12.Text = "Forma de pago:";
            // 
            // txtMontoAPagar
            // 
            txtMontoAPagar.Location = new Point(117, 178);
            txtMontoAPagar.Name = "txtMontoAPagar";
            txtMontoAPagar.Size = new Size(178, 23);
            txtMontoAPagar.TabIndex = 5;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(15, 181);
            label19.Name = "label19";
            label19.Size = new Size(88, 15);
            label19.TabIndex = 4;
            label19.Text = "Monto a pagar:";
            // 
            // txtNoReferencia
            // 
            txtNoReferencia.Location = new Point(117, 149);
            txtNoReferencia.Name = "txtNoReferencia";
            txtNoReferencia.Size = new Size(178, 23);
            txtNoReferencia.TabIndex = 3;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(15, 152);
            label20.Name = "label20";
            label20.Size = new Size(87, 15);
            label20.TabIndex = 2;
            label20.Text = "No. Referencia:";
            // 
            // cbCuentaBancaria
            // 
            cbCuentaBancaria.FormattingEnabled = true;
            cbCuentaBancaria.Location = new Point(117, 111);
            cbCuentaBancaria.Name = "cbCuentaBancaria";
            cbCuentaBancaria.Size = new Size(338, 23);
            cbCuentaBancaria.TabIndex = 1;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(15, 114);
            label21.Name = "label21";
            label21.Size = new Size(96, 15);
            label21.TabIndex = 0;
            label21.Text = "Cuenta bancaria:";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(523, 70);
            label22.Name = "label22";
            label22.Size = new Size(99, 15);
            label22.TabIndex = 20;
            label22.Text = "DATOS DEL PAGO";
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(945, 530);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 29;
            btnCerrar.Text = "CERRAR";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(849, 530);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 28;
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnRegistrarPago
            // 
            btnRegistrarPago.Location = new Point(683, 530);
            btnRegistrarPago.Name = "btnRegistrarPago";
            btnRegistrarPago.Size = new Size(135, 23);
            btnRegistrarPago.TabIndex = 27;
            btnRegistrarPago.Text = "REGISTRAR PAGO";
            btnRegistrarPago.UseVisualStyleBackColor = true;
            // 
            // frmRegistrarPago
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 676);
            Controls.Add(btnCerrar);
            Controls.Add(btnLimpiar);
            Controls.Add(btnRegistrarPago);
            Controls.Add(panel2);
            Controls.Add(label22);
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "frmRegistrarPago";
            Text = "frmRegistrarPago";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private TextBox txtEstadoCuota;
        private Label label7;
        private TextBox txtSaldoPendiente;
        private Label label8;
        private TextBox txtMontoCuota;
        private Label label9;
        private TextBox txtIDcuota;
        private Label label10;
        private ComboBox cbCuotaPendiente;
        private Label label11;
        private TextBox txtEtapa;
        private Label label6;
        private TextBox txtLote;
        private Label label5;
        private TextBox txtCliente;
        private Label label4;
        private TextBox txtIdVenta;
        private Label label3;
        private ComboBox cmbVentaCredito;
        private Label label2;
        private Label label1;
        private Panel panel2;
        private RadioButton rbDeposito;
        private RadioButton rbEfectivo;
        private Label label12;
        private TextBox txtMontoAPagar;
        private Label label19;
        private TextBox txtNoReferencia;
        private Label label20;
        private ComboBox cbCuentaBancaria;
        private Label label21;
        private Label label22;
        private TextBox textBox1;
        private Label label13;
        private Button btnCerrar;
        private Button btnLimpiar;
        private Button btnRegistrarPago;
    }
}