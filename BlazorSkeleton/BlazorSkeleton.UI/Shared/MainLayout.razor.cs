using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radzen;
using BlazorSkeleton.Data;
using BlazorSkeleton.Data.Models;
using BlazorSkeleton.UI.Components.Dialogs;

namespace BlazorSkeleton.UI.Shared
{
    public partial class MainLayout : IDisposable
    {
        [Inject]
        private ApplicationState? ApplicationState { get; set; }

        [Inject]
        public AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        [Inject]
        private IHostEnvironment? HostEnvironment { get; set; }

        [Inject]
        public DialogService? DialogService { get; set; }

        [Inject]
        private ILogger<MainLayout>? Logger { get; set; }

        private static readonly string[] ProductionEnvironmentNames = ["PROD", "Production", "Release"];

        // The banner shows in every environment except production.
        private bool ShowEnvironmentBanner =>
            HostEnvironment != null &&
            !ProductionEnvironmentNames.Any(e => HostEnvironment.EnvironmentName.Equals(e, StringComparison.OrdinalIgnoreCase));

        protected override async Task OnInitializedAsync()
        {
            Logger?.LogInformation("Main Layout loading...");

            if (ApplicationState != null) ApplicationState.OnChange += Refresh;
            if (DialogService != null) DialogService.OnOpen += Open;
            if (DialogService != null) DialogService.OnClose += Close;

            await ValidateAuthenticatedUser();

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Resolves the signed-in user (Windows auth locally, Entra ID elsewhere) and loads their
        /// name and permissions into ApplicationState.CurrentUser. Permissions are the user's role
        /// claims, so assign app roles named after the Constants.Permission* values.
        /// </summary>
        private async Task ValidateAuthenticatedUser()
        {
            if (AuthenticationStateProvider == null || ApplicationState == null) return;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var identity = authState.User.Identity;
            if (identity == null || !identity.IsAuthenticated || identity.Name == null) return;

            var userName = identity.Name;
            Logger?.LogInformation("Authenticated User: {User}", userName);

            // user@domain (Entra ID) or DOMAIN\user (Windows)
            ApplicationState.CurrentUserId = userName.Contains('@')
                ? userName.Split('@').First()
                : userName.Split('\\').Last();

            ApplicationState.CurrentUser = new AppUser
            {
                UserId = ApplicationState.CurrentUserId,
                FullName = authState.User.FindFirst("name")?.Value ?? ApplicationState.CurrentUserId,
                Permissions = authState.User.FindAll(ClaimTypes.Role).Select(c => c.Value).Distinct().ToList()
            };
        }

        public void Dispose()
        {
            if (ApplicationState != null) ApplicationState.OnChange -= Refresh;
            if (DialogService != null) DialogService.OnOpen -= Open;
            if (DialogService != null) DialogService.OnClose -= Close;
        }

        void Refresh() => InvokeAsync(StateHasChanged);

        #region Radzen Dialog

        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            Logger?.LogInformation("{Type} opened", type.Name);
        }

        void Close(dynamic result)
        {
            Logger?.LogInformation("Dialog closed");
        }

        async Task OpenFilterDialog()
        {
            if (DialogService == null) return;

            await DialogService.OpenAsync<FilterDialog>("Filter",
               new Dictionary<string, object>() { { "Options", "" } },
               new DialogOptions() { CloseDialogOnOverlayClick = true });
        }

        #endregion
    }
}
