using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.ExchangeClients.Clients;
using static System.String;

//= remove this ===
var pl = new PathList();
var paths = AttributesHelperExtension.GetStringValues(pl);
PathHelper.CheckForPathExists(paths);
// ================
// add to PathHelper static ctor

var cfg = ConfigInitializer.GetClientConfig();
var client = new NiceHashClient(
    key: cfg.Key, // public key
    secretKey: cfg.SecretKey, // secret key
    organizationId: cfg.OrgID // organization id
);

Console.Write("Какой коин продаем: ");
var sellCoin = Console.ReadLine()?.ToUpper();

Console.Write("Какой коин покупаем: ");
var buyCoin = Console.ReadLine()?.ToUpper();

Console.Write($"Нижняя цена({buyCoin}): ");
var bottomRes = decimal.TryParse(
    Console.ReadLine()?.Replace('.', ','), out var bottomLimit);

Console.Write($"Верхняя цена({buyCoin}): ");
var upperRes = decimal.TryParse(
    Console.ReadLine()?.Replace('.', ','), out var upperLimit);

Console.Write($"Минимальный баланс({sellCoin}): ");
var balanceRes = decimal.TryParse(
    Console.ReadLine()?.Replace('.', ','), out var balanceLimit);

if( upperRes is false ||
    bottomRes is false ||
    balanceRes is false || 
    IsNullOrEmpty(sellCoin) ||
    IsNullOrEmpty(buyCoin))
{
    Console.WriteLine("Ошибка ввода данных!");
    return;
}

Console.WriteLine("Вы уверены, что хотите начать?");
Console.Write("Y\\N: ");
var result = Console.ReadLine()?.ToUpper();

if (result != "Y") return;

var bot = new CryptoBot(client, new CurrencyInfo
{
    FirstCoin = sellCoin,
    SecondCoin = buyCoin,
    BottomPrice = bottomLimit,
    UpperPrice = upperLimit,
    BalanceLimit = balanceLimit
});
bot.StartBot();

