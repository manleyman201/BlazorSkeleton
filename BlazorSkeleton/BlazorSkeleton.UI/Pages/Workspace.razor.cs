using Microsoft.AspNetCore.Components;
using BlazorSkeleton.Data.Services.Interfaces;

namespace BlazorSkeleton.UI.Pages
{
    public partial class Workspace
    {
        [Inject]
        private IPermissionService? PermissionService { get; set; }

        // Only the selected tab renders (see the @if in each tab), so hidden tabs never hit the database.
        private int selectedIndex { get; set; } = 0;

        private bool HasExampleAccess => PermissionService?.CanAccessExample == true;

        private void OnTabChange(int index)
        {
            selectedIndex = index;
        }
    }
}
