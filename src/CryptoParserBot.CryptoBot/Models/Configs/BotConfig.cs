using Newtonsoft.Json;

namespace CryptoParserBot.CryptoBot.Models.Configs;

public sealed class BotConfig
{
    [JsonProperty("ClientKeys")]
    public BotKeys Keys { get; set; }

    [JsonProperty("BotEmail")]
    public SmtpHost Email { get; set; }

    [JsonProperty("CurrencyInfo")]
    public CurrencyInfo CurrencyInfo { get; set; }
}