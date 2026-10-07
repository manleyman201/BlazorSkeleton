namespace BlazorSkeleton.Data
{
    public static partial class Constants
    {
        // Update with valid application name
        public static readonly string ApplicationName = "Blazor Skeleton";

        // Database connection name - must match a "Name" under DBConnections in appsettings.{Environment}.json
        public static readonly string DatabaseConnectionName = "AppDatabase";

        // SQL time limits in minutes (see Utilities/SqlHelper.cs)
        public static readonly int CancellationTokenTimeDuration = 6;
        public static readonly int ExportCancellationTokenTimeDuration = 10;

        // Export Excel Maximum Record Count
        public static readonly int ExportExcelMaximumCount = 500;

        #region Group Names

        public const string ExampleGroupName = "Example";

        #endregion

        // Permission strings expected in ApplicationState.CurrentUser.Permissions, sourced from the
        // signed-in user's role claims (see MainLayout.ValidateAuthenticatedUser).
        #region Permissions

        public static readonly string PermissionEndUser = "BlazorSkeletonEndUser";
        public static readonly string PermissionAdmin = "BlazorSkeletonAdmin";
        public static readonly string PermissionSuperUser = "BlazorSkeletonSuperUser";

        // All tabs in DEV and QA (and locally), but never PROD.
        public static readonly string PermissionDevEngineer = "BlazorSkeletonDEVEngineer";

        // All tabs, every environment.
        public static readonly string PermissionProdEngineer = "BlazorSkeletonPRODEngineer";

        #endregion

        #region Grid Operations

        public static readonly string INSERT = "INSERT";
        public static readonly string DELETE = "DELETE";
        public static readonly string UPDATE = "UPDATE";

        #endregion
    }
}
