using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LotificadoraApp.CuentaBancaria
{
    public partial class frmCrearEditarCuentaBancaria : Form
    {
        private readonly bool _modoEdicion;
        private readonly int _idCuentaBancaria;

        public frmCrearEditarCuentaBancaria()
        {
            InitializeComponent();

            _modoEdicion = false;
            _idCuentaBancaria = 0;

            ConectarEventos();
        }

        public frmCrearEditarCuentaBancaria(int idCuentaBancaria)
        {
            InitializeComponent();

            _modoEdicion = true;
            _idCuentaBancaria = idCuentaBancaria;

            ConectarEventos();
        }

        private void ConectarEventos()
        {
            Load += frmCrearEditarCuentaBancaria_Load;
            btnCrearEditar.Click += btnCrearEditar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void frmCrearEditarCuentaBancaria_Load(object? sender, EventArgs e)
        {
            CargarBancos();
            CargarEtapas();
            CargarTiposCuenta();
            CargarEstados();

            if (_modoEdicion)
            {
                lblTitulo.Text = "EDITAR CUENTA BANCARIA";
                btnCrearEditar.Text = "GUARDAR";

                txtSaldoActual.ReadOnly = true;
                txtSaldoActual.FillColor = Color.FromArgb(245, 245, 245);

                CargarCuentaBancaria();
            }
            else
            {
                lblTitulo.Text = "CREAR CUENTA BANCARIA";
                btnCrearEditar.Text = "CREAR";

                txtSaldoActual.ReadOnly = false;
                txtSaldoActual.FillColor = Color.White;
                txtSaldoActual.Text = "0.00";
            }
        }

        private void CargarBancos()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_banco_listar");

                cmbBanco.DataSource = dt;
                cmbBanco.DisplayMember = "nombreBanco";
                cmbBanco.ValueMember = "id";
                cmbBanco.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar bancos: " + ex.Message);
            }
        }

        private void CargarEtapas()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_etapa_listar");

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

        private void CargarTiposCuenta()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tipoCuenta", typeof(string));

            dt.Rows.Add("ahorro");
            dt.Rows.Add("cheque");
            dt.Rows.Add("monetaria");

            cmbTipoCuenta.DataSource = dt;
            cmbTipoCuenta.DisplayMember = "tipoCuenta";
            cmbTipoCuenta.ValueMember = "tipoCuenta";
            cmbTipoCuenta.SelectedIndex = -1;
        }

        private void CargarEstados()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure("sp_estado_listar");

                cmbEstado2.DataSource = dt;
                cmbEstado2.DisplayMember = "nombre";
                cmbEstado2.ValueMember = "id";
                cmbEstado2.SelectedValue = 4;
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar estados: " + ex.Message);
            }
        }

        private void CargarCuentaBancaria()
        {
            try
            {
                DataTable dt = Db.ExecuteStoredProcedure(
                    "sp_cuenta_bancaria_obtener",
                    new SqlParameter("@idCuentaBancaria", _idCuentaBancaria)
                );

                if (dt.Rows.Count == 0)
                {
                    MostrarMensajeError("No se encontró la cuenta bancaria.");
                    Close();
                    return;
                }

                DataRow row = dt.Rows[0];

                if (row["idBanco"] != DBNull.Value)
                    cmbBanco.SelectedValue = Convert.ToInt32(row["idBanco"]);

                if (row["idEtapa"] != DBNull.Value)
                    cmbEtapa.SelectedValue = Convert.ToInt32(row["idEtapa"]);

                txtNumeroCuenta.Text = row["numeroCuenta"]?.ToString() ?? "";
                txtSaldoActual.Text = row["saldoActual"] == DBNull.Value
                    ? "0.00"
                    : Convert.ToDecimal(row["saldoActual"]).ToString("N2");

                string tipoCuenta = row["tipoCuenta"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(tipoCuenta))
                    cmbTipoCuenta.SelectedValue = tipoCuenta;

                if (row["estadoId"] != DBNull.Value)
                    cmbEstado2.SelectedValue = Convert.ToInt32(row["estadoId"]);
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al cargar cuenta bancaria: " + ex.Message);
            }
        }

        private void btnCrearEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario(out decimal saldoActual))
                    return;

                DataTable dt;

                if (_modoEdicion)
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_cuenta_bancaria_actualizar",
                        new SqlParameter("@idCuentaBancaria", _idCuentaBancaria),
                        new SqlParameter("@idBanco", Convert.ToInt32(cmbBanco.SelectedValue)),
                        new SqlParameter("@idEtapa", Convert.ToInt32(cmbEtapa.SelectedValue)),
                        new SqlParameter("@numeroCuenta", txtNumeroCuenta.Text.Trim()),
                        new SqlParameter("@tipoCuenta", cmbTipoCuenta.SelectedValue.ToString()),
                        new SqlParameter("@saldoActual", saldoActual),
                        new SqlParameter("@estado", Convert.ToInt32(guna2ComboBox2.SelectedValue))
                    );
                }
                else
                {
                    dt = Db.ExecuteStoredProcedure(
                        "sp_cuenta_bancaria_insertar",
                        new SqlParameter("@idBanco", Convert.ToInt32(cmbBanco.SelectedValue)),
                        new SqlParameter("@idEtapa", Convert.ToInt32(cmbEtapa.SelectedValue)),
                        new SqlParameter("@numeroCuenta", txtNumeroCuenta.Text.Trim()),
                        new SqlParameter("@tipoCuenta", cmbTipoCuenta.SelectedValue.ToString()),
                        new SqlParameter("@saldoActual", saldoActual),
                        new SqlParameter("@estado", Convert.ToInt32(guna2ComboBox2.SelectedValue))
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
                    _modoEdicion ? "Cuenta bancaria actualizada correctamente." : "Cuenta bancaria creada correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MostrarMensajeError("Error al guardar cuenta bancaria: " + ex.Message);
            }
        }

        private bool ValidarFormulario(out decimal saldoActual)
        {
            saldoActual = 0;

            if (cmbBanco.SelectedValue == null || cmbBanco.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione un banco.");
                cmbBanco.Focus();
                return false;
            }

            if (cmbEtapa.SelectedValue == null || cmbEtapa.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione una etapa.");
                cmbEtapa.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumeroCuenta.Text))
            {
                MostrarWarning("El numeroCuenta es requerido.");
                txtNumeroCuenta.Focus();
                return false;
            }

            if (cmbTipoCuenta.SelectedValue == null || cmbTipoCuenta.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione el tipoCuenta.");
                cmbTipoCuenta.Focus();
                return false;
            }

            if (!TryParseDecimal(txtSaldoActual.Text, out saldoActual))
            {
                MostrarWarning("saldoActual debe ser un valor numérico válido.");
                txtSaldoActual.Focus();
                return false;
            }

            if (saldoActual < 0)
            {
                MostrarWarning("saldoActual no puede ser negativo.");
                txtSaldoActual.Focus();
                return false;
            }

            if (cmbEstado2.SelectedValue == null || cmbEstado2.SelectedIndex < 0)
            {
                MostrarWarning("Seleccione el estado.");
                cmbEstado2.Focus();
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

        private void pnlFondo_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}