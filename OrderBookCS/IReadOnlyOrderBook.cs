using TradingEngineServer.Orders;

namespace TradingEngineServer.OrderBook
{
    public interface IReadOnlyOrderBook
    {
        bool ContainsOrder(long orderId);      
        OrderBookSpread GetSpread();
        int Count { get; }
    }
}
