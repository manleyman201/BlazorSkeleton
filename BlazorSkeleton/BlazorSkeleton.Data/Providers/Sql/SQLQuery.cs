using Microsoft.Data.SqlClient;

namespace BlazorSkeleton.Data.Providers.Sql
{
    /// <summary>A stored procedure call: the procedure name plus its parameters.</summary>
    public class SQLQuery
    {
        public string StoredProcedureName { get; set; } = string.Empty;
        public List<SqlParameter> Parameters { get; set; } = [];
    }
}
