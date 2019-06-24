using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOnWheels.Models
{
    public class Box : Base
    {
        public int Weight { get; set; }
        public Guid ProductListId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
