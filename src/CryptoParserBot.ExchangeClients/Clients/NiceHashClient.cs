using System.Globalization;
using CryptoParserBot.ExchangeClients.Interfaces;
using CryptoParserBot.ExchangeClients.Models.NiceHash;
using CryptoParserBot.RestApi.Api;
using CryptoParserBot.RestApi.Enums.NiceHash;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CryptoParserBot.ExchangeClients.Clients
{
    public sealed class NiceHashClient : IExchangeClient
    {
        private readonly NiceHashAPI _api;

        private readonly string _uri;
        private readonly string _organizationId;
        private readonly string _key;
        private readonly string _secretKey;

        public NiceHashClient(string key, string secretKey, string organizationId)
        {
            _uri = "https://api2.nicehash.com";
            _key = key;
            _organizationId = organizationId;
            _secretKey = secretKey;

            _api = new NiceHashAPI(_uri, _organizationId, _key, _secretKey);
        }

        public CurrencyPair GetCurrencyInfo(string currency)
        {
            var response = _api.GetResponseContent(Method.Get, NHEndpoint.CurrentPrices);
            var deserialize = JsonConvert.DeserializeObject<JToken>(response);

            foreach (var item in deserialize)
            {
                if (currency != item.Path) continue;

                var currencyPair = new CurrencyPair
                {
                    Currency = currency
                };

                var isSuccessfully = decimal.TryParse(item.First.ToString(), out decimal price);

                if (isSuccessfully == false)
                    throw new Exception("[Parse ERROR] : Cannot parse the selling price");

                currencyPair.SellingPrice = price;

                return currencyPair;
            }

            throw new ArgumentException("[Argument ERROR] : Cannot find the specified currency");
        }

        public decimal GetCurrencyPrice(string currency)
        {
            var result = GetCurrencyInfo(currency);

            return result.SellingPrice;
        }

        public List<CurrencyBalance> GetAccountBalance()
        {
            var response = _api.GetResponseContent(Method.Get, NHEndpoint.Balances, true);
            var parse = JObject.Parse(response);
            var currencies = parse.SelectToken("currencies");

            if (currencies == null)
            {
                throw new JsonException("[SelectToken ERROR] : Unable to deserialize response and get currencies");
            }

            var result = JsonConvert.DeserializeObject<List<CurrencyBalance>>(currencies.ToString());

            return result;
        }

        // todo replace to api method
        public CurrencyBalance GetCurrencyBalance(string currency)
        {
            // get all the balance currency on the exchange
            var allCurrencies = GetAccountBalance();

            // find a necessary currency
            foreach (var item in allCurrencies)
            {
                if (item.Currency == currency)
                {
                    return item;
                }
            }

            // if not found, returns an exception
            throw new ArgumentException("[Argument ERROR] : Invalid currency name");
        }

        // todo fix this method
        public bool CreateSellOrder(string currency, decimal amount, decimal price)
        {
            var strPrice = price.ToString(CultureInfo.InvariantCulture);
            var strQuantity = amount.ToString(CultureInfo.InvariantCulture);

            // create post url
            var query =
                $"?market={currency}&side=SELL&type=LIMIT&quantity={strQuantity}&price={strPrice}";

            // create an order and deserialize the received response
            var response = _api.GetResponseContent(Method.Post, NHEndpoint.Order, true, query, true);
            var deserialize = JsonConvert.DeserializeObject<JToken>(response);

            // if the order id is not empty, we have created an order
            return deserialize?["orderId"]?.ToString() != "";
        }

        public bool CreateSellOrder(string currency, decimal amount)
        {
            var strQuantity = amount.ToString(CultureInfo.InvariantCulture);

            // create post url
            var query =
                $"?market={currency}&side=SELL&type=MARKET&quantity={strQuantity}";

            // create an order and deserialize the received response
            var response = _api.GetResponseContent(Method.Post, NHEndpoint.Order, true, query, true);
            var deserialize = JsonConvert.DeserializeObject<JToken>(response);

            // if the order id is not empty, we have created an order
            return deserialize?["orderId"]?.ToString() != "";
        }
    }
}
