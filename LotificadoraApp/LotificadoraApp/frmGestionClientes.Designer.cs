namespace LotificadoraApp
{
    partial class frmGestionClientes
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
            label20 = new Label();
            panel4 = new Panel();
            txtBuscar = new TextBox();
            label9 = new Label();
            label1 = new Label();
            cbEstadoFiltro = new ComboBox();
            btnActualizar = new Button();
            btnLimpiar = new Button();
            btnBuscar = new Button();
            label2 = new Label();
            panel1 = new Panel();
            lblRegistros = new Label();
            label3 = new Label();
            panel2 = new Panel();
            btnCerrar = new Button();
            btnEliminar = new Button();
            btnEditar = new Button();
            btnNuevo = new Button();
            dgvClientes = new DataGridView();
            panel4.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            SuspendLayout();
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(24, 9);
            label20.Name = "label20";
            label20.Size = new Size(49, 15);
            label20.TabIndex = 39;
            label20.Text = "FILTROS";
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(btnActualizar);
            panel4.Controls.Add(btnLimpiar);
            panel4.Controls.Add(btnBuscar);
            panel4.Controls.Add(cbEstadoFiltro);
            panel4.Controls.Add(label1);
            panel4.Controls.Add(txtBuscar);
            panel4.Controls.Add(label9);
            panel4.Location = new Point(24, 27);
            panel4.Name = "panel4";
            panel4.Size = new Size(1007, 61);
            panel4.TabIndex = 38;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(194, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(178, 23);
            txtBuscar.TabIndex = 34;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(16, 15);
            label9.Name = "label9";
            label9.Size = new Size(172, 15);
            label9.TabIndex = 33;
            label9.Text = "Buscar por nombre / identidad:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(397, 15);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 35;
            label1.Text = "Estado:";
            // 
            // cbEstadoFiltro
            // 
            cbEstadoFiltro.FormattingEnabled = true;
            cbEstadoFiltro.Location = new Point(448, 12);
            cbEstadoFiltro.Name = "cbEstadoFiltro";
            cbEstadoFiltro.Size = new Size(121, 23);
            cbEstadoFiltro.TabIndex = 36;
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(900, 11);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(87, 23);
            btnActualizar.TabIndex = 39;
            btnActualizar.Text = "ACTUALIZAR";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(802, 11);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 38;
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(657, 11);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(118, 23);
            btnBuscar.TabIndex = 37;
            btnBuscar.Text = "BUSCAR";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 105);
            label2.Name = "label2";
            label2.Size = new Size(122, 15);
            label2.TabIndex = 41;
            label2.Text = "LISTADO DE CLIENTES";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dgvClientes);
            panel1.Location = new Point(24, 123);
            panel1.Name = "panel1";
            panel1.Size = new Size(1007, 425);
            panel1.TabIndex = 40;
            // 
            // lblRegistros
            // 
            lblRegistros.AutoSize = true;
            lblRegistros.Location = new Point(24, 555);
            lblRegistros.Name = "lblRegistros";
            lblRegistros.Size = new Size(55, 15);
            lblRegistros.TabIndex = 42;
            lblRegistros.Text = "Registros";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 587);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 44;
            label3.Text = "ACCIONES";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnNuevo);
            panel2.Controls.Add(btnCerrar);
            panel2.Controls.Add(btnEliminar);
            panel2.Controls.Add(btnEditar);
            panel2.Location = new Point(24, 605);
            panel2.Name = "panel2";
            panel2.Size = new Size(1007, 61);
            panel2.TabIndex = 43;
//            panel2.Paint += panel2_Paint;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(347, 21);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(87, 23);
            btnCerrar.TabIndex = 39;
            btnCerrar.Text = "CERRAR";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(249, 21);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 23);
            btnEliminar.TabIndex = 38;
            btnEliminar.Text = "ELIMINAR";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(104, 21);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(118, 23);
            btnEditar.TabIndex = 37;
            btnEditar.Text = "EDITAR";
            btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(16, 21);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(75, 23);
            btnNuevo.TabIndex = 40;
            btnNuevo.Text = "NUEVO";
            btnNuevo.UseVisualStyleBackColor = true;
            // 
            // dgvClientes
            // 
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Location = new Point(16, 16);
            dgvClientes.Name = "dgvClientes";
            dgvClientes.Size = new Size(971, 396);
            dgvClientes.TabIndex = 0;
            // 
            // frmGestionClientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 676);
            Controls.Add(label3);
            Controls.Add(panel2);
            Controls.Add(lblRegistros);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(label20);
            Controls.Add(panel4);
            Name = "frmGestionClientes";
            Text = "frmGestionClientes";
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Label label20;
        private Panel panel4;
        private ComboBox cbEstadoFiltro;
        private Label label1;
        private TextBox txtBuscar;
        private Label label9;
        private Button btnActualizar;
        private Button btnLimpiar;
        private Button btnBuscar;
        private Label label2;
        private Panel panel1;
        private Label lblRegistros;
        private Label label3;
        private Panel panel2;
        private Button btnNuevo;
        private Button btnCerrar;
        private Button btnEliminar;
        private Button btnEditar;
        private DataGridView dgvClientes;
    }
}