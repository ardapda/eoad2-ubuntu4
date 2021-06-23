using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum Rule : int
    {
        BuyRule1 = 1,
        BuyRule2 = 2,
        BuyRule3 = 3,
        BuyRule4 = 4,
        BuyRule5 = 5,
        
        Idle = 0,

        SellRule1 = -1,
        SellRule2 = -2,
        SellRule3 = -3,
        SellRule4 = -4,
        SellRule5 = -5
    }
}