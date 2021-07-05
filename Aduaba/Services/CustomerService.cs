using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public void AddCustomer(Customer customer)
        {
            //if (customer == null)
            //    throw new ArgumentNullException(nameof(customer));
            //else
            //{
            //    _context.Customers.Add(customer);
            //    _context.SaveChanges();
            //}
        }

        public void UpdateCustomerDetails(Customer customer)
        {
            _context.SaveChanges();
        }

        public Customer GetCustomerById(string customerId)
        {
            if (customerId == null) { throw new ArgumentNullException(nameof(customerId)); }
            else
            {
                var customer = _context.Users.FirstOrDefault(p => p.Id == customerId);
                return customer;
            }
        }

        public bool CustomerExists(string customerId)
        {
            if (customerId == null) { throw new ArgumentNullException(nameof(customerId)); }
            else return _context.Users.Any(c => c.Id == customerId);
        }

        public void DeleteCustomer(Customer customer)
        {
            //if(customer == null)
            //{
            //    throw new ArgumentNullException(nameof(customer));
            //}
            //_context.Customers.Remove(customer);
            //_context.SaveChanges();
        }
    }
}
