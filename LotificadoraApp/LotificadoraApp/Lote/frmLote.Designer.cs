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
            textBox1 = new TextBox();
            label2 = new Label();
            cmbBloque = new ComboBox();
            btnConsultar = new Button();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            txtNumeroLote = new TextBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dgvLote).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
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
            label1.Location = new Point(29, 159);
            label1.Name = "label1";
            label1.Size = new Size(79, 25);
            label1.TabIndex = 1;
            label1.Text = "No Lote";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(29, 187);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(191, 31);
            textBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(624, 160);
            label2.Name = "label2";
            label2.Size = new Size(72, 25);
            label2.TabIndex = 3;
            label2.Text = "Bloque";
            // 
            // cmbBloque
            // 
            cmbBloque.FormattingEnabled = true;
            cmbBloque.Location = new Point(624, 187);
            cmbBloque.Name = "cmbBloque";
            cmbBloque.Size = new Size(191, 33);
            cmbBloque.TabIndex = 4;
            // 
            // btnConsultar
            // 
            btnConsultar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConsultar.Image = (Image)resources.GetObject("btnConsultar.Image");
            btnConsultar.ImageAlign = ContentAlignment.MiddleRight;
            btnConsultar.Location = new Point(1248, 161);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.RightToLeft = RightToLeft.No;
            btnConsultar.Size = new Size(182, 57);
            btnConsultar.TabIndex = 5;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
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
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.plus;
            pictureBox1.Location = new Point(1351, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(79, 82);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // txtNumeroLote
            // 
            txtNumeroLote.Location = new Point(29, 187);
            txtNumeroLote.Name = "txtNumeroLote";
            txtNumeroLote.Size = new Size(191, 31);
            txtNumeroLote.TabIndex = 2;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.broom;
            pictureBox2.Location = new Point(1248, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(97, 82);
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // frmLote
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1464, 717);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(btnConsultar);
            Controls.Add(cmbBloque);
            Controls.Add(label2);
            Controls.Add(txtNumeroLote);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(dgvLote);
            Name = "frmLote";
            StartPosition = FormStartPosition.CenterParent;
            Load += frmLote_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLote).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvLote;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private ComboBox cmbBloque;
        private Button btnConsultar;
        private Label label3;
        private PictureBox pictureBox1;
        private TextBox txtNumeroLote;
        private PictureBox pictureBox2;
    }
}