using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOnWheels.Models
{
    public class Product : Base
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Image { get; set; }
        public bool IsCountable { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
    }
}
