using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Managers
{
    public class CustomerManager
    {
        private WebShopContext _context;
        private readonly IContextHelper _contextHelper;

        public CustomerManager(IContextHelper contextHelper)
        {
            _contextHelper = contextHelper;
        }

        public Customer GetCustomer(int id)
        {
            _context = _contextHelper.SetContext();
            var customer = _context.Customer.Include(c => c.ShoppingCart).FirstOrDefault(m => m.CustomerId == id);
            return customer;
        }

        public void AddCustomer(Customer customer)
        {
            _context = _contextHelper.SetContext();
            try
            {
                _context.Add(customer);
                _context.SaveChanges();
            }
            catch
            {
                //TODO
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            _context = _contextHelper.SetContext();
            try
            {
                _context.Update(customer);
                _context.SaveChanges();
            }
            catch
            {
                //TODO
            }
        }

    }
}
