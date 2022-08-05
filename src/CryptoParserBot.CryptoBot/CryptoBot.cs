using CryptoParserBot.AdditionalToolLibrary;
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

    public CryptoBot(IExchangeClient client, CurrencyInfo currencyInfo)
    {
        _client = client;
        _currencyInfo = currencyInfo;
        _botLogger = new BotLogger
        {
            RecipientsEmails = new[]
            {
                "baxtoban555308775@gmail.com",
                //"Roman1199@mail.ru"
            },
            SmtpSender = new SmtpSender(ConfigInitializer.GetSmtpEmailConfig())
        };
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
                
                var parseLog = new CurrencyLog(_currencyInfo, currentPrice);
                Console.WriteLine(parseLog);

                if (currentPrice <= _currencyInfo.UpperPrice &&
                    currentPrice >= _currencyInfo.BottomPrice)
                {
                    ConsoleHelper.LoadingBar(10);
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
                    ConsoleHelper.LoadingBar(10);
                    continue;
                }

                var amount = balances.AvailableBalance;

                // create sell order
                var orderResult = _client.CreateSellOrder(currency, amount);// MARKET ORDER
                
                if (orderResult)
                {
                    var orderLog = new OrderLog(
                    options: _currencyInfo, sellPrice: currentPrice, amount: amount);

                    _botLogger.AddLog(orderLog);
                    ConsoleHelper.LoadingBar(15, "sell coins");
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
            RestartBot(error.Message);
        }
    }

    public void TestSellCurrency() // TEST METHOD
    {
        var currency = "BTCUSDT";
        var amount = 0.01m;
        var currentPrice = _client.GetCurrencyPrice(currency);

        var orderLog = new OrderLog(
            options: _currencyInfo, sellPrice: currentPrice, amount: amount);

        _botLogger.AddLog(orderLog);
        ConsoleHelper.LoadingBar(10, "sell coins");
    }

    public void TestError()
    {
        var log = new ErrorLog("Тест ошибка!");
        _botLogger.AddLog(log);
    }
    
    private void WriteErrorLog(string message)
    {
        var errorLog = new ErrorLog(message);
        _botLogger.AddLog(errorLog);
        Console.WriteLine(errorLog);
    }

    private void RestartBot(string? error = null)
    {
        Console.Clear();
        
        if(string.IsNullOrEmpty(error) is false)
            WriteErrorLog($"Сбой работы бота. {error}");
        
        ConsoleHelper.LoadingBar(30, "restart application", 1, 3);
        Console.Clear();
        
        StartBot();
    }
}