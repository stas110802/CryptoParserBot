using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Attributes;
using CryptoParserBot.CryptoBot;
using CryptoParserBot.ExchangeClients.Clients;
using CryptoParserBot.ExchangeClients.Interfaces;

namespace CryptoParserBot.ConsoleApplication.Commands;

public sealed class ClientCommands : CommandsObject<Func<IExchangeClient>>
{
    public ClientCommands()
    {
        Commands = CommandHelper.GetConsoleCommands<IExchangeClient>(this, typeof(ClientCommands));
    }
    
    [ConsoleCommand(ConsoleKey.D1)]
    public NiceHashClient? CreateNiceHashClientCommand()
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

    public override void PrintCommands()
    {
        Console.Clear();
        ConsoleHelper.Write("[Q]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - вернуться назад", ConsoleColor.Gray);

        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Nice Hash", ConsoleColor.Gray);

        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Binance", ConsoleColor.Gray);
    }
}