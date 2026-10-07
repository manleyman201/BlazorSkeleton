using BlazorSkeleton.Data.Models;

namespace BlazorSkeleton.Data.Providers.Interfaces
{
    /// <summary>
    /// SQL provider for the Example feature. Pattern: one provider per feature/tab, each method wraps
    /// exactly one stored procedure and goes through <see cref="Utilities.SqlHelper"/>.
    /// </summary>
    public interface IExampleSqlProvider
    {
        /// <summary>Fetches all example items.</summary>
        Task<List<ExampleItem>> FetchExampleItemsAsync();

        /// <summary>Updates one example item.</summary>
        /// <returns>Affected row count</returns>
        Task<int> UpdateExampleItemAsync(ExampleItem item);
    }
}
