using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Models.Configs;

namespace CryptoParserBot.CryptoBot;

public static class ConfigInitializer
{
    public static BotKeys GetClientConfig()
    {
        return GetConfig<BotKeys>(ClientPath);
    }

    public static CurrencyInfo GetCurrencyConfig()
    {
        return GetConfig<CurrencyInfo>(CurrencyPath);
    }

    public static BotEmail GetSmtpEmailConfig()
    {
        return GetConfig<BotEmail>(SmtpPath);
    }

    private static string CofigsPath => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..")) + "\\configs\\";
    private static string ClientPath => CofigsPath + "ClientInfo.json";
    private static string CurrencyPath => CofigsPath + "CurrencyInfo.json";
    private static string SmtpPath => CofigsPath + "SmtpEmailInfo.json";

    private static T GetConfig<T>(string path)
        where T : class
    {
        var conf = JsonHelper.GetDeserializeObject<T>(path);

        return conf;
    }
}