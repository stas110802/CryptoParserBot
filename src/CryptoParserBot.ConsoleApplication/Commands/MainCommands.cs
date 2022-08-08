using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Attributes;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.ExchangeClients.Interfaces;
using static System.String;

namespace CryptoParserBot.ConsoleApplication.Commands;

public sealed class MainCommands
{
    private CryptoBot.CryptoBot? _bot;
    private IExchangeClient? _client;
    private readonly Dictionary<ConsoleKey, Func<IExchangeClient>> _clientCommands;
    private readonly Dictionary<ConsoleKey, Action> _configCommands;
    
    public MainCommands()
    {
        var cl = new ClientCommands();// think [GC]
        var cf = new ConfigCommands();// think [GC]
        
        _clientCommands = CommandHelper.GetConsoleCommands<IExchangeClient>(cl);
        _configCommands = CommandHelper.GetConsoleCommands(cf, typeof(ConfigCommands));
    }
    
    public MainCommands(IExchangeClient? client, CryptoBot.CryptoBot? bot)
    {
        _client = client;
        _bot = bot;
    }
    
    [ConsoleCommand(ConsoleKey.D1)]
    public void StartBotCommand()
    {
        Console.Clear();
        if (_bot != null)
        {
            _bot.StartBot();
        }
        else
        {
            Console.WriteLine("[ERROR] You need to create at least 1 order!");
            Thread.Sleep(2500);
        }
    }

    [ConsoleCommand(ConsoleKey.D2)]
    public void CreateSellOrderCommand()
    {
        Console.Clear();
        if (_client == null)
        {
            Console.WriteLine("[ERROR] You need to create client");
            Thread.Sleep(2500);
            return;
        }
        
        Console.Clear();
        Console.Write("Какой коин продаем: ");
        var sellCoin = Console.ReadLine()?.ToUpper();

        Console.Write("Какой коин покупаем: ");
        var buyCoin = Console.ReadLine()?.ToUpper();

        Console.Write($"Рекомендуемая цена({buyCoin}): ");
        var upperRes = decimal.TryParse(
            Console.ReadLine()?.Replace('.', ','), out var upperLimit);

        Console.Write($"Критическая цена({buyCoin}): ");
        var bottomRes = decimal.TryParse(
            Console.ReadLine()?.Replace('.', ','), out var bottomLimit);

        Console.Write($"Минимальный баланс({sellCoin}): ");
        var balanceRes = decimal.TryParse(
            Console.ReadLine()?.Replace('.', ','), out var balanceLimit);
    
        if( upperRes is false ||
            bottomRes is false ||
            balanceRes is false || 
            IsNullOrEmpty(sellCoin) ||
            IsNullOrEmpty(buyCoin))
        {
            Console.WriteLine("Ошибка ввода данных!");
            return;
        }

        Console.WriteLine("Вы уверены, что хотите добавить данный ордер?");
        Console.Write("Y\\N: ");
        var result = Console.ReadLine()?.ToUpper();

        if (result != "Y") return;

        _bot = new CryptoBot.CryptoBot(_client!, new CurrencyInfo
        {
            FirstCoin = sellCoin,
            SecondCoin = buyCoin,
            BottomPrice = bottomLimit,
            UpperPrice = upperLimit,
            BalanceLimit = balanceLimit
        });
        Console.Clear();
    }

    [ConsoleCommand(ConsoleKey.D3)]
    public void CreateClientCommand()
    {
        Console.Clear();
        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Nice Hash client", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Binance client", ConsoleColor.Gray);
        
        var key = ConsoleKey.Delete;
        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey(true).Key;
            var action = _clientCommands.ContainsKey(key) ? _clientCommands[key] : null;
            if (action == null) continue;
            
            _client = action.Invoke();
            break;
        }
        
        Thread.Sleep(2500);
        Console.Clear();
    }
    
    [ConsoleCommand(ConsoleKey.D4)]
    public void EditConfig()
    {
        Console.Clear();
        PrintCommands();
        
        var key = ConsoleKey.Delete;
        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey(true).Key;
            var action = _configCommands.ContainsKey(key) ? _configCommands[key] : null;
            if (action == null) continue;
            
            action.Invoke();
            PrintCommands();
        }
        
        Thread.Sleep(2500);
        Console.Clear();
    }

    private void PrintCommands()
    {
        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - create new config", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - update client keys", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[3]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - update SMTP settings", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[4]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - update recipient mail", ConsoleColor.Gray);
    }
}