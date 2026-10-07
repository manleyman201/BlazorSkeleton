using Microsoft.AspNetCore.Components;

namespace BlazorSkeleton.UI.Components.Dialogs
{
    public partial class AlertDialog
    {
        [Parameter]
        public string? Message { get; set; }
    }
}
