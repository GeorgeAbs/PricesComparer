using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricesComparer
{
    internal class ItemFromDb
    {
        public string Id { get; set; }

        public string Price { get; set; }

        public string DiscountPrice { get; set; }
    }
}
