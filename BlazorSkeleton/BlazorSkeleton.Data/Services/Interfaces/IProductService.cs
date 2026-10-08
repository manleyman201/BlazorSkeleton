using BlazorSkeleton.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSkeleton.Data.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<int> SaveProductAsync(Product product);
    }
}
