using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LotificadoraApp.Bloque
{
    public partial class frmCrearEditarBloque : Form
    {
        private readonly bool _esEdicion;
        private readonly int _idBloque;
        private readonly int _idEtapaInicial;
        private readonly string _nombreInicial;
        private readonly string _descripcionInicial;

        public bool OperacionExitosa { get; private set; } = false;

        public frmCrearEditarBloque()
        {
            InitializeComponent();

            _esEdicion = false;
            _idBloque = 0;
            _idEtapaInicial = 0;
            _nombreInicial = string.Empty;
            _descripcionInicial = string.Empty;

            ConfigurarFormulario();
        }

        public frmCrearEditarBloque(int idBloque, int idEtapa, string nombreBloque, string descripcion)
        {
            InitializeComponent();

            _esEdicion = true;
            _idBloque = idBloque;
            _idEtapaInicial = idEtapa;
            _nombreInicial = nombreBloque ?? string.Empty;
            _descripcionInicial = descripcion ?? string.Empty;

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            Load += frmCrearEditarBloque_Load;

            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;

            AcceptButton = btnCrearEditar;
            CancelButton = btnCancelar;
        }

        private void frmCrearEditarBloque_Load(object sender, EventArgs e)
        {
            try
            {
                CargarEtapas();

                if (_esEdicion)
                {
                    lblBloque.Text = "EDITAR BLOQUE";
                    btnCrearEditar.Text = "EDITAR";

                    guna2TextBox1.Text = _nombreInicial;
                    guna2TextBox2.Text = _descripcionInicial;

                    if (_idEtapaInicial > 0)
                        guna2ComboBox1.SelectedValue = _idEtapaInicial;
                }
                else
                {
                    lblBloque.Text = "CREAR BLOQUE";
                    btnCrearEditar.Text = "CREAR";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar el formulario: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarEtapas()
        {
            try
            {
                var dt = Db.ExecuteQuery(@"
                    SELECT 
                        e.idEtapa,
                        p.nombreProyecto + ' - ' + e.nombreEtapa AS nombreEtapa
                    FROM Etapa e
                    INNER JOIN Proyecto p
                        ON e.idProyecto = p.idProyecto
                    ORDER BY p.nombreProyecto, e.nombreEtapa
                ");

                guna2ComboBox1.DataSource = dt;
                guna2ComboBox1.DisplayMember = "nombreEtapa";
                guna2ComboBox1.ValueMember = "idEtapa";
                guna2ComboBox1.SelectedIndex = -1;
            }
            catch
            {
                throw;
            }
        }

        private void btnCrearEditar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario())
                return;

            try
            {
                int idEtapa = Convert.ToInt32(guna2ComboBox1.SelectedValue);
                string nombreBloque = guna2TextBox1.Text.Trim();
                string descripcion = guna2TextBox2.Text.Trim();

                if (_esEdicion)
                {
                    ActualizarBloque(idEtapa, nombreBloque, descripcion);
                    MessageBox.Show(
                        "Bloque actualizado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    InsertarBloque(idEtapa, nombreBloque, descripcion);
                    MessageBox.Show(
                        "Bloque creado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                OperacionExitosa = true;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al guardar el bloque: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool ValidarFormulario()
        {
            if (guna2ComboBox1.SelectedIndex < 0 || guna2ComboBox1.SelectedValue == null)
            {
                MessageBox.Show(
                    "Debes seleccionar una etapa.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                guna2ComboBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                MessageBox.Show(
                    "El nombre del bloque es requerido.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                guna2TextBox1.Focus();
                return false;
            }

            return true;
        }

        private void InsertarBloque(int idEtapa, string nombreBloque, string descripcion)
        {
            var dt = Db.ExecuteStoredProcedure(
                 "sp_bloque_insertar",
                 new SqlParameter("@idEtapa", idEtapa),
                 new SqlParameter("@nombreBloque", nombreBloque),
                 new SqlParameter("@descripcion", string.IsNullOrWhiteSpace(descripcion) ? DBNull.Value : descripcion)
             );

            ValidarRespuestaError(dt);
        }

        private void ActualizarBloque(int idEtapa, string nombreBloque, string descripcion)
        {
            var dt = Db.ExecuteStoredProcedure(
                "sp_bloque_actualizar",
                new SqlParameter("@idBloque", _idBloque),
                new SqlParameter("@idEtapa", idEtapa),
                new SqlParameter("@nombreBloque", nombreBloque),
                new SqlParameter("@descripcion", string.IsNullOrWhiteSpace(descripcion) ? DBNull.Value : descripcion)
            );

            ValidarRespuestaError(dt);
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}