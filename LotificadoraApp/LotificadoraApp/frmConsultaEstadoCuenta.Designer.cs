namespace LotificadoraApp
{
    partial class frmConsultaEstadoCuenta
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
            Button btnConsultar;
            label1 = new Label();
            txtCliente = new TextBox();
            btnLimpiar = new Button();
            grdEstadoCuenta = new DataGridView();
            btnConsultar = new Button();
            ((System.ComponentModel.ISupportInitialize)grdEstadoCuenta).BeginInit();
            SuspendLayout();
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(908, 37);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(112, 34);
            btnConsultar.TabIndex = 2;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += txtConsultar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 46);
            label1.Name = "label1";
            label1.Size = new Size(90, 25);
            label1.TabIndex = 0;
            label1.Text = "Id Cliente:";
            // 
            // txtCliente
            // 
            txtCliente.Location = new Point(139, 40);
            txtCliente.Name = "txtCliente";
            txtCliente.Size = new Size(150, 31);
            txtCliente.TabIndex = 1;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(1037, 37);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(112, 34);
            btnLimpiar.TabIndex = 3;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // grdEstadoCuenta
            // 
            grdEstadoCuenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdEstadoCuenta.Location = new Point(43, 107);
            grdEstadoCuenta.Name = "grdEstadoCuenta";
            grdEstadoCuenta.RowHeadersWidth = 62;
            grdEstadoCuenta.Size = new Size(1106, 576);
            grdEstadoCuenta.TabIndex = 4;
            // 
            // frmConsultaSpEstadoCuenta
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1210, 733);
            Controls.Add(grdEstadoCuenta);
            Controls.Add(btnLimpiar);
            Controls.Add(btnConsultar);
            Controls.Add(txtCliente);
            Controls.Add(label1);
            Name = "frmConsultaSpEstadoCuenta";
            Text = "frmConsultaSpEstadoCuenta";
            ((System.ComponentModel.ISupportInitialize)grdEstadoCuenta).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtCliente;
        private Button btnLimpiar;
        private DataGridView grdEstadoCuenta;
    }
}