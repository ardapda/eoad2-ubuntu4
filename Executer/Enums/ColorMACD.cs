using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum ColorMACD : int
    {
        YellowSignal = 9,
        GreenSignal = 3,
        Aqua = 2,
        Blue = 1,
        Grey = 0,
        RedDark = -1,
        RedLight = -2,
        RedSignal = -3
    }
}