using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaLotesAptosCliente : Form
    {
        public frmConsultaLotesAptosCliente()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtCliente.ReadOnly = true;
            txtIngresoMensual.ReadOnly = true;
            txtCapacidadMaxima.ReadOnly = true;

            txtCliente.TabStop = false;
            txtIngresoMensual.TabStop = false;
            txtCapacidadMaxima.TabStop = false;

            dgvLotesAptos.ReadOnly = true;
            dgvLotesAptos.AllowUserToAddRows = false;
            dgvLotesAptos.AllowUserToDeleteRows = false;
            dgvLotesAptos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLotesAptos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblRegistros.Text = "Registros: 0";

            txtPrima.Text = "0";
            txtPlazoAnios.Text = "10";
            txtPorcentajeMaxIngreso.Text = "30";
        }

        private void ConectarEventos()
        {
            Load += frmConsultaLotesAptosCliente_Load;
            btnConsultar.Click += btnConsultar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void frmConsultaLotesAptosCliente_Load(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnConsultar_Click(object? sender, EventArgs e)
        {
            Consultar();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void Consultar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIdCliente.Text))
                    throw new Exception("Ingrese el Id del cliente.");

                int idCliente = ParseInt(txtIdCliente.Text);
                decimal prima = ParseDecimal(txtPrima.Text);
                int plazoAnios = ParseInt(txtPlazoAnios.Text);
                decimal porcentajeMaxIngreso = ParseDecimal(txtPorcentajeMaxIngreso.Text);

                if (plazoAnios <= 0)
                    throw new Exception("El plazo en años debe ser mayor que cero.");

                if (porcentajeMaxIngreso <= 0)
                    throw new Exception("El porcentaje máximo debe ser mayor que cero.");

                CargarResumenCliente(idCliente, porcentajeMaxIngreso);
                CargarLotesAptos(idCliente, prima, plazoAnios, porcentajeMaxIngreso);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar lotes aptos:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarResumenCliente(int idCliente, decimal porcentajeMaxIngreso)
        {
            const string sql = @"exec dbo.sp_obtener_resumen_cliente_capacidad_pago @idCliente = @idCliente";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@idCliente", idCliente);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                LimpiarResumen();
                throw new Exception("No se encontró el cliente o no tiene datos laborales registrados.");
            }

            decimal ingresoMensual = Convert.ToDecimal(dr["ingresoMensual"]);
            decimal capacidadMaxima = ingresoMensual * (porcentajeMaxIngreso / 100m);

            txtCliente.Text = dr["cliente"].ToString();
            txtIngresoMensual.Text = ingresoMensual.ToString("N2");
            txtCapacidadMaxima.Text = capacidadMaxima.ToString("N2");
        }

        private void CargarLotesAptos(int idCliente, decimal prima, int plazoAnios, decimal porcentajeMaxIngreso)
        {
            const string sql = @"
                SELECT
                    nombreProyecto,
                    nombreEtapa,
                    numeroLote,
                    areaV2,
                    precioFinalLote,
                    montoFinanciado,
                    cuotaEstimada,
                    porcentajeIngresoComprometido,
                    resultado
                FROM dbo.fn_tvf_lotes_aptos_por_cliente(
                    @idCliente,
                    @prima,
                    @plazoAnios,
                    @porcentajeMaxIngreso
                )
                WHERE resultado = 'APTO'
                ORDER BY nombreProyecto, nombreEtapa, numeroLote;";

            using SqlConnection cn = new SqlConnection(Db.ConnectionString);
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@idCliente", idCliente);
            cmd.Parameters.AddWithValue("@prima", prima);
            cmd.Parameters.AddWithValue("@plazoAnios", plazoAnios);
            cmd.Parameters.AddWithValue("@porcentajeMaxIngreso", porcentajeMaxIngreso);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvLotesAptos.DataSource = null;
            dgvLotesAptos.DataSource = dt;

            lblRegistros.Text = $"Registros: {dt.Rows.Count}";
        }

        private void LimpiarFormulario()
        {
            txtIdCliente.Clear();
            txtPrima.Text = "0";
            txtPlazoAnios.Text = "10";
            txtPorcentajeMaxIngreso.Text = "30";

            LimpiarResumen();

            dgvLotesAptos.DataSource = null;
            lblRegistros.Text = "Registros: 0";
        }

        private void LimpiarResumen()
        {
            txtCliente.Clear();
            txtIngresoMensual.Clear();
            txtCapacidadMaxima.Clear();
        }

        private int ParseInt(string valor)
        {
            if (int.TryParse(valor.Trim(), out int r))
                return r;

            throw new Exception($"El valor '{valor}' no es un entero válido.");
        }

        private decimal ParseDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return 0m;

            valor = valor.Replace(",", "");

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal r))
                return r;

            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.CurrentCulture, out r))
                return r;

            throw new Exception($"El valor '{valor}' no es un número válido.");
        }
    }
}