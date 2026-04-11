using System.Data;
using Microsoft.Data.SqlClient;

namespace LotificadoraApp;

internal static class Db
{
    public static string ConnectionString { get; set; } = "Server=3.128.144.165;Database=DB20192002534;User Id=cindy.soler;Password=CS20192002534;Encrypt=True;TrustServerCertificate=True;";


    public static SqlParameter Parameter(string name, object? value)
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
