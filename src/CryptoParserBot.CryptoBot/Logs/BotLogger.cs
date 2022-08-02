using CryptoParserBot.CryptoBot.Models.Logs;

namespace CryptoParserBot.CryptoBot.Logs;

public sealed class BotLogger
{
    public BotLogger()
    {
        JsonLogger = new JsonLogger();
    }

    private string LogsPath => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..")) + "\\logs\\";
    private string OrderPath => $"{LogsPath}Orders\\{DateTime.Now:dd/MM/yyyy}.json";
    private string CurrencyInfoPath => $"{LogsPath}Prices\\{DateTime.Now:dd/MM/yyyy}.json";
    private string ErrorsInfoPath => $"{LogsPath}Errors\\{DateTime.Now:dd/MM/yyyy}.json";
    public EmailLogger EmailLogger { get; init; }
    public JsonLogger JsonLogger { get; init; }
    public string [] RecipientsEmails { get; set; }

    public void AddOrderInfoGlobalLog(OrderLog value)
    {
        JsonLogger.LogInfoAt(OrderPath, value);

        var emRes = EmailLogger.SendMailMessage("Akira-Bot", value.ToString(), RecipientsEmails);

        if (emRes is false)
        {
            var mailErrorLog = new ErrorLog("Не удалось отправить сообщение на почту");
            JsonLogger.LogInfoAt(ErrorsInfoPath, mailErrorLog);
        }
    }

    public void AddCurrencyInfoLog(CurrencyLog info)
    {
        JsonLogger.LogInfoAt(CurrencyInfoPath, info);
    }

    /// <summary>
    /// Write only in json.
    /// </summary>
    /// <param name="errorInfo"></param>
    public void AddErrorLog(ErrorLog errorInfo)
    {
        JsonLogger.LogInfoAt(ErrorsInfoPath, errorInfo);
    }

    /// <summary>
    /// Write to json and send by email.
    /// Important errors that need to be notified.
    /// </summary>
    /// <param name="errorInfo"></param>
    public void AddErrorLogGlobal(ErrorLog errorInfo)
    {
        JsonLogger.LogInfoAt(ErrorsInfoPath, errorInfo);
        var emRes = EmailLogger.SendMailMessage("Akira-Bot", errorInfo.ToString(), RecipientsEmails);

        if (emRes is false)
        {
            var gmailErrorLog = new ErrorLog("Не удалось отправить сообщение на почту");
            JsonLogger.LogInfoAt(ErrorsInfoPath, gmailErrorLog);
        }
    }
}