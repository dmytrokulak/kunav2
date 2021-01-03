using System;
using KunaV2.Model.Orders;
using System.Collections.Generic;
using System.Linq;

namespace KunaV2
{
    public static class Extensions
    {
        public static decimal AveragePrice(this IEnumerable<OrderTrade> trades)
            => trades.WeightedAverage(o => o.Price, o => o.Volume);

        public static decimal AveragePrice(this IEnumerable<OrderBookEntry> entries)
            => entries.WeightedAverage(o => o.Price, o => o.Amount);

        public static decimal WeightedAverage<T>(this IEnumerable<T> records, Func<T, decimal> value, Func<T, decimal> weight)
        {
            if (records == null || !records.Any())
                return 0;

            var weightedValueSum = records.Sum(x => value(x) * weight(x));
            var weightSum = records.Sum(x => weight(x));

            if (weightSum != 0)
                return weightedValueSum / weightSum;
            else
                throw new DivideByZeroException();
        }

        public static string Truncate(this string input, int number)
        {
            if (input.Length > number)
                return input.Substring(0, number + 1) + "...";
            return input;
        }
    }
}
