using System.Data;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp.Estado
{
    public partial class frmRegistrarEstado : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idEstado;

        public frmRegistrarEstado()
        {
            InitializeComponent();

            _modoEdicion = false;
            _idEstado = 0;

            ConectarEventos();
        }

        public frmRegistrarEstado(int idEstado, string nombre)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idEstado = idEstado;

            ConectarEventos();

            guna2TextBox1.Text = nombre ?? string.Empty;
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarEstado_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmRegistrarEstado_Load(object sender, EventArgs e)
        {
            if (_modoEdicion)
            {
                lblBloque.Text = "EDITAR ESTADO";
                btnCrearEditar.Text = "EDITAR";
            }
            else
            {
                lblBloque.Text = "CREAR ESTADO";
                btnCrearEditar.Text = "CREAR";
                guna2TextBox1.Clear();
            }

            guna2TextBox1.Focus();
        }

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarEstado())
                    return;

                DataTable dataTable;

                if (_modoEdicion)
                {
                    dataTable = Db.ExecuteStoredProcedure(
                        "sp_estado_actualizar",
                        new SqlParameter("@id", _idEstado),
                        new SqlParameter("@nombre", guna2TextBox1.Text.Trim())
                    );
                }
                else
                {
                    dataTable = Db.ExecuteStoredProcedure(
                        "sp_estado_insertar",
                        new SqlParameter("@nombre", guna2TextBox1.Text.Trim())
                    );
                }

                if (dataTable.Rows.Count > 0 &&
                    dataTable.Columns.Contains("MensajeError") &&
                    dataTable.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dataTable.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    _modoEdicion ? "Estado actualizado con éxito" : "Registro guardado con éxito",
                    "Operación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MostrarMensajeError("Ocurrió un error al guardar");
            }
        }

        private bool ValidarEstado()
        {
            string nombre = guna2TextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarWarning("El nombre del estado es requerido");
                guna2TextBox1.Focus();
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