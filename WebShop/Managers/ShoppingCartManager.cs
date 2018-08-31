using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebShop.Models;

namespace WebShop.Managers
{
    public class ShoppingCartManager
    {
        private readonly IContextHelper _contextHelper;
        private WebShopContext _context;

        public ShoppingCartManager(IContextHelper contextHelper)
        {
            _contextHelper = contextHelper;
            _context = _contextHelper.SetContext();

        }

        public List<Product> GetProductsFromCart(int customer_id)
        {
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
