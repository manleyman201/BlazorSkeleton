namespace BlazorSkeleton.Data.Models
{
    /// <summary>Metadata and content of a file picked in the UploadButton.</summary>
    public class UploadFileDetails
    {
        public string? Name { get; set; }
        public long Size { get; set; }
        public string? Content { get; set; }
        public byte[]? ContentBytes { get; set; }
    }
}
