using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOnWheels.Models
{
    public class Order : Base
    {
        public DateTime OrderDate { get; set; }

        public DateTime OrderDeliver { get; set; }

        public int? Frequency { get; set; }

        public double? Total { get; set; }

        public bool? IsActive { get; set; }
        public string UserId { get; set; }

        public List<Product> Products { get; set; }
    }
}
