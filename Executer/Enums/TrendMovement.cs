using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum TrendMovement : int
    {
        Bullish = 1,//lime
        Stable = 0,
        Bearish = -1//red
    }
}