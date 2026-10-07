using Microsoft.AspNetCore.Components;
using BlazorSkeleton.Data;

namespace BlazorSkeleton.UI.Shared
{
    public partial class NavMenu : IDisposable
    {
        [Inject]
        private ApplicationState? AppState { get; set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        protected override Task OnInitializedAsync()
        {
            if (AppState != null && NavigationManager != null)
            {
                AppState.OnChange += Refresh;
                AppState.PagePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            }

            return base.OnInitializedAsync();
        }

        private void NavigateTo(string path)
        {
            if (AppState != null) AppState.PagePath = path;
            NavigationManager?.NavigateTo(path);
        }

        public void Dispose()
        {
            if (AppState != null) AppState.OnChange -= Refresh;
        }

        void Refresh() => InvokeAsync(StateHasChanged);
    }
}
