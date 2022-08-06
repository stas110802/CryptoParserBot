using Newtonsoft.Json;

namespace CryptoParserBot.ExchangeClients.Models.NiceHash;

public sealed class CurrencyBalance
{
    [JsonProperty("active")]
    public bool IsActive { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("totalBalance")]
    public decimal TotalBalance { get; set; }

    [JsonProperty("available")]
    public decimal AvailableBalance { get; set; }

    [JsonProperty("debt")]
    public decimal Debt { get; set; }

    [JsonProperty("pending")]
    public decimal Pending { get; set; }
}

