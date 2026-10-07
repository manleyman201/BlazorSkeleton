using BlazorSkeleton.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSkeleton.Data.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int ID);

        Task<List<Order>> GetOrders();

        Task<string> SaveOrder(Order order);
    }


}
