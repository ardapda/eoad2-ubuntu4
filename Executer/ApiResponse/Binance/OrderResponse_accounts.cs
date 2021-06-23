using System.Collections.Generic;

namespace ApiResponse.Binance
{
    public class OrderResponse_accounts
    {

        /*
         
{'assets': 
[
{
  'baseAsset':  {'asset': 'FET', 'borrowEnabled': True, 'borrowed': '0', 'free': '1463.4144', 'interest': '0', 'locked': '491.3', 'netAsset': '1954.7144', 'netAssetOfBtc': '0.02088936', 'repayEnabled': True, 'totalAsset': '1954.7144'}, 
  'quoteAsset': {'asset': 'USDT', 'borrowEnabled': True, 'borrowed': '889.66509394', 'free': '20.65536756', 'interest': '0.05560407', 'locked': '0', 'netAsset': '-869.06533045', 'netAssetOfBtc': '-0.01545198', 'repayEnabled': True, 'totalAsset': '20.65536756'}, 
  'symbol': 'FETUSDT', 'isolatedCreated': True, 'marginLevel': '1.34371923', 'marginLevelStatus': 'EXCESSIVE', 'marginRatio': '4.2', 'indexPrice': '0.60221962', 'liquidatePrice': '0.51628838', 'liquidateRate': '-14.269087', 'tradeEnabled': True
}
,  
{ 
  'baseAsset':  {'asset': 'BNB', 'borrowEnabled': True, 'borrowed': '0', 'free': '0', 'interest': '0', 'locked': '0', 'netAsset': '0', 'netAssetOfBtc': '0', 'repayEnabled': True, 'totalAsset': '0'}, 
  'quoteAsset': {'asset': 'USDT', 'borrowEnabled': True, 'borrowed': '0', 'free': '0', 'interest': '0', 'locked': '0', 'netAsset': '0', 'netAssetOfBtc': '0', 'repayEnabled': True, 'totalAsset': '0'}, 
  'symbol': 'BNBUSDT', 'isolatedCreated': True, 'marginLevel': '999', 'marginLevelStatus': 'EXCESSIVE', 'marginRatio': '10', 'indexPrice': '381.47869192', 'liquidatePrice': '0', 'liquidateRate': '0', 'tradeEnabled': True
}
,

         * */
        public List<fills> assets
        {
            get;
            set;
        }


    }


    public class assets
    {
        public List<fills> baseAsset
        {
            get;
            set;
        }
        public List<fills> quoteAsset
        {
            get;
            set;
        }
        //'symbol': 'FETUSDT', 'isolatedCreated': True, 'marginLevel': '1.34371923', 
        //'marginLevelStatus': 'EXCESSIVE', 'marginRatio': '4.2', 'indexPrice': '0.60221962', 
        //'liquidatePrice': '0.51628838', 'liquidateRate': '-14.269087', 'tradeEnabled': True
        public string price
        {
            get;
            set;
        }
        public bool isolatedCreated
        {
            get;
            set;
        }
        public double marginLevel
        {
            get;
            set;
        }
        public string EXCESSIVE
        {
            get;
            set;
        }
        public bool marginRatio
        {
            get;
            set;
        }
        public double indexPrice
        {
            get;
            set;
        }
        public double liquidatePrice
        {
            get;
            set;
        }
        public double liquidateRate
        {
            get;
            set;
        }
        public bool tradeEnabled
        {
            get;
            set;
        }
    }
    public class baseAsset
    {
        //'baseAsset': 
        //{'asset': 'FET', 'borrowEnabled': True, 'borrowed': '0', 'free': '1463.4144', 'interest': '0',
        //'locked': '491.3', 'netAsset': '1954.7144', 'netAssetOfBtc': '0.02088936', 'repayEnabled': True,
        //'totalAsset': '1954.7144'}, 

        public string asset
        {
            get;
            set;
        }
        public bool borrowEnabled
        {
            get;
            set;
        }
        public double borrowed
        {
            get;
            set;
        }
        public double free
        {
            get;
            set;
        }
        public double interest
        {
            get;
            set;
        }
        public double locked
        {
            get;
            set;
        }
        public double netAsset
        {
            get;
            set;
        }
        public double netAssetOfBtc
        {
            get;
            set;
        }
        public bool repayEnabled
        {
            get;
            set;
        }
        public double totalAsset
        {
            get;
            set;
        }
    }

    public class quoteAsset
    {
        //   'quoteAsset': {'asset': 'USDT', 'borrowEnabled': True, 'borrowed': '889.66509394', 
        //'free': '20.65536756', 'interest': '0.05560407', 'locked': '0', 'netAsset': '-869.06533045', 
        //'netAssetOfBtc': '-0.01545198', 'repayEnabled': True, 'totalAsset': '20.65536756'}, 

        public string asset
        {
            get;
            set;
        }
        public bool borrowEnabled
        {
            get;
            set;
        }
        public double borrowed
        {
            get;
            set;
        }
        public double free
        {
            get;
            set;
        }
        public double interest
        {
            get;
            set;
        }
        public double locked
        {
            get;
            set;
        }
        public double netAsset
        {
            get;
            set;
        }
        public double netAssetOfBtc
        {
            get;
            set;
        }
        public bool repayEnabled
        {
            get;
            set;
        }
        public double totalAsset
        {
            get;
            set;
        }
    }
    
}
