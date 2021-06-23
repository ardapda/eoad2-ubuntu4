using Executer.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executer.Documents
{
    public class MACDIndicator
    {
        public MACDIndicator()
        { }
        public DateTime DateTime { get; set; }
        public int SlaveNumber { get; set; }
        public decimal MACD { get; set; }
        public decimal Signal { get; set; }
        public decimal Histogram { get; set; }
        public int MACDColor { get; set; }
        public int SignalColor { get; set; }
        public int HistogramColor { get; set; }
        public MACDCrossPoint CrossPoint { get; set; }
        public decimal CurrentValue { get; set; } //Close
    }
}
