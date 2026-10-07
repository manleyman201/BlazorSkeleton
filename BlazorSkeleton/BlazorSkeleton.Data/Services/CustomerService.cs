using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Services.Interfaces;

namespace BlazorSkeleton.Data.Services
{
    public class CustomerService : ICustomerService
    {
        public async Task<List<Customer>> GetAll()
        {
            var customers = new List<Customer>();
            customers = await Task.FromResult(MockData.GetCustomers());

            return customers;
        }
    }
}
