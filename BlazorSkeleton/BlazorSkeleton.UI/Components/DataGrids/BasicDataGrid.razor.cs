using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorSkeleton.Data.Utilities;

namespace BlazorSkeleton.UI.Components.DataGrids
{
    /// <summary>
    /// Generic, reusable grid with paging/sorting/filtering and CSV export.
    /// Usage: &lt;BasicDataGrid TItem="MyModel" Data="@rows"&gt;&lt;Columns&gt;...RadzenDataGridColumn...&lt;/Columns&gt;&lt;/BasicDataGrid&gt;
    /// </summary>
    public partial class BasicDataGrid<TItem> where TItem : class
    {
        [Inject]
        public IJSRuntime? JS { get; set; }

        [Parameter]
        public string Title { get; set; } = "Report Data";

        [Parameter]
        public List<TItem> Data { get; set; } = [];

        [Parameter]
        public bool IsLoading { get; set; }

        [Parameter]
        public RenderFragment? Columns { get; set; }

        [Parameter]
        public int PageSize { get; set; } = 50;

        [Parameter]
        public IEnumerable<int> PageSizeOptions { get; set; } = [5, 10, 25, 50];

        [Parameter]
        public string ExportFilePrefix { get; set; } = "csvExport";

        // saveAsFile is defined in wwwroot/js/saveFile.js
        public async Task ExportToCsv()
        {
            if (JS == null || Data.Count == 0) return;

            var csv = CsvUtilities.CreateCsvFile(Data);
            await JS.InvokeVoidAsync("saveAsFile", $"{ExportFilePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.csv", Convert.ToBase64String(csv));
        }
    }
}
