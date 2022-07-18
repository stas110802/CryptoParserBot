using CryptoParserBot.CryptoBot;
using CryptoParserBot.ExchangeClient_s.Clients;



// write your data here
var client = new NiceHashClient(
    key: "", // public key
    secretKey: "", // secret key
    organizationId: "" // organization id
);

// bot settings
var options = new CryptoBotOptions
{
    FirstCoin = "BTC", // coin for sale
    SecondCoin = "USDT",// coin to buy
    PriceLimit = 25000m // price limit - 25 000 $
};

var bot = new CryptoBot(client, options);

// Bot logic
bot.StartParsing();



// balance test
//client.GetAccountBalance().ForEach(x =>
//{
//    Console.WriteLine($"{x.Currency} : {x.AvailableBalance}");
//});
