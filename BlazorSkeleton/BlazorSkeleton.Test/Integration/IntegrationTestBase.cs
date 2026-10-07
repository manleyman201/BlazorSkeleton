namespace BlazorSkeleton.Test.Integration
{
    /// <summary>Base for tests that hit a real database using DBConnections from appsettings.Test.json.</summary>
    public class IntegrationTestBase
    {
        public IConfiguration Configuration { get; set; }
        public IOptions<DBConnections> Connections { get; set; }

        public IntegrationTestBase()
        {
            Configuration = Providers.ConfigurationProvider.Initialize();
            var conns = Configuration.GetSection("DBConnections").Get<DBConnections>();
            Connections = Options.Create(conns!);
        }
    }
}
