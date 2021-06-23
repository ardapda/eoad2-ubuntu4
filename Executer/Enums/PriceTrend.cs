using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum PriceTrend : int
    {
        upfastfast = 4,
        upfast = 3,
        up = 2,
        upslow = 1,

        stable = 0,

        downslow = -1,
        down = -2,
        downfast = -3,
        downfastfast = -4
    }
}