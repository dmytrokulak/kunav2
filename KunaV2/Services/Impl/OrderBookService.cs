using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using KunaV2.Model.Orders;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace KunaV2.Services.Impl
{
    public class OrderBookService : ServiceBase, IOrderBookService
    {
        private const string CacheKey = "orderBook";

        public async Task<IList<OrderBookEntry>> GetOrderBookAsync(string market)
        {
            var cached = MemoryCache.Default.Get(CacheKey + market);
            if (cached != null)
                return (IList<OrderBookEntry>)cached;

            var response = await ExecuteRequestWithRetry<OrderBook>(Method.GET, Urls.OrderBook, market);
            var asks = response.Asks.Select(r =>
                new OrderBookEntry
                {
                    Price = r[0],
                    Amount = r[1],
                    Kind = OrderKind.Ask
                });
            var bids = response.Bids.Select(r =>
                new OrderBookEntry
                {
                    Price = r[0],
                    Amount = r[1],
                    Kind = OrderKind.Bid
                });
            var orderBookEntries = asks.Concat(bids).ToList();
            MemoryCache.Default.Add(CacheKey + market, orderBookEntries, DateTimeOffset.Now.AddSeconds(100));
            return orderBookEntries;
        }


        public async Task<IList<OrderBookEntry>> GetAskEntriesAsync(string market)
        {
            return (await GetOrderBookAsync(market))
                .Where(o => o.Kind == OrderKind.Ask)
                .OrderBy(o => o.Price)
                .ToList();
        }

        public async Task<IList<OrderBookEntry>> GetAskEntriesAsync(string market, decimal volume)
        {
            var cumulative = 0.0M;

            var orderBookEntries = (await GetOrderBookAsync(market))
                .Where(o => o.Kind == OrderKind.Ask)
                .OrderBy(o => o.Price)
                .TakeWhile(o =>
                {
                    var limitNotReached = cumulative < volume;
                    if (limitNotReached)
                        cumulative += o.Amount * o.Price;
                    return limitNotReached;
                })
                .ToList();


            var last = orderBookEntries.Last();
            cumulative -= last.Amount * last.Price;
            var diff = volume - cumulative;
            var newAmount = diff / last.Price;
            if (newAmount < last.Amount)
            {
                orderBookEntries.Remove(last);
                orderBookEntries.Add(new OrderBookEntry
                {
                    Price = last.Price,
                    Amount = newAmount,
                    Kind = last.Kind
                });
            }

            return orderBookEntries;
        }

        public async Task<IList<OrderBookEntry>> GetBidEntriesAsync(string market)
        {
            return (await GetOrderBookAsync(market))
                .Where(o => o.Kind == OrderKind.Bid)
                .OrderByDescending(o => o.Price)
                .ToList();
        }

        public async Task<IList<OrderBookEntry>> GetBidEntriesAsync(string market, decimal volume)
        {
            var cumulative = 0.0M;
            var orderBook = (await GetOrderBookAsync(market));

            var orderBookEntries = orderBook
                .Where(o => o.Kind == OrderKind.Bid)
                .OrderByDescending(o => o.Price)
                .TakeWhile(o =>
                {
                    var limitNotReached = cumulative < volume;
                    if (limitNotReached)
                        cumulative += o.Amount;
                    return limitNotReached;
                })
                .ToList();

            var last = orderBookEntries.Last();
            cumulative -= last.Amount;

            orderBookEntries.Remove(last);
            orderBookEntries.Add(new OrderBookEntry
            {
                Price = last.Price,
                Amount = volume - cumulative,
                Kind = last.Kind
            });

            return orderBookEntries;
        }

        public OrderBookService(ILogger<ServiceBase> logger, IRestClient client) : base(logger, client)
        {
        }
    }
}
