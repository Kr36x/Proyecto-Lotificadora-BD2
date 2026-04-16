namespace LotificadoraApp.Estado
{
    partial class frmEstado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEstado));
            label4 = new Label();
            dgvEstado = new DataGridView();
            panel1 = new Panel();
            btnRecargar = new Button();
            btnEliminar = new Button();
            btnActualizar = new Button();
            btnCrear = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEstado).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label4.Location = new Point(320, 9);
            label4.Name = "label4";
            label4.Size = new Size(149, 54);
            label4.TabIndex = 47;
            label4.Text = "Estado";
            // 
            // dgvEstado
            // 
            dgvEstado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEstado.Location = new Point(25, 237);
            dgvEstado.Name = "dgvEstado";
            dgvEstado.RowHeadersWidth = 62;
            dgvEstado.Size = new Size(785, 324);
            dgvEstado.TabIndex = 48;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnRecargar);
            panel1.Controls.Add(btnEliminar);
            panel1.Controls.Add(btnActualizar);
            panel1.Controls.Add(btnCrear);
            panel1.Location = new Point(25, 103);
            panel1.Name = "panel1";
            panel1.Size = new Size(785, 109);
            panel1.TabIndex = 49;
            // 
            // btnRecargar
            // 
            btnRecargar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRecargar.Image = (Image)resources.GetObject("btnRecargar.Image");
            btnRecargar.ImageAlign = ContentAlignment.MiddleRight;
            btnRecargar.Location = new Point(204, 30);
            btnRecargar.Name = "btnRecargar";
            btnRecargar.Size = new Size(182, 57);
            btnRecargar.TabIndex = 50;
            btnRecargar.Text = "Recargar";
            btnRecargar.UseVisualStyleBackColor = true;
            btnRecargar.Click += btnRecargar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminar.Image = (Image)resources.GetObject("btnEliminar.Image");
            btnEliminar.ImageAlign = ContentAlignment.MiddleRight;
            btnEliminar.Location = new Point(580, 30);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(182, 57);
            btnEliminar.TabIndex = 52;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnActualizar
            // 
            btnActualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnActualizar.Image = (Image)resources.GetObject("btnActualizar.Image");
            btnActualizar.ImageAlign = ContentAlignment.MiddleRight;
            btnActualizar.Location = new Point(392, 30);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(182, 57);
            btnActualizar.TabIndex = 51;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // btnCrear
            // 
            btnCrear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCrear.Image = (Image)resources.GetObject("btnCrear.Image");
            btnCrear.ImageAlign = ContentAlignment.MiddleRight;
            btnCrear.Location = new Point(16, 30);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(182, 57);
            btnCrear.TabIndex = 50;
            btnCrear.Text = "Crear";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;
            // 
            // frmEstado
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(847, 601);
            Controls.Add(panel1);
            Controls.Add(dgvEstado);
            Controls.Add(label4);
            Name = "frmEstado";
            Load += frmEstado_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEstado).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label4;
        private DataGridView dgvEstado;
        private Panel panel1;
        private Button btnCrear;
        private Button btnActualizar;
        private Button btnEliminar;
        private Button btnRecargar;
    }
}