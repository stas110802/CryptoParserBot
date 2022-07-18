using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoParserBot.CryptoBot
{
    public sealed class CryptoBotOptions
    {
        public string FirstCoin { get; set; }

        public string SecondCoin { get; set; }

        public decimal PriceLimit { get; set; }
    }
}
