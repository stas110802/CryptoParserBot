using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot;

public static class ConfigInitializer
{
    static ConfigInitializer()
    {
        PathHelper.CheckForPathExists(ConfigsPath);
    }
    
    public static BotKeys GetClientConfig()
    {
        return GetConfig<BotKeys>(ClientPath);
    }
    
    public static SmtpHost GetSmtpEmailConfig()
    {
        return GetConfig<SmtpHost>(SmtpPath);
    }

    private static string ConfigsPath => $"{PathHelper.ProjectPath}\\configs\\";
    private static string ClientPath => $"{ConfigsPath}ClientInfo.json";
    private static string SmtpPath => $"{ConfigsPath}SmtpEmailInfo.json";
    
    // mb delete this
    private static T GetConfig<T>(string path)
        where T : class
    {
        return JsonHelper.GetDeserializeObject<T>(path);
    }
}