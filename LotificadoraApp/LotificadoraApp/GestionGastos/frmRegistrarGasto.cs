using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LotificadoraApp.GestionGastos
{
    public partial class frmRegistrarGasto : Form
    {
        public frmRegistrarGasto()
        {
            InitializeComponent();
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmRegistrarGasto_Load;

            cmbProyecto.SelectedIndexChanged += cmbProyecto_SelectedIndexChanged;
            cmbEtapa.SelectedIndexChanged += cmbEtapa_SelectedIndexChanged;

            btnRegistrar.Click += btnRegistrar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmRegistrarGasto_Load(object? sender, EventArgs e)
        {
            CargarProyectos();
            CargarTiposGasto();

            cmbEtapa.DataSource = null;
            cmbCuentaBancaria.DataSource = null;
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

                cmbProyecto.DataSource = dt;
                cmbProyecto.DisplayMember = "nombreProyecto";
                cmbProyecto.ValueMember = "idProyecto";
                cmbProyecto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar proyectos: " + ex.Message);
            }
        }

        private void CargarEtapas(int idProyecto)
        {
            try
            {
                DataTable dt = Db.ExecuteQuery(@"
                    SELECT idEtapa, nombreEtapa
                    FROM Etapa
                    WHERE idProyecto = @idProyecto
                    ORDER BY nombreEtapa;
                ", new SqlParameter("@idProyecto", idProyecto));

                cmbEtapa.DataSource = dt;
                cmbEtapa.DisplayMember = "nombreEtapa";
                cmbEtapa.ValueMember = "idEtapa";
                cmbEtapa.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar etapas: " + ex.Message);
            }
        }

        private void CargarTiposGasto()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_tipo_gasto_listar");

                if (dt.Columns.Contains("estadoId"))
                {
                    DataView view = dt.DefaultView;
                    view.RowFilter = "estadoId = 1";
                    cmbTipoGasto.DataSource = view.ToTable();
                }
                else
                {
                    cmbTipoGasto.DataSource = dt;
                }

                cmbTipoGasto.DisplayMember = "nombreTipoGasto";
                cmbTipoGasto.ValueMember = "idTipoGasto";
                cmbTipoGasto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar tipos de gasto: " + ex.Message);
            }
        }

        private void CargarCuentasBancarias(int idEtapa)
        {
            try
            {
                DataTable dt = Db.ExecuteQuery(@"
                    SELECT idCuentaBancaria, numeroCuenta
                    FROM CuentaBancaria
                    WHERE idEtapa = @idEtapa
                    ORDER BY numeroCuenta;
                ", new SqlParameter("@idEtapa", idEtapa));

                cmbCuentaBancaria.DataSource = dt;
                cmbCuentaBancaria.DisplayMember = "numeroCuenta";
                cmbCuentaBancaria.ValueMember = "idCuentaBancaria";
                cmbCuentaBancaria.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar cuentas bancarias: " + ex.Message);
            }
        }

        private void cmbProyecto_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbProyecto.SelectedValue == null)
                return;

            if (!int.TryParse(cmbProyecto.SelectedValue.ToString(), out int idProyecto) || idProyecto <= 0)
                return;

            CargarEtapas(idProyecto);
            cmbCuentaBancaria.DataSource = null;
        }

        private void cmbEtapa_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbEtapa.SelectedValue == null)
                return;

            if (!int.TryParse(cmbEtapa.SelectedValue.ToString(), out int idEtapa) || idEtapa <= 0)
                return;

            CargarCuentasBancarias(idEtapa);
        }

        private void btnRegistrar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario(out decimal monto))
                    return;

                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_registrar_gasto_proyecto_transaccional",
                    new SqlParameter("@idProyecto", Convert.ToInt32(cmbProyecto.SelectedValue)),
                    new SqlParameter("@idEtapa", Convert.ToInt32(cmbEtapa.SelectedValue)),
                    new SqlParameter("@idTipoGasto", Convert.ToInt32(cmbTipoGasto.SelectedValue)),
                    new SqlParameter("@idCuentaBancaria", Convert.ToInt32(cmbCuentaBancaria.SelectedValue)),
                    new SqlParameter("@descripcion", txtDescripcion.Text.Trim()),
                    new SqlParameter("@monto", monto)
                );

                if (dt.Rows.Count > 0 &&
                    dt.Columns.Contains("MensajeError") &&
                    dt.Rows[0]["MensajeError"] != DBNull.Value)
                {
                    MostrarMensajeError(dt.Rows[0]["MensajeError"].ToString());
                    return;
                }

                MessageBox.Show(
                    "Gasto registrado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al registrar gasto: " + ex.Message);
            }
        }

        private bool ValidarFormulario(out decimal monto)
        {
            monto = 0;

            if (cmbProyecto.SelectedValue == null || cmbProyecto.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione un proyecto.");
                cmbProyecto.Focus();
                return false;
            }

            if (cmbEtapa.SelectedValue == null || cmbEtapa.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione una etapa.");
                cmbEtapa.Focus();
                return false;
            }

            if (cmbTipoGasto.SelectedValue == null || cmbTipoGasto.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione un tipo de gasto.");
                cmbTipoGasto.Focus();
                return false;
            }

            if (cmbCuentaBancaria.SelectedValue == null || cmbCuentaBancaria.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione una cuenta bancaria.");
                cmbCuentaBancaria.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMonto.Text))
            {
                MostrarWarning("El monto es requerido.");
                txtMonto.Focus();
                return false;
            }

            if (!TryParseDecimal(txtMonto.Text, out monto) || monto <= 0)
            {
                MostrarWarning("El monto debe ser un número mayor que 0.");
                txtMonto.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MostrarWarning("La descripción es requerida.");
                txtDescripcion.Focus();
                return false;
            }

            return true;
        }

        private static bool TryParseDecimal(string valor, out decimal resultado)
        {
            valor = valor.Trim();

            if (decimal.TryParse(valor, NumberStyles.Any, new CultureInfo("es-HN"), out resultado))
                return true;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out resultado))
                return true;

            resultado = 0;
            return false;
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