using CryptoWatcher.Domain;
using CryptoWatcher.Service.Clients;

namespace CryptoWatcher.Service.Comparers
{
    public interface IPriceComparer
    {
        Task<decimal> GetDifference(decimal price1, decimal price2);
        Task<bool> IsViableTrade(decimal askPrice, decimal bidPrice, decimal margin);
    }
}
