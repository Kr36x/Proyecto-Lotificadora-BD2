namespace LotificadoraApp
{
    partial class frmRegistrarAval
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
            label1 = new Label();
            label3 = new Label();
            txtIdentidad = new TextBox();
            label4 = new Label();
            txtNombres = new TextBox();
            label5 = new Label();
            txtApellidos = new TextBox();
            label7 = new Label();
            txtTelefono = new TextBox();
            label2 = new Label();
            txtDireccion = new TextBox();
            label12 = new Label();
            txtLugarTrabajo = new TextBox();
            label13 = new Label();
            cbParentesco = new ComboBox();
            panel1 = new Panel();
            txtIngresoMensual = new TextBox();
            label6 = new Label();
            panel4 = new Panel();
            txtIdGenerado = new TextBox();
            label9 = new Label();
            panel3 = new Panel();
            btnCerrar = new Button();
            btnLimpiar = new Button();
            btnGuardar = new Button();
            label20 = new Label();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 18;
            label1.Text = "DATOS DEL AVAL";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 18);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 2;
            label3.Text = "Identidad:";
            // 
            // txtIdentidad
            // 
            txtIdentidad.Location = new Point(146, 15);
            txtIdentidad.Name = "txtIdentidad";
            txtIdentidad.Size = new Size(178, 23);
            txtIdentidad.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 47);
            label4.Name = "label4";
            label4.Size = new Size(59, 15);
            label4.TabIndex = 4;
            label4.Text = "Nombres:";
            // 
            // txtNombres
            // 
            txtNombres.Location = new Point(146, 44);
            txtNombres.Name = "txtNombres";
            txtNombres.Size = new Size(178, 23);
            txtNombres.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 76);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 6;
            label5.Text = "Apellidos:";
            // 
            // txtApellidos
            // 
            txtApellidos.Location = new Point(146, 73);
            txtApellidos.Name = "txtApellidos";
            txtApellidos.Size = new Size(178, 23);
            txtApellidos.TabIndex = 7;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 109);
            label7.Name = "label7";
            label7.Size = new Size(55, 15);
            label7.TabIndex = 10;
            label7.Text = "Teléfono:";
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(146, 106);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(178, 23);
            txtTelefono.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 140);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 14;
            label2.Text = "Dirección:";
            // 
            // txtDireccion
            // 
            txtDireccion.Location = new Point(146, 137);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(178, 23);
            txtDireccion.TabIndex = 15;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(14, 174);
            label12.Name = "label12";
            label12.Size = new Size(96, 15);
            label12.TabIndex = 18;
            label12.Text = "Lugar de trabajo:";
            // 
            // txtLugarTrabajo
            // 
            txtLugarTrabajo.Location = new Point(146, 171);
            txtLugarTrabajo.Name = "txtLugarTrabajo";
            txtLugarTrabajo.Size = new Size(178, 23);
            txtLugarTrabajo.TabIndex = 19;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(14, 242);
            label13.Name = "label13";
            label13.Size = new Size(63, 15);
            label13.TabIndex = 20;
            label13.Text = "Parenteco:";
            // 
            // cbParentesco
            // 
            cbParentesco.FormattingEnabled = true;
            cbParentesco.Location = new Point(146, 242);
            cbParentesco.Name = "cbParentesco";
            cbParentesco.Size = new Size(178, 23);
            cbParentesco.TabIndex = 23;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(txtIngresoMensual);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(cbParentesco);
            panel1.Controls.Add(label13);
            panel1.Controls.Add(txtLugarTrabajo);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(txtDireccion);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtTelefono);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(txtApellidos);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtNombres);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtIdentidad);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(12, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(484, 291);
            panel1.TabIndex = 19;
            // 
            // txtIngresoMensual
            // 
            txtIngresoMensual.Location = new Point(146, 205);
            txtIngresoMensual.Name = "txtIngresoMensual";
            txtIngresoMensual.Size = new Size(178, 23);
            txtIngresoMensual.TabIndex = 25;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 208);
            label6.Name = "label6";
            label6.Size = new Size(97, 15);
            label6.TabIndex = 24;
            label6.Text = "Ingreso mensual:";
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(txtIdGenerado);
            panel4.Controls.Add(label9);
            panel4.Location = new Point(12, 397);
            panel4.Name = "panel4";
            panel4.Size = new Size(484, 50);
            panel4.TabIndex = 39;
            // 
            // txtIdGenerado
            // 
            txtIdGenerado.Location = new Point(154, 13);
            txtIdGenerado.Name = "txtIdGenerado";
            txtIdGenerado.Size = new Size(178, 23);
            txtIdGenerado.TabIndex = 34;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(22, 16);
            label9.Name = "label9";
            label9.Size = new Size(100, 15);
            label9.TabIndex = 33;
            label9.Text = "Id Aval Generado:";
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(btnCerrar);
            panel3.Controls.Add(btnLimpiar);
            panel3.Controls.Add(btnGuardar);
            panel3.Location = new Point(12, 324);
            panel3.Name = "panel3";
            panel3.Size = new Size(484, 51);
            panel3.TabIndex = 38;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(262, 16);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 28;
            btnCerrar.Text = "CERRAR";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(159, 16);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 27;
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(35, 16);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(96, 23);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "GUARDAR ";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(12, 379);
            label20.Name = "label20";
            label20.Size = new Size(69, 15);
            label20.TabIndex = 40;
            label20.Text = "RESULTADO";
            // 
            // frmRegistrarAval
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(509, 465);
            Controls.Add(label20);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "frmRegistrarAval";
            Text = "frmRegistrarAval";
            Load += frmRegistrarAval_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label3;
        private TextBox txtIdentidad;
        private Label label4;
        private TextBox txtNombres;
        private Label label5;
        private TextBox txtApellidos;
        private Label label7;
        private TextBox txtTelefono;
        private Label label2;
        private TextBox txtDireccion;
        private Label label12;
        private TextBox txtLugarTrabajo;
        private Label label13;
        private ComboBox cbParentesco;
        private Panel panel1;
        private TextBox txtIngresoMensual;
        private Label label6;
        private Panel panel4;
        private TextBox txtIdGenerado;
        private Label label9;
        private Panel panel3;
        private Button btnCerrar;
        private Button btnLimpiar;
        private Button btnGuardar;
        private Label label20;
    }
}