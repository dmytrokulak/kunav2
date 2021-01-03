using KunaV2.Model.Orders;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KunaV2.Services.Impl
{
    public class OrderService : ServiceBase, IOrderService
    {
        private readonly ILogger<ServiceBase> _logger;

        public OrderService(ILogger<ServiceBase> logger, IRestClient restClient) : base(logger, restClient)
        {
            _logger = logger;
        }

        public async Task<IList<Order>> GetActiveOrdersAsync(string marketId)
        {
            return await ExecuteRequestWithRetry<IList<Order>>(Method.GET, Urls.OrdersActive, marketId);
        }

        public async Task<IList<OrderTrade>> GetOrdersHistoryAsync(string marketId)
        {
            return await ExecuteRequestWithRetry<IList<OrderTrade>>(Method.GET, Urls.OrdersHistory, marketId);
        }

        public async Task<Order> CreateOrderAsync(string marketId, OrderType type, OrderKind kind, decimal amount, decimal? price = null)
        {
            if (type == OrderType.LIMIT && !(price >= 0))
                throw new InvalidOperationException("Limit orders must have price specified");

            var submission = new OrderSubmission
            {
                side = kind == OrderKind.Bid ? "buy" : "sell",
                volume = amount,
                market = marketId,
                price = type == OrderType.MARKET ? null : price,
                ord_type = type.ToString().ToLower()
            };

            return await ExecuteRequestWithRetry<Order>(Method.POST, Urls.OrderSubmit, null, submission);
        }
        
        public async Task CancelOrderAsync(int orderId)
        {
            await ExecuteRequestWithRetry<object>(Method.POST, Urls.OrderCancel, null, new {id = orderId});
        }
    }

    internal class OrderSubmission
    {
        public string side { get; set; }
        public decimal volume { get; set; }
        public string market { get; set; }
        public decimal? price { get; set; }
        public string ord_type { get; set; }
    }
}
