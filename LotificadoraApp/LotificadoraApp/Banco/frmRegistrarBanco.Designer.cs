namespace LotificadoraApp.Banco
{
    partial class frmRegistrarBanco
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
            label8 = new Label();
            cmbEstado = new ComboBox();
            label7 = new Label();
            label2 = new Label();
            txtNombre = new TextBox();
            btnRegistrar = new Button();
            SuspendLayout();
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label8.Location = new Point(209, 9);
            label8.Name = "label8";
            label8.Size = new Size(320, 54);
            label8.TabIndex = 20;
            label8.Text = "Registrar Banco";
            // 
            // cmbEstado
            // 
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Location = new Point(376, 149);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(300, 33);
            cmbEstado.TabIndex = 35;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(376, 121);
            label7.Name = "label7";
            label7.Size = new Size(69, 25);
            label7.TabIndex = 34;
            label7.Text = "Estado";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(22, 121);
            label2.Name = "label2";
            label2.Size = new Size(81, 25);
            label2.TabIndex = 37;
            label2.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(22, 149);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(300, 31);
            txtNombre.TabIndex = 36;
            // 
            // btnRegistrar
            // 
            btnRegistrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegistrar.Location = new Point(295, 255);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(112, 34);
            btnRegistrar.TabIndex = 38;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // frmRegistrarBanco
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 332);
            Controls.Add(btnRegistrar);
            Controls.Add(label2);
            Controls.Add(txtNombre);
            Controls.Add(cmbEstado);
            Controls.Add(label7);
            Controls.Add(label8);
            Name = "frmRegistrarBanco";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label8;
        private ComboBox cmbEstado;
        private Label label7;
        private Label label2;
        private TextBox txtNombre;
        private Button btnRegistrar;
    }
}