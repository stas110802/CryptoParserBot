using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Models.Logs;

namespace CryptoParserBot.CryptoBot.Logs;

public sealed class BotLogger
{
    public BotLogger()
    {
        JsonLogger = new JsonLogger();
        
        PathHelper.CheckForPathExists(LogsPath, OrderPath, CurrencyPath, ErrorsPath);
    }

    private string LogsPath =>
        Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..")) + "\\logs\\";

    private string OrderFilePath => $"{LogsPath}orders\\{DateTime.Now:dd/MM/yyyy}.json";
    private string CurrencyFilePath => $"{LogsPath}prices\\{DateTime.Now:dd/MM/yyyy}.json";
    private string ErrorsFilePath => $"{LogsPath}errors\\{DateTime.Now:dd/MM/yyyy}.json";
    
    private string OrderPath => $"{LogsPath}orders\\";
    private string CurrencyPath => $"{LogsPath}prices\\";
    private string ErrorsPath => $"{LogsPath}errors\\";
    
    public EmailLogger EmailLogger { get; init; }
    public JsonLogger JsonLogger { get; init; }
    public string [] RecipientsEmails { get; set; }

    public void AddOrderInfoGlobalLog(OrderLog value)
    {
        JsonLogger.LogInfoAt(OrderFilePath, value);

        var emRes = EmailLogger.SendMailMessage("Akira-Bot", value.ToString(), RecipientsEmails);

        if (emRes is false)
        {
            var mailErrorLog = new ErrorLog("Не удалось отправить сообщение на почту");
            JsonLogger.LogInfoAt(ErrorsFilePath, mailErrorLog);
        }
    }

    public void AddCurrencyInfoLog(CurrencyLog info)
    {
        JsonLogger.LogInfoAt(CurrencyFilePath, info);
    }

    /// <summary>
    /// Write only in json.
    /// </summary>
    /// <param name="errorInfo"></param>
    public void AddErrorLog(ErrorLog errorInfo)
    {
        JsonLogger.LogInfoAt(ErrorsFilePath, errorInfo);
    }

    /// <summary>
    /// Write to json and send by email.
    /// Important errors that need to be notified.
    /// </summary>
    /// <param name="errorInfo"></param>
    public void AddErrorLogGlobal(ErrorLog errorInfo)
    {
        JsonLogger.LogInfoAt(ErrorsFilePath, errorInfo);
        var emRes = EmailLogger.SendMailMessage("Akira-Bot", errorInfo.ToString(), RecipientsEmails);

        if (emRes is false)
        {
            var gmailErrorLog = new ErrorLog("Не удалось отправить сообщение на почту");
            JsonLogger.LogInfoAt(ErrorsFilePath, gmailErrorLog);
        }
    }
}