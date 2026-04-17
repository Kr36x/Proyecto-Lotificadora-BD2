using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace LotificadoraApp.GestionGastos
{
    public partial class frmGestionGasto : Form
    {
        public frmGestionGasto()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmGestionGasto_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnCrear.Click += btnCrear_Click;
            chkUsarFechas.CheckedChanged += chkUsarFechas_CheckedChanged;
        }

        private void frmGestionGasto_Load(object? sender, EventArgs e)
        {
            ConfigurarGrid();
            ConfigurarDatePickers();
            CargarProyectos();

            chkUsarFechas.Checked = false;
            dtpFechaInicio.Enabled = false;
            dtpFechaFin.Enabled = false;
        }
        private void chkUsarFechas_CheckedChanged(object? sender, EventArgs e)
        {
            dtpFechaInicio.Enabled = chkUsarFechas.Checked;
            dtpFechaFin.Enabled = chkUsarFechas.Checked;
        }
        private void ConfigurarGrid()
        {
            dgvGastos.AutoGenerateColumns = true;
            dgvGastos.ReadOnly = true;
            dgvGastos.MultiSelect = false;
            dgvGastos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGastos.AllowUserToAddRows = false;
            dgvGastos.AllowUserToDeleteRows = false;
            dgvGastos.AllowUserToResizeRows = false;
            dgvGastos.RowHeadersVisible = false;
            dgvGastos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGastos.ColumnHeadersHeight = 40;
            dgvGastos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvGastos.EnableHeadersVisualStyles = false;
            dgvGastos.BackgroundColor = Color.White;
            dgvGastos.BorderStyle = BorderStyle.None;
            dgvGastos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvGastos.GridColor = Color.FromArgb(210, 210, 210);

            dgvGastos.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvGastos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGastos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvGastos.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvGastos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvGastos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvGastos.DefaultCellStyle.BackColor = Color.White;
            dgvGastos.DefaultCellStyle.ForeColor = Color.Black;
            dgvGastos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGastos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGastos.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvGastos.DefaultCellStyle.Padding = new Padding(3);

            dgvGastos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvGastos.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvGastos.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGastos.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvGastos.RowTemplate.Height = 32;

            dgvGastos.ThemeStyle.BackColor = Color.White;
            dgvGastos.ThemeStyle.GridColor = Color.FromArgb(210, 210, 210);
            dgvGastos.ThemeStyle.HeaderStyle.BackColor = Color.Olive;
            dgvGastos.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvGastos.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvGastos.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvGastos.ThemeStyle.HeaderStyle.Height = 40;
            dgvGastos.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvGastos.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgvGastos.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGastos.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvGastos.ThemeStyle.RowsStyle.Height = 32;
            dgvGastos.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvGastos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvGastos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Black;
        }

        private void ConfigurarDatePickers()
        {
            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpFechaFin.Value = DateTime.Today;

            dtpFechaInicio.FillColor = Color.White;
            dtpFechaFin.FillColor = Color.White;
            dtpFechaInicio.ForeColor = Color.Black;
            dtpFechaFin.ForeColor = Color.Black;
        }

        private void CargarProyectos()
        {
            try
            {
                DataTable dt = Db.ExecuteQuery(@"
                    SELECT idProyecto, nombreProyecto
                    FROM Proyecto
                    WHERE estadoId = 1
                    ORDER BY nombreProyecto;
                ");

                DataRow filaTodos = dt.NewRow();
                filaTodos["idProyecto"] = 0;
                filaTodos["nombreProyecto"] = "Todos";
                dt.Rows.InsertAt(filaTodos, 0);

                cmbProyecto.DataSource = dt;
                cmbProyecto.DisplayMember = "nombreProyecto";
                cmbProyecto.ValueMember = "idProyecto";
                cmbProyecto.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar proyectos: " + ex.Message);
            }
        }

        private void CargarGastos()
        {
            try
            {
                int idProyecto = cmbProyecto.SelectedValue == null ? 0 : Convert.ToInt32(cmbProyecto.SelectedValue);

                if (idProyecto <= 0)
                {
                    dgvGastos.DataSource = new DataTable();
                    return;
                }

                object fechaInicio = DBNull.Value;
                object fechaFin = DBNull.Value;

                if (chkUsarFechas.Checked)
                {
                    if (dtpFechaInicio.Value.Date > dtpFechaFin.Value.Date)
                    {
                        MostrarWarning("fechaInicio no puede ser mayor que fechaFin.");
                        return;
                    }

                    fechaInicio = dtpFechaInicio.Value.Date;
                    fechaFin = dtpFechaFin.Value.Date;
                }

                DataTable dt = Db.ExecuteQuery(@"
            SELECT *
            FROM dbo.fn_tvf_gastos_proyecto(@idProyecto, @fechaInicio, @fechaFin)
            ORDER BY fechaGasto DESC, idGasto DESC;
        ",
                new SqlParameter("@idProyecto", idProyecto),
                new SqlParameter("@fechaInicio", fechaInicio),
                new SqlParameter("@fechaFin", fechaFin));

                dgvGastos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar gastos: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            if (cmbProyecto.SelectedValue == null || Convert.ToInt32(cmbProyecto.SelectedValue) <= 0)
            {
                MostrarWarning("Seleccione un proyecto.");
                cmbProyecto.Focus();
                return;
            }

            if (dtpFechaInicio.Value.Date > dtpFechaFin.Value.Date)
            {
                MostrarWarning("fechaInicio no puede ser mayor que fechaFin.");
                return;
            }

            CargarGastos();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            cmbProyecto.SelectedValue = 0;
            chkUsarFechas.Checked = false;
            dtpFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpFechaFin.Value = DateTime.Today;
            dgvGastos.DataSource = new DataTable();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            if (cmbProyecto.SelectedValue != null && Convert.ToInt32(cmbProyecto.SelectedValue) > 0)
                CargarGastos();
            else
                dgvGastos.DataSource = new DataTable();
        }

        private void btnCrear_Click(object? sender, EventArgs e)
        {
            using frmRegistrarGasto frm = new frmRegistrarGasto();

            if (frm.ShowDialog() == DialogResult.OK)
                CargarGastos();
        }

        private static void MostrarMensajeError(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private static void MostrarWarning(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }
}