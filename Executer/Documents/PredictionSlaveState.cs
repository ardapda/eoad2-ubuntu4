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
    public class PredictionSlaveState
    {
        public PredictionSlaveState()
        { }
        public DateTime SignalSlaveToMaster { get; set; }
        public int SlaveNumber { get; set; }
        public int PriceTrend { get; set; } //Enums.PriceTrend.cs
        public decimal CurrentPrice { get; set; } //Close
        public decimal PredictedPrice { get; set; } //NOT active yet, slave decides on its own
        public DateTime PredictedPriceForDateTime { get; set; }//NOT active yet
        //public bool LastPredictionActive { get; set; }//NOT active yet
    }
}
