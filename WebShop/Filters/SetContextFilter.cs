using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using WebShop.Models;

namespace WebShop.Filters
{
    public class SetContextFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var type = context.HttpContext.Session.GetString("connection");
            var connectionType = ConnectionTypes.SqlServer;
            if (type != null) connectionType = (ConnectionTypes)System.Enum.Parse(typeof(ConnectionTypes), type);
            else context.HttpContext.Session.SetString("connection", ConnectionTypes.SqlServer.ToString());
            context.ActionArguments["type"] = connectionType;
        }
    }
}
