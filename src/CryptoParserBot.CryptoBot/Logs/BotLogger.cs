using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Enums;
using CryptoParserBot.CryptoBot.Models.Logs;

namespace CryptoParserBot.CryptoBot.Logs;

public sealed class BotLogger
{
    public BotLogger()
    {
        JsonWriter = new JsonWriter();
        PathHelper.CheckForPathExists(LogsPath, OrderPath, ErrorsPath);
    }
    
    public SmtpSender SmtpSender { get; init; }
    public JsonWriter JsonWriter { get; }
    public string [] RecipientsEmails { get; init; }

    public void AddOrderInfoGlobalLog(OrderLog log)
    {
        AddLog(log, OrderFilePath, SubjectTheme.Sell, true);
    }

    /// <summary>
    /// Write only in json.
    /// </summary>
    /// <param name="errorInfo"></param>
    public void AddErrorLog(ErrorLog errorInfo)
    {
        AddLog(errorInfo, ErrorsFilePath);
    }

    /// <summary>
    /// Write to json and send by email.
    /// Important errors that need to be notified.
    /// </summary>
    /// <param name="errorInfo"></param>
    public void AddErrorLogGlobal(ErrorLog log)
    {
        AddLog(log, ErrorsFilePath, SubjectTheme.Error, true);
    }

    private string LogsPath => $"{PathHelper.ProjectPath}\\logs\\";
    private string OrderPath => $"{LogsPath}orders\\";
    private string ErrorsPath => $"{LogsPath}errors\\";
    private string OrderFilePath => $"{OrderPath}{DateTime.Now:dd/MM/yyyy}.json";
    private string ErrorsFilePath => $"{ErrorsPath}{DateTime.Now:dd/MM/yyyy}.json";

    private void AddLog<T>(T log, string filePath, SubjectTheme? subjectTheme = null, bool sendMailMessage = false)
        where T : class
    {
        if (log == null) throw new ArgumentNullException(nameof(log));
        
        JsonWriter.LogInfoAt(filePath, log);
        
        if(sendMailMessage is false) return;
        
        var theme = subjectTheme?.ToDescription();
        var emRes = SmtpSender.SendMailMessage(
            $"Akira-Bot [{theme}]", log.ToString(), RecipientsEmails);

        if (emRes) return;
        
        var mailErrorLog = new ErrorLog("Не удалось отправить сообщение на почту");
        JsonWriter.LogInfoAt(ErrorsFilePath, mailErrorLog);
    }
}