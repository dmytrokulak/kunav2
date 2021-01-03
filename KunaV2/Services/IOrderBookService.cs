using KunaV2.Model.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KunaV2.Services
{
    public interface IOrderBookService
    {
        Task<IList<OrderBookEntry>> GetOrderBookAsync(string market);

        /// <summary>
        /// Returns ask (sell-side) entries from order book
        /// sorted by price ascending.
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        Task<IList<OrderBookEntry>> GetAskEntriesAsync(string market);

        /// <summary>
        /// Returns ask (sell-side) entries from order book
        /// sorted by price ascending and restricted to
        /// cheapest entries covering the volume specified.
        /// </summary>
        /// <param name="market"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        Task<IList<OrderBookEntry>> GetAskEntriesAsync(string market, decimal volume);

        /// <summary>
        /// Returns bid (buy-side) entries from order book
        /// sorted by price descending.
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        Task<IList<OrderBookEntry>> GetBidEntriesAsync(string market);

        /// <summary>
        /// Returns ask (buy-side) entries from order book
        /// sorted by price descending and restricted to
        /// most expensive entries covering the volume specified.
        /// </summary>
        /// <param name="market"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        Task<IList<OrderBookEntry>> GetBidEntriesAsync(string market, decimal volume);
    }
}