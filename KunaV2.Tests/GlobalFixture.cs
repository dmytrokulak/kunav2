using System.Threading.Tasks;
using KunaV2.Services;
using KunaV2.Services.Impl;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace KunaV2.Tests
{
    [SetUpFixture]
    public class GlobalFixture
    {
        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            IOrderService orderService = new OrderService(new Logger<ServiceBase>(new LoggerFactory()),
                new RestClientDecorator(new Logger<RestClientDecorator>(new LoggerFactory())));

            var orders = await orderService.GetActiveOrdersAsync("dreamuah");
            foreach (var order in orders)
                await orderService.CancelOrderAsync(order.Id);
        }
    }
}
