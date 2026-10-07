namespace BlazorSkeleton.Test.Providers
{
    internal static class ConfigurationProvider
    {
        /// <summary>
        /// Builds test configuration from appsettings.Test.json plus environment variables.
        /// Real SQL credentials should come from environment variables on the machine or build agent
        /// (e.g. DBConnections__DBConnection__0__UserPassword), never from source.
        /// </summary>
        public static IConfiguration Initialize()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
