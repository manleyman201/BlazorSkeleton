using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSkeleton.Data.Services
{


    public class OrderService : IOrderService
    {

        // Declare variable
        // all of the injections for this class go here
        private readonly IProductService _productService;
        // pass into the constructor
        public OrderService(IProductService productService)
        {
            // assign
            _productService = productService;
        }

        public Task<Order?> GetOrderByIdAsync(int ID)
        {
            return Task.FromResult(MockData.GetOrders().FirstOrDefault(o => o.ID == ID));
        }



        public async Task<List<Order>> GetOrders()

        {
           var orders = new List<Order>();
           orders = await Task.FromResult(MockData.GetOrders());

            return orders;
        }

        public Task<List<Order>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}

