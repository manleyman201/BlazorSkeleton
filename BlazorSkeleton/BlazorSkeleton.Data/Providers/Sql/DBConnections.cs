namespace BlazorSkeleton.Data.Providers.Sql
{
    /// <summary>Binds the "DBConnections" section of appsettings.{Environment}.json.</summary>
    public class DBConnections
    {
        public List<DBConnection> DBConnection { get; set; } = [];
    }

    /// <summary>One named database connection. <see cref="Name"/> is what providers pass as the connection name.</summary>
    public class DBConnection
    {
        public string Name { get; set; } = string.Empty;

        /// <summary>True uses the app's Windows identity (integrated security); false uses <see cref="UserName"/>/<see cref="UserPassword"/>.</summary>
        public bool ImpersonateUser { get; set; }

        public string Instance { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public bool PersistSecurityInfo { get; set; }

        /// <summary>"READONLY" or "READWRITE" (the default).</summary>
        public string? ApplicationIntent { get; set; }
    }
}
