namespace ApiResponse.Bitrue
{
	public class OrderResponse_check
    {
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
    }
}
