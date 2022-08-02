using Newtonsoft.Json;

namespace CryptoParserBot.CryptoBot.Models.Configs;

public sealed class BotConfig
{
    [JsonProperty("BotKeys")]
    public BotKeys Keys { get; set; }

    [JsonProperty("BotEmail")]
    public BotEmail Email { get; set; }

    [JsonProperty("CurrencyInfo")]
    public CurrencyInfo CurrencyInfo { get; set; }
}