using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LotificadoraApp.Etapa
{
    public partial class frmEtapa : Form
    {
        private int _idEtapaSeleccionada = 0;

        public frmEtapa()
        {
            InitializeComponent();

            Load += frmEtapa_Load;

            btnBuscar.Click += btnBuscar_Click;
            btnCrear.Click += btnCrear_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;

            dgvMain.CellClick += dgvMain_CellClick;
            dgvMain.SelectionChanged += dgvMain_SelectionChanged;
            txtBuscador.KeyDown += txtBuscador_KeyDown;
        }

        private void frmEtapa_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarEtapas();
        }

        private void ConfigurarGrid()
        {
            dgvMain.AutoGenerateColumns = true;
            dgvMain.ReadOnly = true;
            dgvMain.MultiSelect = false;
            dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMain.AllowUserToAddRows = false;
            dgvMain.AllowUserToDeleteRows = false;
            dgvMain.AllowUserToResizeRows = false;
            dgvMain.RowHeadersVisible = false;
            dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMain.ColumnHeadersHeight = 38;
            dgvMain.BorderStyle = BorderStyle.None;
            dgvMain.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.BackgroundColor = Color.White;
            dgvMain.GridColor = Color.FromArgb(210, 210, 210);

            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMain.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvMain.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvMain.DefaultCellStyle.BackColor = Color.White;
            dgvMain.DefaultCellStyle.ForeColor = Color.Black;
            dgvMain.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMain.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvMain.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvMain.DefaultCellStyle.Padding = new Padding(3);

            dgvMain.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvMain.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvMain.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMain.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black; dgvMain.ColumnHeadersHeight = 52;
            dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMain.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMain.RowTemplate.Height = 32;
        }

        private void CargarEtapas(string filtro = "")
        {
            try
            {
                var dt = Db.ExecuteStoredProcedure("sp_etapa_listar");

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string texto = filtro.Trim().ToLower();

                    var filas = dt.AsEnumerable().Where(r =>
                        (r["idEtapa"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["idProyecto"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["nombreEtapa"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["fechaInicio"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["fechaFinEstimada"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["areaTotalV2"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["porcentajeAreaVerde"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["porcentajeAreaComun"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["porcentajeAreaLotes"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["precioVaraCuadrada"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["tasaInteresAnual"]?.ToString() ?? "").ToLower().Contains(texto) ||
                        (r["estadoId"]?.ToString() ?? "").ToLower().Contains(texto)
                    );

                    dgvMain.DataSource = filas.Any() ? filas.CopyToDataTable() : dt.Clone();
                }
                else
                {
                    dgvMain.DataSource = dt;
                }

                ConfigurarEncabezados();
                _idEtapaSeleccionada = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar etapas: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarEncabezados()
        {
            if (dgvMain.Columns.Contains("idEtapa"))
            {
                dgvMain.Columns["idEtapa"].HeaderText = "ID Etapa";
                dgvMain.Columns["idEtapa"].FillWeight = 5;
                dgvMain.Columns["idEtapa"].MinimumWidth = 55;
                dgvMain.Columns["idEtapa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvMain.Columns.Contains("idProyecto"))
            {
                dgvMain.Columns["idProyecto"].HeaderText = "ID Proyecto";
                dgvMain.Columns["idProyecto"].FillWeight = 5;
                dgvMain.Columns["idProyecto"].MinimumWidth = 60;
                dgvMain.Columns["idProyecto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvMain.Columns.Contains("nombreEtapa"))
            {
                dgvMain.Columns["nombreEtapa"].HeaderText = "Nombre de etapa";
                dgvMain.Columns["nombreEtapa"].FillWeight = 10; 
                dgvMain.Columns["nombreEtapa"].MinimumWidth = 80;
            }

            if (dgvMain.Columns.Contains("fechaInicio"))
            {
                dgvMain.Columns["fechaInicio"].HeaderText = "F. inicio";
                dgvMain.Columns["fechaInicio"].FillWeight = 9;
                dgvMain.Columns["fechaInicio"].MinimumWidth = 95;
                dgvMain.Columns["fechaInicio"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvMain.Columns["fechaInicio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvMain.Columns.Contains("fechaFinEstimada"))
            {
                dgvMain.Columns["fechaFinEstimada"].HeaderText = "F. fin estimada";
                dgvMain.Columns["fechaFinEstimada"].FillWeight = 10;
                dgvMain.Columns["fechaFinEstimada"].MinimumWidth = 105;
                dgvMain.Columns["fechaFinEstimada"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvMain.Columns["fechaFinEstimada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvMain.Columns.Contains("areaTotalV2"))
            {
                dgvMain.Columns["areaTotalV2"].HeaderText = "Área total (vr²)";
                dgvMain.Columns["areaTotalV2"].FillWeight = 10;
                dgvMain.Columns["areaTotalV2"].MinimumWidth = 90;
                dgvMain.Columns["areaTotalV2"].DefaultCellStyle.Format = "N2";
                dgvMain.Columns["areaTotalV2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvMain.Columns.Contains("porcentajeAreaVerde"))
            {
                dgvMain.Columns["porcentajeAreaVerde"].HeaderText = "% Área verde";
                dgvMain.Columns["porcentajeAreaVerde"].FillWeight = 8;
                dgvMain.Columns["porcentajeAreaVerde"].MinimumWidth = 85;
                dgvMain.Columns["porcentajeAreaVerde"].DefaultCellStyle.Format = "N2";
                dgvMain.Columns["porcentajeAreaVerde"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvMain.Columns.Contains("porcentajeAreaComun"))
            {
                dgvMain.Columns["porcentajeAreaComun"].HeaderText = "% Área común";
                dgvMain.Columns["porcentajeAreaComun"].FillWeight = 8;
                dgvMain.Columns["porcentajeAreaComun"].MinimumWidth = 85;
                dgvMain.Columns["porcentajeAreaComun"].DefaultCellStyle.Format = "N2";
                dgvMain.Columns["porcentajeAreaComun"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvMain.Columns.Contains("porcentajeAreaLotes"))
            {
                dgvMain.Columns["porcentajeAreaLotes"].HeaderText = "% Área lotes";
                dgvMain.Columns["porcentajeAreaLotes"].FillWeight = 8;
                dgvMain.Columns["porcentajeAreaLotes"].MinimumWidth = 85;
                dgvMain.Columns["porcentajeAreaLotes"].DefaultCellStyle.Format = "N2";
                dgvMain.Columns["porcentajeAreaLotes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvMain.Columns.Contains("precioVaraCuadrada"))
            {
                dgvMain.Columns["precioVaraCuadrada"].HeaderText = "Precio Vr²";
                dgvMain.Columns["precioVaraCuadrada"].FillWeight = 9;
                dgvMain.Columns["precioVaraCuadrada"].MinimumWidth = 95;
                dgvMain.Columns["precioVaraCuadrada"].DefaultCellStyle.Format = "N2";
                dgvMain.Columns["precioVaraCuadrada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvMain.Columns.Contains("tasaInteresAnual"))
            {
                dgvMain.Columns["tasaInteresAnual"].HeaderText = " Interés (%)";
                dgvMain.Columns["tasaInteresAnual"].FillWeight = 7;
                dgvMain.Columns["tasaInteresAnual"].MinimumWidth = 75;
                dgvMain.Columns["tasaInteresAnual"].DefaultCellStyle.Format = "N2";
                dgvMain.Columns["tasaInteresAnual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvMain.Columns.Contains("estadoId"))
            {
                dgvMain.Columns["estadoId"].HeaderText = "Estado";
                dgvMain.Columns["estadoId"].FillWeight = 5;
                dgvMain.Columns["estadoId"].MinimumWidth = 55;
                dgvMain.Columns["estadoId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarEtapas(txtBuscador.Text.Trim());
        }

        private void txtBuscador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CargarEtapas(txtBuscador.Text.Trim());
                e.SuppressKeyPress = true;
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using (var frm = new frmRegistrarEtapa())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarEtapas(txtBuscador.Text.Trim());
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idEtapaSeleccionada == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar una etapa para editar.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            using (var frm = new frmRegistrarEtapa(_idEtapaSeleccionada))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarEtapas(txtBuscador.Text.Trim());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idEtapaSeleccionada == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar una etapa para eliminar.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var confirmacion = MessageBox.Show(
                "¿Deseas eliminar la etapa seleccionada?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                var dt = Db.ExecuteStoredProcedure(
                    "sp_etapa_eliminar",
                    Db.Parameter("@idEtapa", _idEtapaSeleccionada)
                );

                ValidarRespuestaError(dt);

                MessageBox.Show(
                    "Etapa eliminada correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarEtapas(txtBuscador.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar etapa: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvMain_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvMain.CurrentRow == null || dgvMain.CurrentRow.Index < 0)
                return;

            _idEtapaSeleccionada = dgvMain.CurrentRow.Cells["idEtapa"].Value == DBNull.Value
                ? 0
                : Convert.ToInt32(dgvMain.CurrentRow.Cells["idEtapa"].Value);
        }

        private void ValidarRespuestaError(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0)
                return;

            bool tieneMensajeError = dt.Columns
                .Cast<DataColumn>()
                .Any(c => c.ColumnName.Equals("MensajeError", StringComparison.OrdinalIgnoreCase));

            if (tieneMensajeError)
            {
                string mensaje = dt.Rows[0]["MensajeError"]?.ToString() ?? "Ocurrió un error.";
                throw new Exception(mensaje);
            }
        }
    }
}