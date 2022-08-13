using CryptoParserBot.ConsoleApplication.Attributes;

namespace CryptoParserBot.ConsoleApplication;

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
    
    public static Dictionary<ConsoleKey, Func<T>> GetConsoleCommands<T>(object target, Type type)
    {
        var result = new Dictionary<ConsoleKey, Func<T>>();
        var methods = type.GetMethods();
        
        foreach (var method in methods)
        {
            if (Attribute.GetCustomAttributes(method, typeof(ConsoleCommandAttribute)).FirstOrDefault() is not ConsoleCommandAttribute attr) continue;

            var action = (Func<T>) Delegate.CreateDelegate(typeof(Func<T>), target, method);
            result.Add(attr.Key, action);
        }

        return result;
    }
    
    public static void StartEvent<T>(Dictionary<ConsoleKey, T> command,  CommandsObject<T> obj, ConsoleKey key)
        where T : class, MulticastDelegate
    {
        var action = command.ContainsKey(key) ? command[key] : null;
        if (action == null) return;
        
        (action as Action)?.Invoke();
        obj.PrintCommands();
    }
    
    public static T? StartEvent<T>(Dictionary<ConsoleKey, Func<T>>? command, ConsoleKey key)
        where T : class
    {
        if (command == null)
            throw new ArgumentNullException($"{command} is null");
        
        var action = command.ContainsKey(key) ? command[key] : null;

        return action?.Invoke();
    }
}