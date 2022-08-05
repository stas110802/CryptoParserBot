namespace CryptoParserBot.CryptoBot.Models.Logs;

public sealed class ErrorLog
{
    public ErrorLog() { }

    public ErrorLog(string message)
    {
        Message = message;
        ErrorDate = DateTime.UtcNow;
    }

    public string Message { get; set; }
    public DateTime ErrorDate { get; set; }

    public override string ToString()
    {
        return $"Дата возникновения ошибки: {ErrorDate}" +
               $"\nОшибка: {Message}";
    }
}

