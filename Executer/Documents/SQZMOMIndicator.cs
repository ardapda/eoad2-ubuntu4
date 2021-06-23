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
    public class SQZMOMIndicator
    {
        public SQZMOMIndicator()
        { }
        public int SlaveNumber { get; set; }
        public DateTime DateTime { get; set; }
        public LinearRegression LinearRegression { get; set; }        
        public int LineColor { get; set; }
        public int SqueezeColor { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
