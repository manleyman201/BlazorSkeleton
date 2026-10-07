using Microsoft.AspNetCore.Components;
using Radzen;

namespace BlazorSkeleton.UI.Components.Dialogs
{
    /// <summary>
    /// Empty template for a basic dialog. Open with:
    /// var ok = await DialogService.OpenAsync&lt;BasicDialog&gt;("Title", new() { { "Message", "..." } });
    /// The result is true (OK) or false (Cancel).
    /// </summary>
    public partial class BasicDialog
    {
        [Inject]
        public DialogService? DialogService { get; set; }

        [Parameter]
        public string? Message { get; set; }

        private void Close(bool result) => DialogService?.Close(result);
    }
}
