using ApiResponse.FTX;
using Executer.Core;
using Executer.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Boolean = Executer.Enums.Boolean;

namespace Executer.Workers
{
    class WorkerDatabase : Worker
    {
        protected static IMongoDatabase _db = null;
        protected static IMongoCollection<BsonDocument> _collection = null;
        private static string Collection = "<your_collection_here>";
        private DateTime DoWorkAsync1TimeLastRun0 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun1 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun2 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun5 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun6 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun7 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun8 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun9 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun10 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRun11 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRunUpdateStatistics = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime DoWorkAsync1TimeLastRunUpdateTradeInfoDB_1 = DateTime.Parse("01-01-2000 00:00:00");
        public const int OffsetSeconds = 0;
        public const int WaitingTimeSeconds = 600 + OffsetSeconds;
        private static int _flipper = (int)Flipper.Init;
        public static decimal FeeFactor = 0.0588m;


        private static bool RunOnce;

        private DateTime TimeLastRun1 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime TimeLastRun2 = DateTime.Parse("01-01-2000 00:00:00");
        private DateTime TimeLastRun3 = DateTime.Parse("01-01-2000 00:00:00");
        private const int WaitingTimeInSeconds = 120;
        private const int WaitingTimeInSecondsRun3 = 600;

        private static WorkerApi wa = null;
        public bool OnlyOnce { get; set; }
        public static int IsOnline { get; set; }

        private bool SELLTOTURNBACKFROMBORROW = false;


        public WorkerDatabase()
        {
            RunOnce = false;
            OnlyOnce = false;
            SELLTOTURNBACKFROMBORROW = false;
            if (wa == null)
            {
                wa = new WorkerApiFTX();
            }
            IsOnline = (int)Boolean._unset;
        }
        public void ConnectDB()
        {
            var connectionString = MongoClientSettings.FromUrl(MongoUrl.Create("<your_mongo_connection_string_here>"));
            MongoClient client = new MongoClient(connectionString);
            _db = client.GetDatabase("<your_db_name_here>");
            _collection = _db.GetCollection<BsonDocument>(Collection);
        }
        public override void DoWork()
        {

            while (IsAlive)
            {
                switch (_flipper)
                {
                    case (int)Flipper.Init:
                        if (OnlyOnce == false)
                        {
                            //reset all status = 0 at the beginning
                            OnlyOnce = true;
                            OnlyOnceAtTheBeginning();
                        }
                        _flipper = (int)Flipper.ReadSettings;
                        break;
                    case (int)Flipper.ReadSettings:
                        if (DoWorkAsync1TimeLastRun10 <= DateTime.UtcNow)
                        {
                            DoWorkAsync1TimeLastRun10 = DateTime.UtcNow.AddSeconds(DoWorkAsync1TimeLastRun10.Second + 10);
                            ReadDBSettingsAsync().Wait();
                        }                        
                        _flipper = (int)Flipper.ResetOpenOrdersAtTheBeginning;
                        break;
                    case (int)Flipper.ResetOpenOrdersAtTheBeginning:
                        if (RunOnce == false)
                        {
                            ResetOpenOrdersAtTheBeginningAsync().Wait();
                            RunOnce = true;
                            _flipper = (int)Flipper.CreateAPIKeys;
                        }
                        else
                        {
                            _flipper = (int)Flipper.CreateAPIKeys;
                        }
                        break;
                    case (int)Flipper.CreateAPIKeys:
                        CreateAPIKeysAsync().Wait();
                        break;
                    case (int)Flipper.SetAPIKeys:
                        SetAPIKeys();
                        _flipper = (int)Flipper.ReadDBAsync;
                        break;
                    case (int)Flipper.ReadDBAsync:
                        ReadDBAsync().Wait();
                        _flipper = (int)Flipper.DeleteDB;
                        break;

                    //--------------------------------------------

                    case (int)Flipper.DeleteDB:

                        if (DoWorkAsync1TimeLastRun6 <= DateTime.UtcNow)
                        {
                            DoWorkAsync1TimeLastRun6 = DateTime.UtcNow.AddSeconds(DoWorkAsync1TimeLastRun6.Second + WaitingTimeSeconds);


                            /*
                            DeleteDB("status", 9004);
                            DeleteDB("status", 9001); 
                            DeleteDB("status", 9006);
                            DeleteDB("status", 9000);
                            DeleteDB("status", 2247);
                            DeleteDB("status", 2230);
                            DeleteDB("status", 9005);
                            DeleteDB("status", 3001);
                            DeleteDB("status", 2248);
                            DeleteDB("status", 1113);
                            DeleteDB("status", 1118);
                            DeleteDB("status", 9005);
                            DeleteDB("status", 3001);
                            DeleteDB("status", 9997);
                            DeleteDB("status", 9004);
                            DeleteDB("status", 9002);                            
                            DeleteDB("status", 2231);
                            DeleteDB("status", 2223);
                            */
                        }
                        _flipper = (int)Flipper.AdjustAmountPerTrade;
                        break;


                    case (int)Flipper.AdjustAmountPerTrade:
                        if (DoWorkAsync1TimeLastRun11 <= DateTime.UtcNow)
                        {
                            DoWorkAsync1TimeLastRun11 = DateTime.UtcNow.AddMinutes(15);
                            //AdjustAmountPerTradeAsync().Wait();
                        }
                        _flipper = (int)Flipper.Init;
                        break;





                    ////----not used 2018--------------------------------------------------------------
                    
                    case (int)Flipper.ReadOpenLoanBalance:
                        if (DoWorkAsync1TimeLastRun2 <= DateTime.UtcNow)
                        {
                            DoWorkAsync1TimeLastRun2 = DateTime.UtcNow.AddSeconds(DoWorkAsync1TimeLastRun2.Second + 60);
                            //AdjustLoanBalance();
                        }
                        _flipper = (int)Flipper.ReadInputMultiplier;
                        break;
                    default:
                        _flipper = (int)Flipper.Init;
                        break;
                }
            }
        }
        public override void DoWork2()
        { }
        public override void DoWork3()
        { }

        protected async Task ResetOpenOrdersAtTheBeginningAsync()
        {
            if (_collection != null)
            {
                try
                {
                    //var filter1 = "{ $or : [ {status : 100}, {status: 140} ], internal_task : 0 }";
                    //var filter = "{ status : 2000, internal_task : 0, coin : '" + coin + "', order_idBuy: {'$exists':true}, avgFillPrice : {'$exists':false} }";
                    var filter1 = "{status: 0}";
                    var docs1 = await _collection.Find(filter1).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs1.Count > 0)
                    {
                        foreach (var doc1 in docs1)
                        {
                            doc1["status"] = (int)StateMaschine.cancel_at_the_beginning_of_app;
                            doc1["comment"] = "cancel order at the beginning of app";
                            UpdateDBAsync(doc1).Wait();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public void ResetAllOpenSellOrders1010()
        {
            //ResetAllAfterErrorTaskAsync().Wait();
            //ResetAllOpenSellOrders1010TaskAsync1().Wait();
            //ResetAllOpenSellOrders1010TaskAsync2().Wait();
            //ResetAllOpenSellOrders1010TaskAsync3().Wait();
        }
        protected async Task ResetAllAfterErrorTaskAsync()
        {
            if (_collection != null)
            {
                try
                {
                    var filter1 = "{ status: {'$lt':500, '$exists':true, '$ne':null}}";

                    var docs1 = await _collection.Find(filter1).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs1.Count == 0)
                    {
                        IMongoCollection<BsonDocument> c;
                        c = _db.GetCollection<BsonDocument>("s3.validator.XTZ");
                        if (c != null)
                        {
                            var filter = "{tradetype: 'long'}";

                            var docs = await c.Find(filter).ToListAsync();
                            if (docs.Count == 1)
                            {
                                docs[0]["status"] = (int)StateMaschine.init;
                                docs[0]["lastactiontime"] = DateTime.UtcNow;
                                var result = await c.ReplaceOneAsync(
                                    item => item["_id"] == docs[0]["_id"],
                                    docs[0],
                                    new UpdateOptions { IsUpsert = true });
                                Console.WriteLine(DateTime.UtcNow + " : " + result.ToJson());
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


        public void Reset_Internal_Status_1(int i, int timer)
        {
            Reset_Internal_Status_1TaskAsync1(i, timer).Wait();
        }
        protected async Task Reset_Internal_Status_1TaskAsync1(int i, int timer)
        {
            if (_collection != null)
            {
                try
                {
                    var filter = "{status: " + i + ", internal_task : 1}";
                    var docs = await _collection.Find(filter).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs.Count > 0)
                    {
                        foreach (var doc1 in docs)
                        {
                            DateTime dt = doc1["lastactiontime"].ToUniversalTime();//.AsBsonDateTime.ToUniversalTime()

                            if (DateTime.UtcNow.AddSeconds(-1 * timer) > dt)
                            {
                                doc1["internal_task"] = 0;
                                UpdateDBAsync(doc1).Wait();
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


        public void UpdateTradeInfoDB_1(int timer)
        {
            UpdateTradeInfoDB_1TaskAsync1(timer).Wait();
        }
        protected async Task UpdateTradeInfoDB_1TaskAsync1(int timer)
        {

            if (_collection != null)
            {
                try
                {
                    var filter = "{status: 1000, internal_task : 0, work : 100, avgFillPrice : 0, filledSize : '0.0'}";
                    var docs = await _collection.Find(filter).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs.Count > 0)
                    {
                        foreach (var doc1 in docs)
                        {
                            DateTime dt = doc1["lastactiontime"].ToUniversalTime();//.AsBsonDateTime.ToUniversalTime()

                            if (DateTime.UtcNow.AddSeconds(-1 * timer) > dt)
                            {
                                doc1["status"] = (int)StateMaschine.buy_api_results_after_trade_success;
                                doc1["internal_task"] = 0;
                                UpdateDBAsync(doc1).Wait();
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

        public void ResetAllOpenSellOrders110()
        {
            ResetAllOpenSellOrders110TaskAsync1().Wait();
        }
        protected async Task ResetAllOpenSellOrders110TaskAsync1()
        {
            if (_collection != null)
            {
                try
                {
                    var filter1 = "{status: 110, internal_task : 1, order_id: {'$ne':null}}";
                    var docs1 = await _collection.Find(filter1).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs1.Count > 0)
                    {
                        foreach (var doc1 in docs1)
                        {
                            DateTime dt = doc1["dbentrytime"].ToUniversalTime();//.AsBsonDateTime.ToUniversalTime()
                            if (DateTime.UtcNow.AddMinutes(-30) > dt)
                            {
                                doc1["internal_task"] = 0;
                                UpdateDBAsync(doc1).Wait();
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
        public void ResetAllOpenSellOrders2234()
        {
            //ResetAllOpenSellOrders2234TaskAsync1().Wait();
        }
        protected async Task ResetAllOpenSellOrders2234TaskAsync1()
        {
            if (_collection != null && WorkerExecuter.WorkerApi != null)
            {
                try
                {
                    var filter1 = "{status: 2234, internal_task : 0, order_id: {'$ne':null}}";
                    var docs1 = await _collection.Find(filter1).SortByDescending(bson => bson["dbentrytime"]).ToListAsync();
                    if (docs1.Count > 0)
                    {
                        foreach (var doc1 in docs1)
                        {
                            //string orderID = doc1["order_id"].ToString();
                            string[] args = new string[7];
                            args[0] = "check";
                            args[1] = doc1["order_id"].ToString();
                            args[2] = "h";
                            args[3] = WorkerExecuter.Pub;
                            args[4] = WorkerExecuter.Sec;
                            args[5] = WorkerExecuter.Phrase;
                            //args[6] = WorkerExecuter.CoinPair;
                            string res = WorkerExecuter.WorkerApi.ApiCheck(args);
                            res = res.Replace("\r", string.Empty).Replace("\n", string.Empty);
                            res = res.Replace(", 'isWorking': False", string.Empty).Replace(", 'isWorking': True", string.Empty);

                            var request = new ApiResponse.KuCoin.OrderResponse_check();
                            request = JsonConvert.DeserializeObject<ApiResponse.KuCoin.OrderResponse_check>(res);

                            if (request != null)
                            {
                                if (request.isActive == false)//Filled
                                {
                                    //SetBuySellFilledValues(doc1, request);
                                }
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
        private void SetBuySellFilledValues(BsonDocument d, ApiResponse.FTX.OrderResponse_buysell r)
        {
            //after buy
            if (r.avgFillPrice == null) { d["avgFillPrice"] = r.price; } else { d["avgFillPrice"] = r.avgFillPrice; }

            double avgFillPrice_n = d["avgFillPrice"].ToDouble();
            d["avgFillPrice_n"] = avgFillPrice_n;

            d["filledSize"] = d["filledSizeBuy"];

            //as USD
            if (r.price == null) { d["price"] = d["lastclose"]; } else { d["price"] = r.price; }
            if (r.remainingSize == null) { d["remainingSize"] = 0; } else { d["remainingSize"] = r.remainingSize; }
            d["size"] = r.size;//as coin
            d["createdAt"] = r.createdAt;
            d["filledAt"] = DateTime.UtcNow;

            decimal avgFillPriceBuy = d["avgFillPriceBuy"].ToDecimal();
            decimal filledSizeBuy = d["filledSizeBuy"].ToDecimal();
            decimal inputTrade = avgFillPriceBuy * filledSizeBuy;

            decimal avgFillPriceSell = d["avgFillPrice"].ToDecimal();
            decimal filledSizeSell = d["filledSize"].ToDecimal();
            decimal outputTrade = avgFillPriceSell * filledSizeSell;

            decimal gain_usd = outputTrade - inputTrade;
            d["gain_usd"] = gain_usd;
            //x = ((p2 - p1). 100) / p1
            decimal gain_pc = 0;
            if (inputTrade != 0)
            {
                gain_pc = ((outputTrade - inputTrade) * 100) / inputTrade;
            }
            d["gain_pc"] = gain_pc;
        }
        public void ResetInternalTask()
        {
            ResetInternalTaskAsync().Wait();
        }
        protected async Task ResetInternalTaskAsync()
        {
            if (_collection != null)
            {
                var filter = "{ status: {'$lt':500, '$exists':true, '$ne':null}, internal_task : 1}";
                var docs = await _collection.Find(filter).ToListAsync();
                if (docs.Count > 0)
                {
                    foreach (var doc in docs)
                    {
                        doc["internal_task"] = 0;
                        UpdateDBAsync(doc).Wait();
                    }
                }
            }
        }


        public void CorrectionDateTimeFormat()
        {
            CorrectionDateTimeFormatAsync().Wait();
        }
        protected async Task CorrectionDateTimeFormatAsync()
        {
            if (_collection != null)
            {
                var filter = "{ status: {'$gte':500, '$exists':true, '$ne':null}}";
                var docs = await _collection.Find(filter).ToListAsync();
                if (docs.Count > 0)
                {
                    foreach (var doc in docs)
                    {
                        UpdateDBAsync(doc).Wait();
                    }
                }
            }
        }
        protected async Task ReadDBAsync()
        {
            //_collection = _db.GetCollection<BsonDocument>(Collection);
            if (_collection != null)
            {
                var filter = "{ status: {'$lt':500, '$exists':true, '$ne':null}, internal_task : 0}";
                //var filter = "{ status: {'$lt':500, '$exists':true, '$ne':null}, internal_task : 0, tradetype : 'long' }";
                //var filter2 = "{ $and : [ {status : {'$ne':'100'}}, {status: {'$lt':'500'}} ], internal_task : 0, tradetype : 'long' }";
                //var filter = "{ status : 0, $and : [ {rule : {'$ne':'-4'}}, {rule: {'$ne':'4'}} ]}";

                try
                {
                    var docs = await _collection.Find(filter).ToListAsync();
                    if (docs.Count > 0)
                    {
                        foreach (var doc in docs)
                        {
                            doc["internal_task"] = 1;
                            UpdateDBAsync(doc).Wait();
                            WorkerExecuter.FillDirectory(doc);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void ReadTotalBalanceAsync(string coin)
        {
            WorkerApi wa = new WorkerApiBinance();
            try
            {
                string[] args = new string[9];
                args[0] = "account";
                args[1] = "h";
                args[2] = "h";
                args[3] = WorkerExecuter.Pub;
                args[4] = WorkerExecuter.Sec;
                args[5] = "h";
                args[6] = coin.ToString().ToUpper();
                args[7] = "h";
                args[8] = "h";
                string res = wa.ApiCheck(args);
                res = res.Replace("\r", string.Empty).Replace("\n", string.Empty);
                res = res.Replace("None", "null");
                res = res.Replace("False", "false").Replace("True", "true");
                res = res.Replace("}, {", "}# {");

                if (res.Contains("exit") || res.Contains("error"))
                {
                    Console.WriteLine("error on api call//account");
                }
                var asset = new ApiResponse.Binance.OrderResponse_asset();
                asset = JsonConvert.DeserializeObject<ApiResponse.Binance.OrderResponse_asset>(res);
                WorkerExecuter.FillDirectoryDocsAssets(coin, asset);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
                
        protected async Task ExitAppIfLoop()
        {
            if (_collection != null)
            {
                try
                {
                    var filter = "{ status : 0, internal_task : 0 }";
                    var docs2 = await _collection.Find(filter).ToListAsync();
                    if (docs2.Count > 5)
                    {
                        Console.WriteLine("more then " + docs2.Count + " status 0, exiting..");
                        Environment.Exit(0);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        protected void ReadExchangePositionsLongAsync()
        {
            WorkerApi wa = new WorkerApiFTX();

            //for long
            try
            {
                string[] args = new string[8];
                args[0] = "getpositions";
                args[1] = "h";
                args[2] = "h";
                args[3] = WorkerExecuter.Pub;
                args[4] = WorkerExecuter.Sec;
                args[5] = WorkerExecuter.Phrase;
                args[6] = "h";
                args[7] = "sublong";
                string res = wa.ApiCheck(args);
                res = res.Replace("\r", string.Empty).Replace("\n", string.Empty);
                res = res.Replace("None", "null");
                res = res.Replace("False", "false").Replace("True", "true");
                res = res.Replace("[", string.Empty);
                res = res.Replace("]", string.Empty);
                var request = new OrderResponse_positions();
                request = JsonConvert.DeserializeObject<ApiResponse.FTX.OrderResponse_positions>(res);
                if (request != null)
                {
                    //update db open orders in the db
                    //UpdateDBAsyncPosiitons(request, "sublong");
                    //read out once for long/short objects
                    //UpdateExchangeStatusAsync().Wait();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //handle error case
                _flipper = (int)Flipper.Init;
            }
        }
        protected void ReadExchangePositionsAsync(BsonDocument doc)
        {
            //TODO, not ready!
            
            WorkerApi wa = new WorkerApiFTX();

            //for long
            try
            {
                string[] args = new string[8];
                args[0] = "getpositions";
                args[1] = "h";
                args[2] = "h";
                args[3] = WorkerExecuter.Pub;
                args[4] = WorkerExecuter.Sec;
                args[5] = WorkerExecuter.Phrase;
                args[6] = "h";
                args[7] = doc["indicator"].ToString();
                string res = wa.ApiCheck(args);
                res = res.Replace("\r", string.Empty).Replace("\n", string.Empty);
                res = res.Replace("None", "null");
                res = res.Replace("False", "false").Replace("True", "true");
                res = res.Replace("[", string.Empty);
                res = res.Replace("]", string.Empty);
                res = res.Replace("}, {", "}# {");
                string[] array = res.Split('#');
                /*
                OrderResponse_positions[] arrayRequest;
                for (int i = 0; i < array.Length; i++)
                {
                    var pos = new OrderResponse_positions();
                    pos = JsonConvert.DeserializeObject<ApiResponse.FTX.OrderResponse_positions>(array[i]);
                    //arrayRequest[i] = pos;

                }
                */
                var request = new OrderResponse_positions();
                request = JsonConvert.DeserializeObject<ApiResponse.FTX.OrderResponse_positions>(res);
                if (request != null)
                {

                    //update db open orders
                    UpdateDBAsyncPosiitons(request, "sublong");

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //handle error case
                _flipper = (int)Flipper.Init;
            }


                
        }

        
        protected async Task ReadDBSettingsAsync()
        {
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("alt4.executersettings");
            if (c != null)
            {
                var filter = "{ }";
                try
                {
                    var docs = await c.Find(filter).ToListAsync();
                    if (docs.Count == 1)
                    {
                        foreach (var doc in docs)
                        {
                            WorkerExecuter.IsLoanAdjustActive = doc["IsLoanAdjustActive"].AsInt32;
                            WorkerExecuter.LoanApplicationValue = doc["LoanApplicationValue"].AsInt32;
                            WorkerExecuter.MaxBorrowableLoanValue = doc["MaxBorrowableLoanValue"].AsInt32;
                            WorkerExecuter.IsForceSellActive = doc["IsForceSellActive"].AsInt32;
                            WorkerExecuter.ForceSellActiveHr = doc["ForceSellActiveHr"].AsInt32;

                            WorkerExecuter.PubHashed = doc["pubhashed"].AsByteArray;
                            WorkerExecuter.SecHashed = doc["sechashed"].AsByteArray;
                            WorkerExecuter.PhraseHashed = doc["phrasehashed"].AsByteArray;
                            WorkerExecuter.PubInput = doc["pubinput"].AsString;
                            WorkerExecuter.SecInput = doc["secinput"].AsString;
                            WorkerExecuter.PhraseInput = doc["phraseinput"].AsString;

                            WorkerExecuter.Fee = doc["FeeInput"].ToDecimal();

                            //OffsetForBuySellPriceOnOrders = doc["OffsetForBuySellPriceOnOrders"].ToDecimal();


                            WorkerExecuter.MarketName = (int)MarketNames.NotImplementedYet;
                            int m = doc["MarketName"].AsInt32;
                            switch (m)
                            {
                                case (int)MarketNames.NotImplementedYet:
                                    WorkerExecuter.MarketName = (int)MarketNames.NotImplementedYet;
                                    break;
                                case (int)MarketNames.Bitrue:
                                    WorkerExecuter.MarketName = (int)MarketNames.Bitrue;
                                    break;
                                case (int)MarketNames.KuCoin:
                                    WorkerExecuter.MarketName = (int)MarketNames.KuCoin;
                                    break;
                                case (int)MarketNames.Binance:
                                    WorkerExecuter.MarketName = (int)MarketNames.Binance;
                                    break;
                                case (int)MarketNames.Bitmex:
                                    WorkerExecuter.MarketName = (int)MarketNames.Bitmex;
                                    break;
                                case (int)MarketNames.Kraken:
                                    WorkerExecuter.MarketName = (int)MarketNames.Kraken;
                                    break;
                                case (int)MarketNames.FTX:
                                    WorkerExecuter.MarketName = (int)MarketNames.FTX;
                                    break;
                                case (int)MarketNames.AAX:
                                    WorkerExecuter.MarketName = (int)MarketNames.AAX;
                                    break;
                                default:
                                    WorkerExecuter.MarketName = (int)MarketNames.NotImplementedYet;
                                    break;
                            }

                            IsOnline = doc["online"].AsInt32;
                            //WorkerExecuter.CoinPair = doc["CoinPair"].AsString;
                            WorkerExecuter.MaxTimeframeForPriceAdjust = doc["MaxTimeframeForPriceAdjust"].AsInt32;
                            WorkerExecuter.TradeType = doc["tradetype"].ToString();
                            //WorkerExecuter.CountAllowTradeWhenSellTrend = doc["CountAllowTradeWhenSellTrend"].AsInt32;
                            WorkerExecuter.StopLoss = doc["StopLoss"].ToDouble();


                            var cn = doc["CollectionNames"].AsBsonArray;
                            var cpn = doc["CoinPairNames"].AsBsonArray;
                            var cptvUSD = doc["CoinPairTradeValueUSD"].AsBsonArray;
                            var cpprec_price = doc["CoinPairPrecisionPrice"].AsBsonArray;
                            var cpprec_amount = doc["CoinPairPrecisionAmount"].AsBsonArray;
                            var cpgainpercentage = doc["CoinGainPercentage"].AsBsonArray;
                            var cpgainpercentagespecial = doc["CoinGainPercentageSpecial"].AsBsonArray;
                            var cpoffsetforbuysell = doc["CoinOffsetForBuySellPriceOnOrders"].AsBsonArray;
                            var cpcoinonline = doc["CoinOnline"].AsBsonArray;


                            var cpcoinmultipliers = doc["CoinMultipliers"].AsBsonArray;
                            var cpcoinaccountvaluemultiplier = doc["CoinAccountValueMultiplier"].AsBsonArray;
                            var cpcoincountallowtradewhenselltrend = doc["CoinCountAllowTradeWhenSellTrend"].AsBsonArray;


                            bool flag = false;
                            if (WorkerExecuter.DocsCoinPairs.Count != cpn.Count)
                            {
                                WorkerExecuter.DocsCoinPairs.Clear();
                                WorkerExecuter.DocsCoinTradeValueUSDT.Clear();
                                WorkerExecuter.DocsCoinPrecisionPrice.Clear(); 
                                WorkerExecuter.DocsCoinPrecisionAmount.Clear();
                                WorkerExecuter.DocsCoinGainPercentage.Clear(); 
                                WorkerExecuter.DocsCoinGainPercentageSpecial.Clear();
                                WorkerExecuter.DocsCoinOffsetForBuySellPriceOnOrders.Clear();
                                WorkerExecuter.DocsCoinOnline.Clear();
                                WorkerExecuter.DocsLastUsedOnlineCoin.Clear();
                                WorkerExecuter.DocsCoinMultipliers.Clear();
                                WorkerExecuter.DocsCoinAccountValueMultiplier.Clear();
                                WorkerExecuter.DocsCoinCountAllowTradeWhenSellTrend.Clear();
                                flag = true;
                            }
                            if (WorkerExecuter.DocsCoinTradeValueUSDT.Count > 0)
                            {
                                for (int i = 0; i < cpn.Count; i++)
                                {
                                    /*
                                    double d1 = cptvUSD[i].ToDouble();
                                    double mult = doc["multiplier"].ToDouble();
                                    d1 = d1 * mult;
                                    double d2 = WorkerExecuter.DocsCoinTradeValueUSDT[cn[i].ToString()];                                    
                                    */

                                    double p1 = cpgainpercentage[i].ToDouble();
                                    double p2 = WorkerExecuter.DocsCoinGainPercentage[cn[i].ToString()];

                                    double ps1 = cpgainpercentagespecial[i].ToDouble();
                                    double ps2 = WorkerExecuter.DocsCoinGainPercentageSpecial[cn[i].ToString()];

                                    int i1 = cpcoinonline[i].ToInt32();
                                    int i2 = WorkerExecuter.DocsCoinOnline[cn[i].ToString()];

                                    double a1 = cpprec_amount[i].ToDouble();
                                    double a2 = WorkerExecuter.DocsCoinPrecisionAmount[cn[i].ToString()];

                                    double accmultip1 = cpcoinaccountvaluemultiplier[i].ToDouble();
                                    double accmultip2 = WorkerExecuter.DocsCoinAccountValueMultiplier[cn[i].ToString()];

                                    if (/*(d1 != d2) ||*/ (p1 != p2) || (ps1 != ps2) || (i1 != i2) || (a1 != a2) || (accmultip1 != accmultip2))
                                    {
                                        WorkerExecuter.DocsCoinPairs.Clear();
                                        WorkerExecuter.DocsCoinTradeValueUSDT.Clear();
                                        WorkerExecuter.DocsCoinPrecisionPrice.Clear(); 
                                        WorkerExecuter.DocsCoinPrecisionAmount.Clear(); 
                                        WorkerExecuter.DocsCoinGainPercentage.Clear();
                                        WorkerExecuter.DocsCoinGainPercentageSpecial.Clear();
                                        WorkerExecuter.DocsCoinOffsetForBuySellPriceOnOrders.Clear();
                                        WorkerExecuter.DocsCoinOnline.Clear();
                                        WorkerExecuter.DocsLastUsedOnlineCoin.Clear();
                                        WorkerExecuter.DocsCoinMultipliers.Clear();
                                        WorkerExecuter.DocsCoinAccountValueMultiplier.Clear();
                                        WorkerExecuter.DocsCoinCountAllowTradeWhenSellTrend.Clear();
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                            if (flag == true)
                            {
                                for (int i = 0; i < cpn.Count; i++)
                                {
                                    WorkerExecuter.FillDirectoryDocsCoinPairs(cn[i].ToString(), cpn[i].ToString());
                                    WorkerExecuter.FillDirectoryDocsCoinTradeValue(cn[i].ToString(), cptvUSD[i].ToDouble()/* * doc["multiplier"].ToDouble()*/);
                                    WorkerExecuter.FillDirectoryDocsCoinPrecisionPrice(cn[i].ToString(), cpprec_price[i].ToDouble()); 
                                    WorkerExecuter.FillDirectoryDocsCoinPrecisionAmount(cn[i].ToString(), cpprec_amount[i].ToDouble());
                                    WorkerExecuter.FillDirectoryDocsCoinGainPercentage(cn[i].ToString(), cpgainpercentage[i].ToDouble());
                                    WorkerExecuter.FillDirectoryDocsCoinGainPercentageSpecial(cn[i].ToString(), cpgainpercentagespecial[i].ToDouble());
                                    WorkerExecuter.FillDirectoryDocsCoinOffsetForBuySellPriceOnOrders(cn[i].ToString(), cpoffsetforbuysell[i].ToDouble());
                                    WorkerExecuter.FillDirectoryDocsCoinOnline(cn[i].ToString(), cpcoinonline[i].ToInt32());
                                    WorkerExecuter.FillDirectoryDocsLastUsedOnlineCoin(cn[i].ToString(), false);
                                    WorkerExecuter.FillDirectoryDocsCoinMultipliers(cn[i].ToString(), cpcoinmultipliers[i].ToDouble());
                                    WorkerExecuter.FillDirectoryDocsCoinAccountValueMultiplier(cn[i].ToString(), cpcoinaccountvaluemultiplier[i].ToDouble());
                                    WorkerExecuter.FillDirectoryDocsCoinCountAllowTradeWhenSellTrend(cn[i].ToString(), cpcoincountallowtradewhenselltrend[i].ToInt32());
                                }
                            }

                            //WorkerExecuter.AccountValueMultiplier = doc["AccountValueMultiplier"].AsInt32;

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

        protected async Task AdjustAmountPerTradeAsync()
        {
            if (WorkerExecuter.DocsLastClosePrice == null || WorkerExecuter.DocsLastClosePrice.Count() == 0)
            {
                return;
            }

            bool flag = false;
            int totalOnlineCoins = 0;
            double assetValueTotalUSDT = 0;
            double assetValuePerEachCoinUSDT = 0;

            for (int i = 0; i < WorkerExecuter.DocsCoinOnline.Count; i++)
            {
                string key = WorkerExecuter.DocsCoinOnline.ElementAt(i).Key;
                int val = WorkerExecuter.DocsCoinOnline.ElementAt(i).Value;
                if (val == (int)Boolean._true)
                {
                    totalOnlineCoins++;
                    ReadTotalBalanceAsync(key);

                    //Find total asset value in USDT
                    if (WorkerExecuter.DocsLastClosePrice.ContainsKey(key) == true)
                    {
                        double tokenPrice = WorkerExecuter.DocsLastClosePrice[key];
                        assetValueTotalUSDT = assetValueTotalUSDT + (WorkerExecuter.DocsAssets[key].free * tokenPrice) + (WorkerExecuter.DocsAssets[key].locked * tokenPrice);
                    }
                }
            }
            ReadTotalBalanceAsync("usdt");
            assetValueTotalUSDT = assetValueTotalUSDT + (WorkerExecuter.DocsAssets["usdt"].free * 1) + (WorkerExecuter.DocsAssets["usdt"].locked * 1);


            //Find asset value in USDT per each
            if (assetValueTotalUSDT == 0 || totalOnlineCoins == 0)
            {
                return;
            }
            assetValuePerEachCoinUSDT = assetValueTotalUSDT / totalOnlineCoins;
            if (assetValuePerEachCoinUSDT == 0)
            {
                return;
            }

            //Find ratio of asset corrolation
            /*  each  FET own         
                200   300
                100    x
                x = 3000 / 200 = 150
            */
            double newAmount = 0;
            ConcurrentDictionary<string, double> DocCoinPairPrecisionAmount = new ConcurrentDictionary<string, double>();

            for (int i = 0; i < WorkerExecuter.DocsAssets.Count; i++)
            {
                string key = WorkerExecuter.DocsAssets.ElementAt(i).Key;
                if (key == "usdt" || WorkerExecuter.DocsLastClosePrice.ContainsKey(key) == false)
                {
                    continue;
                }
                double tokenPrice = WorkerExecuter.DocsLastClosePrice[key];
                double usedRatio = 100 * ((WorkerExecuter.DocsAssets[key].free * tokenPrice) + (WorkerExecuter.DocsAssets[key].locked * tokenPrice))
                                    / assetValuePerEachCoinUSDT;
                                
                newAmount = 0;//reset


                /*
                Special cases for debug/experiments
                FET, BNB
                */
                if (key == "fet")
                {
                    if (usedRatio >= 100) { newAmount = 0; } 
                    else if (usedRatio <= 100 && usedRatio > 90) { newAmount = 10; }
                    else if (usedRatio <= 90 && usedRatio > 80) { newAmount = 15; }
                    else if (usedRatio <= 80 && usedRatio > 70) { newAmount = 20; }
                    else if (usedRatio <= 70 && usedRatio > 50) { newAmount = 30; }
                    else if (usedRatio <= 50 && usedRatio > 20) { newAmount = 40; }
                    else if (usedRatio <= 20) { newAmount = 50; }
                    flag = true;
                    DocCoinPairPrecisionAmount[key] = newAmount;
                }
                else if (key == "bnb")
                {
                    if (usedRatio >= 100) { newAmount = 0; }
                    else if (usedRatio <= 100 && usedRatio > 90) { newAmount = 0.01; }
                    else if (usedRatio <= 90 && usedRatio > 80) { newAmount = 0.02; }
                    else if (usedRatio <= 80 && usedRatio > 70) { newAmount = 0.03; }
                    else if (usedRatio <= 70 && usedRatio > 50) { newAmount = 0.04; }
                    else if (usedRatio <= 50 && usedRatio > 20) { newAmount = 0.05; }
                    else if (usedRatio <= 20) { newAmount = 0.1; }
                    flag = true;
                    DocCoinPairPrecisionAmount[key] = newAmount;
                }
            }

            
            //btc, txz, etc are not active yet!!!!
            if (flag == false)
            {
                return;
            }

            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("alt4.executersettings");
            if (c != null)
            {
                var filter = "{ }";
                try
                {
                    var docs = await c.Find(filter).ToListAsync();
                    if (docs.Count == 1)
                    {
                        foreach (var doc in docs)
                        {

                            var cpprec_amount = doc["CoinPairPrecisionAmount"].AsBsonArray;

                            foreach (var item in DocCoinPairPrecisionAmount)
                            {

                                int p = -1;
                                var cn = doc["CollectionNames"].AsBsonArray;
                                for (int i = 0; i < cn.Count; i++)
                                {
                                    string cc = cn[i].ToString();
                                    if (cc == item.Key)
                                    {
                                        p = i;
                                        break;
                                    }
                                }
                                if (p == -1)
                                {
                                    return;
                                }
                                cpprec_amount[p] = item.Value;
                            }
                            doc["CoinPairPrecisionAmount"] = cpprec_amount;                            
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

        protected void OnlyOnceAtTheBeginning()
        {
            try
            {
                IMongoCollection<BsonDocument> c;
                c = _db.GetCollection<BsonDocument>("alt4.orders");
                if (c != null)
                {
                    var filter = "{status : 0}";
                    var docs = c.Find(filter).ToListAsync();

                    if (docs.Result.Count > 0)
                    {
                        foreach (var doc in docs.Result)
                        {
                            doc["status"] = (int)StateMaschine.error_at_the_beginning_of_app_init;
                            UpdateDBAsync(doc).Wait();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        protected async Task CreateAPIKeysAsync()
        {
            if (WorkerExecuter.PubInput == null)
            {
                _flipper = (int)Flipper.ReadInputMultiplier;
            }
            else
            {            
                if (WorkerExecuter.PubInput.Length > 0
                    &&
                    WorkerExecuter.SecInput.Length > 0
                    &&
                    WorkerExecuter.PhraseInput.Length > 0
                    )
                {
                WorkerExecuter.PubHashed = Common.Encrypt(WorkerExecuter.PubInput);
                WorkerExecuter.SecHashed = Common.Encrypt(WorkerExecuter.SecInput);
                WorkerExecuter.PhraseHashed = Common.Encrypt(WorkerExecuter.PhraseInput);

                IMongoCollection<BsonDocument> c;
                    c = _db.GetCollection<BsonDocument>("<your_setting_db_here>");
                    if (c != null)
                    {
                        var filter = "{ }";
                        try
                        {
                            var docs = await c.Find(filter).ToListAsync();
                            if (docs.Count == 1)
                            {
                                foreach (var doc in docs)
                                {
                                    doc["pubhashed"] = WorkerExecuter.PubHashed;
                                    doc["sechashed"] = WorkerExecuter.SecHashed;
                                    doc["phrasehashed"] = WorkerExecuter.PhraseHashed;
                                    doc["pubinput"] = "";
                                    doc["secinput"] = "";
                                    doc["phraseinput"] = "";
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
                _flipper = (int)Flipper.SetAPIKeys;
            }
        }
        protected void SetAPIKeys()
        {
            if (WorkerExecuter.Pub == "" || WorkerExecuter.Sec == "")
            {
                if (WorkerExecuter.PubHashed == null || WorkerExecuter.PubHashed.Length == 0
                    ||
                    WorkerExecuter.SecHashed == null || WorkerExecuter.SecHashed.Length == 0
                    ||
                    WorkerExecuter.PhraseHashed == null || WorkerExecuter.PhraseHashed.Length == 0)
                {
                    return;
                }

                WorkerExecuter.Pub = Common.Decrypt(WorkerExecuter.PubHashed);
                WorkerExecuter.Sec = Common.Decrypt(WorkerExecuter.SecHashed);
                WorkerExecuter.Phrase = Common.Decrypt(WorkerExecuter.PhraseHashed);
            }
        }
        public static void DeleteDB(string key, int value)
        {
            var filter = "{ " + key + " : " + value + "}";
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>(Collection);
            if (c != null)
            {
                var result = c.DeleteMany(filter);
                Console.WriteLine(DateTime.UtcNow + " : " + result.ToJson());
            }
        }


        public static async Task UpdateDBSettingsAsync(int value, string coin, string function)
        {
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>("<your_setting_db_here>");
            if (c != null)
            {
                var filter = "{ }";
                try
                {
                    var docs = await c.Find(filter).ToListAsync();
                    if (docs.Count == 1)
                    {
                        foreach (var doc in docs)
                        {
                            //CoinCountAllowTradeWhenSellTrend
                            if (function == "CoinCountAllowTradeWhenSellTrend")
                            {
                                WorkerExecuter.DocsCoinCountAllowTradeWhenSellTrend[coin] = value;
                                var cn = doc["CollectionNames"].AsBsonArray;
                                int index = 0;
                                for (int i = 0; i < cn.Count(); ++i)
                                {
                                    if (cn[i].ToString() == coin)
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                                doc["CoinCountAllowTradeWhenSellTrend"][index] = value;
                            }
                            //CoinMultipliers
                            else if (function == "CoinMultipliers")
                            {
                                WorkerExecuter.DocsCoinMultipliers[coin] = value;
                                var cn = doc["CollectionNames"].AsBsonArray;
                                int index = 0;
                                for (int i = 0; i < cn.Count(); ++i)
                                {
                                    if (cn[i].ToString() == coin)
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                                doc["CoinMultipliers"][index] = value;
                            }


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



        public static async Task UpdateDBAsync(BsonDocument d)
        {
            BsonDocument doc = d;
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>(Collection);
            if (c != null)
            {
                doc["lastactiontime"] = DateTime.UtcNow;
                var result = await c.ReplaceOneAsync(
                    item => item["_id"] == doc["_id"],
                    doc,
                    new UpdateOptions { IsUpsert = true });
                Console.WriteLine(DateTime.UtcNow + " : " + result.ToJson());
            }
        }
        
        protected async Task UpdateOrderInAfterSellAsync(string searchStr, string updateStr)
        {
            if (_collection != null)
            {
                var filter = "{ order_id : '" + searchStr + "' }";
                try
                {
                    var docs = await _collection.Find(filter).ToListAsync();
                    if (docs.Count == 1)
                    {
                        foreach (var doc in docs)
                        {
                            doc["order_id_longTP"] = updateStr;
                            doc["comment"] = "chain is over";
                            doc["internal_task"] = 0;                            
                            UpdateDBAsync(doc).Wait();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        protected async Task UpdateOrderInAfterBuyAsync(string searchStr, string updateStr)
        {
            if (_collection != null)
            {
                var filter = "{ order_id : '" + searchStr + "' }";
                try
                {
                    var docs = await _collection.Find(filter).ToListAsync();
                    if (docs.Count == 1)
                    {
                        foreach (var doc in docs)
                        {
                            doc["order_id_shortTP"] = updateStr;
                            doc["comment"] = "chain is over";
                            doc["internal_task"] = 0;
                            UpdateDBAsync(doc).Wait();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        protected async Task CancelOrderInDBAsync(string id, bool subshort)
        {
            //short
            if (subshort == true)
            {
            }
            //long
            else
            {
                if (_collection != null)
                {
                    var filter = "{ order_id : '" + id + "' }";
                    try
                    {
                        var docs = await _collection.Find(filter).ToListAsync();
                        if (docs.Count == 1)
                        {
                            foreach (var doc in docs)
                            {
                                doc["status"] = (int)StateMaschine.buy_error_cancel_order;
                                doc["comment"] = "buy_error_cancel_order";
                                UpdateDBAsync(doc).Wait();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        protected async Task ReadDBAsyncToSellForceAsync()
        {
            if (_collection != null)
            {
                //collection.Find(bson => true).SortBy(bson => bson["SortByMeAscending"]).ThenByDescending(bson => bson["ThenByMeDescending"]).ToListAsync()
                //var filter = "{ status: {'$gte':1000, '$lte':1000, '$exists':true, '$ne':null}, internal_task : 0}";
                var filter = "{ status: {'$gte':1000, '$lte':1000, '$exists':true, '$ne':null}}";
                var docs = await _collection.Find(filter).SortByDescending(bson => bson["dbentrytime"]).Limit(3).ToListAsync();

                if (docs.Count > 0)
                {
                    foreach (var doc in docs)
                    {
                        doc["internal_task"] = 1;
                        doc["status"] = (int)StateMaschine.buy_price_lower_then_sell;
                        WorkerExecuter.FillDirectorySell(doc);
                        UpdateDBAsync(doc).Wait();
                    }
                }
            }
        }
        protected async Task SetBackDBAsyncToSellAsync()
        {
            if (_collection != null)
            {
                //collection.Find(bson => true).SortBy(bson => bson["SortByMeAscending"]).ThenByDescending(bson => bson["ThenByMeDescending"]).ToListAsync()
                var filter = "{ status: {'$gte':1001, '$lte':1001, '$exists':true, '$ne':null}, internal_task : 1}";
                var docs = await _collection.Find(filter).SortBy(bson => bson["dbentrytime"]).ToListAsync();
                if (docs.Count > 0)
                {
                    foreach (var doc in docs)
                    {
                        doc["status"] = 1000;
                        doc["internal_task"] = 0;
                        UpdateDBAsync(doc).Wait();
                    }
                }
            }
        }
        protected async Task SetCountOpenSlotsDBAsync()
        {
            if (_collection != null)
            {
                //collection.Find(bson => true).SortBy(bson => bson["SortByMeAscending"]).ThenByDescending(bson => bson["ThenByMeDescending"]).ToListAsync()
                //var filter = "{ status: {'$gte':1000, '$lte':1000, '$exists':true, '$ne':null}, internal_task : 0}";
                var filter = "{ status: {'$gte':1000, '$lte':1000, '$exists':true, '$ne':null}}";
                var docs = await _collection.Find(filter).SortBy(bson => bson["dbentrytime"]).ToListAsync();
                if (docs.Count > 0)
                {
                    int openslot = 1;
                    foreach (var doc in docs)
                    {
                        doc["openslot"] = openslot++;
                        UpdateDBAsync(doc).Wait();
                    }
                }
            }
        }
        protected async Task SetDocsSellErrorAsync()
        {
            if (_collection != null)
            {
                var filter = "{ status: {'$gte':1010, '$lte':1010, '$exists':true, '$ne':null}, internal_task : 1}";
                var docs = await _collection.Find(filter).ToListAsync();
                if (docs.Count > 0)
                {
                    foreach (var doc in docs)
                    {
                        doc["internal_task"] = 0;
                        doc["openslot"] = 0;
                        doc["status"] = (int)StateMaschine.sell_error_insufficient_balance_catch_1;
                        UpdateDBAsync(doc).Wait();
                    }
                }
            }
        }        
        private void UpdateDBAsyncPosiitons(OrderResponse_positions request, string sub)
        {
            BsonDocument doc = new BsonDocument();
            doc["lastactiontime"] = DateTime.UtcNow;
            doc["longOrderSize"] = request.longOrderSize;
            doc["netSize"] = request.netSize;
            doc["openSize"] = request.openSize;
            doc["recentAverageOpenPrice"] = "0";
            if (request.recentAverageOpenPrice != null)
            {
                doc["recentAverageOpenPrice"] = request.recentAverageOpenPrice;
            }
            doc["recentBreakEvenPrice"] = "0";
            if (request.recentBreakEvenPrice != null)
            {
                doc["recentBreakEvenPrice"] = request.recentBreakEvenPrice;
            }
            doc["shortOrderSize"] = request.shortOrderSize;
            doc["side"] = request.side;
            doc["size"] = request.size;
            doc["subaccount"] = sub;

            string typ = "s3.positions.XTZ";
            IMongoCollection<BsonDocument> c;
            c = _db.GetCollection<BsonDocument>(typ);
            if (c != null)
            {
                var result = c.ReplaceOneAsync(
                    item => item["subaccount"] == sub,
                    doc,
                    new UpdateOptions { IsUpsert = true });
                Console.WriteLine(DateTime.UtcNow + " : " + result.ToJson());
            }

        }

    }
    
}
 