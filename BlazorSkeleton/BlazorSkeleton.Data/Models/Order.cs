namespace BlazorSkeleton.Data.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public List<Product> Products { get; set; }
        public int CustomerId { get; set; }

    }
}
