using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetQuartzSample.Models
{
    public class SaleTransferDto
    {
        public List<Sale> Sales{ get; set; }
        public double TotalAmount { get; set; }
    }
}
