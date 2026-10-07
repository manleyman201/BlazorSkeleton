using Microsoft.Extensions.Logging;
using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Providers.Interfaces;
using BlazorSkeleton.Data.Services.Interfaces;

namespace BlazorSkeleton.Data.Services
{
    public class ExampleService : IExampleService
    {
        private readonly IExampleSqlProvider _sqlProvider;
        private readonly ILogger<ExampleService> _logger;

        public ExampleService(IExampleSqlProvider sqlProvider, ILogger<ExampleService> logger)
        {
            _sqlProvider = sqlProvider ?? throw new ArgumentNullException(nameof(sqlProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<ExampleItem>> FetchExampleItemsAsync()
        {
            try
            {
                return await _sqlProvider.FetchExampleItemsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} -> Unable to get data: {Message}", nameof(FetchExampleItemsAsync), ex.Message);
                throw;
            }
        }

        public async Task<int> UpdateExampleItemAsync(ExampleItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            try
            {
                return await _sqlProvider.UpdateExampleItemAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} -> Unable to update data: {Message}", nameof(UpdateExampleItemAsync), ex.Message);
                throw;
            }
        }
    }
}
