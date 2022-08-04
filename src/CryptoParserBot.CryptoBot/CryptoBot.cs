using CryptoParserBot.CryptoBot.Logs;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.CryptoBot.Models.Logs;
using CryptoParserBot.ExchangeClients.Interfaces;

namespace CryptoParserBot.CryptoBot;

public sealed class CryptoBot
{
    private readonly IExchangeClient _client;
    private readonly BotLogger _botLogger;
    private readonly CurrencyInfo _currencyInfo;
    
    public CryptoBot(IExchangeClient client)
    {
        _client = client;

        _botLogger = new BotLogger
        {
            RecipientsEmails = new[]
            {
                "baxtoban555308775@gmail.com",
                "Roman1199@mail.ru"
            },
            EmailLogger = new EmailLogger(ConfigInitializer.GetSmtpEmailConfig())
        };
    }

    public CryptoBot(IExchangeClient client, CurrencyInfo currencyInfo) 
        : this(client)
    {
        _currencyInfo = currencyInfo;
    }

    /// <summary>
    /// Starts parsing prices from the exchange
    /// and sells coins if the price has reached the required limit
    /// </summary>
    public void StartBot()
    {
        Console.WriteLine("Бот начал свою работу");
        Thread.Sleep(2000);

        var currency = _currencyInfo.FirstCoin + _currencyInfo.SecondCoin;

        try
        {
            while (true)
            {
                Console.Clear();
                var currentPrice = _client.GetCurrencyPrice(currency);

                // create currency info log
                var parseLog = new CurrencyLog(_currencyInfo, currentPrice);
                Console.WriteLine(parseLog);

                if (currentPrice <= _currencyInfo.PriceLimit)
                {
                    LoadingBar(10);
                    continue;
                }

                // get account balance
                var balances = _client
                    .GetAccountBalance()
                    .FirstOrDefault(x => x.Currency == _currencyInfo.FirstCoin);

                // if the balance is not active, then do not create an order
                if (balances!.IsActive is false)
                {
                    WriteErrorLog("Заброкированнный баланс!");
                    continue;
                }

                if (balances.AvailableBalance < _currencyInfo.BalanceLimit)
                {
                    LoadingBar(10);
                    continue;
                }

                // amount coin on the balance
                var amount = balances.AvailableBalance;

                // create sell order
                var orderResult = _client.CreateSellOrder(currency, amount);// MARKET ORDER
                
                if (orderResult)
                { 
                    // create order log
                    var orderLog = new OrderLog(
                    options: _currencyInfo, sellPrice: currentPrice, amount: amount);
                    // write info to json
                    _botLogger.AddOrderInfoGlobalLog(orderLog);
                    LoadingBar(15, "sell coins");
                }
                else
                {
                    WriteErrorLog(
                        $"Неудачная попытка разместить ордер на продажу {_currencyInfo.FirstCoin}-{_currencyInfo.SecondCoin}");
                }
            }
        }
        catch (Exception error)
        {
            Console.Clear();
            WriteErrorLog($"Сбой работы бота. {error.Message}");
            // restart application after 30 sec
            LoadingBar(30, "restart application", 1, 3);
            Console.Clear();
            StartBot();
        }
    }

    public void Test() // TEST METHOD
    {
        var currency = "BTCUSDT";
        var amount = 0.01m;
        var currentPrice = _client.GetCurrencyPrice(currency);

        var orderLog = new OrderLog(
            options: _currencyInfo, sellPrice: currentPrice, amount: amount);

        _botLogger.AddOrderInfoGlobalLog(orderLog);
        LoadingBar(15, "sell coins");
    }
    
    private void WriteErrorLog(string message)
    {
        var errorLog = new ErrorLog(message);
        _botLogger.AddErrorLog(errorLog);
        Console.WriteLine(errorLog);
    }

    private static void LoadingBar(int sec, string text = "updating data", int x = 1, int y = 5)
    {
        const string border = "\u2551";
        const int max = 20;
        var thrSleep = sec * 1000 / max;
        var empty = new string(' ', max);
        
        Console.CursorVisible = false;
        Console.SetCursorPosition(1, 1);
        
        for (var i = 0; i < max; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;// bar color
            Console.SetCursorPosition(x, y);
        
            for (var j = 0; j < i; j++)
            {
                Console.Write(border);
            }
            
            Console.Write(empty + (i + 1) + $" / {max} {text}...");
            empty = empty.Remove(empty.Length - 1);
            Thread.Sleep(thrSleep);
        }

        Console.ForegroundColor = ConsoleColor.White;
    }
}