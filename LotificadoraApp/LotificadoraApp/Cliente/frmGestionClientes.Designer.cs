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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGestionClientes));
            label20 = new Label();
            panel4 = new Panel();
            btnActualizar = new Button();
            btnLimpiar = new Button();
            btnBuscar = new Button();
            cbEstadoFiltro = new ComboBox();
            label1 = new Label();
            txtBuscar = new TextBox();
            label9 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            dgvClientes = new DataGridView();
            lblRegistros = new Label();
            label3 = new Label();
            panel2 = new Panel();
            btnNuevo = new Button();
            btnCerrar = new Button();
            btnEliminar = new Button();
            btnEditar = new Button();
            label4 = new Label();
            panel4.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(34, 112);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(77, 25);
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
            panel4.Location = new Point(34, 142);
            panel4.Margin = new Padding(4, 5, 4, 5);
            panel4.Name = "panel4";
            panel4.Size = new Size(1439, 102);
            panel4.TabIndex = 38;
            // 
            // btnActualizar
            // 
            btnActualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnActualizar.Image = (Image)resources.GetObject("btnActualizar.Image");
            btnActualizar.ImageAlign = ContentAlignment.MiddleRight;
            btnActualizar.Location = new Point(1261, 27);
            btnActualizar.Margin = new Padding(4, 5, 4, 5);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(169, 49);
            btnActualizar.TabIndex = 39;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiar.Image = (Image)resources.GetObject("btnLimpiar.Image");
            btnLimpiar.ImageAlign = ContentAlignment.MiddleRight;
            btnLimpiar.Location = new Point(1084, 25);
            btnLimpiar.Margin = new Padding(4, 5, 4, 5);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(169, 51);
            btnLimpiar.TabIndex = 38;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image");
            btnBuscar.ImageAlign = ContentAlignment.MiddleRight;
            btnBuscar.Location = new Point(907, 25);
            btnBuscar.Margin = new Padding(4, 5, 4, 5);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(169, 51);
            btnBuscar.TabIndex = 37;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // cbEstadoFiltro
            // 
            cbEstadoFiltro.FormattingEnabled = true;
            cbEstadoFiltro.Location = new Point(456, 53);
            cbEstadoFiltro.Margin = new Padding(4, 5, 4, 5);
            cbEstadoFiltro.Name = "cbEstadoFiltro";
            cbEstadoFiltro.Size = new Size(273, 33);
            cbEstadoFiltro.TabIndex = 36;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(456, 23);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(69, 25);
            label1.TabIndex = 35;
            label1.Text = "Estado";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(23, 55);
            txtBuscar.Margin = new Padding(4, 5, 4, 5);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(273, 31);
            txtBuscar.TabIndex = 34;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label9.Location = new Point(23, 25);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(273, 25);
            label9.TabIndex = 33;
            label9.Text = "Buscar por nombre / identidad";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 272);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(188, 25);
            label2.TabIndex = 41;
            label2.Text = "LISTADO DE CLIENTES";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dgvClientes);
            panel1.Location = new Point(34, 302);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1439, 619);
            panel1.TabIndex = 40;
            // 
            // dgvClientes
            // 
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Location = new Point(23, 27);
            dgvClientes.Margin = new Padding(4, 5, 4, 5);
            dgvClientes.Name = "dgvClientes";
            dgvClientes.RowHeadersWidth = 62;
            dgvClientes.Size = new Size(1387, 660);
            dgvClientes.TabIndex = 0;
            // 
            // lblRegistros
            // 
            lblRegistros.AutoSize = true;
            lblRegistros.Location = new Point(34, 926);
            lblRegistros.Margin = new Padding(4, 0, 4, 0);
            lblRegistros.Name = "lblRegistros";
            lblRegistros.Size = new Size(85, 25);
            lblRegistros.TabIndex = 42;
            lblRegistros.Text = "Registros";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(34, 979);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(97, 25);
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
            panel2.Location = new Point(34, 1009);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(1439, 102);
            panel2.TabIndex = 43;
            // 
            // btnNuevo
            // 
            btnNuevo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevo.Image = (Image)resources.GetObject("btnNuevo.Image");
            btnNuevo.ImageAlign = ContentAlignment.MiddleRight;
            btnNuevo.Location = new Point(4, 29);
            btnNuevo.Margin = new Padding(4, 5, 4, 5);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(169, 51);
            btnNuevo.TabIndex = 40;
            btnNuevo.Text = "Crear";
            btnNuevo.UseVisualStyleBackColor = true;
            // 
            // btnCerrar
            // 
            btnCerrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCerrar.Image = (Image)resources.GetObject("btnCerrar.Image");
            btnCerrar.ImageAlign = ContentAlignment.MiddleRight;
            btnCerrar.Location = new Point(535, 29);
            btnCerrar.Margin = new Padding(4, 5, 4, 5);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(169, 51);
            btnCerrar.TabIndex = 39;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminar.Image = (Image)resources.GetObject("btnEliminar.Image");
            btnEliminar.ImageAlign = ContentAlignment.MiddleRight;
            btnEliminar.Location = new Point(358, 29);
            btnEliminar.Margin = new Padding(4, 5, 4, 5);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(169, 51);
            btnEliminar.TabIndex = 38;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            btnEditar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditar.Image = (Image)resources.GetObject("btnEditar.Image");
            btnEditar.ImageAlign = ContentAlignment.MiddleRight;
            btnEditar.Location = new Point(181, 29);
            btnEditar.Margin = new Padding(4, 5, 4, 5);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(169, 51);
            btnEditar.TabIndex = 37;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label4.Location = new Point(721, 20);
            label4.Name = "label4";
            label4.Size = new Size(154, 54);
            label4.TabIndex = 45;
            label4.Text = "Cliente";
            // 
            // frmGestionClientes
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1524, 1127);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(panel2);
            Controls.Add(lblRegistros);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(label20);
            Controls.Add(panel4);
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmGestionClientes";
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            panel2.ResumeLayout(false);
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
        private Label label4;
    }
}