using System.Data;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp;

internal static class Db
{
    // Ajusta este valor a tu instancia de SQL Server si es necesario.
    public static string ConnectionString { get; set; } =
        "Data Source=LAPTOP-MKNJTV9V\\SQLEXPRESS;Initial Catalog=Grupo8;Integrated Security=True;Trust Server Certificate=True";

    public static SqlParameter P(string name, object? value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }

    public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(sql, connection);

        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }

        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public static DataTable ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(procedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }

        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }
}
