namespace BlazorSkeleton.Data.Models
{
    /// <summary>The signed-in user, built from their authentication claims in MainLayout.</summary>
    public class AppUser
    {
        public string UserId { get; set; } = string.Empty;
        public string? FullName { get; set; }

        /// <summary>Role claims. <see cref="Services.PermissionService"/> matches these against the Constants.Permission* names.</summary>
        public List<string> Permissions { get; set; } = [];
    }
}
