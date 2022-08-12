using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Commands;

namespace CryptoParserBot.ConsoleApplication;

public sealed class StartUp
{
    private readonly Dictionary<ConsoleKey, Action> _mainCommands;
    
    public StartUp()
    {
        var commands = new MainCommands();
        _mainCommands = CommandHelper.GetConsoleCommands(commands, typeof(MainCommands));
    }
    
    public void PrintStartUpMessage()
    {
        ConsoleHelper.Write("Akira-Bot ", ConsoleColor.Red);
        ConsoleHelper.Write($"greets you ", ConsoleColor.Gray);
        ConsoleHelper.Write($"{Environment.UserName}", ConsoleColor.Green);
        Thread.Sleep(3000);
        Console.Clear();
    }
    
    
    public void ReadCommands()
    {
        var key = ConsoleKey.Delete;
        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey(true).Key;
            var action = _mainCommands.ContainsKey(key) ? _mainCommands[key] : null;
            if (action == null) continue;
            
            action.Invoke();
            MainCommands.PrintCommands();
        }
    }
    
    public void CheckFoldersExists()
    {
        var pl = new PathList();
        var paths = AttributesHelperExtension.GetStringValues(pl);
        PathHelper.CheckForPathExists(paths);
    }
}