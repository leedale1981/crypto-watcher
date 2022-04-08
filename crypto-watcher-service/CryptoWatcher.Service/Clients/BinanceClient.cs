using Binance.Spot;
using CryptoWatcher.Domain;
using System.Text.Json;

namespace CryptoWatcher.Service.Clients
{
    public class BinanceClient : IExchangeClient
    {
        private readonly IExchange _exchange;

        public BinanceClient(IExchange exchange)
        {
            this._exchange = exchange;
        }

        public async Task<bool> Buy(string symbol, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<BookTickerResult> GetPrice(string symbol)
        {
            var websocket = new MarketDataWebSocket($"{symbol}@bookTicker");

            var onlyOneMessage = new TaskCompletionSource<string>();

            websocket.OnMessageReceived(
                async (data) =>
                {
                    onlyOneMessage.SetResult(data);
                }, CancellationToken.None);

            await websocket.ConnectAsync(CancellationToken.None);

            BookTickerResult? result = this.ParseData(await onlyOneMessage.Task);

            return result;
        }

        private BookTickerResult? ParseData(string data)
        {
            int startIndex = data.IndexOf('{');
            int endIndex = data.LastIndexOf('}');
            int length = endIndex - startIndex;
            string jsonData = data.Substring(startIndex, length + 1);
            return JsonSerializer.Deserialize<BookTickerResult>(jsonData);
        }

        public async Task<bool> Sell(string symbol, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Transfer(string symbol, string amount)
        {
            throw new NotImplementedException();
        }

        public string GetExchangeName()
        {
            return this._exchange.Name;
        }
    }
}
