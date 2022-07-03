using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoParserBot.ExchangeClient_s.Models.NiceHash
{
    public sealed class CurrencyPair
    {
        public string Currency { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal TradingVolume { get; set; }
    }
}
