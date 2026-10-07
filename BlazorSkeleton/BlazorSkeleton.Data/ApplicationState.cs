using BlazorSkeleton.Data.Models;

namespace BlazorSkeleton.Data
{
    /// <summary>
    /// Scoped (per-circuit) cross-cutting state: the current user, current page and uploaded data.
    /// Call <see cref="UpdateState"/> after changing it so subscribed components re-render.
    /// </summary>
    public class ApplicationState
    {
        public string? CurrentUserId { get; set; }
        public AppUser? CurrentUser { get; set; }
        public string? PagePath { get; set; }

        /// <summary>Rows parsed by the UploadButton. Change the type to the model your upload maps to.</summary>
        public List<ExampleItem> UploadedItems { get; set; } = [];

        public event Action? OnChange;

        public void UpdateState()
        {
            OnChange?.Invoke();
        }
    }
}
