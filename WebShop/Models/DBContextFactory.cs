using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class DBContextFactory
    {
        public static WebShopContext CreateContext(string connectionString = null)
        {
            var options = new DbContextOptionsBuilder();
            if (connectionString == null)
            {
                options.UseInMemoryDatabase("inMemory");

                return new WebShopContext(options.Options, "InMemory");
            }
            else
            {
                options.UseSqlServer(connectionString);
                return new WebShopContext(options.Options, "SQL");
            }
        }
    }
}
