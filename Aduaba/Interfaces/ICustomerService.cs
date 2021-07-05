using Aduaba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface ICustomerService
    {
        void AddCustomer(Customer customer);
        Customer GetCustomerById(string customerId);
        void UpdateCustomerDetails(Customer customer);
        bool CustomerExists(string customerId);
        void DeleteCustomer(Customer customer);

    }
}
