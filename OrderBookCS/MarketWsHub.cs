using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TradingEngineServer.OrderBook
{
    public interface IMarketWsHub
    {
        ChannelReader<string> Subscribe();
        void Publish(string json);
    }

    // Lock-free fanout using Channel
    public sealed class MarketWsHub : IMarketWsHub
    {
        // Keep a set of channels (per-subscriber)
        private readonly HashSet<Channel<string>> _subscribers = new();
        private readonly object _lock = new();

        public ChannelReader<string> Subscribe()
        {
            var ch = Channel.CreateUnbounded<string>();
            lock (_lock) _subscribers.Add(ch);
            // On GC/complete, remove
            _ = ch.Reader.Completion.ContinueWith(_ => { lock (_lock) _subscribers.Remove(ch); });
            return ch.Reader;
        }

        public void Publish(string json)
        {
            Channel<string>[] snapshot;
            lock (_lock) snapshot = _subscribers.ToArray();
            foreach (var sub in snapshot) sub.Writer.TryWrite(json);
        }
    }

    public static class MarketWsEndpoint
    {
        public static async Task Handle(WebSocket socket, IMarketWsHub hub, CancellationToken ct)
        {
            // subscribe
            var reader = hub.Subscribe();

            // ignore inbound messages, just stream outbound
            var send = Task.Run(async () =>
            {
                await foreach (var msg in reader.ReadAllAsync(ct))
                {
                    var bytes = Encoding.UTF8.GetBytes(msg);
                    await socket.SendAsync(bytes, WebSocketMessageType.Text, true, ct);
                }
            }, ct);

            // read loop (discard input, keep alive)
            var buffer = new byte[1 << 12];
            while (!ct.IsCancellationRequested && socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, ct);
                if (result.MessageType == WebSocketMessageType.Close) break;
            }
        }
    }
}
