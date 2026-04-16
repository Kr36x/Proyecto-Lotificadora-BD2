namespace LotificadoraApp.Banco
{
    partial class frmBanco
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBanco));
            label4 = new Label();
            dgvBanco = new DataGridView();
            panel1 = new Panel();
            btnLimpiar = new Button();
            btnBuscar = new Button();
            cmbEstado = new ComboBox();
            label2 = new Label();
            btnActualizar = new Button();
            btnRecargar = new Button();
            btnCrear = new Button();
            panel2 = new Panel();
            btnEliminar = new Button();
            label20 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBanco).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label4.Location = new Point(413, 9);
            label4.Name = "label4";
            label4.Size = new Size(138, 54);
            label4.TabIndex = 46;
            label4.Text = "Banco";
            // 
            // dgvBanco
            // 
            dgvBanco.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBanco.Location = new Point(21, 223);
            dgvBanco.Name = "dgvBanco";
            dgvBanco.RowHeadersWidth = 62;
            dgvBanco.Size = new Size(912, 353);
            dgvBanco.TabIndex = 47;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnLimpiar);
            panel1.Controls.Add(btnBuscar);
            panel1.Controls.Add(cmbEstado);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(21, 91);
            panel1.Name = "panel1";
            panel1.Size = new Size(912, 110);
            panel1.TabIndex = 48;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiar.Image = (Image)resources.GetObject("btnLimpiar.Image");
            btnLimpiar.ImageAlign = ContentAlignment.MiddleRight;
            btnLimpiar.Location = new Point(705, 26);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(182, 57);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image");
            btnBuscar.ImageAlign = ContentAlignment.MiddleRight;
            btnBuscar.Location = new Point(495, 26);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(182, 57);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // cmbEstado
            // 
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Location = new Point(25, 50);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(239, 33);
            cmbEstado.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(25, 22);
            label2.Name = "label2";
            label2.Size = new Size(69, 25);
            label2.TabIndex = 0;
            label2.Text = "Estado";
            // 
            // btnActualizar
            // 
            btnActualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnActualizar.Image = (Image)resources.GetObject("btnActualizar.Image");
            btnActualizar.ImageAlign = ContentAlignment.MiddleRight;
            btnActualizar.Location = new Point(479, 36);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(182, 57);
            btnActualizar.TabIndex = 9;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // btnRecargar
            // 
            btnRecargar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRecargar.Image = (Image)resources.GetObject("btnRecargar.Image");
            btnRecargar.ImageAlign = ContentAlignment.MiddleRight;
            btnRecargar.Location = new Point(253, 36);
            btnRecargar.Name = "btnRecargar";
            btnRecargar.Size = new Size(182, 57);
            btnRecargar.TabIndex = 8;
            btnRecargar.Text = "Recargar";
            btnRecargar.UseVisualStyleBackColor = true;
            btnRecargar.Click += btnRecargar_Click;
            // 
            // btnCrear
            // 
            btnCrear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCrear.Image = (Image)resources.GetObject("btnCrear.Image");
            btnCrear.ImageAlign = ContentAlignment.MiddleRight;
            btnCrear.Location = new Point(25, 36);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(182, 57);
            btnCrear.TabIndex = 3;
            btnCrear.Text = "Crear";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnEliminar);
            panel2.Controls.Add(btnActualizar);
            panel2.Controls.Add(btnCrear);
            panel2.Controls.Add(btnRecargar);
            panel2.Location = new Point(21, 650);
            panel2.Name = "panel2";
            panel2.Size = new Size(912, 133);
            panel2.TabIndex = 49;
            // 
            // btnEliminar
            // 
            btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminar.Image = (Image)resources.GetObject("btnEliminar.Image");
            btnEliminar.ImageAlign = ContentAlignment.MiddleRight;
            btnEliminar.Location = new Point(705, 36);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(182, 57);
            btnEliminar.TabIndex = 10;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(21, 63);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(77, 25);
            label20.TabIndex = 40;
            label20.Text = "FILTROS";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 622);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(97, 25);
            label3.TabIndex = 50;
            label3.Text = "ACCIONES";
            // 
            // frmBanco
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(969, 809);
            Controls.Add(label3);
            Controls.Add(label20);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(dgvBanco);
            Controls.Add(label4);
            Name = "frmBanco";
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)dgvBanco).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label4;
        private DataGridView dgvBanco;
        private Panel panel1;
        private Button btnLimpiar;
        private Button btnCrear;
        private Button btnBuscar;
        private ComboBox cmbEstado;
        private Label label2;
        private Button btnRecargar;
        private Button btnActualizar;
        private Panel panel2;
        private Button btnEliminar;
        private Label label20;
        private Label label3;
    }
}