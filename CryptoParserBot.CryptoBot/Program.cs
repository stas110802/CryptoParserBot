using CryptoParserBot.ExchangeClient_s.Clients;

var client = new NiceHashClient();
var balances = client.GetAccountBalance();
balances.ForEach(x =>
    Console.WriteLine($"Cur: {x.Currency}\nAvail bal: {x.AvailableBalance}"));
