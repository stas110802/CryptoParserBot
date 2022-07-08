using CryptoParserBot.ExchangeClient_s.Clients;

var client = new NiceHashClient();


// пока просто хардкодим валюту и максимальную цену
var currency = "BTCUSDT";
var setPrice = 25000;

// примерная логика бота (просто пример)
while (true)
{
    Thread.Sleep(5000);

    // получаем текущую цену BTCUSDT
    var totalPrice = client.GetCurrencyPrice(currency);

    // если текущая цена меньше установленной, то ничего не делаем
    if (totalPrice <= setPrice) continue;

    // если цена больше установленной
    // получаем баланс BTC на аккаунте
    var balances =
        client.GetAccountBalance().FirstOrDefault(x => x.Currency == "BTC");

    // создаем ордер на продажу всех BTC на аккаунте
    var order = client.CreateSellOrder(currency, balances.AvailableBalance, totalPrice);
}