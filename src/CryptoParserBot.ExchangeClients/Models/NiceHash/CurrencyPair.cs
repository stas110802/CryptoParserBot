namespace CryptoParserBot.ExchangeClients.Models.NiceHash;

public sealed class CurrencyPair
{
    public string Currency { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal TradingVolume { get; set; }
}

