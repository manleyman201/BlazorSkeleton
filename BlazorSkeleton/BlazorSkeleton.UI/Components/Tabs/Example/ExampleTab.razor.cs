using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorSkeleton.Data;
using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Services.Interfaces;
using BlazorSkeleton.Data.Utilities;
using Radzen;

namespace BlazorSkeleton.UI.Components.Tabs.Example
{
    /// <summary>
    /// Example feature tab. Pattern: inject the feature's service (never its SQL provider), track a
    /// busy flag for the UI, and surface failures through DialogService while logging the detail.
    /// </summary>
    public partial class ExampleTab
    {
        [Inject]
        private IExampleService? ExampleService { get; set; }

        [Inject]
        private DialogService? DialogService { get; set; }

        [Inject]
        private ILogger<ExampleTab>? Logger { get; set; }

        private List<ExampleItem> items = [];
        private bool isLoading;

        private async Task LoadAsync()
        {
            if (ExampleService == null) return;

            isLoading = true;
            try
            {
                items = await ExampleService.FetchExampleItemsAsync();
            }
            catch (Exception ex)
            {
                if (Logger != null)
                    ValidationHelper.LogUiError(ex, "Unable to load example items", Logger, nameof(LoadAsync), nameof(ExampleTab));

                if (DialogService != null)
                    await DialogService.Alert($"Something went wrong while processing your request. {ex.Message}",
                        Constants.ExampleGroupName, new AlertOptions() { OkButtonText = "OK" });
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
