using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarBeneficiario : Form
    {
        public int? IdBeneficiarioGenerado { get; private set; }

        public frmRegistrarBeneficiario()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtIdGenerado.ReadOnly = true;
            txtIdGenerado.TabStop = false;
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarBeneficiario_Load;
            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void frmRegistrarBeneficiario_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarParentescos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar formulario:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarParentescos()
        {
            const string sql = @"
                SELECT id, descripcion
                FROM Parentesco
                ORDER BY descripcion;";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cbParentesco.DataSource = dt;
            cbParentesco.DisplayMember = "descripcion";
            cbParentesco.ValueMember = "id";
            cbParentesco.SelectedIndex = -1;
        }

        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                ValidarFormulario();

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_beneficiario_insertar", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@identidad", txtIdentidad.Text.Trim());
                cmd.Parameters.AddWithValue("@nombres", txtNombres.Text.Trim());
                cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text.Trim());
                cmd.Parameters.AddWithValue("@telefono", ValorDb(txtTelefono.Text));
                cmd.Parameters.AddWithValue("@parentescoId", Convert.ToInt32(cbParentesco.SelectedValue));
                cmd.Parameters.AddWithValue("@direccion", ValorDb(txtDireccion.Text));

                DataTable dt = new DataTable();
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                    throw new Exception("No se devolvió el id del beneficiario generado.");

                if (dt.Columns.Contains("MensajeError") && dt.Rows[0]["MensajeError"] != DBNull.Value)
                    throw new Exception(dt.Rows[0]["MensajeError"].ToString());

                IdBeneficiarioGenerado = Convert.ToInt32(dt.Rows[0]["idBeneficiarioGenerado"]);
                txtIdGenerado.Text = IdBeneficiarioGenerado.ToString();

                MessageBox.Show(
                    "Beneficiario registrado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar beneficiario:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtIdentidad.Text))
                throw new Exception("La identidad es obligatoria.");

            if (string.IsNullOrWhiteSpace(txtNombres.Text))
                throw new Exception("Los nombres son obligatorios.");

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
                throw new Exception("Los apellidos son obligatorios.");

            if (cbParentesco.SelectedIndex < 0)
                throw new Exception("Seleccione el parentesco.");
        }

        private object ValorDb(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : valor.Trim();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtIdentidad.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            cbParentesco.SelectedIndex = -1;
            txtIdGenerado.Clear();
            IdBeneficiarioGenerado = null;
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}