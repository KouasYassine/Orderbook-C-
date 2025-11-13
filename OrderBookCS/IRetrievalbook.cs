using System.Collections.Generic;
using TradingEngineServer.Orders;

namespace TradingEngineServer.OrderBook
{
   
    public interface IRetrievalbook : IOrderEntryOrderBook
    {
        List<OrderBookEntry> GetAskOrders();
        List<OrderBookEntry> GetBidOrders();
    }
}
