using CryptoParserBot.CryptoBot.Enums;
using Newtonsoft.Json;

namespace CryptoParserBot.CryptoBot.Interfaces;

public interface ILog
{
    public string FilePath { get; }
    public SubjectTheme? Theme { get; init; }
}