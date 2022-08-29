using NetQuartzSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetQuartzSample
{
    public class SaleManager
    {
        private static SaleTransferDto saleTransfer = new SaleTransferDto();
        private static List<Sale> MySales = new List<Sale>();

        private static double TotalAmount;
        public static SaleTransferDto GetSales
        { 
            get 
            { 
                saleTransfer.Sales = MySales;
                saleTransfer.TotalAmount = GetTotalAmount;
                return saleTransfer; 
            } 
        }
        public static double GetTotalAmount { get { return TotalAmount; } }
        public static void AddSale(Sale sale)
        {
            MySales.Add(sale);
            TotalAmount += (sale.Quantity * sale.Price);
        }
    }
}
