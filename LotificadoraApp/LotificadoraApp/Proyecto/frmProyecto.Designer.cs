namespace LotificadoraApp.Proyecto
{
    partial class frmProyecto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProyecto));
            dgvProyecto = new DataGridView();
            label1 = new Label();
            panel1 = new Panel();
            btnLimpiar = new Button();
            btnCrear = new Button();
            btnBuscar = new Button();
            cmbEstado = new ComboBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProyecto).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvProyecto
            // 
            dgvProyecto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProyecto.Location = new Point(19, 152);
            dgvProyecto.Margin = new Padding(2, 2, 2, 2);
            dgvProyecto.Name = "dgvProyecto";
            dgvProyecto.RowHeadersWidth = 62;
            dgvProyecto.Size = new Size(916, 310);
            dgvProyecto.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label1.Location = new Point(423, 5);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(133, 37);
            label1.TabIndex = 1;
            label1.Text = "Proyecto";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnLimpiar);
            panel1.Controls.Add(btnCrear);
            panel1.Controls.Add(btnBuscar);
            panel1.Controls.Add(cmbEstado);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(19, 71);
            panel1.Margin = new Padding(2, 2, 2, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(916, 67);
            panel1.TabIndex = 2;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiar.Image = (Image)resources.GetObject("btnLimpiar.Image");
            btnLimpiar.ImageAlign = ContentAlignment.MiddleRight;
            btnLimpiar.Location = new Point(634, 16);
            btnLimpiar.Margin = new Padding(2, 2, 2, 2);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(127, 34);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnCrear
            // 
            btnCrear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCrear.Image = (Image)resources.GetObject("btnCrear.Image");
            btnCrear.ImageAlign = ContentAlignment.MiddleRight;
            btnCrear.Location = new Point(778, 16);
            btnCrear.Margin = new Padding(2, 2, 2, 2);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(127, 34);
            btnCrear.TabIndex = 3;
            btnCrear.Text = "Crear";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image");
            btnBuscar.ImageAlign = ContentAlignment.MiddleRight;
            btnBuscar.Location = new Point(490, 16);
            btnBuscar.Margin = new Padding(2, 2, 2, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(127, 34);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // cmbEstado
            // 
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Location = new Point(18, 30);
            cmbEstado.Margin = new Padding(2, 2, 2, 2);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(168, 23);
            cmbEstado.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(18, 13);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 0;
            label2.Text = "Estado";
            // 
            // frmProyecto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(951, 469);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(dgvProyecto);
            Margin = new Padding(2, 2, 2, 2);
            Name = "frmProyecto";
            Load += frmProyecto_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProyecto).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvProyecto;
        private Label label1;
        private Panel panel1;
        private Label label2;
        private ComboBox cmbEstado;
        private Button btnBuscar;
        private Button btnCrear;
        private Button btnLimpiar;
    }
}