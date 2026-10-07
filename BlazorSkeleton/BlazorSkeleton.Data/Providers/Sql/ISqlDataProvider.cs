using System.Data;

namespace BlazorSkeleton.Data.Providers.Sql
{
    /// <summary>
    /// Low-level stored procedure execution against a named connection from <see cref="DBConnections"/>.
    /// Feature providers go through <see cref="Utilities.SqlHelper"/> rather than calling this directly.
    /// </summary>
    public interface ISqlDataProvider
    {
        /// <summary>Executes a procedure and returns its first result set.</summary>
        DataTable GetDataTableFromProcedure(SQLQuery sqlQuery, string connectionName);

        /// <summary>Executes a procedure and returns every result set.</summary>
        DataSet GetDataSetFromProcedure(SQLQuery sqlQuery, string connectionName);

        /// <summary>Executes a procedure that returns no rows and returns the affected row count.</summary>
        int ExecuteStoredProcedure(SQLQuery sqlQuery, string connectionName);
    }
}
