using MongoDB.Bson;

namespace Executer.Enums
{

    public enum  Currency : int
    {
        BNB = 0,
        BTC,
        ETH,
        XRP,
        USDT,
        TUSD,
        PAX,
        USDC,

        NOTSETYET = -1,
        UNKNOWN = 999999999
    }
}

