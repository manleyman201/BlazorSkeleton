using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSkeleton.Data.Services
{
    public class ProductService : IProductService
    {
        public async Task<List<Product>> GetProductsAsync()
        {
            var products = new List<Product>();
            products = await Task.FromResult(MockData.GetProducts());

            return products;
        }

        public async Task<int> SaveProductAsync(Product product)
        {
            //validate product does not exist

            var mockSavedObject = new Product();
            mockSavedObject = product;
            return 199;
        }
    }


}
