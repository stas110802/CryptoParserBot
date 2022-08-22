using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Attributes;
using CryptoParserBot.ExchangeClients.Interfaces;
using static System.String;

namespace CryptoParserBot.ConsoleApplication.Commands;

public sealed class OrderCommands : CommandsObject<Action>
{
    private readonly IExchangeClient? _client;

    public OrderCommands(IExchangeClient? client)
    {
        _client = client;
        Commands = CommandHelper.GetConsoleCommands(this, typeof(OrderCommands));
    }
    
    [ConsoleCommand(ConsoleKey.D1)]
    public void CreateMarketOrder()
    {
        Console.Clear();
        CreateOrder(out var buy, out var sell, out var amount);
        
        if (IsNullOrEmpty(buy) || IsNullOrEmpty(sell) || amount <= 0)
            return;
        
        Console.WriteLine("Вы уверне, что хотите создать ордер?");
        var res = Console.ReadLine()?.ToUpper();
        if(res != "Y")
            return;
        
        _client?.CreateSellOrder(buy + sell, amount);

        Console.WriteLine("Market ордер успешно создан!");
        Thread.Sleep(1000);
        Console.Clear();
    }
    
    [ConsoleCommand(ConsoleKey.D2)]
    public void CreateLimitOrder()
    {
        Console.Clear();
        CreateOrder(out var buy, out var sell, out var amount);
        
        if (IsNullOrEmpty(buy) || IsNullOrEmpty(sell) || amount <= 0)
            return;
        
        var price = GetPrice(buy);
        Console.WriteLine("Вы уверне, что хотите создать ордер?");
        Console.WriteLine("Y\\N");
        
        var res = Console.ReadLine()?.ToUpper();
        if(res != "Y")
            return;

        if (price > 0)
        {
            _client?.CreateSellOrder(buy + sell, amount, price);
            Console.WriteLine("Limit ордер успешно создан!");
            Thread.Sleep(1000);
            Console.Clear();
        }
    }

    private void CreateOrder(out string? sellCoin, out string? buyCoin, out decimal amount)
    {
        sellCoin = null;
        buyCoin = null;
        amount = 0m;
        
        Console.Clear();
        if (_client == null)
        {
            Console.WriteLine("[ERROR] Вы должны выбрать биржу!");
            Thread.Sleep(2500);
            return;
        }
        
        Console.Clear();
        Console.Write("Какой коин продаем: ");
        sellCoin = Console.ReadLine()?.ToUpper();
        
        Console.Write("Какой коин покупаем: ");
        buyCoin = Console.ReadLine()?.ToUpper();
        
        Console.Write($"Сколько продаем({sellCoin}): ");
        var upperRes = decimal.TryParse(
            Console.ReadLine()?.Replace('.', ','), out amount);
        
        if( IsNullOrEmpty(sellCoin) ||
            IsNullOrEmpty(buyCoin))
        {
            Console.WriteLine("Ошибка ввода данных!");
        }
    }

    private decimal GetPrice(string buyCoin)
    {
        Console.Write($"Курс продажи({buyCoin}): ");
        decimal.TryParse(
            Console.ReadLine()?.Replace('.', ','), out var price);
        
        return price;
    }
    
    public override void PrintCommands()
    {
        ConsoleHelper.Write("[Q]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - вернуться назад", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Market ордер", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - Limit ордер", ConsoleColor.Gray);
    }
}