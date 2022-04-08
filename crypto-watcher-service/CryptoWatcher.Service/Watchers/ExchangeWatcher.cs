using CryptoWatcher.Domain;
using CryptoWatcher.Service.Clients;
using CryptoWatcher.Service.Comparers;

namespace CryptoWatcher.Service.Watchers
{
    public class ExchangeWatcher : IExchangeWatcher
    {
        private readonly IExchangeClient _exchangeClient1;
        private readonly IExchangeClient _exchangeClient2;
        private readonly IPriceComparer _priceComparer;

        public ExchangeWatcher(
            IExchangeClient exchangeClient1, 
            IExchangeClient exchangeClient2,
            IPriceComparer priceComparer)
        {
            this._exchangeClient1 = exchangeClient1;
            this._exchangeClient2 = exchangeClient2;
            this._priceComparer = priceComparer;
        }

        public async Task Start()
        {
            TradeType tradeType = TradeType.Balanced;
            int tryCount = 1;

            while (true)
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine($"Round {tryCount}");
                Console.Write(Environment.NewLine);

                string binanceSymbol = "ethusdt";
                string coinbaseSymbol = "ETH-USD";
                decimal capital = 100;
                Console.WriteLine("Getting price from exchanges...");
                Console.Write(Environment.NewLine);
                BookTickerResult exchange1Result = await this._exchangeClient1.GetPrice(binanceSymbol);
                BookTickerResult exchange2Result = await this._exchangeClient2.GetPrice(coinbaseSymbol);

                decimal askPrice1 = decimal.Parse(exchange1Result.a);
                decimal bidPrice1 = decimal.Parse(exchange1Result.b);
                decimal askPrice2 = decimal.Parse(exchange2Result.a);
                decimal bidPrice2 = decimal.Parse(exchange2Result.b);

                Console.WriteLine($"{this._exchangeClient1.GetExchangeName()} ask price (buy) {askPrice1}");
                Console.WriteLine($"{this._exchangeClient2.GetExchangeName()} bid price (sell) {bidPrice2}");
                Console.Write(Environment.NewLine);
                Console.WriteLine($"{this._exchangeClient2.GetExchangeName()} ask price (buy) {askPrice2}");
                Console.WriteLine($"{this._exchangeClient1.GetExchangeName()} bid price (sell) {bidPrice1}");
                Console.Write(Environment.NewLine);
                
                bool viableTrade1 = await this._priceComparer.IsViableTrade(askPrice1, bidPrice2, 0.00001M);

                if (viableTrade1 && (tradeType == TradeType.Left || tradeType == TradeType.Balanced))
                {
                    Console.WriteLine($"Found a viable trade with difference " +
                        $"{this._priceComparer.GetDifference(askPrice1, bidPrice2)}");

                    Console.WriteLine($"Buy from {this._exchangeClient1.GetExchangeName()} at {askPrice1}" +
                        $"Sell on {this._exchangeClient2.GetExchangeName()} at {bidPrice2}");

                    //await this._exchangeClient1.Buy(binanceSymbol, capital);
                    //await this._exchangeClient2.Sell(binanceSymbol, capital);
                    tradeType = TradeType.Right;
                }

                bool viableTrade2 = await this._priceComparer.IsViableTrade(askPrice2, bidPrice1, 0.00001M);

                if (viableTrade2 && (tradeType == TradeType.Right || tradeType == TradeType.Balanced))
                {
                    Console.WriteLine($"Found a viable trade with difference " +
                            $"{this._priceComparer.GetDifference(askPrice2, bidPrice1)}");

                    Console.WriteLine($"Buy from {this._exchangeClient2.GetExchangeName()} at {askPrice2}" +
                        $"Sell on {this._exchangeClient1.GetExchangeName()} at {bidPrice1}");

                    //await this._exchangeClient2.Buy(binanceSymbol, capital);
                    //await this._exchangeClient1.Sell(binanceSymbol, capital);
                    tradeType = TradeType.Left;   
                }

                if (!viableTrade1 && !viableTrade2)
                {
                    Console.WriteLine("No viable trade.");
                }
                Console.WriteLine("--------------------------------------------");
                tryCount++;
                Thread.Sleep(10000);
            }
        }
    }
}
