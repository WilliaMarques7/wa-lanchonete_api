using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItensOrderMercadoPago
    {
        public int? IdItem { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? QuantityItem { get; set; }
        public decimal? TotalAmountItem { get; set; }
        public string? TitleItem { get; set; }
    }
}
