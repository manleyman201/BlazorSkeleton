using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace BlazorSkeleton.Data.Providers.Sql
{
    /// <inheritdoc cref="ISqlDataProvider"/>
    public class SqlDataProvider : ISqlDataProvider
    {
        private readonly DBConnections _connections;

        public SqlDataProvider(IOptions<DBConnections> connections)
        {
            _connections = connections?.Value ?? throw new ArgumentNullException(nameof(connections));
        }

        public DataTable GetDataTableFromProcedure(SQLQuery sqlQuery, string connectionName)
        {
            var ds = GetDataSetFromProcedure(sqlQuery, connectionName);
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }

        public DataSet GetDataSetFromProcedure(SQLQuery sqlQuery, string connectionName)
        {
            using var connection = new SqlConnection(BuildConnectionString(connectionName));
            using var command = CreateCommand(sqlQuery, connection);
            using var adapter = new SqlDataAdapter(command);

            var ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        public int ExecuteStoredProcedure(SQLQuery sqlQuery, string connectionName)
        {
            using var connection = new SqlConnection(BuildConnectionString(connectionName));
            using var command = CreateCommand(sqlQuery, connection);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        private static SqlCommand CreateCommand(SQLQuery sqlQuery, SqlConnection connection)
        {
            var command = new SqlCommand(sqlQuery.StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddRange(sqlQuery.Parameters.ToArray());
            return command;
        }

        private string BuildConnectionString(string connectionName)
        {
            var config = _connections.DBConnection.FirstOrDefault(c => string.Equals(c.Name, connectionName, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"No DBConnection named '{connectionName}' is configured.");

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = config.Instance,
                InitialCatalog = config.Database,
                IntegratedSecurity = config.ImpersonateUser,
                PersistSecurityInfo = config.PersistSecurityInfo,
                ApplicationIntent = string.Equals(config.ApplicationIntent, "READONLY", StringComparison.OrdinalIgnoreCase)
                    ? ApplicationIntent.ReadOnly
                    : ApplicationIntent.ReadWrite
            };

            if (!config.ImpersonateUser)
            {
                builder.UserID = config.UserName;
                builder.Password = config.UserPassword;
            }

            return builder.ConnectionString;
        }
    }
}
