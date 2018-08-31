using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Managers
{
    public class ProductManager
    {
        private readonly IContextHelper _contextHelper;
        private WebShopContext _context;

        public ProductManager(IContextHelper contextHelper)
        {
            _contextHelper = contextHelper;
        }

        public List<Product> GetAllProducts()
        {
            _context = _contextHelper.SetContext();
            var products = _context.Product.ToList();
            return products;
        }

        public Product GetProduct(int id)
        {
            _context = _contextHelper.SetContext();
            var product = _context.Product.FirstOrDefault(m => m.ProductId == id);
            return product;
        }

        public void AddProduct(Product product)
        {
            _context = _contextHelper.SetContext();
            try
            {
                _context.Add(product);
                _context.SaveChanges();
            }
            catch
            {
                //TODO
            }
        }

        public void UpdateProduct(Product product)
        {
            _context = _contextHelper.SetContext();
            try
            {
                _context.Update(product);
                _context.SaveChanges();
            }
            catch
            {
                //TODO
            }
        }

        public void DeleteProduct(int id)
        {
            _context = _contextHelper.SetContext();
            try
            {
                var product = GetProduct(id);
                _context.Product.Remove(product);
                _context.SaveChanges();
            }
            catch
            {
                //TODO
            }
        }
    }
}
