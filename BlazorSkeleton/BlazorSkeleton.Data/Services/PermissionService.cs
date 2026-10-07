using Microsoft.Extensions.Hosting;
using BlazorSkeleton.Data.Services.Interfaces;

namespace BlazorSkeleton.Data.Services
{
    /// <inheritdoc cref="IPermissionService"/>
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationState? _applicationState;
        private readonly IHostEnvironment? _hostEnvironment;

        public PermissionService(ApplicationState? applicationState, IHostEnvironment? hostEnvironment)
        {
            _applicationState = applicationState;
            _hostEnvironment = hostEnvironment;
        }

        // Case-insensitive on purpose: a casing mismatch in group names would silently grant nothing.
        private bool Has(string permission) =>
            _applicationState?.CurrentUser?.Permissions?
                .Any(p => string.Equals(p, permission, StringComparison.OrdinalIgnoreCase)) == true;

        // End user and super user are only honored in PROD and QA - they grant nothing in DEV or local Development.
        private bool IsProdOrQaEnvironment =>
            _hostEnvironment != null
            && (_hostEnvironment.IsEnvironment("PROD") || _hostEnvironment.IsEnvironment("QA"));

        private bool IsProdEnvironment =>
            _hostEnvironment != null && _hostEnvironment.IsEnvironment("PROD");

        public bool IsAdmin => Has(Constants.PermissionAdmin);

        public bool IsSuperUser => IsProdOrQaEnvironment && Has(Constants.PermissionSuperUser);

        public bool HasEndUserAccess => IsProdOrQaEnvironment && Has(Constants.PermissionEndUser);

        public bool IsDevEngineer => !IsProdEnvironment && Has(Constants.PermissionDevEngineer);

        public bool IsProdEngineer => Has(Constants.PermissionProdEngineer);

        // Both engineer roles get every screen, so they sit alongside admin in each check.
        private bool IsFullAccess => IsAdmin || IsDevEngineer || IsProdEngineer;

        // Standard screen: every tier. For an elevated screen, drop HasEndUserAccess.
        public bool CanAccessExample => HasEndUserAccess || IsSuperUser || IsFullAccess;
    }
}
