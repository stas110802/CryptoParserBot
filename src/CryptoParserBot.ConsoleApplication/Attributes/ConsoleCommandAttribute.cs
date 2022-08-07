using System.Runtime.Serialization;

namespace CryptoParserBot.ConsoleApplication.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommandAttribute : Attribute
{
    public ConsoleCommandAttribute(ConsoleKey key)
    {
        Key = key;
    }

    public ConsoleKey Key { get; }
}