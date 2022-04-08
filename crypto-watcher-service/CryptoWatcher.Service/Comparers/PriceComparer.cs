using CryptoWatcher.Domain;

namespace CryptoWatcher.Service.Comparers
{
    public class PriceComparer : IPriceComparer
    {
        public async Task<decimal> GetDifference(decimal askPrice, decimal bidPrice)
        {
            return await Task.FromResult(Math.Abs(bidPrice - askPrice));
        }

        public async Task<bool> IsViableTrade(decimal askPrice, decimal bidPrice, decimal margin)
        {
            bool viable = false;

            if (bidPrice > askPrice)
            {
                decimal difference = await this.GetDifference(askPrice, bidPrice);
                viable = difference > (bidPrice * margin); 
            }

            return viable;
        }
    }
}
