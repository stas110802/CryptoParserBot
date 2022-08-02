using CryptoParserBot.ExchangeClients.Models.NiceHash;

namespace CryptoParserBot.ExchangeClients.Interfaces;

public interface IExchangeClient
{
    /// <summary>
    /// Returns information about a currency pair
    /// </summary>
    /// <param name="currency">Currency pair</param>
    /// <returns></returns>
    public CurrencyPair GetCurrencyInfo(string currency);

    /// <summary>
    /// Returns the current trading value of a currency
    /// </summary>
    /// <param name="currency">Currency pair</param>
    /// <returns></returns>
    public decimal GetCurrencyPrice(string currency);

    /// <summary>
    /// Returns a list of coins and their amount on the account
    /// </summary>
    /// <returns></returns>
    public List<CurrencyBalance> GetAccountBalance();

    /// <summary>
    /// Returns the currency balance on the account
    /// </summary>
    /// <param name="currency">Solo currency</param>
    /// <returns></returns>
    public CurrencyBalance GetCurrencyBalance(string currency);

    /// <summary>
    /// Creates an order to sell a currency pair
    /// </summary>
    /// <param name="currency">CurrencyPair pair</param>
    /// <param name="amount">Amount to sell</param>
    /// /// <param name="price">Selling price</param>
    /// <returns></returns>
    public bool CreateSellOrder(string currency, decimal amount, decimal price);

    /// <summary>
    /// Create a MARKET order to sell a currency pair
    /// </summary>
    /// <param name="currency">CurrencyPair pair</param>
    /// <param name="amount">Amount to sell</param>
    /// <returns></returns>
    public bool CreateSellOrder(string currency, decimal amount);
}

