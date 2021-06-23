using System.Collections.Generic;

namespace ApiResponse.Binance
{
	public class OrderResponse_buysell
    {
        //res "{'symbol': 'FETUSDT', 'orderId': 238962040, 'clientOrderId': 'kpXHQru29s78Tnnn09APCc', 'transactTime': 1617812815359, 'price': '0',          'origQty': '40',         'executedQty': '40',            'cummulativeQuoteQty': '24.0232', 'status': 'FILLED', 'timeInForce': 'GTC', 'type': 'MARKET', 'side': 'BUY',       'fills': [{'price': '0.60058', 'qty': '40',         'commission': '0.04',       'commissionAsset': 'FET'}], 'isIsolated': true}"
        //     {'symbol': 'FETUSDT', 'orderId': 240339260, 'clientOrderId': 'EZJewNLleo2DRPTnv1CAiC', 'transactTime': 1617886243525, 'price': '0.00000000', 'origQty': '40.00000000', 'executedQty': '40.00000000', 'cummulativeQuoteQty': '25.30720000', 'status': 'FILLED', 'timeInForce': 'GTC', 'type': 'MARKET', 'side': 'SELL', 'fills': [{'price': '0.63268000', 'qty': '40.00000000', 'commission': '0.02530720', 'commissionAsset': 'USDT', 'tradeId': 15353455}]}

        public string symbol
        {
			get;
			set;
        }
        public string orderId
        {
            get;
            set;
        }
        public string orderListId
        {
            get;
            set;
        }
        public string clientOrderId
        {
            get;
            set;
        }        
        public string transactTime
        {
            get;
            set;
        }
        public double price
        {
            get;
            set;
        }
        public double origQty
        {
            get;
            set;
        }
        public double executedQty
        {
            get;
            set;
        }
        public double cummulativeQuoteQty
        {
            get;
            set;
        }
        public string status
        {
            get;
            set;
        }
        public string timeInForce
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }
        public string side
        {
            get;
            set;
        }   
        public List<fills> fills      
        {
            get;
            set;
        }
    }
    public class fills
    {
        /*
'fills': [{'price': '6145.41000000', 'qty': '0.00330000', 'commission': '0.00127000', 'commissionAsset': 'BNB', 'tradeId': 275772536}]}"	string
'fills': [{'price': '0.60058', 'qty': '40', 'commission': '0.04', 'commissionAsset': 'FET'}],
*/

        public double price
        {
            get;
            set;
        }

        public double qty
        {
            get;
            set;
        }
        public double commission
        {
            get;
            set;
        }
        public string commissionAsset
        {
            get;
            set;
        }
        public string tradeId
        {
            get;
            set;
        }        
    }
}
