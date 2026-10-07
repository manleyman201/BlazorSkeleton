using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorSkeleton.Data;
using Radzen;

namespace BlazorSkeleton.UI.Pages
{
    public partial class Index : IDisposable
    {
        [Inject]
        private ILogger<Index>? Logger { get; set; }

        [Inject]
        public ApplicationState? ApplicationState { get; set; }

        [Inject]
        public SessionState? SessionState { get; set; }

        [Inject]
        public DialogService? DialogService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (ApplicationState != null) ApplicationState.OnChange += Refresh;
            if (SessionState != null) SessionState.OnChange += Refresh;
            if (DialogService != null) DialogService.OnOpen += Open;

            await base.OnInitializedAsync();
        }

        public void Dispose()
        {
            if (ApplicationState != null) ApplicationState.OnChange -= Refresh;
            if (SessionState != null) SessionState.OnChange -= Refresh;
            if (DialogService != null) DialogService.OnOpen -= Open;
        }

        void Refresh() => InvokeAsync(StateHasChanged);

        #region Radzen Dialog
        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            Logger?.LogInformation("{Type} opened", type.Name);
        }
        #endregion
    }
}
