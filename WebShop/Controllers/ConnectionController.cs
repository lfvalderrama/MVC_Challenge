using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using Microsoft.AspNetCore.Http;

namespace WebShop.Controllers
{
    public class ConnectionController : Controller
    {
        private readonly IIndex<ConnectionTypes, WebShopContext> _contexts;
        private readonly ConnectionTypes defaultConnection = ConnectionTypes.SqlServer;

        public ConnectionController(IIndex<ConnectionTypes, WebShopContext> contexts)
        {
            _contexts = contexts;
        }
        public IActionResult SwitchConnection()
        {
            var test = HttpContext.Session.GetString("connection");
            HttpContext.Session.SetString("connection", "InMemory");
            return RedirectToAction("Index","Home");
        }
    }
}