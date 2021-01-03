using System;

namespace KunaV2.Model.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string Side { get; set; }
        public string OrdType { get; set; }
        public OrderKind Kind => Side == "bid" || Side == "buy" ? OrderKind.Bid : OrderKind.Ask;
        public OrderType Type => OrdType == "limit" ? OrderType.LIMIT : OrderType.MARKET;
        public decimal Price { get; set; }
        
        /// <summary>
        /// Average trade price for the order, for new order — 0.
        /// </summary>
        public decimal AvgPrice { get; set; }

        /// ToDo:: Always 'wait' ?
        public string State { get; set; }
        public string Market { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public decimal Volume { get; set; }
        public decimal RemainingVolume { get; set; }

        /// <summary>
        /// Executed amount, for new order — 0.
        /// </summary>
        public decimal ExecutedVolume { get; set; }
        public int TradesCount { get; set; }
    }
}
