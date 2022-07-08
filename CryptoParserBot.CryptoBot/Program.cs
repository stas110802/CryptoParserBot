using CryptoParserBot.CryptoBot;
using CryptoParserBot.ExchangeClient_s.Clients;

var bot = new CryptoBot( // create NiceHash bot
          new NiceHashClient());

// Bot logic
bot.StartParsing();