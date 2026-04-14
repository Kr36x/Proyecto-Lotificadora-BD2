namespace LotificadoraApp.Lote
{
    partial class frmLote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLote));
            dgvLote = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            cmbBloque = new ComboBox();
            btnBuscar = new Button();
            label3 = new Label();
            txtNumeroLote = new TextBox();
            panel1 = new Panel();
            btnCrear = new Button();
            btnLimpiar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLote).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvLote
            // 
            dgvLote.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLote.Location = new Point(29, 254);
            dgvLote.Name = "dgvLote";
            dgvLote.RowHeadersWidth = 62;
            dgvLote.Size = new Size(1401, 441);
            dgvLote.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(16, 17);
            label1.Name = "label1";
            label1.Size = new Size(79, 25);
            label1.TabIndex = 1;
            label1.Text = "No Lote";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(312, 16);
            label2.Name = "label2";
            label2.Size = new Size(72, 25);
            label2.TabIndex = 3;
            label2.Text = "Bloque";
            // 
            // cmbBloque
            // 
            cmbBloque.FormattingEnabled = true;
            cmbBloque.Location = new Point(312, 43);
            cmbBloque.Name = "cmbBloque";
            cmbBloque.Size = new Size(191, 33);
            cmbBloque.TabIndex = 4;
            // 
            // btnBuscar
            // 
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image");
            btnBuscar.ImageAlign = ContentAlignment.MiddleRight;
            btnBuscar.Location = new Point(797, 19);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.RightToLeft = RightToLeft.No;
            btnBuscar.Size = new Size(182, 57);
            btnBuscar.TabIndex = 5;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnConsultar_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label3.Location = new Point(673, 12);
            label3.Name = "label3";
            label3.Size = new Size(105, 54);
            label3.TabIndex = 6;
            label3.Text = "Lote";
            // 
            // txtNumeroLote
            // 
            txtNumeroLote.Location = new Point(16, 45);
            txtNumeroLote.Name = "txtNumeroLote";
            txtNumeroLote.Size = new Size(191, 31);
            txtNumeroLote.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnCrear);
            panel1.Controls.Add(btnLimpiar);
            panel1.Controls.Add(btnBuscar);
            panel1.Controls.Add(txtNumeroLote);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(cmbBloque);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(29, 123);
            panel1.Name = "panel1";
            panel1.Size = new Size(1401, 97);
            panel1.TabIndex = 9;
            // 
            // btnCrear
            // 
            btnCrear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCrear.Image = (Image)resources.GetObject("btnCrear.Image");
            btnCrear.ImageAlign = ContentAlignment.MiddleRight;
            btnCrear.Location = new Point(1204, 19);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(182, 57);
            btnCrear.TabIndex = 10;
            btnCrear.Text = "Crear";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiar.Image = (Image)resources.GetObject("btnLimpiar.Image");
            btnLimpiar.ImageAlign = ContentAlignment.MiddleRight;
            btnLimpiar.Location = new Point(1004, 19);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(182, 57);
            btnLimpiar.TabIndex = 6;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // frmLote
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1464, 717);
            Controls.Add(label3);
            Controls.Add(dgvLote);
            Controls.Add(panel1);
            Name = "frmLote";
            StartPosition = FormStartPosition.CenterParent;
            Load += frmLote_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLote).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvLote;
        private Label label1;
        private Label label2;
        private ComboBox cmbBloque;
        private Button btnBuscar;
        private Label label3;
        private TextBox txtNumeroLote;
        private Panel panel1;
        private Button btnLimpiar;
        private Button btnCrear;
    }
}