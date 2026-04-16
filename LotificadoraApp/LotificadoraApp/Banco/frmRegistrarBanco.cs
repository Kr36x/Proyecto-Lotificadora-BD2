using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Banco
{
    public partial class frmRegistrarBanco : Form
    {
        private readonly bool _esEdicion;
        private readonly int _idBanco;
        private readonly string _nombreBancoInicial;
        private readonly string _estadoInicial;

        public frmRegistrarBanco()
        {
            InitializeComponent();

            _esEdicion = false;
            _idBanco = 0;
            _nombreBancoInicial = string.Empty;
            _estadoInicial = string.Empty;

            ConfigurarFormulario();
        }

        public frmRegistrarBanco(int idBanco, string nombreBanco, string estado)
        {
            InitializeComponent();

            _esEdicion = true;
            _idBanco = idBanco;
            _nombreBancoInicial = nombreBanco ?? string.Empty;
            _estadoInicial = estado ?? string.Empty;

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            Load += frmRegistrarBanco_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;

            AcceptButton = btnCrearEditar;
            CancelButton = btnCancelar;
        }

        private void frmRegistrarBanco_Load(object sender, EventArgs e)
        {
            CargarEstados();

            if (_esEdicion)
            {
                lblBanco.Text = "EDITAR BANCO";
                btnCrearEditar.Text = "EDITAR";

                txtNombre.Text = _nombreBancoInicial;

                if (!string.IsNullOrWhiteSpace(_estadoInicial) && cmbEstado.DataSource != null)
                {
                    for (int i = 0; i < cmbEstado.Items.Count; i++)
                    {
                        DataRowView item = (DataRowView)cmbEstado.Items[i];
                        string nombreEstado = item["nombre"]?.ToString() ?? string.Empty;

                        if (string.Equals(nombreEstado, _estadoInicial, StringComparison.OrdinalIgnoreCase))
                        {
                            cmbEstado.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                lblBanco.Text = "CREAR BANCO";
                btnCrearEditar.Text = "CREAR";
                cmbEstado.SelectedIndex = -1;
            }
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dtEstados = Db.ExecuteStoredProcedure(
                    BancoQueries.QR001,
                    new SqlParameter("@Ids", "1,2")
                );

                cmbEstado.DataSource = dtEstados;
                cmbEstado.DisplayMember = "nombre";
                cmbEstado.ValueMember = "id";
                cmbEstado.SelectedIndex = -1;
            }
            catch
            {
                MostrarMensajeError("Error al cargar los estados.");
            }
        }

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarBanco())
                    return;

                DataTable dt;

                if (_esEdicion)
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_banco_actualizar",
                        new SqlParameter("@idBanco", _idBanco),
                        new SqlParameter("@nombreBanco", txtNombre.Text.Trim()),
                        new SqlParameter("@estado", Convert.ToInt32(cmbEstado.SelectedValue))
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_banco_insertar",
                        new SqlParameter("@nombreBanco", txtNombre.Text.Trim()),
                        new SqlParameter("@estado", Convert.ToInt32(cmbEstado.SelectedValue))
                    );
                }

                if (dt.Columns.Contains("MensajeError") &&
                    dt.Rows.Count > 0 &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    _esEdicion ? "Banco actualizado con éxito." : "Banco guardado con éxito.",
                    "Operación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MostrarMensajeError("Ocurrió un error al guardar.");
            }
        }

        private bool ValidarBanco()
        {
            string nombreBanco = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreBanco))
            {
                MostrarWarning("El nombre del banco es requerido.");
                txtNombre.Focus();
                return false;
            }

            if (cmbEstado.SelectedValue == null || cmbEstado.SelectedIndex < 0)
            {
                MostrarWarning("El estado es requerido.");
                cmbEstado.Focus();
                return false;
            }

            int estadoId = Convert.ToInt32(cmbEstado.SelectedValue);
            if (estadoId <= 0)
            {
                MostrarWarning("El estado es requerido.");
                cmbEstado.Focus();
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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