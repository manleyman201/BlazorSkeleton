using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Providers.Interfaces;
using BlazorSkeleton.Data.Utilities;
using BlazorSkeleton.Data.Extensions;
using BlazorSkeleton.Data.Providers.Sql;
using BlazorSkeleton.Data.Services.Interfaces;

namespace BlazorSkeleton.Data.Providers
{
    public class ExampleSqlProvider : IExampleSqlProvider
    {
        private readonly ILogger<ExampleSqlProvider> _logger;
        private readonly ISqlDataProvider _dataProvider;

     

        public ExampleSqlProvider(ILogger<ExampleSqlProvider> logger, ISqlDataProvider dataProvider)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
           
        }

        public async Task<List<ExampleItem>> FetchExampleItemsAsync()
        {
            try
            {
                
              
                _logger.LogEntry();
                var query = new SQLQuery()
                {
                    // TODO: replace with a real procedure
                    StoredProcedureName = "[dbo].[usp_Get_ExampleItems]"
                };
                return await SqlHelper.ExecuteStoredProcedureAsync<ExampleItem>(query, Constants.DatabaseConnectionName,
                    nameof(FetchExampleItemsAsync), _logger, _dataProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} -> Unable to get data: {Message}", nameof(FetchExampleItemsAsync), ex.Message);
                throw;
            }
            finally
            {
                _logger.LogExit();
            }
        }

        public async Task<int> UpdateExampleItemAsync(ExampleItem item)
        {
            try
            {
                _logger.LogEntry();
                var query = new SQLQuery()
                {
                    // TODO: replace with a real procedure
                    StoredProcedureName = "[dbo].[usp_Update_ExampleItem]",
                    Parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("ID", item.ID),
                        new SqlParameter("NAME", (object?)item.NAME ?? DBNull.Value)
                    }
                };
                return await SqlHelper.ExecuteStoredProcedureForDMLAsync(query, Constants.DatabaseConnectionName,
                    nameof(UpdateExampleItemAsync), _logger, _dataProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} -> Unable to update data: {Message}", nameof(UpdateExampleItemAsync), ex.Message);
                throw;
            }
            finally
            {
                _logger.LogExit();
            }
        }
    }
}
