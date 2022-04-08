namespace CryptoWatcher.Domain
{
    public interface IExchange
    {
        string ApiKey { get; set; }
        string Secret { get; set; }
        string Name { get; set; }
    }
}
