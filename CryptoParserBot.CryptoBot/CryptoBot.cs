using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoParserBot.ExchangeClient_s.Interfaces;

namespace CryptoParserBot.CryptoBot
{
    public sealed class CryptoBot
    {
        private IExchangeClient _client;

        private decimal _priceLimit;// example: 24500$   (write user)
        private string _firstCoin;// btc
        private string _secondCoin;// usdt

        public CryptoBot(IExchangeClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Starts parsing prices from the exchange
        /// and sells coins if the price has reached the required limit
        /// </summary>
        public void StartParsing()// start command
        {
            var currency = _firstCoin + _secondCoin;

            while (true)
            {
                Thread.Sleep(5000);

                var totalPrice = _client.GetCurrencyPrice(currency);

                if (totalPrice <= _priceLimit) continue;

                var balances =
                    _client.GetAccountBalance().FirstOrDefault(x => x.Currency == "BTC");

                var order = _client.CreateSellOrder(currency, balances.AvailableBalance, totalPrice);
            }
        }
    }
}
