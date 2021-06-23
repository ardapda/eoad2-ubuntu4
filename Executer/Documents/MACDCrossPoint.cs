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
    public class MACDCrossPoint
    {
        public MACDCrossPoint()
        { }
        public DateTime DateTime { get; set; }
        public decimal MACD { get; set; }
        public decimal Signal { get; set; }
        public decimal Histogram { get; set; }
        public int MACDColor { get; set; }
        public int SignalColor { get; set; }
        public int HistogramColor { get; set; }
        public int CrossColor { get; set; }
        public decimal CrossSignalPosition { get; set; }
        public decimal CurrentValue { get; set; } //Close
    }
}
