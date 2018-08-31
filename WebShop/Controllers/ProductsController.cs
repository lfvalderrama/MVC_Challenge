using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using Microsoft.AspNetCore.Http;
using WebShop.Filters;

namespace WebShop.Controllers
{
    public class ProductsController : Controller
    {
        private WebShopContext _context;
        private readonly IIndex<ConnectionTypes, WebShopContext> _contexts;
        private readonly ConnectionTypes defaultConnection = ConnectionTypes.SqlServer;

        public ProductsController(IIndex<ConnectionTypes, WebShopContext> contexts)
        {
            _contexts = contexts;
            _context = _contexts[defaultConnection];
        }

        private void SetContext()
        {
            var type = HttpContext.Session.GetString("connection");
            var connectionType = (ConnectionTypes)System.Enum.Parse(typeof(ConnectionTypes), type);
            _context = _contexts[connectionType];
        }

        [SetContextFilter]
        // GET: Products
        public async Task<IActionResult> Index(ConnectionTypes type)
        {
            _context = _contexts[type];
            return View(await _context.Product.ToListAsync());
        }

        [SetContextFilter]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [SetContextFilter]
        // GET: Products/Create
        public IActionResult Create(ConnectionTypes type)
        {
            _context = _contexts[type];
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SetContextFilter]
        public IActionResult Create([Bind("ProductId,Name,Description,Price,Quantity")] Product product, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (ModelState.IsValid)
            {
                _context.Product.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product); 
        }

        [SetContextFilter]
        // GET: Products/Edit/5
        public IActionResult Edit(int? id, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (id == null)
            {
                return NotFound();
            }

            var product =  _context.Product.FindAsync(id);
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
        [SetContextFilter]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,Name,Description,Price,Quantity")] Product product, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Product.Update(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [SetContextFilter]
        public async Task<IActionResult> Delete(int? id, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SetContextFilter]
        public async Task<IActionResult> DeleteConfirmed(int id, ConnectionTypes type)
        {
            _context = _contexts[type];
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
