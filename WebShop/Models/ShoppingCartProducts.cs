namespace WebShop.Models
{
    public partial class ShoppingCartProducts
    {
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

        public Product Product { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
