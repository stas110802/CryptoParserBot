namespace CryptoParserBot.ConsoleApplication;

public abstract class CommandsObject<T>
    where T : class, MulticastDelegate
{
    public Dictionary<ConsoleKey, T>? Commands { get; set; }
    
    public abstract void PrintCommands();

    public virtual void InvokeActionCommand(ConsoleKey key)
    {
        if (Commands != null)
            CommandHelper.StartEvent(Commands, this, key);
        else
            throw new NullReferenceException($"{nameof(Commands)} uninitialized");
    }

    public virtual void ReadActionCommandKey()
    {
        var key = new ConsoleKey();
        while (key != ConsoleKey.Q)
        {
            key = Console.ReadKey(true).Key;
            InvokeActionCommand(key);
        }
    }

    public virtual TValue? InvokeFuncCommand<TValue>(ConsoleKey key)
        where TValue : class
    {
        if (Commands == null) 
            throw new NullReferenceException($"{nameof(Commands)} uninitialized");
        
        var res = CommandHelper.StartEvent(Commands as Dictionary<ConsoleKey,Func<TValue>>, key);
            
        return res;

    }
    
    public virtual TValue? ReadFuncCommandKey<TValue>()
        where TValue : class
    {
        var key = new ConsoleKey();
        TValue? result = null;
        
        while (key != ConsoleKey.Q &&
               result == null)
        {
            key = Console.ReadKey(true).Key;
            result = InvokeFuncCommand<TValue>(key);
        }
        
        return result;
    }
}