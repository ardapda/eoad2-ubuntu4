namespace ApiResponse.Binance
{
	public class OrderResponse_check
    {
        /*

   order  {'symbol': 'BTCUSDT', 'orderId': 1579117860, 'orderListId': -1, 'clientOrderId': 'bgcgxMnQbLLQkKFLUAhoDN', 
   'price': '6000.00000000', 'origQty': '0.00370000', 'executedQty': '0.00000000', 
   'cummulativeQuoteQty': '0.00000000', 'status': 'NEW', 'timeInForce': 'GTC', 'type': 'LIMIT', 
   'side': 'BUY', 'stopPrice': '0.00000000', 'icebergQty': '0.00000000', 'time': 1584830340061,
   'updateTime': 1584830340061, 'isWorking': True, 'origQuoteOrderQty': '0.00000000'}
error check



res	"{'symbol': 'BTCUSDT', 'orderId': 1580101959, 'orderListId': -1, 'clientOrderId': 'te78G8jSFfiPWgOJdcyKBZ', 'price': '5800.00000000', 'origQty': '0.00340000', 'executedQty': '0.00000000', 'cummulativeQuoteQty': '0.00000000', 'status': 'NEW', 'timeInForce': 'GTC', 'type': 'LIMIT', 'side': 'BUY', 'stopPrice': '0.00000000', 'icebergQty': '0.00000000', 'time': 1584838215129, 'updateTime': 1584838215129, 'isWorking': True, 'origQuoteOrderQty': '0.00000000'}\r\n"	string

*/

        public string symbol
        {
			get;
			set;
		}
        
		public int orderId
        {
			get;
			set;
		}
		public int orderListId
        {
			get;
			set;
		}

        public string clientOrderId
        {
            get;
            set;
        }
        public string price
        {
            get;
            set;
        }
        public string origQty
        {
            get;
            set;
        }
        public string executedQty
        {
            get;
            set;
        }
        public string cummulativeQuoteQty
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
        public string stopPrice
        {
            get;
            set;
        }
        public string icebergQty
        {
            get;
            set;
        }
        public long time
        {
            get;
            set;
        }
        public long updateTime
        {
            get;
            set;
        }
        public bool isWorking
        {
            get;
            set;
        }
        public string origQuoteOrderQty
        {
            get;
            set;
        }
    }
}
