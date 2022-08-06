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

    public static void InitConfig()
    {
        Config = GetConfig<BotConfig>(CofnigPath);
    }
    
    private static BotConfig? Config { get; set; }
    private static string CofnigPath => $"{PathHelper.PathList.ConfigsPath}config.json";
    
    private static T GetConfig<T>(string path)
        where T : class
    {
        return JsonHelper.GetDeserializeObject<T>(path);
    }
}