using Microsoft.AspNetCore.Components;
using BlazorSkeleton.Data;

namespace BlazorSkeleton.UI.Shared
{
    public partial class LoginDisplay : IDisposable
    {
        [Inject]
        public ApplicationState? ApplicationState { get; set; }

        protected override Task OnInitializedAsync()
        {
            if (ApplicationState != null) ApplicationState.OnChange += Refresh;

            return base.OnInitializedAsync();
        }

        void Refresh() => InvokeAsync(StateHasChanged);

        public void Dispose()
        {
            if (ApplicationState != null) ApplicationState.OnChange -= Refresh;
        }
    }
}
