using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.CryptoBot.Logs;
using CryptoParserBot.CryptoBot.Models.Configs;
using CryptoParserBot.CryptoBot.Models.Logs;
using CryptoParserBot.ExchangeClients.Interfaces;
using CryptoParserBot.ExchangeClients.Models.NiceHash;

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
        ConsoleHelper.BeautifyWrite("Запуск бота", 1);
        var currency = _currencyInfo.FirstCoin + _currencyInfo.SecondCoin;
        
        try
        {
            var launchLog = GetTotalCurrencyInfo(currency);
            _botLogger.AddLog(launchLog);
            Thread.Sleep(2000);
            
            while (true)
            {
                var balance = _client.GetAccountBalance()
                    .First(x => x.Currency == _currencyInfo.FirstCoin);
                
                var parseLog = GetTotalCurrencyInfo(currency, balance);
                Console.WriteLine(parseLog);
                
                var currentPrice = parseLog.TotalPrice;
                if (currentPrice <= _currencyInfo.UpperPrice &&
                    currentPrice >= _currencyInfo.BottomPrice)
                {
                    ConsoleHelper.LoadingBar(10);
                    continue;
                }

                // if the balance is not active, then do not create an order
                if (balance!.IsActive is false)
                {
                    WriteErrorLog("Заброкированнный баланс!");
                    continue;
                }

                var amount = balance.AvailableBalance;
                if (amount < _currencyInfo.BalanceLimit)
                {
                    ConsoleHelper.LoadingBar(10);
                    continue;
                }

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

    public void TestSellCurrency()// test DELETE
    {
        var currency = "BTCUSDT";
        var amount = 0.016542m;
        var currentPrice = _client.GetCurrencyPrice(currency);

        var orderLog = new OrderLog(
            options: _currencyInfo, sellPrice: currentPrice, amount: amount);

        _botLogger.AddLog(orderLog);
        ConsoleHelper.LoadingBar(10, "sell coins");
    }

    public void TestError()// test DELETE
    {
        var log = new ErrorLog("Тест ошибка!");
        _botLogger.AddLog(log);
    }

    public void TestStart()// test DELETE
    {
        var currency = "BTCUSDT";
        var price = _client.GetCurrencyPrice(currency);
        var balance = 0.016542m;
        var log = new CurrencyLog(_currencyInfo, price, balance);
        _botLogger.AddLog(log);
    }

    private CurrencyLog GetTotalCurrencyInfo(string currency, CurrencyBalance? balance = null)
    {
        Console.Clear();
        var price = _client.GetCurrencyPrice(currency); 
        balance ??= _client
            .GetAccountBalance()
            .First(x => x.Currency == _currencyInfo.FirstCoin);
        var log = new CurrencyLog(_currencyInfo, price, balance.AvailableBalance);

        return log;
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