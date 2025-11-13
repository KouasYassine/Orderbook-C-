using System;

namespace TradingEngineServer.Orders
{
    public class IOrderCore
    {
        public long OrderId { get;}
        public string Username { get; }
        public int SecurityId { get; }
    }
}
