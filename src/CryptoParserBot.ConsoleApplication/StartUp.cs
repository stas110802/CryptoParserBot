using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Commands;
using CryptoParserBot.CryptoBot;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.ExchangeClients.Clients;
using CryptoParserBot.ExchangeClients.Interfaces;
using static System.String;

namespace CryptoParserBot.ConsoleApplication;

public sealed class StartUp
{
    private MainCommands _commands;
    private Dictionary<ConsoleKey, Action> _mainCommands;
    
    public StartUp()
    {
        _commands = new MainCommands();
        _mainCommands = CommandHelper.GetConsoleCommands(_commands);
    }
    
    public void PrintStartUpMessage()
    {
        ConsoleHelper.Write("Akira-Bot ", ConsoleColor.Red);
        ConsoleHelper.Write($"greets you ", ConsoleColor.Gray);
        ConsoleHelper.Write($"{Environment.UserName}", ConsoleColor.Green);
        Thread.Sleep(3000);
        Console.Clear();
    }
    
    public void PrintCommands()
    {
        ConsoleHelper.WriteLine("Commands: ", ConsoleColor.Green);
        // start bot
        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - start bot", ConsoleColor.Gray);
        // orders
        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - create sell order", ConsoleColor.Gray);
        // orders
        ConsoleHelper.Write("[3]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - create client", ConsoleColor.Gray);
        
        Thread.Sleep(2000);
    }

    public void ReadCommands()
    {
        var key = ConsoleKey.Delete;
        
        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey(true).Key;
            var action = _mainCommands.ContainsKey(key) ? _mainCommands[key] : null;
            if (action != null)
            {
                action.Invoke();
                PrintCommands();
            }
        }
    }
    
    public void CheckFoldersExists()
    {
        var pl = new PathList();
        var paths = AttributesHelperExtension.GetStringValues(pl);
        PathHelper.CheckForPathExists(paths);
    }
}