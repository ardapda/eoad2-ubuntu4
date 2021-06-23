using MongoDB.Bson;

namespace Executer.Enums
{    
    public enum WorkStatus : int
    {        
        buy0 = 100,//buy init
        buy1 = 101,//sell 1.
        buy2 = 102,//sell 2.
        buy3 = 103,//sell 3.
        buy4 = 104,//sell 4. last
        init = 0,
        sell0 = 200,//sell init
        sell1 = 201,//buy 1.
        sell2 = 202,//buy 2.
        sell3 = 203,//buy 3.
        sell4 = 204,//buy 4. last

        executersettingupdate = 300,

        check_stoploss = 400
    }
}