using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmRegistrarAval : Form
    {
        public int? IdAvalGenerado { get; private set; }

        public frmRegistrarAval()
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
            Load += frmRegistrarAval_Load;
            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void frmRegistrarAval_Load(object? sender, EventArgs e)
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
                using SqlCommand cmd = new SqlCommand("dbo.sp_aval_insertar", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@identidad", txtIdentidad.Text.Trim());
                cmd.Parameters.AddWithValue("@nombres", txtNombres.Text.Trim());
                cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text.Trim());
                cmd.Parameters.AddWithValue("@telefono", ValorDb(txtTelefono.Text));
                cmd.Parameters.AddWithValue("@direccion", ValorDb(txtDireccion.Text));
                cmd.Parameters.AddWithValue("@lugarTrabajo", ValorDb(txtLugarTrabajo.Text));
                cmd.Parameters.AddWithValue("@ingresoMensual", ValorDbDecimal(txtIngresoMensual.Text));
                cmd.Parameters.AddWithValue("@parentescoId", Convert.ToInt32(cbParentesco.SelectedValue));

                DataTable dt = new DataTable();
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                    throw new Exception("No se devolvió el id del aval generado.");

                if (dt.Columns.Contains("MensajeError") && dt.Rows[0]["MensajeError"] != DBNull.Value)
                    throw new Exception(dt.Rows[0]["MensajeError"].ToString());

                IdAvalGenerado = Convert.ToInt32(dt.Rows[0]["idAvalGenerado"]);
                txtIdGenerado.Text = IdAvalGenerado.ToString();

                MessageBox.Show(
                    "Aval registrado correctamente.",
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
                    "Error al registrar aval:\n" + ex.Message,
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

            if (!string.IsNullOrWhiteSpace(txtIngresoMensual.Text) && ParseDecimal(txtIngresoMensual.Text) < 0)
                throw new Exception("El ingreso mensual no puede ser negativo.");
        }

        private object ValorDb(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : valor.Trim();
        }

        private object ValorDbDecimal(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? DBNull.Value : ParseDecimal(valor);
        }

        private decimal ParseDecimal(string valor)
        {
            valor = valor.Replace(",", "");

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal r))
                return r;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.CurrentCulture, out r))
                return r;

            throw new Exception($"El valor '{valor}' no es un número válido.");
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtIdentidad.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtLugarTrabajo.Clear();
            txtIngresoMensual.Clear();
            cbParentesco.SelectedIndex = -1;
            txtIdGenerado.Clear();
            IdAvalGenerado = null;
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}