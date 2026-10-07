namespace BlazorSkeleton.Data.Models
{
    /// <summary>General-purpose result for procedures that report success plus a message.</summary>
    public class ProcExecutionResult
    {
        public bool? Result { get; set; }
        public string? StatusMessage { get; set; }
    }
}
