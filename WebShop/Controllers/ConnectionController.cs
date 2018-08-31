using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using Microsoft.AspNetCore.Http;

namespace WebShop.Controllers
{
    public class ConnectionController : Controller
    {
        public ConnectionController()
        {
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