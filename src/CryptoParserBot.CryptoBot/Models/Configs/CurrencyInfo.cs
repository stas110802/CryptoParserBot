namespace CryptoParserBot.CryptoBot.Models.Configs;

public sealed class CurrencyInfo
{
    public string FirstCoin { get; set; }
    public string SecondCoin { get; set; }
    public decimal PriceLimit { get; set; }
    public decimal BalanceLimit { get; set; }// мб, потом убрать
}