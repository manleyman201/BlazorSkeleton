namespace BlazorSkeleton.Data.Models
{
    /// <summary>
    /// Example model. SqlHelper maps stored-procedure result columns onto these properties via
    /// Utilities.EntityFactory.GetEntities&lt;T&gt;, so keep the names aligned with the procedure's result set.
    /// </summary>
    public class ExampleItem
    {
        public int ID { get; set; }
        public string? NAME { get; set; }
    }
}

