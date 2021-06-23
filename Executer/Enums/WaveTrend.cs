using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum WaveTrend : int
    {
        sell = 2,
        searchingsell = 1,
        stable = 0,
        searchingbuy = -1,
        buy = -2
    }
}