using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaPagos : Form
    {
        private DataTable _ventasCliente = new DataTable();

        public frmConsultaPagos()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConectarEventos();
        }

        private void ConfigurarFormulario()
        {
            txtCliente.ReadOnly = true;
            txtCliente.TabStop = false;

            txtTotalPagado.ReadOnly = true;
            txtCantidadPagos.ReadOnly = true;
            txtTotalPagado.TabStop = false;
            txtCantidadPagos.TabStop = false;

            dgvPagos.ReadOnly = true;
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cbVenta.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVenta.Enabled = false;

            chkTodasLasVentas.Checked = true;

            dtpFechaInicio.Format = DateTimePickerFormat.Custom;
            dtpFechaInicio.CustomFormat = "yyyy-MM-dd";
            dtpFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            dtpFechaFin.Format = DateTimePickerFormat.Custom;
            dtpFechaFin.CustomFormat = "yyyy-MM-dd";
            dtpFechaFin.Value = DateTime.Today;
        }

        private void ConectarEventos()
        {
            Load += frmConsultaPagos_Load;
            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCerrar.Click += btnCerrar_Click;
            chkTodasLasVentas.CheckedChanged += chkTodasLasVentas_CheckedChanged;
            txtIDCliente.Leave += txtIDCliente_Leave;
            txtIDCliente.KeyDown += txtIDCliente_KeyDown;
        }

        private void frmConsultaPagos_Load(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void txtIDCliente_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CargarVentasPorCliente();
            }
        }

        private void txtIDCliente_Leave(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIDCliente.Text))
                CargarVentasPorCliente();
        }

        private void chkTodasLasVentas_CheckedChanged(object? sender, EventArgs e)
        {
            cbVenta.Enabled = !chkTodasLasVentas.Checked;

            if (chkTodasLasVentas.Checked)
                cbVenta.SelectedIndex = -1;
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            ConsultarPagos();
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void CargarVentasPorCliente()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIDCliente.Text))
                {
                    LimpiarVentasCliente();
                    return;
                }

                int idCliente = ParseInt(txtIDCliente.Text);

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_listar_ventas_credito_activas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                using SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (!dt.Columns.Contains("idCliente"))
                    throw new Exception("El procedimiento sp_listar_ventas_credito_activas no devolvió la columna idCliente.");

                var filas = dt.AsEnumerable()
                    .Where(r => r["idCliente"] != DBNull.Value &&
                                Convert.ToInt32(r["idCliente"]) == idCliente);

                _ventasCliente = dt.Clone();

                foreach (var fila in filas)
                    _ventasCliente.ImportRow(fila);

                if (_ventasCliente.Rows.Count == 0)
                {
                    LimpiarVentasCliente();
                    throw new Exception("El cliente no tiene ventas a crédito activas.");
                }

                cbVenta.DataSource = _ventasCliente;
                cbVenta.DisplayMember = "descripcion";
                cbVenta.ValueMember = "idVenta";
                cbVenta.SelectedIndex = -1;

                if (_ventasCliente.Columns.Contains("cliente"))
                {
                    txtCliente.Text = _ventasCliente.Rows[0]["cliente"]?.ToString() ?? "";
                }
                else
                {
                    // Si el SP no trae nombre del cliente, al menos dejamos el id visible
                    txtCliente.Text = $"Cliente #{idCliente}";
                }

                cbVenta.Enabled = !chkTodasLasVentas.Checked;
            }
            catch (Exception ex)
            {
                LimpiarVentasCliente();

                MessageBox.Show(
                    "Error al cargar ventas del cliente:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConsultarPagos()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIDCliente.Text))
                    throw new Exception("Ingrese el ID del cliente.");

                int idCliente = ParseInt(txtIDCliente.Text);

                if (dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date)
                    throw new Exception("La fecha fin no puede ser menor que la fecha inicio.");

                object idVenta = DBNull.Value;

                if (!chkTodasLasVentas.Checked)
                {
                    if (cbVenta.SelectedIndex < 0)
                        throw new Exception("Seleccione una venta o marque la opción de todas las ventas.");

                    idVenta = Convert.ToInt32(cbVenta.SelectedValue);
                }

                using SqlConnection cn = new SqlConnection(Db.ConnectionString);
                using SqlCommand cmd = new SqlCommand("dbo.sp_consultar_pagos_cliente_venta", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cmd.Parameters.AddWithValue("@fechaInicio", dtpFechaInicio.Value.Date);
                cmd.Parameters.AddWithValue("@fechaFin", dtpFechaFin.Value.Date);

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPagos.DataSource = null;
                dgvPagos.DataSource = dt;

                decimal totalPagado = 0m;

                if (dt.Columns.Contains("montoPagado"))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["montoPagado"] != DBNull.Value)
                            totalPagado += Convert.ToDecimal(row["montoPagado"]);
                    }
                }

                txtTotalPagado.Text = totalPagado.ToString("N2");
                txtCantidadPagos.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar pagos:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LimpiarFormulario()
        {
            txtIDCliente.Clear();
            txtCliente.Clear();

            cbVenta.DataSource = null;
            cbVenta.Items.Clear();
            cbVenta.Enabled = false;

            chkTodasLasVentas.Checked = true;

            dtpFechaInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpFechaFin.Value = DateTime.Today;

            txtTotalPagado.Clear();
            txtCantidadPagos.Clear();

            dgvPagos.DataSource = null;

            _ventasCliente = new DataTable();
        }

        private void LimpiarVentasCliente()
        {
            txtCliente.Clear();
            cbVenta.DataSource = null;
            cbVenta.Items.Clear();
            cbVenta.Enabled = false;
            _ventasCliente = new DataTable();
        }

        private int ParseInt(string valor)
        {
            if (int.TryParse(valor.Trim(), out int r))
                return r;

            throw new Exception($"El valor '{valor}' no es un entero válido.");
        }
    }
}