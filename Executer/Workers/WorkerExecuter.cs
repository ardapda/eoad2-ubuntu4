using ExchangeSharp;
using Executer.Core;
using Executer.Documents;
using Executer.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Boolean = Executer.Enums.Boolean;
using ApiResponse.Binance;
using System.Threading.Tasks;

namespace Executer.Workers
{

    class WorkerExecuter : WorkerDatabase
    {

        private DateTime TimeLastRun = DateTime.Parse("01-01-2000");
        public const int WaitingTimeInSeconds = 5;
        
        public static ConcurrentDictionary<BsonObjectId, Entry> Docs;
        public static ConcurrentDictionary<BsonObjectId, Entry> DocsSell;
        public static ConcurrentDictionary<BsonObjectId, Entry> DocsBuy;
        public static ConcurrentDictionary<BsonObjectId, Entry> DocsSellST;
        

        public static int CountOfOpenSlots { get; set; }
        public static int MarketName { get; set; }
        public static decimal Fee { get; set; }
        public static string Pub { get; set; }
        public static string Sec { get; set; }
        public static string Phrase { get; set; }
        public static string PubInput { get; set; }
        public static string SecInput { get; set; }
        public static string PhraseInput { get; set; }
        public static string TradeType { get; set; }
        public static byte[] PubHashed { get; set; }
        public static byte[] SecHashed { get; set; }
        public static byte[] PhraseHashed { get; set; }
        public static double StopLoss { get; set; }

        public static int MaxTimeframeForPriceAdjust { get; set; }

        public static ConcurrentDictionary<string, string> DocsCoinPairs;
        public static ConcurrentDictionary<string, double> DocsCoinTradeValueUSDT;
        public static ConcurrentDictionary<string, double> DocsCoinPrecisionPrice;
        public static ConcurrentDictionary<string, double> DocsCoinPrecisionAmount;
        public static ConcurrentDictionary<string, double> DocsCoinGainPercentage;
        public static ConcurrentDictionary<string, double> DocsCoinGainPercentageSpecial;
        public static ConcurrentDictionary<string, double> DocsCoinOffsetForBuySellPriceOnOrders;
        public static ConcurrentDictionary<string, int> DocsCoinOnline;
        public static ConcurrentDictionary<string, bool> DocsLastUsedOnlineCoin;
        public static ConcurrentDictionary<string, double> DocsCoinMultipliers;
        public static ConcurrentDictionary<string, double> DocsCoinAccountValueMultiplier;
        public static ConcurrentDictionary<string, int> DocsCoinCountAllowTradeWhenSellTrend;
        public static ConcurrentDictionary<string, double> DocsLastClosePrice;
        public static ConcurrentDictionary<string, OrderResponse_asset> DocsAssets;


        public static int IsLoanAdjustActive { get; set; }
        public static decimal LoanApplicationValue{ get; set; }// = 5000 USDT
        public static decimal MaxBorrowableLoanValue { get; set; }// = 45000 USDT
        public static int IsForceSellActive { get; set; }
        public static int ForceSellActiveHr { get; set; }
        public const int DictResult1mCandlesticks1Count = 1000;
        public static decimal EachTradeMax = 0m;
        public static decimal PercentageForEachTrade = 80m / 100m;
        public static WorkerApi WorkerApi;
        public static int LongOrderStatus = (int)OrderStatus.Unset;

        public WorkerExecuter()
        {
            if (Docs == null)
            {
                Docs = new ConcurrentDictionary<BsonObjectId, Entry>();
            }
            if (DocsSell == null)
            {
                DocsSell = new ConcurrentDictionary<BsonObjectId, Entry>();
            }
            if (DocsBuy == null)
            {
                DocsBuy = new ConcurrentDictionary<BsonObjectId, Entry>();
            }            
            if (DocsSellST == null)
            {
                DocsSellST = new ConcurrentDictionary<BsonObjectId, Entry>();
            }            
            if (DocsCoinPairs == null)
            {
                DocsCoinPairs = new ConcurrentDictionary<string, string>();
            }
            if (DocsCoinTradeValueUSDT == null)
            {
                DocsCoinTradeValueUSDT = new ConcurrentDictionary<string, double>();
            }
            if (DocsCoinPrecisionPrice == null)
            {
                DocsCoinPrecisionPrice = new ConcurrentDictionary<string, double>();
            }
            if (DocsCoinPrecisionAmount == null)
            {
                DocsCoinPrecisionAmount = new ConcurrentDictionary<string, double>();
            }            
            if (DocsCoinGainPercentage == null)
            {
                DocsCoinGainPercentage = new ConcurrentDictionary<string, double>();
            }
            if (DocsCoinGainPercentageSpecial == null)
            {
                DocsCoinGainPercentageSpecial = new ConcurrentDictionary<string, double>();
            }            
            if (DocsCoinOffsetForBuySellPriceOnOrders == null)
            {
                DocsCoinOffsetForBuySellPriceOnOrders = new ConcurrentDictionary<string, double>();
            }
            if (DocsCoinOnline == null)
            {
                DocsCoinOnline = new ConcurrentDictionary<string, int>();
            }
            if (DocsLastUsedOnlineCoin == null)
            {
                DocsLastUsedOnlineCoin = new ConcurrentDictionary<string, bool>();
            }
            if (DocsCoinMultipliers == null)
            {
                DocsCoinMultipliers = new ConcurrentDictionary<string, double>();
            }
            if (DocsCoinAccountValueMultiplier == null)
            {
                DocsCoinAccountValueMultiplier = new ConcurrentDictionary<string, double>();
            }
            if (DocsCoinCountAllowTradeWhenSellTrend == null)
            {
                DocsCoinCountAllowTradeWhenSellTrend = new ConcurrentDictionary<string, int>();
            }
            if (DocsLastClosePrice == null)
            {
                DocsLastClosePrice = new ConcurrentDictionary<string, double>();
            }
            if (DocsAssets == null)
            {
                DocsAssets = new ConcurrentDictionary<string, OrderResponse_asset>();
            }
            



        CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;


            //initial time for insuff balance issue
            //this will be overriden from db setting
            ForceSellActiveHr = 12;
            Pub = "";
            Sec = "";
            MarketName = (int)MarketNames.NotImplementedYet;
            Fee = 0m;
            EachTradeMax = 0;
            MaxTimeframeForPriceAdjust = 1;
            LongOrderStatus = (int)Enums.OrderStatus.Unset;
            TradeType = "";
            StopLoss = 1;
        }


        public override void DoWork()
        {
            while (_isAlive)
            {
                if (Docs.Count < 1)
                {
                    continue;
                }
                if (OneTimeFlag == false)// Has not been fires yet, it will not be used for a second time
                {
                    OneTimeFlag = true;
                    HasToWait = false;
                }
                if (Pub.Length == 0 || Sec.Length == 0)
                {
                    Pub = "";
                    Sec = "";
                }
                if (WorkerApi == null && MarketName != (int)MarketNames.NotImplementedYet)
                {
                    switch (MarketName)
                    {
                        case (int)MarketNames.NotImplementedYet:
                            WorkerApi = null;
                            break;
                        case (int)MarketNames.Bitrue:
                            WorkerApi = new WorkerApiBitrue();
                            break;
                        case (int)MarketNames.KuCoin:
                            WorkerApi = new WorkerApiKuCoin();
                            break;
                        case (int)MarketNames.Binance:
                            WorkerApi = new WorkerApiBinance();
                            break;
                        case (int)MarketNames.Bitmex:
                            WorkerApi = new WorkerApiBitmex();
                            break;
                        case (int)MarketNames.Kraken:
                            WorkerApi = new WorkerApiKraken();
                            break;
                        case (int)MarketNames.FTX:
                            WorkerApi = new WorkerApiFTX();
                            break;
                        case (int)MarketNames.AAX:
                            WorkerApi = new WorkerApiAAX();
                            break;
                        default:
                            break;
                    }
                    
                }
                if (HasToWait == false && WorkerApi != null)
                {
                    HasToWait = true;
                    ConcurrentDictionary<BsonObjectId, Entry> docs;
                    lock (Docs)
                    {
                        docs = new ConcurrentDictionary<BsonObjectId, Entry>(Docs); 
                        Docs.Clear();
                    }
                    foreach (var _d in docs)
                    {
                        ProcessStep(_d.Value);
                    }
                    docs = null;
                    HasToWait = false;
                }
            }
        }
        public static void FillDirectoryDocsCoinPairs(string k, string v)
        {
            if (DocsCoinPairs == null)
            {
                return;
            }
            DocsCoinPairs[k] = v;
        }
        public static void FillDirectoryDocsCoinTradeValue(string k, double v)
        {
            if (DocsCoinTradeValueUSDT == null)
            {
                return;
            }
            DocsCoinTradeValueUSDT[k] = v;
        }
        public static void FillDirectoryDocsCoinPrecisionPrice(string k, double v)
        {
            if (DocsCoinPrecisionPrice == null)
            {
                return;
            }
            DocsCoinPrecisionPrice[k] = v;
        }
        public static void FillDirectoryDocsCoinPrecisionAmount(string k, double v)
        {
            if (DocsCoinPrecisionAmount == null)
            {
                return;
            }
            DocsCoinPrecisionAmount[k] = v;
        }
        public static void FillDirectoryDocsCoinGainPercentage(string k, double v)
        {
            if (DocsCoinGainPercentage == null)
            {
                return;
            }
            DocsCoinGainPercentage[k] = v;
        }
        public static void FillDirectoryDocsCoinGainPercentageSpecial(string k, double v)
        {
            if (DocsCoinGainPercentageSpecial == null)
            {
                return;
            }
            DocsCoinGainPercentageSpecial[k] = v;
        }        
        public static void FillDirectoryDocsCoinOffsetForBuySellPriceOnOrders(string k, double v)
        {
            if (DocsCoinOffsetForBuySellPriceOnOrders == null)
            {
                return;
            }
            DocsCoinOffsetForBuySellPriceOnOrders[k] = v;
        }
        public static void FillDirectoryDocsCoinOnline(string k, int v)
        {
            if (DocsCoinOnline == null)
            {
                return;
            }
            DocsCoinOnline[k] = v;
        }
        public static void FillDirectoryDocsLastUsedOnlineCoin(string k, bool v)
        {
            if (DocsLastUsedOnlineCoin == null)
            {
                return;
            }
            DocsLastUsedOnlineCoin[k] = v;
        }
        public static void FillDirectoryDocsCoinMultipliers(string k, double v)
        {
            if (DocsCoinMultipliers == null)
            {
                return;
            }
            DocsCoinMultipliers[k] = v;
        }
        public static void FillDirectoryDocsCoinAccountValueMultiplier(string k, double v)
        {
            if (DocsCoinAccountValueMultiplier == null)
            {
                return;
            }
            DocsCoinAccountValueMultiplier[k] = v;
        }
        public static void FillDirectoryDocsCoinCountAllowTradeWhenSellTrend(string k, int v)
        {
            if (DocsCoinCountAllowTradeWhenSellTrend == null)
            {
                return;
            }
            DocsCoinCountAllowTradeWhenSellTrend[k] = v;
        }
        public static void FillDirectoryDocsLastClosePrice(string k, double v)
        {
            if (DocsLastClosePrice == null)
            {
                return;
            }


            for (int i = 0; i < DocsCoinOnline.Count; i++)
            {
                string key = DocsCoinOnline.ElementAt(i).Key;
                int val = DocsCoinOnline.ElementAt(i).Value;
                if (val == (int)Boolean._true)
                {
                    DocsLastClosePrice[k] = v;
                }
                else
                {
                    DocsLastClosePrice.TryRemove(key, out _);
                }
            }
        }

        public static void FillDirectoryDocsAssets(string k, OrderResponse_asset a)
        {
            if (DocsAssets == null)
            {
                return;
            }
            DocsAssets[k] = a;
        }

    public static void FillDirectory(BsonDocument doc)
        {
            if (Docs == null)
            {
                return;
            }
            Entry entry = new Entry(doc);
            Docs[doc["_id"].AsObjectId] = entry;
        }
        public static void FillDirectorySell(BsonDocument doc)
        {
            if (DocsSell == null)
            {
                return;
            }
            Entry entry = new Entry(doc);
            DocsSell[doc["_id"].AsObjectId] = entry;
        }
        public static void FillDirectoryBuy(BsonDocument doc)
        {
            if (DocsBuy == null)
            {
                return;
            }
            Entry entry = new Entry(doc);
            DocsBuy[doc["_id"].AsObjectId] = entry;
        }
        

        public static void FillDirectorySellST(BsonDocument doc)
        {
            if (DocsSellST == null)
            {
                return;
            }
            Entry entry = new Entry(doc);
            DocsSellST[doc["_id"].AsObjectId] = entry;
        }
        private decimal ST_FindAmountSell()
        {
            decimal outputAmount = 0m;
            lock (DocsSellST)
            {
                ConcurrentDictionary<BsonObjectId, Entry> _docsSellST = new ConcurrentDictionary<BsonObjectId, Entry>(DocsSellST);
                foreach (var _d in _docsSellST)
                {
                    Entry e = _d.Value;
                    outputAmount = outputAmount + e.doc["filledSize"].ToDecimal();
                }
            }
            return outputAmount;
        }
        private void ProcessStep(Entry entry)
        {
            BsonDocument doc = entry.doc;
            if (IsOnline != (int)Boolean._true)
            {
                SetStatus(doc, StateMaschine.error_at_the_beginning_of_app);
            }

            string coin = doc["coin"].ToString();
            int i = DocsCoinOnline[coin];
            if (i != (int)Boolean._true)
            {
                SetStatus(doc, StateMaschine.error_coin_offline);
            }


            //ProcessStep
            switch (doc["status"].ToInt32())
            {
                case (int)StateMaschine.init:
                    if (doc["work"].ToInt32() == (int)WorkStatus.buy0 ||
                        doc["work"].ToInt32() == (int)WorkStatus.buy1 ||
                        doc["work"].ToInt32() == (int)WorkStatus.buy2 ||
                        doc["work"].ToInt32() == (int)WorkStatus.buy3 ||
                        doc["work"].ToInt32() == (int)WorkStatus.buy4)
                    {
                        SetStatus(doc, StateMaschine.buy);
                        break;
                    }
                    if (doc["work"].ToInt32() == (int)WorkStatus.sell0 ||
                        doc["work"].ToInt32() == (int)WorkStatus.sell1 ||
                        doc["work"].ToInt32() == (int)WorkStatus.sell2 ||
                        doc["work"].ToInt32() == (int)WorkStatus.sell3 ||
                        doc["work"].ToInt32() == (int)WorkStatus.sell4)
                    {
                        SetStatus(doc, StateMaschine.sell);
                        break;
                    }
                    else
                    {
                        SetStatus(doc, StateMaschine.not_yet_implemented);
                        SetComment(doc, "unknown");
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;

                //plot_0:1 long//action:buy = start, open position
                case (int)StateMaschine.buy:
                    try
                    {
                        switch (doc["work"].ToInt32())
                        {
                            //imp:done
                            //test:open
                            case (int)WorkStatus.buy0:
                                {
                                    Execute_buysell(doc, true);
                                }
                                break;
                            //imp:open
                            //test:open
                            case (int)WorkStatus.buy1:
                            case (int)WorkStatus.buy2:
                            case (int)WorkStatus.buy3:
                            case (int)WorkStatus.buy4:
                                {
                                    //first find the entries which can be sold
                                    SetStatus(doc, StateMaschine.find_querydb_open_longs); 
                                }
                                break;
                            default:
                                {
                                    SetComment(doc, "work_buy_not_set");
                                    SetStatus(doc, StateMaschine.work_buy_not_set);
                                }
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        SetComment(doc, e.Message + " catch");
                        SetStatus(doc, StateMaschine.buy_error);
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;



                case (int)StateMaschine.buy_api_results_after_trade_success:
                    try
                    {
                        Execute_api_call_result_after_trade(doc, true);
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.buy_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    break;

                case (int)StateMaschine.find_querydb_open_longs:
                    try
                    {
                        FindQueryDBOpenLongsAsync(doc).Wait();
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.buy_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;

                case (int)StateMaschine.buy_completed_fire_sell:
                    try
                    {
                        Execute_buy1234(doc);
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.sell_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;

                case (int)StateMaschine.fire_sell_completes_set_ids:
                    try
                    {
                        Fire_Sell_Completes_Set_ID(doc);
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.sell_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;

                    







                case (int)StateMaschine.sell:
                    try
                    {
                        switch (doc["work"].ToInt32())
                        {
                            case (int)WorkStatus.sell0:
                                {
                                    Execute_buysell(doc, false);
                                }
                                break;
                            //imp:open
                            //test:open
                            case (int)WorkStatus.sell1:
                            case (int)WorkStatus.sell2:
                            case (int)WorkStatus.sell3:
                            case (int)WorkStatus.sell4:
                                {
                                    //first find the entries which can be bought back
                                    SetStatus(doc, StateMaschine.find_querydb_open_shorts);
                                }
                                break;

                            default:
                                {
                                    SetComment(doc, "work_buy_not_set");
                                    SetStatus(doc, StateMaschine.work_buy_not_set);
                                }
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        SetComment(doc, e.Message + " catch");
                        SetStatus(doc, StateMaschine.buy_error);
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;


                case (int)StateMaschine.sell_api_results_after_trade_success:
                    try
                    {
                        Execute_api_call_result_after_trade(doc, false);
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.sell_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    break;

                case (int)StateMaschine.find_querydb_open_shorts:
                    try
                    {
                        FindQueryDBOpenShortsAsync(doc).Wait();
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.buy_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;

                case (int)StateMaschine.sell_completed_fire_buy:
                    try
                    {
                        Execute_sell1234(doc);
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.sell_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;


                case (int)StateMaschine.fire_buy_completes_set_ids:
                    try
                    {
                        Fire_Buy_Completes_Set_ID(doc);
                    }
                    catch (Exception e)
                    {
                        SetCountOfOpenSlots(doc, 0);
                        SetStatus(doc, StateMaschine.sell_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;


                    






                case (int)StateMaschine.executersettingupdate:
                    try
                    {
                        if (doc["ordAction"].ToString() == "buy" && doc["indicator"].ToString().Contains("alt.s7") == true)
                        {
                            //MultiplierInput = 1.5;
                            UpdateDBSettingsAsync(2, coin, "CoinMultipliers").Wait();
                        }
                        else if (doc["ordAction"].ToString() == "sell" && doc["indicator"].ToString().Contains("alt.s7") == true)
                        {
                            //MultiplierInput = 1;
                            UpdateDBSettingsAsync(1, coin, "CoinMultipliers").Wait();
                        }
                        SetStatus(doc, StateMaschine.executersettingupdatefinised);
                    }
                    catch (Exception e)
                    {
                        SetStatus(doc, StateMaschine.executersettingupdateerror);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;

   




               
                    //-----stoploss begin-------------------------------------
                case (int)StateMaschine.executer_check_stoploss:
                    try
                    {
                        //ST_ReadDBAsyncToSellAsync(doc).Wait();
                        SetStatus(doc, StateMaschine.executer_sell_stoploss);
                    }
                    catch (Exception e)
                    {
                        SetStatus(doc, StateMaschine.sell_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;

                case (int)StateMaschine.executer_sell_stoploss:
                    try
                    {
                        //ST_Execute_sell(doc);
                        SetStatus(doc, StateMaschine.st_sell_error_DocsSell_count_zero);
                    }
                    catch (Exception e)
                    {
                        SetStatus(doc, StateMaschine.sell_error);
                        Console.WriteLine(e.Message);
                        SetComment(doc, e.Message.ToString());
                    }
                    TimeLastRun = DateTime.UtcNow;
                    break;
                //-----stoploss end-------------------------------------

                    

                //bot is offline
                case (int)StateMaschine.error_at_the_beginning_of_app:
                    try
                    {
                        SetComment(doc, "bot (setting) offline");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                //coin is offline
                case (int)StateMaschine.error_coin_offline:
                    try
                    {
                        SetComment(doc, "coin (setting) offline");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                    
                default:
                    try
                    {
                        SetStatus(doc, StateMaschine.not_yet_implemented);
                        SetCountOfOpenSlots(doc, 0);
                        SetComment(doc, "default not_yet_implemented");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
            }


            try
            {
                doc["internal_task"] = 0;
                UpdateDBAsync(doc).Wait();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }


        #region executive_functions
        private decimal FindAmountSell()
        {
            decimal outputAmount = 0m;
            lock (DocsSell)
            {
                ConcurrentDictionary<BsonObjectId, Entry> _docsSell = new ConcurrentDictionary<BsonObjectId, Entry>(DocsSell);
                foreach (var _d in _docsSell)
                {
                    Entry e = _d.Value;
                    outputAmount = outputAmount + e.doc["filledSize"].ToDecimal();
                }
            }
            return outputAmount;
        }
        private decimal FindAmountBuy(BsonDocument doc)
        {
            decimal marktSellPrice = doc["ordPrice"].ToDecimal();
            decimal outputAmount = 0m;
            lock (DocsBuy)
            {
                ConcurrentDictionary<BsonObjectId, Entry> _docsBuy = new ConcurrentDictionary<BsonObjectId, Entry>(DocsBuy);
                foreach (var _d in _docsBuy)
                {
                    Entry e = _d.Value;
                    decimal localValue = e.doc["usedUSD"].ToDecimal() / marktSellPrice;
                    outputAmount = outputAmount + localValue;
                    //outputAmount = outputAmount + e.doc["filledSize"].ToDecimal();
                }
            }
            return outputAmount;
        }
        private void Execute_buysell(BsonDocument doc, bool buy)
        {
            try
            {
                string indicator = doc["indicator"].ToString();
                string coin = doc["coin"].ToString();
                string postfix = buy ? "long" : "short";
                string account = "alt4." + coin + "." + postfix;
                decimal _amount = doc["ordContracts"].ToDecimal();
                //decimal divider = (decimal)DocsCoinAccountValueMultiplier[coin];
                //_amount = _amount / divider;
                decimal _price = 0;
                string cp = DocsCoinPairs[coin].ToString();
                string direction = buy ? "buy" : "sell";
                string[] args = new string[9];
                args[0] = direction;
                args[1] = _price.ToString();
                args[2] = _amount.ToString();
                args[3] = Pub;
                args[4] = Sec;
                args[5] = Phrase;
                args[6] = cp;
                args[7] = account;
                args[8] = TradeType;
                Execute_api_call_buysell(args, doc, buy);
            }
            catch (Exception e)
            {
                SetComment(doc, e.Message + " catch");
                SetStatus(doc, StateMaschine.sell_error);
            }
            TimeLastRun = DateTime.UtcNow;
        }
        protected async Task FindQueryDBOpenLongsAsync(BsonDocument d)
        {
            if (_collection != null)
            {
                double ordPrice = d["ordPrice"].ToDouble();//from actual TP to close trade
                string coin = d["coin"].ToString();
                double p = DocsCoinGainPercentage[coin];

                //double lowerPrice = d["avgFillPrice"].IsBsonNull ? d["lastclose"].ToDouble() : d["avgFillPrice"].ToDouble();
                double minPriceAfterFee =
                                ordPrice - // buy price in last buy trade
                                (ordPrice * ((double)Fee * 2) / 100) - // fee from buy trade
                                (ordPrice * p / 100);
                var filter = "{ status: {'$gte':600, '$lte':600, '$exists':true, '$ne':null}, internal_task : 0, coin : '" + coin + "', avgFillPrice_n: {'$lte': " + minPriceAfterFee + ", '$exists':true} }";
                try
                {
                    DocsSell.Clear();
                    var docs = await _collection.Find(filter).ToListAsync();
                    if (docs.Count > 0)
                    {
                        foreach (var doc in docs)
                        {
                            doc["internal_task"] = 1;
                            doc["status"] = (int)StateMaschine.open_long_success_overtaken;
                            FillDirectorySell(doc);
                            UpdateDBAsync(doc).Wait();
                        }
                        SetStatus(d, StateMaschine.buy_completed_fire_sell);
                    }
                    else
                    {
                        //price very low
                        SetStatus(d, StateMaschine.buy_success);
                        SetComment(d, "Sell triggered: " + ordPrice + " no entry lower then: " + minPriceAfterFee);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void Execute_buy1234(BsonDocument doc)
        {
            try
            {
                string indicator = doc["indicator"].ToString();
                string coin = doc["coin"].ToString();
                string account = "alt4." + coin + ".long";
                decimal _amount = FindAmountSell(); 
                //decimal divider = (decimal)DocsCoinAccountValueMultiplier[coin];
                //_amount = _amount / divider;
                decimal _price = 0;
                string cp = DocsCoinPairs[coin].ToString();
                string[] args = new string[9];
                args[0] = "sell";
                args[1] = _price.ToString();
                args[2] = _amount.ToString();
                args[3] = Pub;
                args[4] = Sec;
                args[5] = Phrase;
                args[6] = cp;
                args[7] = account;
                args[8] = TradeType;
                Execute_api_call_buysell_TP(args, doc, false);
            }
            catch (Exception e)
            {
                SetComment(doc, e.Message + " catch");
                SetStatus(doc, StateMaschine.sell_error);
            }
            TimeLastRun = DateTime.UtcNow;
        }
        private void Fire_Sell_Completes_Set_ID(BsonDocument d)
        {
            if (DocsSell == null)
            {
                SetCountOfOpenSlots(d, 0);
                SetStatus(d, StateMaschine.sell_error_DocsSell_null);
                SetComment(d, "DocsSell null, error_sell_completed_set_orderids_fom_buy");
            }
            if (DocsSell.Count() <= 0)
            {
                SetCountOfOpenSlots(d, 0);
                SetStatus(d, StateMaschine.sell_error_DocsSell_count_zero);
                SetComment(d, "DocsSell zero element, error_sell_completed_set_orderids_fom_buy");
            }


            ConcurrentDictionary<BsonObjectId, Entry> docs;
            lock (DocsSell)
            {
                docs = new ConcurrentDictionary<BsonObjectId, Entry>(DocsSell);
                DocsSell.Clear();

                foreach (var _d in docs)
                {
                    BsonDocument docsell = new BsonDocument();

                    docsell = _d.Value.doc;
                    string orderid_to_search = docsell["order_id"].ToString();
                    string orderid_from_sell = d["order_id"].ToString();

                    UpdateOrderInAfterSellAsync(orderid_to_search, orderid_from_sell).Wait();
                }
            }
            docs = null;

            SetStatus(d, StateMaschine.close_long_success);
        }



        protected async Task FindQueryDBOpenShortsAsync(BsonDocument d)
        {
            if (_collection != null)
            {
                double ordPrice = d["ordPrice"].ToDouble();//from actual TP to close trade
                string coin = d["coin"].ToString();
                double p = DocsCoinGainPercentage[coin];

                //double lowerPrice = d["avgFillPrice"].IsBsonNull ? d["lastclose"].ToDouble() : d["avgFillPrice"].ToDouble();
                double maxPriceAfterFee =
                                ordPrice + // sell price in last sell trade
                                (ordPrice * ((double)Fee * 2) / 100);// + // fee from sell trade
                                //(ordPrice * p / 100);
                var filter = "{ status: {'$gte':700, '$lte':700, '$exists':true, '$ne':null}, internal_task : 0, coin : '" + coin + "', avgFillPrice_n: {'$gte': " + maxPriceAfterFee + ", '$exists':true} }";
                try
                {
                    DocsBuy.Clear();
                    var docs = await _collection.Find(filter).ToListAsync();
                    if (docs.Count > 0)
                    {
                        foreach (var doc in docs)
                        {
                            doc["internal_task"] = 1;
                            doc["status"] = (int)StateMaschine.open_short_success_overtaken;
                            FillDirectoryBuy(doc);
                            UpdateDBAsync(doc).Wait();
                        }
                        SetStatus(d, StateMaschine.sell_completed_fire_buy);
                    }
                    else
                    {
                        //price very high
                        SetStatus(d, StateMaschine.sell_success);
                        SetComment(d, "Buy triggered: " + ordPrice + " no entry higher then: " + maxPriceAfterFee);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                /*
                filter = "{ status: {'$gte':700, '$lte':700, '$exists':true, '$ne':null}, internal_task : 0, coin : '" + coin + "' }";
                try
                {
                    var docs = await _collection.Find(filter).ToListAsync();
                    if (docs.Count > 0)
                    {
                        foreach (var doc in docs)
                        {
                            doc["internal_task"] = 0;
                            doc["status"] = (int)StateMaschine.sell_success;
                            SetComment(doc, "Buy triggered: " + ordPrice + " no entry higher then: " + maxPriceAfterFee);
                            UpdateDBAsync(doc).Wait();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                */
            }
        }
        private void Execute_sell1234(BsonDocument doc)
        {
            try
            {
                string indicator = doc["indicator"].ToString();
                string coin = doc["coin"].ToString();
                string account = "alt4." + coin + ".short";

                decimal _amount = FindAmountBuy(doc);
                //decimal divider = (decimal)DocsCoinAccountValueMultiplier[coin];
                //_amount = _amount / divider;
                decimal tickSize = (decimal)DocsCoinPrecisionAmount[coin];
                var outputQty = Common.FloorPrice(tickSize, _amount);

                decimal _price = 0;
                string cp = DocsCoinPairs[coin].ToString();
                string[] args = new string[9];
                args[0] = "buy";
                args[1] = _price.ToString();
                args[2] = outputQty.ToString();
                args[3] = Pub;
                args[4] = Sec;
                args[5] = Phrase;
                args[6] = cp;
                args[7] = account;
                args[8] = TradeType;
                Execute_api_call_buysell_TP(args, doc, true);
            }
            catch (Exception e)
            {
                SetComment(doc, e.Message + " catch");
                SetStatus(doc, StateMaschine.buy_error);
            }
            TimeLastRun = DateTime.UtcNow;
        }
        private void Fire_Buy_Completes_Set_ID(BsonDocument d)
        {

            if (DocsBuy == null)
            {
                SetCountOfOpenSlots(d, 0);
                SetStatus(d, StateMaschine.buy_error);
                SetComment(d, "DocsSell null, error_sell_completed_set_orderids_fom_buy");
            }
            if (DocsBuy.Count() <= 0)
            {
                SetCountOfOpenSlots(d, 0);
                SetStatus(d, StateMaschine.buy_error);
                SetComment(d, "DocsSell zero element, error_sell_completed_set_orderids_fom_buy");
            }


            ConcurrentDictionary<BsonObjectId, Entry> docs;
            lock (DocsBuy)
            {
                docs = new ConcurrentDictionary<BsonObjectId, Entry>(DocsBuy);
                DocsBuy.Clear();

                foreach (var _d in docs)
                {
                    BsonDocument docbuy = new BsonDocument();

                    docbuy = _d.Value.doc;
                    string orderid_to_search = docbuy["order_id"].ToString();
                    string orderid_from_sell = d["order_id"].ToString();

                    UpdateOrderInAfterBuyAsync(orderid_to_search, orderid_from_sell).Wait();
                }
            }
            docs = null;

            SetStatus(d, StateMaschine.close_short_success);
        }
        



        private void Execute_api_call_buysell(string[] a, BsonDocument doc, bool bBuy)
        {
            try
            {
                string[] args = a;
                string res = WorkerApi.ApiSell(args);
                res = res.Replace("\r", string.Empty).Replace("\n", string.Empty);
                res = res.Replace("None", "null");
                res = res.Replace("False", "false").Replace("True", "true");
                if (res == "error sell" || res == "" || res == "error buy")
                {
                    SetComment(doc, res);
                    SetStatus(doc, StateMaschine.error_api_call);
                    TimeLastRun = DateTime.UtcNow;
                    return;
                }
                string[] array = res.Split('{');
                res = array[1];
                res = "{" + res;

                var request = new ApiResponse.FTX.OrderResponse_buysell();
                request = JsonConvert.DeserializeObject<ApiResponse.FTX.OrderResponse_buysell>(res);
                if (request.id.ToString() != "")
                {
                    if (bBuy == true)
                    {
                        SetStatus(doc, StateMaschine.buy_api_results_after_trade_success);
                        SetComment(doc, "buy_api_results_after_trade_success");
                    }
                    else
                    {
                        SetStatus(doc, StateMaschine.sell_api_results_after_trade_success);
                        SetComment(doc, "sell_api_results_after_trade_success");
                    }
                    SetOrderNumberBuySell(doc, request);
                }
                else
                {
                    if (bBuy == true)
                    {
                        SetComment(doc, "errorbuy");
                        SetStatus(doc, StateMaschine.buy_error);
                    }
                    else
                    {
                        SetComment(doc, "errorsell");
                        SetStatus(doc, StateMaschine.sell_error);
                    }
                }

            }
            catch (Exception e)
            {
                SetComment(doc, e.Message + " catch");
                if (bBuy == true)
                {
                    SetStatus(doc, StateMaschine.buy_error);
                }
                else
                {
                    SetStatus(doc, StateMaschine.sell_error);
                }
            }


        }
        private void Execute_api_call_buysell_TP(string[] a, BsonDocument doc, bool bBuy)
        {
            try
            {
                string[] args = a;
                string res = WorkerApi.ApiSell(args);
                res = res.Replace("\r", string.Empty).Replace("\n", string.Empty);
                res = res.Replace("None", "null");
                res = res.Replace("False", "false").Replace("True", "true");
                if (res == "error sell" || res == "" || res == "error buy")
                {
                    SetComment(doc, res);
                    SetStatus(doc, StateMaschine.error_api_call);
                    TimeLastRun = DateTime.UtcNow;
                    return;
                }
                string[] array = res.Split('{');
                res = array[1];
                res = "{" + res;

                var request = new ApiResponse.FTX.OrderResponse_buysell();
                request = JsonConvert.DeserializeObject<ApiResponse.FTX.OrderResponse_buysell>(res);
                if (request.id.ToString() != "")
                {
                    if (bBuy == true)
                    {
                        //to close a short trade
                        SetStatus(doc, StateMaschine.fire_buy_completes_set_ids);
                        SetComment(doc, "fire_buy_completes_set_ids");
                    }
                    else
                    {
                        //to close a long trade
                        SetStatus(doc, StateMaschine.fire_sell_completes_set_ids);
                        SetComment(doc, "fire_sell_completes_set_ids");
                    }
                    SetOrderNumberBuySell(doc, request);
                }
                else
                {
                    if (bBuy == true)
                    {
                        SetComment(doc, "errorbuy");
                        SetStatus(doc, StateMaschine.buy_error);
                    }
                    else
                    {
                        SetComment(doc, "errorsell");
                        SetStatus(doc, StateMaschine.sell_error);
                    }
                }

            }
            catch (Exception e)
            {
                SetComment(doc, e.Message + " catch");
                if (bBuy == true)
                {
                    SetStatus(doc, StateMaschine.buy_error);
                }
                else
                {
                    SetStatus(doc, StateMaschine.sell_error);
                }
            }

        }
        private void Execute_api_call_result_after_trade(BsonDocument doc, bool bBuy)
        {
            if (TimeLastRun.AddSeconds(WaitingTimeInSeconds) <= DateTime.UtcNow)
            {
                try
                {
                    string coin = doc["coin"].ToString();
                    string direction = bBuy ? "long" : "short";
                    string subacc = "alt4." + coin + "." + direction;

                    string[] args = new string[9];
                    args[0] = "check";
                    args[1] = doc["order_id"].ToString();
                    args[2] = "h";
                    args[3] = Pub;
                    args[4] = Sec;
                    args[5] = Phrase;
                    args[6] = "h";
                    args[7] = subacc;
                    args[8] = "h";
                    string res = WorkerApi.ApiCheck(args);
                    res = res.Replace("\r", string.Empty).Replace("\n", string.Empty);
                    res = res.Replace("None", "null");
                    res = res.Replace("False", "false").Replace("True", "true");

                    string[] array = res.Split('{');
                    res = array[1];
                    res = "{" + res;

                    var request = new ApiResponse.FTX.OrderResponse_buysell();
                    request = JsonConvert.DeserializeObject<ApiResponse.FTX.OrderResponse_buysell>(res);
                    if (request != null &&
                        request.status == "closed" &&
                        request.avgFillPrice != null)
                    {
                        string comment = "";
                        if (bBuy == true)
                        {
                            SetStatus(doc, StateMaschine.open_long_success);
                            comment = "open_long_success";
                        }
                        else
                        {
                            SetStatus(doc, StateMaschine.open_short_success);
                            comment = "open_short_success";
                        }
                        SetBuySellFilledValues(doc, request);
                        SetComment(doc, comment);
                    }
                    else
                    {
                        string comment = "buy/check error";
                        StateMaschine state = StateMaschine.buy_error;
                        if (bBuy == false)
                        {
                            comment = "sell/check error";
                            state = StateMaschine.sell_error;
                        }
                        SetComment(doc, comment);
                        SetStatus(doc, state);
                    }
                    TimeLastRun = DateTime.UtcNow;
                }
                catch (Exception e)
                {
                    SetStatus(doc, StateMaschine.sell_timeout_error);
                    Console.WriteLine(e.Message);
                    SetComment(doc, e.Message.ToString());
                    TimeLastRun = DateTime.UtcNow;
                }
            }
        }        
        #endregion



















        private void SetComment(BsonDocument d, string s) =>
            d["comment"] = s;
        private void SetStatus(BsonDocument d, StateMaschine value) =>
            d["status"] = value;
        private void SetCountOfOpenSlots(BsonDocument d, int i) =>
            d["openslot"] = i;
        private void SetOrderNumberBuySell(BsonDocument d, ApiResponse.Bitrue.OrderResponse_buysell r) =>
            d["order_id"] = r.orderId.ToString(); 
        private void SetOrderNumberBuySell(BsonDocument d, ApiResponse.Binance.OrderResponse_buysell r) =>
            d["order_id"] = r.orderId.ToString(); 
        private void SetOrderNumberBuySell(BsonDocument d, ApiResponse.KuCoin.OrderResponse_buysell r) =>
            d["order_id"] = r.orderId.ToString();
        private void SetOrderNumberBuySell(BsonDocument d, ApiResponse.FTX.OrderResponse_buysell r) =>
            d["order_id"] = r.id.ToString();
        private void SetBuySellFilledValues(BsonDocument d, ApiResponse.FTX.OrderResponse_buysell r)
        {
            //after buy
            if (r.avgFillPrice == null) { d["avgFillPrice"] = 0; } else { d["avgFillPrice"] = r.avgFillPrice; }
            double avgFillPrice_n = d["avgFillPrice"].ToDouble();
            d["avgFillPrice_n"] = avgFillPrice_n;
            d["filledSize"] = r.filledSize;
            //as USD
            if (r.price == null) { d["price"] = d["lastclose"]; } else { d["price"] = r.price; }
            if (r.remainingSize == null) { d["remainingSize"] = 0; } else { d["remainingSize"] = r.remainingSize; }
            d["size"] = r.size;//as coin

            //used USD for this trade
            double size = d["size"].ToDouble();
            double usedUSD = size * avgFillPrice_n;
            d["usedUSD"] = usedUSD;

            d["createdAt"] = r.createdAt;
            d["filledAt"] = DateTime.UtcNow;

            //


            //d["ord_price"] = d["lastclose"];//as usdt
            //d["ord_executed_value"] = r.size;//as coin
            //decimal dd = d["ord_executed_value"].ToDecimal() * d["ord_price"].ToDecimal();//in usdt
            //d["ord_executed_value"] = String.Format("{0:0.0000000000}", dd);//in usdt
            //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //DateTime date = start.AddMilliseconds(r.time).ToLocalTime();
            //d["ord_created_date"] = r.createdAt;
        }
    }
}
