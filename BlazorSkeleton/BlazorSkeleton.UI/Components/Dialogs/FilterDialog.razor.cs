using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using BlazorSkeleton.Data;
using BlazorSkeleton.Data.Models;

namespace BlazorSkeleton.UI.Components.Dialogs
{
    /// <summary>Dialog used for filtering global variables.</summary>
    public partial class FilterDialog
    {
        [Inject]
        public ILogger<FilterDialog>? Logger { get; set; }

        [Inject]
        public ApplicationState? ApplicationState { get; set; }

        [Inject]
        public DialogService? DialogService { get; set; }

        [Parameter]
        public string? Options { get; set; }

        public AppUser? CurrentUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentUser = ApplicationState?.CurrentUser;

            await base.OnInitializedAsync();
        }

        void CloseDialog()
        {
            DialogService?.Close();
        }
    }
}
