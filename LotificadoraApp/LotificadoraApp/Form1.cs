namespace LotificadoraApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BuildMenu();
        }

        private void BuildMenu()
        {
            Text = "Menu de Consultas - Lotificadora";
            Width = 900;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(20)
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var titulo = new Label
            {
                Text = "Consultas solicitadas por rubrica",
                Dock = DockStyle.Fill,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 14, FontStyle.Bold),
                Padding = new Padding(0, 0, 0, 10)
            };
            root.SetColumnSpan(titulo, 2);

            var gbVistas = new GroupBox
            {
                Text = "2 Formularios con Vistas",
                Dock = DockStyle.Fill,
                Padding = new Padding(12)
            };
            var pnlVistas = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            pnlVistas.Controls.Add(CreateOpenButton("Vista - Lotes Disponibles", () => new frmConsultarPlanPago()));
            pnlVistas.Controls.Add(CreateOpenButton("Vista - Creditos Activos", () => new ConsultaVistaCreditosForm()));
            gbVistas.Controls.Add(pnlVistas);

            var gbSp = new GroupBox
            {
                Text = "2 Formularios con Stored Procedures",
                Dock = DockStyle.Fill,
                Padding = new Padding(12)
            };
            var pnlSp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            pnlSp.Controls.Add(CreateOpenButton("SP - Estado Cuenta Cliente", () => new ConsultaSpEstadoCuentaForm()));
            pnlSp.Controls.Add(CreateOpenButton("SP - Recaudacion Etapa", () => new ConsultaSpRecaudacionForm()));
            gbSp.Controls.Add(pnlSp);

            var gbFn = new GroupBox
            {
                Text = "2 Formularios con Funciones Tabla",
                Dock = DockStyle.Fill,
                Padding = new Padding(12)
            };
            var pnlFn = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            pnlFn.Controls.Add(CreateOpenButton("Funcion - Lotes Disponibles", () => new ConsultaFnLotesForm()));
            pnlFn.Controls.Add(CreateOpenButton("Funcion - Estado Cuenta", () => new ConsultaFnEstadoCuentaForm()));
            gbFn.Controls.Add(pnlFn);

            var pnlConexion = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true
            };

            root.SetColumnSpan(pnlConexion, 2);

            root.Controls.Add(titulo, 0, 0);
            root.Controls.Add(gbVistas, 0, 1);
            root.Controls.Add(gbSp, 1, 1);
            root.Controls.Add(gbFn, 0, 2);
            root.Controls.Add(pnlConexion, 0, 3);

            Controls.Clear();
            Controls.Add(root);
        }

        private Button CreateOpenButton(string text, Func<Form> formFactory)
        {
            var button = new Button
            {
                Text = text,
                Width = 290,
                Height = 42,
                Margin = new Padding(0, 0, 0, 10)
            };

            button.Click += (_, _) =>
            {
                using var form = formFactory();
                form.ShowDialog(this);
            };

            return button;
        }
    }
}
