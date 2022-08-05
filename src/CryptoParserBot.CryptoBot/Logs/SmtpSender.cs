using System.Net;
using System.Net.Mail;
using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot.Logs;

public sealed class SmtpSender
{
    private readonly SmtpHost _config;

    public SmtpSender(SmtpHost config)
    {
        _config = config;
    }

    public bool SendMailMessage(string subject, string content, string[] recipients)
    {
        if (recipients == null || recipients.Length == 0)
            throw new ArgumentException("recipients count == 0");

        // create mail.ru smtp client 
        using var smtpClient = new SmtpClient
        {
            Host = _config.Host,
            Port = _config.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _config.Login,
                _config.Password)
        };

        using var msg = new MailMessage(_config.Login, recipients[0], subject, content);

        // if we have > 1 recipients
        for (var i = 1; i < recipients.Length; i++)
            msg.To.Add(recipients[i]);

        // try to send a mail
        try
        {
            smtpClient.Send(msg);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}