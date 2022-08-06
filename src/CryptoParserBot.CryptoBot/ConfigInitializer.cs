using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot;

public static class ConfigInitializer
{
    static ConfigInitializer()
    {
        InitConfig();
    }
    
    public static BotKeys GetClientConfig()
    {
        return Config!.Client;
    }
    
    public static SmtpHost GetSmtpEmailConfig()
    {
        return Config!.Smtp;
    }

    public static string[] GetRecipientMails()
    {
        return Config!.Recipients.ToArray();
    }

    public static void InitConfig()
    {
        Config = GetConfig<BotConfig>(ConfigPath);
    }
    
    private static BotConfig? Config { get; set; }
    private static string ConfigPath => $"{PathHelper.PathList.ConfigsPath}config.json";
    
    private static T GetConfig<T>(string path)
        where T : class
    {
        return JsonHelper.GetDeserializeObject<T>(path);
    }
}