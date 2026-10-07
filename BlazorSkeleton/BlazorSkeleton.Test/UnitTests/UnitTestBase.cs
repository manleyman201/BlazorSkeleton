namespace BlazorSkeleton.Test.UnitTests
{
    public class UnitTestBase
    {
        public IConfiguration Configuration { get; set; }

        public UnitTestBase()
        {
            Configuration = Providers.ConfigurationProvider.Initialize();
        }
    }
}
