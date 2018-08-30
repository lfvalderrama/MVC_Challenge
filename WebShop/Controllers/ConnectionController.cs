﻿using System;
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
            var current = HttpContext.Session.GetString("connection");
            if (current == null)
            {
                current = ConnectionTypes.SqlServer.ToString();
                HttpContext.Session.SetString("connection", current);
            }
            ViewData["current"] = current;
            ViewData["success"] = false;
            return View();
        }

        [HttpPost]
        public IActionResult SwitchConnection(ConnectionTypes connection)
        {
            HttpContext.Session.SetString("connection", connection.ToString());
            ViewData["current"] = connection.ToString();
            ViewData["success"] = true;
            return View();
        }
    }
}