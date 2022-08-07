using CryptoParserBot.ConsoleApplication.Attributes;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.ExchangeClients.Clients;

namespace CryptoParserBot.ConsoleApplication.Commands;

public sealed class ClientCommands
{
    private readonly BotKeys _cfg;
    
    public ClientCommands(BotKeys cfg)
    {
        _cfg = cfg;
    }
    
    [ConsoleCommand(ConsoleKey.D1)]
    public NiceHashClient CreateClientCommand()
    {
        var client = new NiceHashClient( 
            key: _cfg.Key, 
            secretKey: _cfg.SecretKey, 
            organizationId: _cfg.OrgID 
        );
        Console.WriteLine("Nice Hash client created.");

        return client;
    }
}