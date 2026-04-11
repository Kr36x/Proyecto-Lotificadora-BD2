namespace LotificadoraApp
{
    partial class frmConsultaVistaLotes
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
            label = new Label();
            txtIdProyecto = new TextBox();
            txtIdEtapa = new TextBox();
            label1 = new Label();
            dgvVistaLotes = new DataGridView();
            btnConsultar = new Button();
            btnLimpiar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvVistaLotes).BeginInit();
            SuspendLayout();
            // 
            // label
            // 
            label.AutoSize = true;
            label.Location = new Point(34, 21);
            label.Name = "label";
            label.Size = new Size(64, 15);
            label.TabIndex = 0;
            label.Text = "idProyecto";
            // 
            // txtIdProyecto
            // 
            txtIdProyecto.Location = new Point(104, 18);
            txtIdProyecto.Name = "txtIdProyecto";
            txtIdProyecto.Size = new Size(100, 23);
            txtIdProyecto.TabIndex = 1;
            // 
            // txtIdEtapa
            // 
            txtIdEtapa.Location = new Point(308, 21);
            txtIdEtapa.Name = "txtIdEtapa";
            txtIdEtapa.Size = new Size(100, 23);
            txtIdEtapa.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(238, 24);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 2;
            label1.Text = "id Etapas";
            // 
            // dgvVistaLotes
            // 
            dgvVistaLotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVistaLotes.Location = new Point(34, 76);
            dgvVistaLotes.Name = "dgvVistaLotes";
            dgvVistaLotes.Size = new Size(998, 585);
            dgvVistaLotes.TabIndex = 4;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(823, 24);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(75, 23);
            btnConsultar.TabIndex = 5;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(924, 24);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 23);
            btnLimpiar.TabIndex = 6;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // frmConsultaVistaLotes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 676);
            Controls.Add(btnLimpiar);
            Controls.Add(btnConsultar);
            Controls.Add(dgvVistaLotes);
            Controls.Add(txtIdEtapa);
            Controls.Add(label1);
            Controls.Add(txtIdProyecto);
            Controls.Add(label);
            Name = "frmConsultaVistaLotes";
            Text = "frmConsultaVistaLotes";
            Load += frmConsultaVistaLotes_Load;
            ((System.ComponentModel.ISupportInitialize)dgvVistaLotes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label;
        private TextBox txtIdProyecto;
        private TextBox txtIdEtapa;
        private Label label1;
        private DataGridView dgvVistaLotes;
        private Button btnConsultar;
        private Button btnLimpiar;
    }
}