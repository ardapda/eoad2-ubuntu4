namespace Executer.Enums
{    
    public enum Flipper : int
    {
        Init = 0,
        ReadSettings = 1,
        ReadInputMultiplier = 2,
        ResetOpenOrdersAtTheBeginning = 3,
        ReadDBAsync = 4,
        ReadCountOfOpenSlotsAsync = 5,
        ResetAllOpenOrders = 6,
        ResetAllOpenSellOrders110 = 7,
        ResetAllOpenSellOrders2234 = 8,
        ReadOpenLoanBalance = 9,
        CreateAPIKeys = 10,
        SetAPIKeys = 11,

        ReadExchangeOpenOrdersAsync_xrp = 1201,
        ReadExchangeAPIOpenOrdersAsync_xrp = 1301,
        ReadExchangeOpenOrdersAsync_btc = 1202,
        ReadExchangeAPIOpenOrdersAsync_btc = 1302,
        ReadExchangeOpenOrdersAsync_eth = 1203,
        ReadExchangeAPIOpenOrdersAsync_eth = 1303,
        ReadExchangeOpenOrdersAsync_link = 1204,
        ReadExchangeAPIOpenOrdersAsync_link = 1304,
        ReadExchangeOpenOrdersAsync_yfi = 1205,
        ReadExchangeAPIOpenOrdersAsync_yfi = 1305,


        UpdateExchangeStatus = 14,
        UpdateStatistics = 15,
        Reset_100_1 = 16,
        Reset_180_1 = 17,
        Reset_3000_1 = 18,
        Reset_235_1 = 19,
        Reset_200_1 = 20,
        DeleteDB = 21,
        UpdateTradeInfoDB_1 = 22, 
        Reset_0_1 = 23,
        Reset_0_0 = 24, // exit app after 3 status 0, then after restart externally
        AdjustAmountPerTrade = 25
    }
}