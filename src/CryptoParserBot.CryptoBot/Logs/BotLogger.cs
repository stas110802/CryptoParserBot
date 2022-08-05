using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Enums;
using CryptoParserBot.CryptoBot.Interfaces;
using CryptoParserBot.CryptoBot.Models.Logs;

namespace CryptoParserBot.CryptoBot.Logs;

public sealed class BotLogger
{
    public BotLogger()
    {
        JsonWriter = new JsonWriter();
    }
    
    public SmtpSender SmtpSender { get; init; }
    public JsonWriter JsonWriter { get; }
    public string [] RecipientsEmails { get; init; }

    public void AddLog<T>(T log)
        where T : ILog
    {
        AddLog(log, log.FilePath, log.Theme, true);
    }
    
    private void AddLog<T>(T log, string filePath, SubjectTheme? subjectTheme = null, bool sendMailMessage = false)
    {
        if (log == null) throw new ArgumentNullException(nameof(log));
        
        JsonWriter.LogInfoAt(filePath, log);
        
        if(sendMailMessage is false) return;
        
        var theme = subjectTheme?.ToDescription();
        var emRes = SmtpSender.SendMailMessage(
            $"Akira-Bot [{theme}]", log.ToString(), RecipientsEmails);

        if (emRes) return;
        
        var mailErrorLog = new ErrorLog("Не удалось отправить сообщение на почту");
        JsonWriter.LogInfoAt(mailErrorLog.FilePath, mailErrorLog);
    }
}