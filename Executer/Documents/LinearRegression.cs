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
    public class LinearRegression
    {
        public LinearRegression()
        { }
        public DateTime DateTime { get; set; }
        public decimal rsquared { get; set; }
        public decimal yintercept { get; set; }
        public decimal slope { get; set; }
        public decimal histogram { get; set; }
    }
}
