using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Managers
{
    public class ShoppingCartManager
    {
        private readonly IHttpContextAccessor _accessor;
        private WebShopContext _context;
        private readonly IIndex<ConnectionTypes, WebShopContext> _contexts;
        private readonly ConnectionTypes defaultConnection = ConnectionTypes.SqlServer;

        public ShoppingCartManager(IHttpContextAccessor accessor, IIndex<ConnectionTypes, WebShopContext> contexts)
        {
            _accessor = accessor;
            _contexts = contexts;
            _context = _contexts[defaultConnection];
        }

        private void SetContext()
        {
            var type = _accessor.HttpContext.Session.GetString("connection");
            var connectionType = ConnectionTypes.SqlServer;
            if (type != null) connectionType = (ConnectionTypes)System.Enum.Parse(typeof(ConnectionTypes), type);
            else _accessor.HttpContext.Session.SetString("connection", ConnectionTypes.SqlServer.ToString());
            _context = _contexts[connectionType];
        }

        public List<Product> GetProductsFromCart(int customer_id)
        {
            SetContext();
            try
            {
                var customer = _context.Customer.Include(c => c.ShoppingCart).Where(c => c.CustomerId == customer_id).FirstOrDefault();
                var shoppingCartProducts = _context.ShoppingCartProducts.Include(scp => scp.Product).Where(scp => scp.ShoppingCartId == customer.ShoppingCartId);
                var products = new List<Product>();
                foreach (var scp in shoppingCartProducts)
                {
                    var product = scp.Product;
                    product.Quantity = scp.Quantity;
                    products.Add(scp.Product);
                }
                return products;
            }
            catch 
            {
                return new List<Product>();
            }
        }

        public void DeleteFromCart(int customer_id, int productId)
        {
            SetContext();
            try
            {
                var customer = _context.Customer.Include(c => c.ShoppingCart).Where(c => c.CustomerId == customer_id).FirstOrDefault();
                var shoppingCartProduct = _context.ShoppingCartProducts.Where(scp => scp.ShoppingCartId == customer.ShoppingCartId && scp.ProductId == productId).FirstOrDefault();
                _context.ShoppingCartProducts.Remove(shoppingCartProduct);
                _context.SaveChanges();
            }
            catch
            {
                //TODO
            }
        }

        public void AddToCart(int customer_id, Product product)
        {
            SetContext();
            try
            {
                var customer = _context.Customer.Include(c => c.ShoppingCart).Where(c => c.CustomerId == customer_id).FirstOrDefault();
                if (customer.ShoppingCartId == null)
                {
                    var newShoppingCart = new ShoppingCart();
                    customer.ShoppingCart = newShoppingCart;
                }
                var shoppingCartProducts = _context.ShoppingCartProducts.Where(sc => sc.ShoppingCartId == customer.ShoppingCart.ShoppingCartId && sc.ProductId == product.ProductId).FirstOrDefault();

                if (shoppingCartProducts != null)
                {
                    shoppingCartProducts.Quantity = product.Quantity;
                }
                else
                {
                    shoppingCartProducts = new ShoppingCartProducts
                    {
                        ProductId = product.ProductId,
                        ShoppingCart = customer.ShoppingCart,
                        Quantity = product.Quantity
                    };
                    customer.ShoppingCart.ShoppingCartProducts.Add(shoppingCartProducts);
                }
                _context.SaveChanges();
            }
            catch
            {
                //Todo
            }
        }
    }
}
