using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetQuartzSample.Models
{
    public class Sale
    {
        public string ProductName { get; set; }
        public DateTime SaleTime { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
