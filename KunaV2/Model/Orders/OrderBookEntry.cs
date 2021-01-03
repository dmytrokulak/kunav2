namespace KunaV2.Model.Orders
{
    public class OrderBookEntry
    {
        /// <summary>
        /// Announced of the trade.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Amount of currency to trade.
        /// </summary>
        public decimal Amount { get; set; }
     
        /// <summary>
        /// Bid or ask.
        /// </summary>
        public OrderKind Kind { get; set; }
    }
}
