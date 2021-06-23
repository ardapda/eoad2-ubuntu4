using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum OrderStatus : int
    {
        LongClosedSell = 4,
        LongOpenedSell = 3,
        LongClosedBuy = 2,
        LongOpenedBuy = 1,
        Unset = 0,
        ShortOpenedSell = -1,
        ShortClosedSell = -2,
        ShortOpenedBuy = -3,
        ShortClosedBuy = -4
    }
}