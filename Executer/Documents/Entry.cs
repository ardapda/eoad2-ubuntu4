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
    //abstract public class Entry
    public class Entry
    {
        public Entry()
        {        }
        public Entry(BsonDocument doc)
        {
            this.doc = doc;
        }
        public BsonDocument doc { get; set; }
    }
}
