namespace LotificadoraApp
{
    partial class frmRegistrarLote
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
            cmbBloque = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            txtLote = new TextBox();
            txtArea = new TextBox();
            label3 = new Label();
            chkEsquina = new CheckBox();
            chkParque = new CheckBox();
            chkCalleCerrada = new CheckBox();
            label4 = new Label();
            txtPrecioBase = new TextBox();
            label5 = new Label();
            txtRecargoTotal = new TextBox();
            label6 = new Label();
            txtPrecioFinal = new TextBox();
            cmbEstado = new ComboBox();
            label7 = new Label();
            btnRegistrar = new Button();
            SuspendLayout();
            // 
            // cmbBloque
            // 
            cmbBloque.FormattingEnabled = true;
            cmbBloque.Location = new Point(45, 65);
            cmbBloque.Name = "cmbBloque";
            cmbBloque.Size = new Size(310, 33);
            cmbBloque.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(45, 37);
            label1.Name = "label1";
            label1.Size = new Size(67, 25);
            label1.TabIndex = 1;
            label1.Text = "Bloque";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(468, 37);
            label2.Name = "label2";
            label2.Size = new Size(79, 25);
            label2.TabIndex = 2;
            label2.Text = "No. Lote";
            // 
            // txtLote
            // 
            txtLote.Location = new Point(468, 65);
            txtLote.Name = "txtLote";
            txtLote.Size = new Size(310, 31);
            txtLote.TabIndex = 3;
            // 
            // txtArea
            // 
            txtArea.Location = new Point(850, 65);
            txtArea.Name = "txtArea";
            txtArea.Size = new Size(310, 31);
            txtArea.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(850, 37);
            label3.Name = "label3";
            label3.Size = new Size(74, 25);
            label3.TabIndex = 5;
            label3.Text = "Área V2";
            // 
            // chkEsquina
            // 
            chkEsquina.AutoSize = true;
            chkEsquina.Location = new Point(45, 162);
            chkEsquina.Name = "chkEsquina";
            chkEsquina.Size = new Size(137, 29);
            chkEsquina.TabIndex = 6;
            chkEsquina.Text = "¿Es Esquina?";
            chkEsquina.UseVisualStyleBackColor = true;
            // 
            // chkParque
            // 
            chkParque.AutoSize = true;
            chkParque.Location = new Point(468, 162);
            chkParque.Name = "chkParque";
            chkParque.Size = new Size(221, 29);
            chkParque.TabIndex = 7;
            chkParque.Text = "¿Está cerca del parque?";
            chkParque.UseVisualStyleBackColor = true;
            // 
            // chkCalleCerrada
            // 
            chkCalleCerrada.AutoSize = true;
            chkCalleCerrada.Location = new Point(850, 162);
            chkCalleCerrada.Name = "chkCalleCerrada";
            chkCalleCerrada.Size = new Size(173, 29);
            chkCalleCerrada.TabIndex = 8;
            chkCalleCerrada.Text = "¿Es calle cerrada?";
            chkCalleCerrada.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(45, 267);
            label4.Name = "label4";
            label4.Size = new Size(101, 25);
            label4.TabIndex = 9;
            label4.Text = "Precio Base";
            // 
            // txtPrecioBase
            // 
            txtPrecioBase.Location = new Point(45, 295);
            txtPrecioBase.Name = "txtPrecioBase";
            txtPrecioBase.Size = new Size(310, 31);
            txtPrecioBase.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(468, 267);
            label5.Name = "label5";
            label5.Size = new Size(118, 25);
            label5.TabIndex = 11;
            label5.Text = "Recargo Total";
            // 
            // txtRecargoTotal
            // 
            txtRecargoTotal.Location = new Point(468, 295);
            txtRecargoTotal.Name = "txtRecargoTotal";
            txtRecargoTotal.Size = new Size(310, 31);
            txtRecargoTotal.TabIndex = 12;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(850, 267);
            label6.Name = "label6";
            label6.Size = new Size(101, 25);
            label6.TabIndex = 13;
            label6.Text = "Precio Final";
            // 
            // txtPrecioFinal
            // 
            txtPrecioFinal.Location = new Point(850, 295);
            txtPrecioFinal.Name = "txtPrecioFinal";
            txtPrecioFinal.Size = new Size(310, 31);
            txtPrecioFinal.TabIndex = 14;
            // 
            // cmbEstado
            // 
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Location = new Point(45, 456);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(310, 33);
            cmbEstado.TabIndex = 15;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(45, 428);
            label7.Name = "label7";
            label7.Size = new Size(66, 25);
            label7.TabIndex = 16;
            label7.Text = "Estado";
            // 
            // btnRegistrar
            // 
            btnRegistrar.Location = new Point(551, 558);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(112, 34);
            btnRegistrar.TabIndex = 17;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // frmRegistrarLote
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1245, 663);
            Controls.Add(btnRegistrar);
            Controls.Add(label7);
            Controls.Add(cmbEstado);
            Controls.Add(txtPrecioFinal);
            Controls.Add(label6);
            Controls.Add(txtRecargoTotal);
            Controls.Add(label5);
            Controls.Add(txtPrecioBase);
            Controls.Add(label4);
            Controls.Add(chkCalleCerrada);
            Controls.Add(chkParque);
            Controls.Add(chkEsquina);
            Controls.Add(label3);
            Controls.Add(txtArea);
            Controls.Add(txtLote);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cmbBloque);
            Name = "frmRegistrarLote";
            Text = "frmRegistrarLote";
            Load += frmRegistrarLote_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbBloque;
        private Label label1;
        private Label label2;
        private TextBox txtLote;
        private TextBox txtArea;
        private Label label3;
        private CheckBox chkEsquina;
        private CheckBox chkParque;
        private CheckBox chkCalleCerrada;
        private Label label4;
        private TextBox txtPrecioBase;
        private Label label5;
        private TextBox txtRecargoTotal;
        private Label label6;
        private TextBox txtPrecioFinal;
        private ComboBox cmbEstado;
        private Label label7;
        private Button btnRegistrar;
    }
}