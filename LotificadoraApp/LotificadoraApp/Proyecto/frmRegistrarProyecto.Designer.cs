namespace LotificadoraApp.Proyecto
{
    partial class frmRegistrarProyecto
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
            label1 = new Label();
            txtNombreProyecto = new TextBox();
            txtDescripcion = new TextBox();
            label2 = new Label();
            pkrFechaInicio = new DateTimePicker();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtAreaTotal = new TextBox();
            pkrFechaFin = new DateTimePicker();
            label6 = new Label();
            txtAnioFinanciamiento = new TextBox();
            label7 = new Label();
            cmbEstado = new ComboBox();
            btnRegistrar = new Button();
            SuspendLayout();
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label8.Location = new Point(266, 9);
            label8.Name = "label8";
            label8.Size = new Size(373, 54);
            label8.TabIndex = 19;
            label8.Text = "Registrar Proyecto";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(34, 131);
            label1.Name = "label1";
            label1.Size = new Size(162, 25);
            label1.TabIndex = 20;
            label1.Text = "Nombre Proyecto";
            // 
            // txtNombreProyecto
            // 
            txtNombreProyecto.Location = new Point(34, 159);
            txtNombreProyecto.Name = "txtNombreProyecto";
            txtNombreProyecto.Size = new Size(223, 31);
            txtNombreProyecto.TabIndex = 21;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(341, 159);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(223, 31);
            txtDescripcion.TabIndex = 22;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(341, 131);
            label2.Name = "label2";
            label2.Size = new Size(111, 25);
            label2.TabIndex = 23;
            label2.Text = "Descripcion";
            // 
            // pkrFechaInicio
            // 
            pkrFechaInicio.Location = new Point(34, 288);
            pkrFechaInicio.Name = "pkrFechaInicio";
            pkrFechaInicio.Size = new Size(300, 31);
            pkrFechaInicio.TabIndex = 24;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(34, 260);
            label3.Name = "label3";
            label3.Size = new Size(113, 25);
            label3.TabIndex = 25;
            label3.Text = "Fecha Inicio";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(570, 260);
            label4.Name = "label4";
            label4.Size = new Size(173, 25);
            label4.TabIndex = 26;
            label4.Text = "Fecha Fin Estimada";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(647, 131);
            label5.Name = "label5";
            label5.Size = new Size(126, 25);
            label5.TabIndex = 27;
            label5.Text = "Área Total V2";
            // 
            // txtAreaTotal
            // 
            txtAreaTotal.Location = new Point(647, 159);
            txtAreaTotal.Name = "txtAreaTotal";
            txtAreaTotal.Size = new Size(223, 31);
            txtAreaTotal.TabIndex = 28;
            // 
            // pkrFechaFin
            // 
            pkrFechaFin.Location = new Point(570, 288);
            pkrFechaFin.Name = "pkrFechaFin";
            pkrFechaFin.Size = new Size(300, 31);
            pkrFechaFin.TabIndex = 29;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(34, 396);
            label6.Name = "label6";
            label6.Size = new Size(264, 25);
            label6.TabIndex = 30;
            label6.Text = "Máximo Años Financiamiento";
            // 
            // txtAnioFinanciamiento
            // 
            txtAnioFinanciamiento.Location = new Point(34, 439);
            txtAnioFinanciamiento.Name = "txtAnioFinanciamiento";
            txtAnioFinanciamiento.Size = new Size(300, 31);
            txtAnioFinanciamiento.TabIndex = 31;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(570, 396);
            label7.Name = "label7";
            label7.Size = new Size(69, 25);
            label7.TabIndex = 32;
            label7.Text = "Estado";
            // 
            // cmbEstado
            // 
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Location = new Point(570, 437);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(300, 33);
            cmbEstado.TabIndex = 33;
            // 
            // btnRegistrar
            // 
            btnRegistrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegistrar.Location = new Point(390, 532);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(112, 34);
            btnRegistrar.TabIndex = 34;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = true;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // frmRegistrarProyecto
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 590);
            Controls.Add(btnRegistrar);
            Controls.Add(cmbEstado);
            Controls.Add(label7);
            Controls.Add(txtAnioFinanciamiento);
            Controls.Add(label6);
            Controls.Add(pkrFechaFin);
            Controls.Add(txtAreaTotal);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(pkrFechaInicio);
            Controls.Add(label2);
            Controls.Add(txtDescripcion);
            Controls.Add(txtNombreProyecto);
            Controls.Add(label1);
            Controls.Add(label8);
            Name = "frmRegistrarProyecto";
            StartPosition = FormStartPosition.CenterScreen;
            Load += frmRegistrarProyecto_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label8;
        private Label label1;
        private TextBox txtNombreProyecto;
        private TextBox txtDescripcion;
        private Label label2;
        private DateTimePicker pkrFechaInicio;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtAreaTotal;
        private DateTimePicker pkrFechaFin;
        private Label label6;
        private TextBox txtAnioFinanciamiento;
        private Label label7;
        private ComboBox cmbEstado;
        private Button btnRegistrar;
    }
}