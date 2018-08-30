using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Filters;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private WebShopContext _context;
        private readonly IIndex<ConnectionTypes, WebShopContext> _contexts;
        private readonly ConnectionTypes defaultConnection = ConnectionTypes.SqlServer;
        private readonly int _customer_id = 1;

        public ShoppingCartController(IIndex<ConnectionTypes, WebShopContext> contexts)
        {
            _contexts = contexts;
            _context = _contexts[defaultConnection];
        }

        [SetContextFilter]
        public IActionResult AddToCart([Bind("ProductId,Name,Description,Price,Quantity")] Product product)
        {
            var customer = _context.Customer.Include(c=>c.ShoppingCart).Where(c=>c.CustomerId==_customer_id).FirstOrDefault();
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
            return RedirectToAction("Details", "Products", new { id = product.ProductId });
        }
    }
}