using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot.Models.Logs;

public sealed class CurrencyLog
{
    public CurrencyLog() { }

    public CurrencyLog(CurrencyInfo options, decimal totalPrice)
    {
        ParsingDate = DateTime.UtcNow;

        FirstCoin = options.FirstCoin;
        SecondCoin = options.SecondCoin;
        LimitPrice = options.PriceLimit;

        TotalPrice = totalPrice;
    }

    //[Required]
    public string FirstCoin { get; set; }
    public string SecondCoin { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal LimitPrice { get; set; }
    public DateTime ParsingDate { get; set; }

    public override string ToString()
    {
        return $"Продаем: {FirstCoin} за {SecondCoin}\nДата: {ParsingDate}\nЛимит цена: {LimitPrice} {SecondCoin}\n" +
               $"Текущая цена: {TotalPrice} {SecondCoin}";
    }
}