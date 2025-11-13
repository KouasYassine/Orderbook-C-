namespace TradingEngineServer.OrderBook
{
    public interface IMatchingOrderBook : IRetrievalbook
    {
        MatchResult Match();   
    }
}
