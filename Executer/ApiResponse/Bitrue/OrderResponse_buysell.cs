namespace ApiResponse.Bitrue
{
	public class OrderResponse_buysell
    {
       // result	"{'symbol': 'BTCUSDT', 'orderId': 110143040, 'clientOrderId': '', 'transactTime': 1582076754513}
        public string symbol
        {
			get;
			set;
		}

		public long orderId
        {
			get;
			set;
		}

		public string clientOrderId
        {
			get;
			set;
		}

		public long transactTime
        {
			get;
			set;
		}
        public string side
        {
            get;
            set;
        }
    }
}
