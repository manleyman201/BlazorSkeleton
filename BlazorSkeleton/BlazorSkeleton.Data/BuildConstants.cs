using System.Reflection;

namespace BlazorSkeleton.Data
{
    public static partial class Constants
    {
        /// <summary>
        /// Time the Data assembly was compiled, stamped by the AssemblyMetadata item in the .csproj.
        /// (Using DateTime.Now here directly would show the app's start time, not its build time.)
        /// </summary>
        public static readonly string LastBuildDate =
            typeof(Constants).Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(a => a.Key == "BuildDate")?.Value ?? "Unknown";
    }
}
