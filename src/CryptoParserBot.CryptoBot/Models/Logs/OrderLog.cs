using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot.Models.Logs;

public sealed class OrderLog
{
    public OrderLog() { }

    public OrderLog(CurrencyInfo options, decimal sellPrice, decimal amount)
    {
        OrderDate = DateTime.UtcNow;
        Info = options;
        SellPrice = sellPrice;
        Amount = amount;
    }

    public decimal SellPrice { get; set; }
    public decimal Amount { get; set; }
    public CurrencyInfo Info { get; set; }
    public DateTime OrderDate { get; set; }

    public override string ToString()
    {
        return $"Продаем: {Info.FirstCoin} за {Info.SecondCoin}\n" +
               $"Дата: {OrderDate}\n" +
               $"Верхняя цена: {Info.UpperPrice} {Info.SecondCoin}\n" +
               $"Нижняя цена: {Info.BottomPrice} {Info.SecondCoin}\n" +
               $"Цена продажи: {SellPrice} {Info.SecondCoin}\n" +
               $"Количество: {Amount} {Info.FirstCoin}";
    }
}