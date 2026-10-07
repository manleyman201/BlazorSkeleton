using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using BlazorSkeleton.Data;
using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Utilities;
using BlazorSkeleton.UI.Components.Dialogs;

namespace BlazorSkeleton.UI.Components.Buttons
{
    /// <summary>Basic upload button for strongly typed CSV data.</summary>
    public partial class UploadButton
    {
        [Inject]
        public ApplicationState? ApplicationState { get; set; }

        [Inject]
        public DialogService? DialogService { get; set; }

        UploadFileDetails fileDetails { get; set; } = new();

        public async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var singleFile = e.File;
            fileDetails.Name = singleFile.Name;
            fileDetails.Size = singleFile.Size;

            if (!string.Equals(Path.GetExtension(singleFile.Name), ".csv", StringComparison.OrdinalIgnoreCase))
            {
                if (DialogService != null)
                    await DialogService.OpenAsync<AlertDialog>("Error", new Dictionary<string, object> { { "Message", "File must be in CSV format" } });
                return;
            }

            await using var stream = singleFile.OpenReadStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            var content = Encoding.UTF8.GetString(bytes);

            fileDetails.Content = content;
            fileDetails.ContentBytes = bytes;

            // Replace ExampleItem with the model your upload maps to (and the type of ApplicationState.UploadedItems).
            var csvData = CsvUtilities.GetCsvRecords<ExampleItem>(content);
            if (ApplicationState != null)
            {
                ApplicationState.UploadedItems = csvData;
                ApplicationState.UpdateState();
            }
        }
    }
}
