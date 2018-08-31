using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Filters;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class CustomersController : Controller
    {
        private  WebShopContext _context;
        private readonly IIndex<ConnectionTypes, WebShopContext> _contexts;
        private readonly ConnectionTypes defaultConnection = ConnectionTypes.SqlServer;

        public CustomersController(IIndex<ConnectionTypes, WebShopContext> contexts)
        {
            _contexts = contexts;
            _context = _contexts[defaultConnection];
        }

        [SetContextFilter]
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.ShoppingCart)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [SetContextFilter]
        public IActionResult Create(ConnectionTypes type)
        {
            _context = _contexts[type];
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCart, "ShoppingCartId", "ShoppingCartId");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SetContextFilter]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email,Age,ShoppingCartId")] Customer customer, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = customer.CustomerId });
            }
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCart, "ShoppingCartId", "ShoppingCartId", customer.ShoppingCartId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        [SetContextFilter]
        public async Task<IActionResult> Edit(int? id, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCart, "ShoppingCartId", "ShoppingCartId", customer.ShoppingCartId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SetContextFilter]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Email,Age,ShoppingCartId")] Customer customer, ConnectionTypes type)
        {
            _context = _contexts[type];
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = customer.CustomerId});
            }
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCart, "ShoppingCartId", "ShoppingCartId", customer.ShoppingCartId);
            return View(customer);
        }                

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
