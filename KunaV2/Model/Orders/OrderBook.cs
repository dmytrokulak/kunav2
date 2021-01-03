using System;
using System.Collections.Generic;
using System.Text;

namespace KunaV2.Model.Orders
{
   public class OrderBook
    {
        public string TimeStamp { get; set; }
        public IList<IList<decimal>> Asks { get; set; }
        public IList<IList<decimal>> Bids { get; set; }
    }
}
