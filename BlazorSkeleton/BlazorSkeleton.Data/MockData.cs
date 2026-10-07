using BlazorSkeleton.Data.Models;

namespace BlazorSkeleton.Data
{
    /// <summary>
    /// Hard-coded sample data for testing and UI development before the SQL providers exist.
    /// Every Order.ProductID matches a Product.ID below, and every Order.CustomerName matches a Customer below. Each call returns new instances, so callers can modify them freely.
    /// </summary>
    public static class MockData
    {
        public static List<Product> GetProducts() =>
        [
            new() { ID = 1, Name = "Organic Bananas", Category = "Produce", Price = 0.69m, InStock = true },
            new() { ID = 2, Name = "Hass Avocado", Category = "Produce", Price = 1.25m, InStock = true },
            new() { ID = 3, Name = "Honeycrisp Apples (3 lb)", Category = "Produce", Price = 5.99m, InStock = true },
            new() { ID = 4, Name = "Organic Baby Spinach", Category = "Produce", Price = 3.99m, InStock = false },
            new() { ID = 5, Name = "Almond Milk (Unsweetened)", Category = "Dairy & Alternatives", Price = 3.49m, InStock = true },
            new() { ID = 6, Name = "Greek Yogurt (32 oz)", Category = "Dairy & Alternatives", Price = 5.49m, InStock = true },
            new() { ID = 7, Name = "Cage-Free Eggs (Dozen)", Category = "Dairy & Alternatives", Price = 4.79m, InStock = true },
            new() { ID = 8, Name = "Sourdough Bread", Category = "Bakery", Price = 4.99m, InStock = true },
            new() { ID = 9, Name = "Rolled Oats (42 oz)", Category = "Bulk & Pantry", Price = 6.29m, InStock = true },
            new() { ID = 10, Name = "Raw Almonds (1 lb)", Category = "Bulk & Pantry", Price = 7.99m, InStock = false },
            new() { ID = 11, Name = "Extra Virgin Olive Oil", Category = "Bulk & Pantry", Price = 9.99m, InStock = true },
            new() { ID = 12, Name = "Wild Atlantic Salmon Fillet", Category = "Meat & Seafood", Price = 12.99m, InStock = true },
            new() { ID = 13, Name = "Boneless Chicken Breast", Category = "Meat & Seafood", Price = 8.49m, InStock = true },
            new() { ID = 14, Name = "Dark Chocolate Bar (72%)", Category = "Snacks", Price = 2.99m, InStock = true },
            new() { ID = 15, Name = "Kombucha (Ginger)", Category = "Beverages", Price = 3.79m, InStock = true },
        ];

        public static List<Order> GetOrders() =>
        [
            new() { ID = 1001, CustomerId = 1, Quantity = 6, OrderDate = new DateTime(2026, 9, 1, 9, 15, 0), Status = "Delivered" },
            new() { ID = 1002, CustomerId = 2, Quantity = 2, OrderDate = new DateTime(2026, 9, 3, 14, 30, 0), Status = "Delivered" },
            new() { ID = 1003, CustomerId = 3, Quantity = 3, OrderDate = new DateTime(2026, 9, 7, 11, 5, 0), Status = "Cancelled" },
            new() { ID = 1004, CustomerId = 4, Quantity = 1, OrderDate = new DateTime(2026, 9, 10, 8, 45, 0), Status = "Delivered" },
            new() { ID = 1005, CustomerId = 5, Quantity = 2, OrderDate = new DateTime(2026, 9, 14, 16, 20, 0), Status = "Shipped" },
            new() { ID = 1006, CustomerId = 6, Quantity = 1, OrderDate = new DateTime(2026, 9, 18, 10, 0, 0), Status = "Shipped" },
            new() { ID = 1007, CustomerId = 2, Quantity = 4, OrderDate = new DateTime(2026, 9, 21, 13, 10, 0), Status = "Processing" },
            new() { ID = 1008, CustomerId = 4, Quantity = 3, OrderDate = new DateTime(2026, 9, 24, 17, 55, 0), Status = "Processing" },
            new() { ID = 1009, CustomerId = 6, Quantity = 10, OrderDate = new DateTime(2026, 9, 26, 12, 40, 0), Status = "Processing" },
            new() { ID = 1010, CustomerId = 1, Quantity = 5, OrderDate = new DateTime(2026, 9, 28, 9, 30, 0), Status = "Pending" },
            new() { ID = 1011, CustomerId = 1, Quantity = 6, OrderDate = new DateTime(2026, 9, 29, 15, 15, 0), Status = "Pending" },
            new() { ID = 1012, CustomerId = 1, Quantity = 2, OrderDate = new DateTime(2026, 9, 30, 18, 5, 0), Status = "Pending" },
        ];

        /// <summary>
        /// Each customer's Orders are the orders from GetOrders() whose CustomerName matches "FirstName LastName".
        /// Robert Kim has no orders, to cover the empty case.
        /// </summary>
        public static List<Customer> GetCustomers()
        {
            List<Customer> customers =
            [
                new() { Id = 1, FirstName = "Maria", LastName = "Garcia" },
                new() { Id = 2, FirstName = "James", LastName = "Chen" },
                new() { Id = 3, FirstName = "Aisha", LastName = "Patel" },
                new() { Id = 4, FirstName = "Daniel", LastName = "Brooks" },
                new() { Id = 5, FirstName = "Emily", LastName = "Nguyen" },
                new() { Id = 6, FirstName = "Robert", LastName = "Kim" },
            ];

            var orders = GetOrders();
            foreach (var customer in customers)
            {
                customer.Orders = orders.Where(o => o.CustomerId == customer.Id).ToList();
            }

            return customers;
        }
    }
}
