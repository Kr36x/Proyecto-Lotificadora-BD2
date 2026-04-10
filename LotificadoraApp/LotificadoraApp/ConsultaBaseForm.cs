using System.Data;

namespace LotificadoraApp;

public abstract class ConsultaBaseForm : Form
{
    protected readonly FlowLayoutPanel FiltrosPanel;
    private readonly DataGridView _grid;
    private readonly Label _lblResumen;

    protected ConsultaBaseForm(string titulo)
    {
        Text = titulo;
        Width = 1080;
        Height = 680;
        StartPosition = FormStartPosition.CenterParent;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(10)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80
        };

        FiltrosPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            Padding = new Padding(0, 8, 0, 0)
        };

        var actionsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = 240,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 8, 0, 0)
        };

        var btnBuscar = new Button
        {
            Text = "Buscar",
            Width = 100,
            Height = 34
        };
        btnBuscar.Click += (_, _) => EjecutarBusqueda();

        var btnLimpiar = new Button
        {
            Text = "Limpiar",
            Width = 100,
            Height = 34
        };
        btnLimpiar.Click += (_, _) =>
        {
            LimpiarFiltros();
            EjecutarBusqueda();
        };

        actionsPanel.Controls.Add(btnBuscar);
        actionsPanel.Controls.Add(btnLimpiar);

        topPanel.Controls.Add(FiltrosPanel);
        topPanel.Controls.Add(actionsPanel);

        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false
        };

        _lblResumen = new Label
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(0, 8, 0, 4),
            Text = "Registros: 0"
        };

        root.Controls.Add(topPanel, 0, 0);
        root.Controls.Add(_grid, 0, 1);
        root.Controls.Add(_lblResumen, 0, 2);

        Controls.Add(root);
        Load += (_, _) => EjecutarBusqueda();
    }

    protected abstract DataTable Consultar();

    protected virtual void LimpiarFiltros()
    {
    }

    protected int? ParseNullableInt(TextBox textBox)
    {
        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            return null;
        }

        if (!int.TryParse(textBox.Text.Trim(), out var value))
        {
            throw new InvalidOperationException($"El valor '{textBox.Text}' no es un numero entero valido.");
        }

        return value;
    }

    protected int ParseRequiredInt(TextBox textBox, string fieldName)
    {
        var value = ParseNullableInt(textBox);
        if (!value.HasValue)
        {
            throw new InvalidOperationException($"El campo {fieldName} es obligatorio.");
        }

        return value.Value;
    }

    protected static Label CreateLabel(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0, 8, 0, 0),
            Margin = new Padding(0, 0, 8, 0)
        };
    }

    protected static TextBox CreateTextBox(int width = 90)
    {
        return new TextBox
        {
            Width = width,
            Margin = new Padding(0, 4, 14, 0)
        };
    }

    private void InitializeComponent()
    {

    }

    private void EjecutarBusqueda()
    {
        try
        {
            var data = Consultar();
            _grid.DataSource = data;
            _lblResumen.Text = $"Registros: {data.Rows.Count}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
