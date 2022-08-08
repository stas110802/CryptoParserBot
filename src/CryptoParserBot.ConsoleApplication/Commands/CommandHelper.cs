using CryptoParserBot.ConsoleApplication.Attributes;

namespace CryptoParserBot.ConsoleApplication.Commands;

public static class CommandHelper
{
    public static Dictionary<ConsoleKey, Action> GetConsoleCommands(object target, Type type)
    {
        var result = new Dictionary<ConsoleKey, Action>();
        var methods = type.GetMethods();
        
        foreach (var method in methods)
        {
            if (Attribute.GetCustomAttributes(method, typeof(ConsoleCommandAttribute)).FirstOrDefault() is not ConsoleCommandAttribute attr) continue;

            var action = (Action) Delegate.CreateDelegate(typeof(Action), target, method);
            result.Add(attr.Key, action);
        }

        return result;
    }
    
    public static Dictionary<ConsoleKey, Func<T>> GetConsoleCommands<T>(object target)
    {
        var result = new Dictionary<ConsoleKey, Func<T>>();
        var methods = typeof(ClientCommands).GetMethods();
        
        foreach (var method in methods)
        {
            if (Attribute.GetCustomAttributes(method, typeof(ConsoleCommandAttribute)).FirstOrDefault() is not ConsoleCommandAttribute attr) continue;

            var action = (Func<T>) Delegate.CreateDelegate(typeof(Func<T>), target, method);
            result.Add(attr.Key, action);
        }

        return result;
    }
}