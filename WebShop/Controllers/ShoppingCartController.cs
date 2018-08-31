using Microsoft.AspNetCore.Mvc;
using WebShop.Filters;
using WebShop.Models;
using WebShop.Managers;

namespace WebShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartManager _shoppingCartManager;
        private readonly int _customer_id = 1;

        public ShoppingCartController(ShoppingCartManager shoppingCartManager)
        {
            _shoppingCartManager = shoppingCartManager;
        }

        [SetContextFilter]
        // GET: Products/Details/5
        public IActionResult Details(ConnectionTypes type)
        {
            var products = _shoppingCartManager.GetProductsFromCart(_customer_id);
            return View(products);
        }

        [SetContextFilter]
        public IActionResult DeleteFromCart(int productId ,ConnectionTypes type)
        {
            _shoppingCartManager.DeleteFromCart(_customer_id, productId);
            return RedirectToAction("Details");
        }

        [SetContextFilter]
        public IActionResult AddToCart([Bind("ProductId,Name,Description,Price,Quantity")] Product product)
        {
            _shoppingCartManager.AddToCart(_customer_id, product);
            return RedirectToAction("Details", "Products", new { id = product.ProductId });
        }
    }
}