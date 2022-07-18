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
        private CryptoBotOptions _options;

        public CryptoBot(IExchangeClient client)
        {
            _client = client;
        }

        public CryptoBot(IExchangeClient client, CryptoBotOptions options) 
            : this(client)
        {
            _options = options;
        }

        /// <summary>
        /// Starts parsing prices from the exchange
        /// and sells coins if the price has reached the required limit
        /// </summary>
        public void StartParsing()// start command
        {
            var currency =
                _options.FirstCoin + _options.SecondCoin;

            while (true)
            {
                Thread.Sleep(5000);

                var totalPrice = _client.GetCurrencyPrice(currency);

                if (totalPrice <= _options.PriceLimit)
                    continue;

                var balances =
                    _client.GetAccountBalance().FirstOrDefault(x => x.Currency == "BTC");

                var order = _client.CreateSellOrder(currency, balances.AvailableBalance, totalPrice);
            }
        }
    }
}
