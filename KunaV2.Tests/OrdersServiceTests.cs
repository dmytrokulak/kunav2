using System.Threading.Tasks;
using KunaV2.Model.Orders;
using KunaV2.Services;
using KunaV2.Services.Impl;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace KunaV2.Tests
{
    [TestFixture]
    public class OrdersServiceTests
    {
        private readonly IOrderService _orderService = new OrderService(new Logger<ServiceBase>(new LoggerFactory()),
            new RestClientDecorator(new Logger<RestClientDecorator>(new LoggerFactory())));


        [Test]
        [Order(0)]
        public async Task CanGetOrdersHistory()
        {
            var orders = await _orderService.GetOrdersHistoryAsync("dreamuah");
            Assert.That(orders, Has.Count.GreaterThan(1));
        }


        [Test]
        [Order(1)]
        public async Task CanCreateOrder()
        {
            var order = await _orderService.CreateOrderAsync("dreamuah", OrderType.LIMIT, OrderKind.Bid, 0.001M, 0.1M);
            Assert.That(order.Id, Is.Not.Null);
        }


        [Test]
        [Order(2)]
        public async Task CanGetActiveOrders()
        {
            var orders = await _orderService.GetActiveOrdersAsync("dreamuah");
            Assert.That(orders, Has.Count.EqualTo(1), $"Actual {orders.Count}.");
        }


        [Test]
        [Order(3)]
        public async Task CanCancelOrder()
        {
            var orders = await _orderService.GetActiveOrdersAsync("dreamuah");
            await _orderService.CancelOrderAsync(orders[0].Id);
            Assert.Pass();
        }
    }
}
