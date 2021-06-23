using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
namespace Executer.Workers
{
    class WorkerOnline : Worker
    {
        protected static IMongoDatabase _db = null;
        private DateTime TimeLastRun = DateTime.Parse("01-01-2000 00:00:00");
        public const int WaitingTimeInSeconds = 300; // every 5 mins
        public WorkerOnline()
        { }
        public override void DoWork2()
        { }
        public override void DoWork3()
        { }
        public override void DoWork()
        {
            while (_isAlive)
            {
                if (TimeLastRun.AddSeconds(WaitingTimeInSeconds) <= DateTime.UtcNow)
                {
                    UpdateOnlineAsync().Wait();
                    TimeLastRun = DateTime.UtcNow;
                }
            }
        }


        protected async Task UpdateOnlineAsync()
        {
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("online");
            if (c != null)
            {
                var filter = "{ bin: '<your_binary_to_be_checked_here>' }";
                try
                {
                    var docs = await c.Find(filter).ToListAsync();
                    if (docs.Count == 1)
                    {
                        foreach (var doc in docs)
                        {
                            doc["lastactiontime"] = DateTime.UtcNow;
                            var result = await c.ReplaceOneAsync(
                                item => item["_id"] == doc["_id"],
                                doc,
                                new UpdateOptions { IsUpsert = true });
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public void ConnectDB()
        {
            var connectionString = MongoClientSettings.FromUrl(MongoUrl.Create("<your_binary_online_db_here>"));
            MongoClient client = new MongoClient(connectionString);
            _db = client.GetDatabase("<your_collection_d_here>");
        }
    }
}
