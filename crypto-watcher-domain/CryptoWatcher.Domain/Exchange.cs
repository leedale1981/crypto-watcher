namespace CryptoWatcher.Domain;
public class Exchange : IExchange
{
    public string ApiKey { get; set; }
    public string Secret { get; set; }
    public string Name { get; set; }
}
