using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Attributes;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.ExchangeClients.Clients;
using CryptoParserBot.ExchangeClients.Interfaces;
using static System.String;

namespace CryptoParserBot.ConsoleApplication.Commands;

public sealed class MainCommands
{
    private CryptoBot.CryptoBot? _bot;
    private IExchangeClient? _client;
    private readonly Dictionary<ConsoleKey, Func<IExchangeClient>> _clientCommands;
    private readonly Dictionary<ConsoleKey, Action> _configCommands;
    private Dictionary<ConsoleKey, Action> _orderCommands;

    public MainCommands()
    {
        var cl = new ClientCommands();// think [GC]
        var cf = new ConfigCommands();// think [GC]
        
        _clientCommands = CommandHelper.GetConsoleCommands<IExchangeClient>(cl, typeof(ClientCommands));
        _configCommands = CommandHelper.GetConsoleCommands(cf, typeof(ConfigCommands));
    }
    
    public MainCommands(IExchangeClient? client, CryptoBot.CryptoBot? bot)
    {
        _client = client;
        _bot = bot;
    }
    
    public static void PrintCommands()
    {
        ConsoleHelper.WriteLine("Команды: ", ConsoleColor.Green);
        
        ConsoleHelper.Write("[Q]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - выход", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - работа с ботом", ConsoleColor.Gray);
       
        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - выбрать биржу", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[3]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - создать/обновить конфиг", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[4]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - баланс аккаунта", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[5]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - создать ордер на продажу", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[6]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - отменить все ордеры на продажу", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[7]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - информация о приложении", ConsoleColor.Gray);
    }
    
    public void StartBot()
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

    [ConsoleCommand(ConsoleKey.D1)]
    public void StartBotCommand()
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

        Console.WriteLine("Вы уверены, что хотите запустить бота с данными параметрами?");
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
        StartBot();
    }

    [ConsoleCommand(ConsoleKey.D2)]
    public void CreateClientCommand()
    {
        Console.Clear();
        
        ConsoleHelper.Write("[Q]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - вернуться назад", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Nice Hash", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Binance", ConsoleColor.Gray);
        
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
    
    [ConsoleCommand(ConsoleKey.D3)]
    public void EditConfig()
    {
        Console.Clear();
        ConfigCommands.PrintCommands();
        
        var key = ConsoleKey.Delete;
        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey(true).Key;
            var action = _configCommands.ContainsKey(key) ? _configCommands[key] : null;
            if (action == null) continue;
            
            action.Invoke();
            ConfigCommands.PrintCommands();
        }
        
        Console.Clear();
    }

    [ConsoleCommand(ConsoleKey.D4)]
    public void PrintAccountBalanceCommand()
    {
        if (_client == null)
        {
            Console.WriteLine("Сначала выберите биржу(2 команда).");
            Thread.Sleep(2000);
            Console.Clear();
            return;
        }

        Console.Clear();
        ConsoleHelper.WriteLine("Активы аккаунта:", ConsoleColor.Green);
        var balance = _client.GetAccountBalance();
        balance.ForEach(x =>
            Console.WriteLine($"{x.Currency} = {x.AvailableBalance}"));
        
        Console.WriteLine("Нажмите любую клавишу, чтобы выйти");
        Console.ReadKey(true);
        Console.Clear();
    }

    [ConsoleCommand(ConsoleKey.D5)]
    public void CreateSellOrderCommand()
    {
        if (_client == null)
        {
            Console.WriteLine("[ERROR] Сначала выберите биржу");
            Thread.Sleep(2000);
            Console.Clear();// вывести эти 3 строчки в метод!
            return;
        }
        
        Console.Clear();
        OrderCommands.PrintCommands();
        
        _orderCommands = CommandHelper.GetConsoleCommands(
            new OrderCommands(_client), typeof(OrderCommands));
        
        var key = ConsoleKey.NoName;
        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey(true).Key;
            var action = _orderCommands.ContainsKey(key) ? _orderCommands[key] : null;
            action?.Invoke();
            
            OrderCommands.PrintCommands();
        }
        
        Console.Clear();
    }

    [ConsoleCommand(ConsoleKey.D6)]
    public void CancelAllSellOrdersCommand()
    {
        
    }
    
    [ConsoleCommand(ConsoleKey.D7)]
    public void PrintAppInfoCommand()
    {
        Console.Clear();
        Console.WriteLine("Akira Bot - крипто-торговый бот");
        Console.ReadKey(true);
        Console.Clear();
    }
}