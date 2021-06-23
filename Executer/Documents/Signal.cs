using System;

namespace Executer.Documents
{
    public class Signal
    {
        public Signal()
        { }
        public int SearchingSell { get; set; }
        public int Sell { get; set; }
        public int SearchingBuy { get; set; }
        public int Buy { get; set; }
        public decimal DeltaHistograme { get; set; } //Delta of previous one
    }
}
