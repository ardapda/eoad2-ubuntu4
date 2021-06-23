using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Executer.Enums;

namespace Executer.Workers
{
    class WorkerTVSignals : WorkerDatabase
    {
        private static bool _onlyOnce { get; set; }    
        public WorkerTVSignals()
        {
            _onlyOnce = false;
        }
        
        public override void DoWork()
        {
            while (IsAlive)
            {

                if (_onlyOnce == false)
                {
                    //reset all status = 0 at the beginning
                    _onlyOnce = true;
                    OnlyOnceAtTheBeginning();
                    
                    continue;
                }
                if (IsOnline == (int)Enums.Boolean._true && _onlyOnce == true)
                {
                    DistributeEntry();
                }
                else if (IsOnline == (int)Enums.Boolean._false)
                {
                    Status9000Offline();
                }
                Cleaner();
            }
            return;
        }
        protected void Status9000Offline()
        {
            try
            {
                IMongoCollection<BsonDocument> c;
                c = _db.GetCollection<BsonDocument>("<your_signal_db_here>");
                if (c != null)
                {
                    var filter = "{$and : [ {status : 0}, {alerttime: {'$ne':null}} ]}";
                    var docs = c.Find(filter).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs.Result.Count > 0)
                    {
                        foreach (var doc in docs.Result)
                        {
                            doc["status"] = (int)StateMaschine.error_at_the_beginning_of_app;
                            UpdateDBAsync(doc).Wait();
                        }
                        Console.WriteLine(DateTime.UtcNow + "\t" + "error_at_the_beginning_of_app");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        protected new void OnlyOnceAtTheBeginning()
        {
            try
            {
                IMongoCollection<BsonDocument> c;
                c = _db.GetCollection<BsonDocument>("<your_signal_db_here>");
                if (c != null)
                {
                    var filter = "{internal_task : 0}";
                    var docs = c.Find(filter).ToListAsync();

                    if (docs.Result.Count > 0)
                    {
                        foreach (var doc in docs.Result)
                        {
                            doc["status"] = (int)StateMaschine.skip_at_the_beginning_of_app;
                            UpdateDBAsync(doc).Wait();
                        }
                    }
                }


                c = _db.GetCollection<BsonDocument>("<your_order_db_here>");
                if (c != null)
                {
                    var filter = "{status : 0}";
                    var docs = c.Find(filter).ToListAsync();

                    if (docs.Result.Count > 0)
                    {
                        foreach (var doc in docs.Result)
                        {
                            doc["status"] = (int)StateMaschine.skip_at_the_beginning_of_app;
                            UpdateDBAsyncOrders(doc).Wait();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }        
        protected void DistributeEntry()
        {
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("<your_signal_db_here>");
            if (c != null)
            {
                try
                {
                    //var filter = "{status : 0}";
                    //var filter = "{ status : 0, $and : [ {rule : {'$ne':'-4'}}, {rule: {'$ne':'4'}} ]}";
                    //var filter = "{ status : 0, alerttime : {'$ne':'null'}}";
                    var filter = "{$and : [ {status : 0}, {alerttime: {'$ne':null}} ]}";
                    var docs = c.Find(filter).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs.Result.Count > 0)
                    {
                        BsonDocument _doc = docs.Result.ElementAt(0);
                        UpdateDBAsyncNew(_doc).Wait();
                        _doc["status"] = 1;
                        UpdateDBAsync(_doc).Wait();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public new static async Task UpdateDBAsync(BsonDocument d)
        {
            BsonDocument doc = d;
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("<your_signal_db_here>");
            if (c != null)
            {
                doc["lastactiontime"] = DateTime.UtcNow;
                var result = await c.ReplaceOneAsync(
                    item => item["_id"] == doc["_id"],
                    doc,
                    new UpdateOptions { IsUpsert = true });
            }
        }
        public static async Task UpdateDBAsyncOrders(BsonDocument d)
        {
            BsonDocument doc = d;
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("<your_order_db_here>");
            if (c != null)
            {
                doc["lastactiontime"] = DateTime.UtcNow;
                var result = await c.ReplaceOneAsync(
                    item => item["_id"] == doc["_id"],
                    doc,
                    new UpdateOptions { IsUpsert = true });
            }
        }
        public static async Task UpdateDBAsyncNew(BsonDocument d)
        {
            bool bFlag = false;
            int work = (int)StateMaschine.init;
                       

            //find work
            d["work"] = work;
            //%1 regulation, special case:
            if (d["indicator"].ToString().Contains("alt.sec") == true)
            {
                if (d["p0"].ToString() == "null" && d["p1"].ToString() != "null" && d["p2"].ToString() == "null" && d["p3"].ToString() == "1")
                {
                    d["work"] = (int)StateMaschine.sell0;
                    bFlag = true;
                }
                else if (d["p0"].ToString() == "null" && d["p1"].ToString() == "null" && d["p2"].ToString() != "null" && d["p3"].ToString() == "1")
                {
                    d["work"] = (int)StateMaschine.executer_check_stoploss;
                    bFlag = true;
                }
                else if (d["p0"].ToString() != "null" && d["p1"].ToString() == "null" && d["p2"].ToString() != "null" && d["p3"].ToString() == "1")
                {
                    d["work"] = (int)StateMaschine.executer_check_stoploss;
                    bFlag = true;
                }
                else if (d["p0"].ToString() != "null" && d["p1"].ToString() == "null" && d["p2"].ToString() == "null" && d["p3"].ToString() == "1")
                {
                    d["work"] = (int)StateMaschine.executer_check_stoploss;
                    bFlag = true;
                }
            }
            //Update executersettings db only
            else if (d["indicator"].ToString().Contains("alt.s7") == true)
            {
                d["work"] = (int)StateMaschine.executersettingupdate;
                bFlag = true;
            }
            //t11.lowfinder.xxx, where xxx = coins..
            else if (d["indicator"].ToString().ToUpper().Contains("ALT") == true)
            {
                //open long
                if (d["ordAction"].ToString().ToUpper() == "BUY" &&
                    d["ordComment"].ToString().ToUpper() == "ENTRYLONG")
                {
                    d["work"] = (int)StateMaschine.buy0;
                    bFlag = true;
                }
                //first TP
                else if (d["ordAction"].ToString().ToUpper() == "SELL" &&
                    d["ordComment"].ToString().ToUpper() == "TP1")
                {
                    d["work"] = (int)StateMaschine.buy1;
                    bFlag = true;
                }
                //second TP
                else if (d["ordAction"].ToString().ToUpper() == "SELL" &&
                    d["ordID"].ToString().ToUpper() == "TP2" &&
                    d["ordComment"].ToString().ToUpper() == "TP2")
                {
                    d["work"] = (int)StateMaschine.buy2;
                    bFlag = true;
                }
                //third TP
                else if (d["ordAction"].ToString().ToUpper() == "SELL" &&
                    d["ordID"].ToString().ToUpper() == "TP3" &&
                    d["ordComment"].ToString().ToUpper() == "TP3")
                {
                    d["work"] = (int)StateMaschine.buy3;
                    bFlag = true;
                }


                //open short 1
                else if (d["ordAction"].ToString().ToUpper() == "SELL" &&
                    d["ordComment"].ToString().ToUpper() == "ENTRYSHORT")
                {
                    d["work"] = (int)StateMaschine.sell0;
                    bFlag = true;
                }
                //first TP
                else if (d["ordAction"].ToString().ToUpper() == "BUY" &&
                    d["ordComment"].ToString().ToUpper() == "TP2")
                {
                    d["work"] = (int)StateMaschine.sell1;
                    bFlag = true;
                }
                //second TP
                else if (d["ordAction"].ToString().ToUpper() == "BUY" &&
                    d["ordID"].ToString().ToUpper() == "TP2" &&
                    d["ordComment"].ToString().ToUpper() == "TP2")
                {
                    d["work"] = (int)StateMaschine.sell2;
                    bFlag = true;
                }
                //third TP
                else if (d["ordAction"].ToString().ToUpper() == "BUY" &&
                    d["ordID"].ToString().ToUpper() == "TP3" &&
                    d["ordComment"].ToString().ToUpper() == "TP3")
                {
                    d["work"] = (int)StateMaschine.sell3;
                    bFlag = true;
                }
            }




            //market name per default ftx
            int marketname = (int)MarketNames.FTX;


            string indicator = d["indicator"].ToString();
            if (indicator.ToUpper().Contains("ALT"))
            {
                indicator = indicator.Remove(0, 5);
            }
            string coin = indicator;

            



            if (bFlag == true)
            {
                BsonDocument doc = new BsonDocument();
                doc["dbentrytime"] = d["dbentrytime"];
                doc["alerttime"] = d["alerttime"];
                doc["lastactiontime"] = DateTime.UtcNow;
                doc["indicator"] = d["indicator"];
                doc["coin"] = coin;
                doc["status"] = d["status"];
                doc["work"] = d["work"];
                doc["internal_task"] = 0;
                doc["ordAction"] = d["ordAction"];
                doc["ordContracts"] = d["ordContracts"];
                doc["ordPrice"] = d["ordPrice"];
                doc["ordID"] = d["ordID"];
                doc["ordComment"] = d["ordComment"];
                doc["ordAlertMessage"] = d["ordAlertMessage"];
                doc["posSize"] = d["posSize"];
                doc["markPos"] = d["markPos"];
                doc["markPosSize"] = d["markPosSize"];
                doc["prMarkPos"] = d["prMarkPos"];
                doc["prMarkPosSize"] = d["prMarkPosSize"];
                doc["p0"] = d["p0"];
                doc["p1"] = d["p1"];
                doc["p2"] = d["p2"];
                doc["p3"] = d["p3"];
                doc["p4"] = d["p4"];
                doc["lastclose"] = d["close"];
                doc["marketname"] = marketname;


                string collection = "<your_orders_db_here>";
                IMongoCollection<BsonDocument> c;
                c = _db.GetCollection<BsonDocument>(collection);
                try
                {
                    if (c != null)
                    {
                        await c.InsertOneAsync(doc);
                        Console.WriteLine(DateTime.UtcNow +
                            "\t" + "work: " + doc["work"].ToString() +
                            "\t" + "indicator: " + doc["indicator"].ToString() +
                            "\t" + "coin: " + doc["coin"].ToString() +
                            "\t" + "posSize: " + doc["posSize"].ToString() +
                            "\t" + "lastclose: " + doc["lastclose"].ToString() +
                            "\t" + "ordComment: " + doc["ordComment"].ToString().ToUpper()
                            );
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }





            //Capture the last close price for online coins
            if (WorkerExecuter.DocsLastClosePrice != null)
            {                
                WorkerExecuter.FillDirectoryDocsLastClosePrice(coin, d["close"].ToDouble());
            }

        }
        private void Cleaner()
        {
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("<your_signal_db_here>");
            if (c != null)
            {
                try
                {
                    //var dt1 = DateTime.UtcNow.AddMinutes(-10);
                    var filter = "{}";
                    var docs = c.Find(filter).SortBy(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs.Result.Count > 0)
                    {
                        foreach (var doc in docs.Result)
                        {
                            DateTime dt = doc["dbentrytime"].ToUniversalTime();
                            if (dt < DateTime.UtcNow.AddMinutes(-3600))
                            {
                                DeleteDBAsync(doc).Wait();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public static async Task DeleteDBAsync(BsonDocument d)
        {
            BsonDocument doc = d;
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("<your_signal_db_here>");
            if (c != null)
            {
                doc["lastactiontime"] = DateTime.UtcNow;
                var result = await c.DeleteOneAsync(item => item["_id"] == doc["_id"]);
            }
        }
    }
}
