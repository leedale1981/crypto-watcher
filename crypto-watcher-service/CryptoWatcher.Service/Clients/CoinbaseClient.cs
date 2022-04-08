using Coinbase.Models;
using CryptoWatcher.Domain;

namespace CryptoWatcher.Service.Clients
{
    public class CoinbaseClient : IExchangeClient
    {
        private readonly IExchange _exchange;

        public CoinbaseClient(IExchange exchange)
        {
            this._exchange = exchange;
        }

        public async Task<bool> Buy(string symbol, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<BookTickerResult> GetPrice(string symbol)
        {
            Coinbase.ApiKeyConfig config = new()
            {
                ApiKey = this._exchange.ApiKey,
                ApiSecret = this._exchange.Secret,
            };

            var client = new Coinbase.CoinbaseClient(config);
            Response<Money> askResponse = await client.Data.GetBuyPriceAsync(symbol);
            decimal askPrice = askResponse.Data.Amount;

            Response<Money> bidResponse = await client.Data.GetSellPriceAsync(symbol);
            decimal bidPrice = bidResponse.Data.Amount;

            BookTickerResult result = new()
            {
                a = askPrice.ToString(),
                b = bidPrice.ToString(),
            };

            return result;
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
