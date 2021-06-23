namespace ApiResponse.FTX
{
	public class OrderResponse_accounts
    {
        //		res	"{'availableWithoutBorrow': 0.0, 'coin': 'ETH', 'free': 0.0, 
        //'spotBorrow': 0.0, 'total': 41.5161866, 'usdValue': 30642.667381373667}# 
        
        public double availableWithoutBorrow
        {
            get;
            set;
        }
        public string coin
        {
            get;
            set;
        }
        public double free
        {
            get;
            set;
        }        
        public double spotBorrow
        {
            get;
            set;
        }
        public double total
        {
            get;
            set;
        }
        public double usdValue
        {
            get;
            set;
        }
    }
}
