using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot.Models.Logs;

public sealed class CurrencyLog
{
    public CurrencyLog() { }

    public CurrencyLog(CurrencyInfo options, decimal totalPrice)
    {
        ParsingDate = DateTime.UtcNow;
        Info = options;
        TotalPrice = totalPrice;
    }
    
    public CurrencyInfo Info { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime ParsingDate { get; set; }

    public override string ToString()
    {
        return $"Продаем: {Info.FirstCoin} за {Info.SecondCoin}\n" +
               $"Дата: {ParsingDate}\n" +
               $"Верхняя цена: {Info.UpperPrice} {Info.SecondCoin}\n" +
               $"Нижняя цена: {Info.BottomPrice} {Info.SecondCoin}\n" +
               $"Текущая цена: {TotalPrice} {Info.SecondCoin}";
    }
}