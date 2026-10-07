using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;
using BlazorSkeleton.Data;

namespace BlazorSkeleton.UI.Components
{
    public partial class EnvironmentBanner : IDisposable
    {
        [Inject]
        private ApplicationState? ApplicationState { get; set; }

        [Inject]
        private IHostEnvironment? HostEnvironment { get; set; }

        private string? BannerClass { get; set; }

        protected override Task OnInitializedAsync()
        {
            if (ApplicationState != null) ApplicationState.OnChange += Refresh;

            SetBannerColor();

            return base.OnInitializedAsync();
        }

        public void Dispose()
        {
            if (ApplicationState != null) ApplicationState.OnChange -= Refresh;
        }

        void Refresh() => InvokeAsync(StateHasChanged);

        private void SetBannerColor()
        {
            if (HostEnvironment != null) BannerClass = HostEnvironment.EnvironmentName switch
            {
                "PROD" or "Production" or "Release" => "bg-danger",
                "QA" => "bg-success",
                _ => "bg-warning",
            };
        }
    }
}
