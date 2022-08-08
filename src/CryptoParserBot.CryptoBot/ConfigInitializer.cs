using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Logs;
using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot;

public static class ConfigInitializer
{
    static ConfigInitializer()
    {
        PathHelper.CheckForFileExists(ConfigFilePath);
        InitConfig();
    }

    public static BotConfig? Config { get; set; }

    public static BotKeys? GetClientConfig()
    {
        return Config?.Client;
    }

    public static SmtpHost? GetSmtpEmailConfig()
    {
        return Config?.Smtp;
    }

    public static string[]? GetRecipientMails()
    {
        return Config?.Recipients?.ToArray();
    }

    public static void InitConfig()
    {
        Config = GetConfig<BotConfig>(ConfigFilePath);
    }

    public static void WriteNewConfig(BotConfig cfg)
    {
        JsonHelper.WriteDataAt(ConfigFilePath ,cfg);
        Config = cfg;
    }
    
    public static void UpdateClientInfo(BotKeys client)
    {
        if (Config == null) return;
        Config.Client = client;
        WriteNewConfig(Config);
    }

    public static void UpdateSmtpInfo(SmtpHost smtpHost)
    {
        if (Config == null) return;
        Config.Smtp = smtpHost;
        WriteNewConfig(Config);
    }

    public static void UpdateRecipientInfo(List<string> recipients)
    {
        if (Config == null) return;
        Config.Recipients = recipients;
        WriteNewConfig(Config);
    }

    private static string ConfigFilePath => $"{PathHelper.PathList.ConfigsPath}config.json";
    
    private static T? GetConfig<T>(string path)
        where T : class
    {
        return JsonHelper.GetDeserializeObject<T>(path);
    }
}