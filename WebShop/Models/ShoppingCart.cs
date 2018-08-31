using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class ShoppingCart
    {
        public ShoppingCart()
        {
            Customer = new HashSet<Customer>();
            ShoppingCartProducts = new HashSet<ShoppingCartProducts>();
        }

        public int ShoppingCartId { get; set; }

        public ICollection<Customer> Customer { get; set; }
        public ICollection<ShoppingCartProducts> ShoppingCartProducts { get; set; }
    }
}
