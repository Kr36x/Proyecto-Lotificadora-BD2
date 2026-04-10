using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp
{
    public partial class frmConsultaVistaLotes : Form
    {
        private readonly string connectionString =
            "Server=3.128.144.165;Database=DB20222030195;User Id=carlos.alvarez;Password=CA20222030195;Encrypt=True;TrustServerCertificate=True;";

        public frmConsultaVistaLotes()
        {
            InitializeComponent();

            btnConsultar.Click += btnConsultar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
        }

        private void frmConsultaVistaLotes_Load(object sender, EventArgs e)
        {
            ConsultarLotes();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            ConsultarLotes();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdProyecto.Clear();
            txtIdEtapa.Clear();
            ConsultarLotes();
        }

        private void ConsultarLotes()
        {
            try
            {
                int? idProyecto = ParseNullableInt(txtIdProyecto.Text);
                int? idEtapa = ParseNullableInt(txtIdEtapa.Text);

                string sql = @"
                    SELECT *
                    FROM dbo.vw_lotes_disponibles
                    WHERE (@idProyecto IS NULL OR idProyecto = @idProyecto)
                      AND (@idEtapa IS NULL OR idEtapa = @idEtapa)
                    ORDER BY idProyecto, idEtapa, idBloque, numeroLote;";

                using SqlConnection connection = new SqlConnection(connectionString);
                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@idProyecto", (object?)idProyecto ?? DBNull.Value);
                command.Parameters.AddWithValue("@idEtapa", (object?)idEtapa ?? DBNull.Value);

                using SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvVistaLotes.DataSource = table;

                // Opcional: mejora visual automática
                dgvVistaLotes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvVistaLotes.ReadOnly = true;
                dgvVistaLotes.AllowUserToAddRows = false;
                dgvVistaLotes.AllowUserToDeleteRows = false;
                dgvVistaLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar lotes disponibles:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private int? ParseNullableInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (int.TryParse(value.Trim(), out int result))
                return result;

            throw new Exception($"El valor '{value}' no es un número entero válido.");
        }
    }
}