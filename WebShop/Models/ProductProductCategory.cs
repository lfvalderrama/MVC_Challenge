﻿namespace WebShop.Models
{
    public partial class ProductProductCategory
    {
        public int ProductCategoryId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
