using BlazorSkeleton.Data.Models;

namespace BlazorSkeleton.Data.Services.Interfaces
{
    /// <summary>
    /// Service for the Example feature. Components inject services (never providers); services own
    /// any business rules and delegate data access to the matching SQL provider.
    /// </summary>
    public interface IExampleService
    {
        Task<List<ExampleItem>> FetchExampleItemsAsync();
        Task<int> UpdateExampleItemAsync(ExampleItem item);
    }
}
