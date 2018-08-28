﻿using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
