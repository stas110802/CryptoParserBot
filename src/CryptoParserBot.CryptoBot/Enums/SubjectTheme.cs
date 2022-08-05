using System.ComponentModel;

namespace CryptoParserBot.CryptoBot.Enums;

public enum SubjectTheme
{
    [Description("Ошибка")] Error,
    [Description("Продажа")] Sell,
    [Description("Активация продажи")] StartSales
}