using System.Runtime.Serialization;
using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Enums;
using CryptoParserBot.CryptoBot.Interfaces;

namespace CryptoParserBot.CryptoBot.Models.Logs;

[DataContract]
public sealed class ErrorLog : ILog
{
    public ErrorLog() { }

    public ErrorLog(string message)
    {
        Message = message;
        ErrorDate = DateTime.UtcNow;
        Theme = SubjectTheme.Error;
    }
    
    [DataMember]
    public string Message { get; set; }
    
    [DataMember]
    public DateTime ErrorDate { get; set; }

    public string FilePath => $"{PathHelper.PathList.ErrorsPath}{DateTime.Now:dd/MM/yyyy}.json";
    public SubjectTheme? Theme { get; init; }

    public override string ToString()
    {
        return $"Дата возникновения ошибки: {ErrorDate}\n" +
               $"Ошибка: {Message}";
    }
}

