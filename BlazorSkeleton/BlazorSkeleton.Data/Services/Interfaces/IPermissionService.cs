namespace BlazorSkeleton.Data.Services.Interfaces
{
    /// <summary>
    /// Central place for the application's permission rules. Permission strings arrive in
    /// ApplicationState.CurrentUser.Permissions, sourced from the signed-in user's role claims.
    /// Add one CanAccessX property per tab/screen.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>Admin - every screen, every environment.</summary>
        bool IsAdmin { get; }

        /// <summary>Super user - every screen, but only in PROD and QA.</summary>
        bool IsSuperUser { get; }

        /// <summary>End user - the base tier, only honored in PROD and QA.</summary>
        bool HasEndUserAccess { get; }

        /// <summary>DEV engineer - every screen, in every environment except PROD.</summary>
        bool IsDevEngineer { get; }

        /// <summary>PROD engineer - every screen, every environment.</summary>
        bool IsProdEngineer { get; }

        bool CanAccessExample { get; }
    }
}
