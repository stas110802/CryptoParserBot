using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Attributes;
using CryptoParserBot.CryptoBot;
using CryptoParserBot.ExchangeClients.Clients;

namespace CryptoParserBot.ConsoleApplication.Commands;

public sealed class ClientCommands
{
    [ConsoleCommand(ConsoleKey.D1)]
    public NiceHashClient? CreateClientCommand()
    {
        var cfg = ConfigInitializer.GetClientConfig();
        
        if (cfg == null)
        {
            Console.WriteLine("Конфиг пуст. Сначала заполните его.");
            Thread.Sleep(2500);
            return null;
        }
        
        var client = new NiceHashClient( 
            key: cfg.Key, 
            secretKey: cfg.SecretKey, 
            organizationId: cfg.OrgID 
        );
        Console.Write("Биржа ");
        ConsoleHelper.Write("Nice Hash ", ConsoleColor.Green);
        Console.WriteLine("выбрана.");
        
        return client;
    }
}