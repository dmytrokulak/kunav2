using KunaV2.Model.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KunaV2.Services
{
    public interface IOrderService
    {
        Task<IList<Order>> GetActiveOrdersAsync(string marketId);

        Task<IList<OrderTrade>> GetOrdersHistoryAsync(string marketId);

        Task<Order> CreateOrderAsync(string marketId, OrderType type, OrderKind kind, decimal amount, decimal? price = null);

        Task CancelOrderAsync(int orderId);
    }
}