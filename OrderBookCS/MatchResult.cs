using System.Collections.Generic;
using TradingEngineServer.Orders;

namespace TradingEngineServer.OrderBook
{
    public sealed class MatchResult
    {
        public bool Matched { get; init; }
        public List<OrderRecord> Trades { get; init; } = new(); 
        public uint ExecutedQuantity { get; init; }
        public uint RemainingQuantity { get; init; }
        public long? LastTradePrice { get; init; }
    }
}
