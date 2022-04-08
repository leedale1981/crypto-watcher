namespace CryptoWatcher.Service.Clients
{
    public interface IExchangeClient
    {
        string GetExchangeName();
        Task<BookTickerResult> GetPrice(string symbol);
        Task<bool> Buy(string currencysymbolCode, decimal amount);
        Task<bool> Sell(string symbol, decimal amount);   
        Task<bool> Transfer(string symbol, string amount);
    }
}
