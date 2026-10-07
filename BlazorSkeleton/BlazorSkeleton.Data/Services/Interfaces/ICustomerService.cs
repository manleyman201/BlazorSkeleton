using BlazorSkeleton.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorSkeleton.Data.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAll();

    }
}
