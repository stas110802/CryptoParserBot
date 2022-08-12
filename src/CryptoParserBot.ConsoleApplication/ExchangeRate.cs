using System.Globalization;
using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ExchangeClients.Interfaces;

namespace CryptoParserBot.ConsoleApplication;

public class ExchangeRate
{
    private IExchangeClient _client;

    public ExchangeRate(IExchangeClient client)
    {
        _client = client;
    }

    public void PrintExchangeRate()
    {
        var ethUsdt = _client.GetCurrencyPrice("ETHUSDT").ToString(CultureInfo.InvariantCulture);
        var btcUsdt = _client.GetCurrencyPrice("BTCUSDT").ToString(CultureInfo.InvariantCulture);
        
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