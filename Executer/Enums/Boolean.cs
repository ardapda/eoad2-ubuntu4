using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum Boolean : int
    {
        _true = 1,
        _unset = 0,
        _false = -1
    }
}