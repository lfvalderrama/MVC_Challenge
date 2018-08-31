using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int? ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
