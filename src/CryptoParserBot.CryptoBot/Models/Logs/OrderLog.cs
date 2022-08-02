using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot.Models.Logs;

public sealed class OrderLog
{
    public OrderLog() { }

    public OrderLog(CurrencyInfo options, decimal sellPrice, decimal amount)
    {
        OrderDate = DateTime.UtcNow;

        FirstCoin = options.FirstCoin;
        SecondCoin = options.SecondCoin;
        LimitPrice = options.PriceLimit;

        SellPrice = sellPrice;
        Amount = amount;
    }

    //[Required]
    public string FirstCoin { get; set; }
    public string SecondCoin { get; set; }
    public decimal SellPrice { get; set; }
    public decimal LimitPrice { get; set; }
    public decimal Amount { get; set; }
    public DateTime OrderDate { get; set; }

    public override string ToString()
    {
        return $"Продаем: {FirstCoin} за {SecondCoin}\nДата: {OrderDate}\nЛимит цена: {LimitPrice} {SecondCoin}\n" +
               $"Цена продажи: {SellPrice} {SecondCoin}\nКоличество: {Amount} {FirstCoin}";
    }
}