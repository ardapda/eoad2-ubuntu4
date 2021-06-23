namespace ApiResponse.Binance
{
	public class OrderResponse_asset
    {
        /*

   status:  {'asset': 'FET', 'free': '500.31440000', 'locked': '0.00000000'}
*/

        public string asset
        {
			get;
			set;
        }
        public double free
        {
            get;
            set;
        }
        public double locked
        {
            get;
            set;
        }
    }
}
