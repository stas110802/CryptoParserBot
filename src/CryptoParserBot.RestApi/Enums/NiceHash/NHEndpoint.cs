using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoParserBot.RestApi.Enums.NiceHash
{
    public enum NHEndpoint
    {
        [Description("/api/v2/time")] ServerTime,
        [Description("/main/api/v2/accounting/accounts2")] Balances,
        [Description("/exchange/api/v2/info/prices")] CurrentPrices,
        [Description("/exchange/api/v2/order")] Order,
        [Description("/exchange/api/v2/info/myOrders")] MyOrders
    }
}
