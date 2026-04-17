using Microsoft.Data.SqlClient;
using System.Data;

namespace LotificadoraApp.Gasto
{
    public partial class frmCrearEditarTipoGasto : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idTipoGasto;

        public frmCrearEditarTipoGasto()
        {
            InitializeComponent();

            _modoEdicion = false;
            _idTipoGasto = 0;

            ConectarEventos();
        }

        public frmCrearEditarTipoGasto(int idTipoGasto, string nombreTipoGasto, int estadoId)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idTipoGasto = idTipoGasto;

            ConectarEventos();

            txtTipoGasto.Text = nombreTipoGasto ?? string.Empty;
            _estadoInicial = estadoId;
        }

        private int _estadoInicial = 0;

        private void ConectarEventos()
        {
            Load += frmCrearEditarTipoGasto_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmCrearEditarTipoGasto_Load(object? sender, EventArgs e)
        {
            CargarEstados();

            if (_modoEdicion)
            {
                lblBloque.Text = "EDITAR TIPO DE GASTO";
                btnCrearEditar.Text = "EDITAR";

                if (_estadoInicial > 0)
                    cbmEstado.SelectedValue = _estadoInicial;
            }
            else
            {
                lblBloque.Text = "CREAR TIPO DE GASTO";
                btnCrearEditar.Text = "CREAR";
                cbmEstado.SelectedValue = 1;
            }
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_estado_listar");

                cbmEstado.DataSource = dt;
                cbmEstado.DisplayMember = "nombre";
                cbmEstado.ValueMember = "id";
                cbmEstado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar estados: " + ex.Message);
            }
        }

        private void btnCrearEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario())
                    return;

                DataTable dt;

                if (_modoEdicion)
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_tipo_gasto_actualizar",
                        new SqlParameter("@idTipoGasto", _idTipoGasto),
                        new SqlParameter("@nombreTipoGasto", txtTipoGasto.Text.Trim()),
                        new SqlParameter("@estado", Convert.ToInt32(cbmEstado.SelectedValue))
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_tipo_gasto_insertar",
                        new SqlParameter("@nombreTipoGasto", txtTipoGasto.Text.Trim()),
                        new SqlParameter("@estado", Convert.ToInt32(cbmEstado.SelectedValue))
                    );
                }

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    _modoEdicion ? "Tipo de gasto actualizado correctamente." : "Tipo de gasto creado correctamente.",
                    "Operación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Ocurrió un error al guardar: " + ex.Message);
            }
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtTipoGasto.Text))
            {
                MostrarWarning("El nombreTipoGasto es requerido.");
                txtTipoGasto.Focus();
                return false;
            }

            if (cbmEstado.SelectedValue == null || cbmEstado.SelectedIndex < 0)
            {
                MostrarWarning("El estado es requerido.");
                cbmEstado.Focus();
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
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