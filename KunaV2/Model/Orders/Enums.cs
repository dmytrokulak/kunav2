namespace KunaV2.Model.Orders
{
    /// <summary>
    /// Bid (buy) or ask (sell).
    /// </summary>
    public enum OrderKind
    {
        /// <summary>
        /// Order to buy.
        /// </summary>
        Bid,
        /// <summary>
        /// Order to sell.
        /// </summary>
        Ask
    }

    /// <summary>
    /// Limit or market.
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Executed at specified price.
        /// </summary>
        LIMIT,
        /// <summary>
        /// Executed at best market price.
        /// </summary>
        MARKET
    }

    public enum OrderSort
    {
        Asc = 1,
        Desc = -1
    }

    /// <summary>
    /// TBD
    /// </summary>
    public enum OrderStatus
    {
        ACTIVE,
        EXECUTED,
        CANCELED
    }

    public enum TradeFeeType
    {
        Maker = 1,
        Taker = -1
    }
}