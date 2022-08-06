using System.Runtime.Serialization;
using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Enums;
using CryptoParserBot.CryptoBot.Interfaces;
using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot.Models.Logs;

[DataContract]
public sealed class CurrencyLog : ILog
{
    public CurrencyLog() { }

    public CurrencyLog(CurrencyInfo options, decimal totalPrice, decimal availableBalance)
    {
        ParsingDate = DateTime.UtcNow;
        Info = options;
        TotalPrice = totalPrice;
        AvailableBalance = availableBalance;
        Theme = SubjectTheme.StartSales;
    }
    
    [DataMember]
    public CurrencyInfo Info { get; set; }
    
    [DataMember]
    public decimal AvailableBalance { get; set; }
    
    [DataMember]
    public decimal TotalPrice { get; set; }

    [DataMember]
    public DateTime ParsingDate { get; set; }

    public string FilePath => $"{PathHelper.PathList.LaunchesPath}{DateTime.Now:dd/MM/yyyy}.json";
    
    public SubjectTheme? Theme { get; init; }

    public override string ToString()
    {
        return $"Продаем: {Info.FirstCoin} за {Info.SecondCoin}\n" +
               $"Дата: {ParsingDate}\n" +
               $"Верхняя цена: {Info.UpperPrice} {Info.SecondCoin}\n" +
               $"Нижняя цена: {Info.BottomPrice} {Info.SecondCoin}\n" +
               $"Текущий курс: {TotalPrice} {Info.SecondCoin}\n" +
               $"Лимит баланса: {Info.BalanceLimit} {Info.FirstCoin}\n" +
               $"Баланс: {AvailableBalance} {Info.FirstCoin}";
    }
}