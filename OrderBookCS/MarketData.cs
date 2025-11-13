using System;
using System.Text.Json;

namespace TradingEngineServer.OrderBook
{
    // Wire format for WebSocket clients
    public abstract record MdEvent(string Type);

    public sealed record MdTrade(long TakerOrderId, long MakerOrderId, decimal Price, uint Qty, DateTime Ts)
        : MdEvent("trade");

    public sealed record MdTopOfBook(decimal? BestBid, decimal? BestAsk, decimal? Mid)
        : MdEvent("top");

    public static class MdJson
    {
        private static readonly JsonSerializerOptions _opts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static string Serialize(object evt) => JsonSerializer.Serialize(evt, _opts);
    }
}
