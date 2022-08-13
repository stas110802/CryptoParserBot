using System.Globalization;
using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ExchangeClients.Interfaces;

namespace CryptoParserBot.ConsoleApplication;

public sealed class ExchangeRate
{
    private readonly IExchangeClient _publicClient;

    public ExchangeRate(IExchangeClient publicClient)
    {
        _publicClient = publicClient;
    }

    public void PrintExchangeRate()
    {
        var ethUsdt = _publicClient.GetCurrencyPrice("ETHUSDT").ToString(CultureInfo.InvariantCulture);
        var btcUsdt = _publicClient.GetCurrencyPrice("BTCUSDT").ToString(CultureInfo.InvariantCulture);
        
        //Console.Write("USD-RUB: ");
        //ConsoleHelper.Write("60.003 ", ConsoleColor.Red);
        
        Console.Write("BTC-USDT: ");
        ConsoleHelper.WriteLine(btcUsdt, ConsoleColor.Red);
        
        //Console.Write("EUR-RUB: ");
        //ConsoleHelper.Write("63.003 ", ConsoleColor.Red);
        
        Console.Write("ETH-USDT: ");
        ConsoleHelper.WriteLine(ethUsdt, ConsoleColor.Red);
    }
}