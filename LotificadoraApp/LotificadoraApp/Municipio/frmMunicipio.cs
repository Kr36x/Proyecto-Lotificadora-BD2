using System.Data;
using System.Drawing;

namespace LotificadoraApp.Municipio
{
    public partial class frmMunicipio : Form
    {
        private int _municipioIdSeleccionado = 0;

        public frmMunicipio()
        {
            InitializeComponent();
        }

        private void frmMunicipio_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ObtenerMunicipios();
        }

        private void ObtenerMunicipios()
        {
            try
            {
                LimpiarSeleccion();

                DataTable dataTable = Db.ExecuteStoredProcedure(
                    MunicipioQueries.QR001);

                dgvMunicipio.DataSource = dataTable;
                //ConfigurarColumnas();
            }
            catch
            {
                MostrarMensajeError("Error al obtener los municipios");
            }
        }

        private void ConfigurarGrid()
        {
            dgvMunicipio.AutoGenerateColumns = true;
            dgvMunicipio.ReadOnly = true;
            dgvMunicipio.MultiSelect = false;
            dgvMunicipio.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMunicipio.AllowUserToAddRows = false;
            dgvMunicipio.AllowUserToDeleteRows = false;
            dgvMunicipio.AllowUserToResizeRows = false;
            dgvMunicipio.RowHeadersVisible = false;
            dgvMunicipio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMunicipio.ColumnHeadersHeight = 52;
            dgvMunicipio.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvMunicipio.EnableHeadersVisualStyles = false;
            dgvMunicipio.BackgroundColor = Color.White;
            dgvMunicipio.BorderStyle = BorderStyle.None;
            dgvMunicipio.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMunicipio.GridColor = Color.FromArgb(210, 210, 210);

            dgvMunicipio.ColumnHeadersDefaultCellStyle.BackColor = Color.Olive;
            dgvMunicipio.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMunicipio.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Olive;
            dgvMunicipio.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvMunicipio.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvMunicipio.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMunicipio.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvMunicipio.DefaultCellStyle.BackColor = Color.White;
            dgvMunicipio.DefaultCellStyle.ForeColor = Color.Black;
            dgvMunicipio.DefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMunicipio.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvMunicipio.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvMunicipio.DefaultCellStyle.Padding = new Padding(3);

            dgvMunicipio.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 239);
            dgvMunicipio.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvMunicipio.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(196, 210, 155);
            dgvMunicipio.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvMunicipio.RowTemplate.Height = 32;
        }

        private void ConfigurarColumnas()
        {
            if (dgvMunicipio.Columns.Contains("id"))
            {
                dgvMunicipio.Columns["id"].FillWeight = 15;
                dgvMunicipio.Columns["id"].MinimumWidth = 70;
                dgvMunicipio.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvMunicipio.Columns.Contains("nombre"))
            {
                dgvMunicipio.Columns["nombre"].FillWeight = 35;
                dgvMunicipio.Columns["nombre"].MinimumWidth = 160;
                dgvMunicipio.Columns["nombre"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvMunicipio.Columns.Contains("departamento"))
            {
                dgvMunicipio.Columns["departamento"].FillWeight = 30;
                dgvMunicipio.Columns["departamento"].MinimumWidth = 150;
                dgvMunicipio.Columns["departamento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvMunicipio.Columns.Contains("estado"))
            {
                dgvMunicipio.Columns["estado"].FillWeight = 20;
                dgvMunicipio.Columns["estado"].MinimumWidth = 120;
                dgvMunicipio.Columns["estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult confirmacion = MessageBox.Show(
                "¿Desea eliminar el municipio seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                DataTable dataTable = Db.ExecuteStoredProcedure(
                  MunicipioQueries.QR002,
                  Db.Parameter("@id", _municipioIdSeleccionado));

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Municipio eliminado correctamente",
                    "Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ObtenerMunicipios();
            }
            catch (Exception)
            {
                MostrarMensajeError("Ocurrió un error al eliminar");
            }
        }

        private void dgvMunicipio_SelectionChanged(object sender, EventArgs e)
        {
            ObtenerSeleccion();
        }

        private void dgvMunicipio_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ObtenerSeleccion();
        }

        private void ObtenerSeleccion()
        {
            if (dgvMunicipio.CurrentRow == null || dgvMunicipio.CurrentRow.Index < 0)
                return;

            if (dgvMunicipio.CurrentRow.Cells["id"].Value == null ||
                dgvMunicipio.CurrentRow.Cells["id"].Value == DBNull.Value)
            {
                LimpiarSeleccion();
                return;
            }

            _municipioIdSeleccionado = Convert.ToInt32(dgvMunicipio.CurrentRow.Cells["id"].Value);
        }

        private void LimpiarSeleccion()
        {
            _municipioIdSeleccionado = 0;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            using frmRegistrarMunicipio frmRegistrarMunicipio = new();

            frmRegistrarMunicipio.FormClosed += (_, _) => ObtenerMunicipios();
            frmRegistrarMunicipio.ShowDialog();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            if (_municipioIdSeleccionado <= 0)
            {
                MostrarWarning("Seleccione un municipio para editar");
                return;
            }

            using frmRegistrarMunicipio frmRegistrarMunicipio =
                new(_municipioIdSeleccionado);

            frmRegistrarMunicipio.FormClosed += (_, _) => ObtenerMunicipios();
            frmRegistrarMunicipio.ShowDialog();
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