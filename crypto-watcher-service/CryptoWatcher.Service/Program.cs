// See https://aka.ms/new-console-template for more information
using CryptoWatcher.Domain;
using CryptoWatcher.Service.Clients;
using CryptoWatcher.Service.Comparers;
using CryptoWatcher.Service.Watchers;

Exchange binanceExchange = new()
{
    ApiKey = "",
    Secret = "",
    Name = "Binance",
};

BinanceClient binanceClient = new(binanceExchange);

Exchange coinbaseExchange = new()
{
    ApiKey = "",
    Secret = "",
    Name = "Coinbase",
};

CoinbaseClient coinbaseClient = new(coinbaseExchange);

ExchangeWatcher watcher = new(
    binanceClient,
    coinbaseClient,
    new PriceComparer());

Console.WriteLine($"Checking for viable trades between {binanceExchange.Name} and {coinbaseExchange.Name} ...");
await watcher.Start();

