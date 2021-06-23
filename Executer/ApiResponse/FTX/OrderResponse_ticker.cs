using System.Collections.Generic;

namespace ApiResponse.FTX
{
    public class OrderResponse_ticker
    {
        public List<List<decimal>> asks { get; set; }

        public List<List<decimal>> bids { get; set; }

 
    }
}
