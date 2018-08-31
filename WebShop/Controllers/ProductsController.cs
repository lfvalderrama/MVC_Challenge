using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using WebShop.Managers;

namespace WebShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductManager _productManager;

        public ProductsController(ProductManager productManager)
        {
            _productManager = productManager;
        }

        // GET: Products
        public IActionResult Index()
        {
            var products = _productManager.GetAllProducts();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var product = _productManager.GetProduct((int)id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create(ConnectionTypes type)
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductId,Name,Description,Price,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productManager.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product); 
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            var product = _productManager.GetProduct((int) id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,Name,Description,Price,Quantity")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _productManager.UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            var product = _productManager.GetProduct((int)id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productManager.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
