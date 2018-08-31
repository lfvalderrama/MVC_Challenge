using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
            ProductProductCategory = new HashSet<ProductProductCategory>();
            ShoppingCartProducts = new HashSet<ShoppingCartProducts>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }

        public ICollection<OrderDetail> OrderDetail { get; set; }
        public ICollection<ProductProductCategory> ProductProductCategory { get; set; }
        public ICollection<ShoppingCartProducts> ShoppingCartProducts { get; set; }
    }
}
