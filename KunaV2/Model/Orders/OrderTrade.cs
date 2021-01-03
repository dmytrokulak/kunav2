using System;

namespace KunaV2.Model.Orders
{
    public class OrderTrade
    {
        public ulong Id { get; set; }
        public OrderKind Kind => Side == "bid" || Side == "buy" ? OrderKind.Bid : OrderKind.Ask;
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public decimal Funds { get; set; }
        public string Market { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Side { get; set; }
    }
}
