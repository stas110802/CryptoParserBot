using CryptoParserBot.CryptoBot;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.ExchangeClients.Clients;
using static System.String;

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

Console.Write($"Лимит цена({buyCoin}): ");
var currencyRes = decimal.TryParse(
    Console.ReadLine()?.Replace('.', ','), out var currencyLimit);

Console.Write($"Минимальный баланс({sellCoin}): ");
var balanceRes = decimal.TryParse(
    Console.ReadLine()?.Replace('.', ','), out var balanceLimit);

if( currencyRes is false ||
    balanceRes is false || 
    IsNullOrEmpty(sellCoin) ||
    IsNullOrEmpty(buyCoin))
{
    Console.WriteLine("Ошибка ввода данных!");
    return;
}

var bot = new CryptoBot(client, new CurrencyInfo
{
    FirstCoin = sellCoin,
    SecondCoin = buyCoin,
    PriceLimit = currencyLimit,
    BalanceLimit = balanceLimit
});
bot.StartBot();