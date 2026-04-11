namespace LotificadoraApp
{
    partial class frmConsultaRecaudacion
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
            Label label2;
            label1 = new Label();
            txtEtapa = new TextBox();
            lblFechaFin = new Label();
            pkrFechaInicio = new DateTimePicker();
            pkrFechaFin = new DateTimePicker();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            grdRecaudacion = new DataGridView();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)grdRecaudacion).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(220, 22);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 2;
            label2.Text = "Fecha Inicio:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 22);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 0;
            label1.Text = "Id Etapa:";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // txtEtapa
            // 
            txtEtapa.Location = new Point(96, 18);
            txtEtapa.Margin = new Padding(2, 2, 2, 2);
            txtEtapa.Name = "txtEtapa";
            txtEtapa.Size = new Size(106, 23);
            txtEtapa.TabIndex = 1;
            // 
            // lblFechaFin
            // 
            lblFechaFin.AutoSize = true;
            lblFechaFin.Location = new Point(422, 20);
            lblFechaFin.Margin = new Padding(2, 0, 2, 0);
            lblFechaFin.Name = "lblFechaFin";
            lblFechaFin.Size = new Size(60, 15);
            lblFechaFin.TabIndex = 3;
            lblFechaFin.Text = "Fecha Fin:";
            // 
            // pkrFechaInicio
            // 
            pkrFechaInicio.Location = new Point(306, 17);
            pkrFechaInicio.Margin = new Padding(2, 2, 2, 2);
            pkrFechaInicio.Name = "pkrFechaInicio";
            pkrFechaInicio.Size = new Size(98, 23);
            pkrFechaInicio.TabIndex = 4;
            // 
            // pkrFechaFin
            // 
            pkrFechaFin.Location = new Point(489, 17);
            pkrFechaFin.Margin = new Padding(2, 2, 2, 2);
            pkrFechaFin.Name = "pkrFechaFin";
            pkrFechaFin.Size = new Size(107, 23);
            pkrFechaFin.TabIndex = 5;
            pkrFechaFin.Value = new DateTime(2026, 4, 10, 13, 47, 5, 0);
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(852, 14);
            btnBuscar.Margin = new Padding(2, 2, 2, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(90, 26);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(957, 13);
            btnLimpiar.Margin = new Padding(2, 2, 2, 2);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(88, 27);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // grdRecaudacion
            // 
            grdRecaudacion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdRecaudacion.Location = new Point(27, 54);
            grdRecaudacion.Margin = new Padding(2, 2, 2, 2);
            grdRecaudacion.Name = "grdRecaudacion";
            grdRecaudacion.RowHeadersWidth = 62;
            grdRecaudacion.Size = new Size(1018, 600);
            grdRecaudacion.TabIndex = 8;
            // 
            // frmConsultaRecaudacion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 676);
            Controls.Add(grdRecaudacion);
            Controls.Add(btnLimpiar);
            Controls.Add(btnBuscar);
            Controls.Add(pkrFechaFin);
            Controls.Add(pkrFechaInicio);
            Controls.Add(lblFechaFin);
            Controls.Add(label2);
            Controls.Add(txtEtapa);
            Controls.Add(label1);
            Margin = new Padding(2, 2, 2, 2);
            Name = "frmConsultaRecaudacion";
            Text = "frmConsultaSpRecaudacion";
            ((System.ComponentModel.ISupportInitialize)grdRecaudacion).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtEtapa;
        private Label lblFechaFin;
        private DateTimePicker pkrFechaInicio;
        private DateTimePicker pkrFechaFin;
        private Button btnBuscar;
        private Button btnLimpiar;
        private DataGridView grdRecaudacion;
    }
}