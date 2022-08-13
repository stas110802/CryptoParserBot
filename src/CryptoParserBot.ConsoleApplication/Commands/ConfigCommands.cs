using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.ConsoleApplication.Attributes;
using CryptoParserBot.CryptoBot;
using CryptoParserBot.CryptoBot.Models.Configs;
using static System.String;

namespace CryptoParserBot.ConsoleApplication.Commands;

public sealed class ConfigCommands : CommandsObject<Action>
{
    public ConfigCommands()
    {
        Commands = CommandHelper.GetConsoleCommands(this, typeof(ConfigCommands));
    }
    
    [ConsoleCommand(ConsoleKey.D1)]
    public void CreateNewConfig()
    {
        var cfg = new BotConfig
        {
            Client = GetClient(),
            Smtp = GetSmtp(),
            Recipients = GetRecipients()
        };
        ConfigInitializer.WriteNewConfig(cfg);
        Console.WriteLine("Конфиг успешно создан.");
        Thread.Sleep(2000);
        Console.Clear();
    }
    
    [ConsoleCommand(ConsoleKey.D2)]
    public void UpdateClientInfo()
    { 
        Console.Clear();
        ConfigInitializer.UpdateClientInfo(GetClient());
        PrintSuccessMessage();
    }
    
    [ConsoleCommand(ConsoleKey.D3)]
    public void UpdateSmtpInfo()
    {
        Console.Clear();
        ConfigInitializer.UpdateSmtpInfo(GetSmtp());
        PrintSuccessMessage();
    }
    
    [ConsoleCommand(ConsoleKey.D4)]
    public void UpdateRecipientInfo()
    {
        Console.Clear();
        ConfigInitializer.UpdateRecipientInfo(GetRecipients());
        PrintSuccessMessage();
    }
    
    public override void PrintCommands()
    {
        ConsoleHelper.Write("[Q]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - вернуться назад", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[1]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - создать новый конфиг", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[2]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - изменить ключи API", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[3]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - изменить SMTP настройки", ConsoleColor.Gray);
        
        ConsoleHelper.Write("[4]", ConsoleColor.Red);
        ConsoleHelper.WriteLine(" - изменить почту для получения уведомлений", ConsoleColor.Gray);
    }

    private BotKeys GetClient()
    {
        Console.WriteLine("Client setting");
        ReadData("Key: ", out var key);
        ReadData("Secret key: ", out var secretKey);
        ReadData("Organization ID: ", out var orgId);

        return new BotKeys
        {
            Key = key,
            SecretKey = secretKey,
            OrgID = orgId
        };
    }

    private SmtpHost GetSmtp()
    {
        Console.WriteLine("SMTP настройки");
        ReadData("Login: ", out var login);
        ReadData("Password: ", out var password);
        ReadData("Host: ", out var host);
        ReadData("Port: ", out var portStr);
        
        var res = int.TryParse(portStr, out var port);

        if (res is false)
            throw new Exception("Wrong port format");
        
        return new SmtpHost
        {
            Login = login,
            Password = password,
            Host = host,
            Port = port
        };
    }

    private List<string> GetRecipients()
    {
        var recipients = new List<string>();
        Console.WriteLine("Почта(ы) на которую(ые) вы хотите получать уведомления о работе бота");
        Console.WriteLine("Чтобы завершить ввод, просто нажмите 'ENTER', необходимо ввести хотя-бы 1 почту");
        
        var mail = "NotNull";
        while (IsNullOrEmpty(mail) is false)
        {
            mail = Console.ReadLine();
            if(IsNullOrEmpty(mail) is false)
                recipients.Add(mail);
        }

        return recipients;
    }
    
    private void ReadData(string parameter, out string data)
    {
        Console.Write($"{parameter}: ");
        data = Console.ReadLine()!;
        
        if(IsNullOrEmpty(data))
            throw new Exception("Error data input");
    }

    private void PrintSuccessMessage()
    {
        Console.WriteLine("Данные конфига обновлены!");
        Thread.Sleep(2000);
        Console.Clear();
    }
}