using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum ColorSqzMom : int
    {
        Black = 3,//SqzOn
        GreenLight = 2,
        GreenDark = 1,
        Blue = 0, //NoSqz
        RedDark = -1,
        RedLight = -2,
        Grey = -3//SqzOff
    }
}