using Binance.Net.Objects;
using Executer.Documents;
using Executer.Enums;
using MathNet.Numerics.LinearRegression;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Executer.Core
{
    class Common
    {
        public static int salve1 = 0;
        public static int salve2 = 0;
        public static int salve3 = 0;
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("dd-MM-yyyy HH:mm:ss.ffff");
        }
        public static DateTime GetDateTimeFromString(string s)
        {
            //"Tue Jan 01 2019 09:20:11 GMT+0100 (CET)"
            //jan feb mar apr may jun jul aug sep oct nov dec
            //out: yyyy-MM-dd HH:mm:ss

            string sformated = "1970-01-01 00:00:00";

            DateTime dt = DateTime.ParseExact(sformated, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            string _month = "00";
            switch (s.Substring(4, 3).ToUpper())
            {
                case "JAN": _month = "01"; break;
                case "FEB": _month = "02"; break;
                case "MAR": _month = "03"; break;
                case "APR": _month = "04"; break;
                case "MAY": _month = "05"; break;
                case "JUN": _month = "06"; break;
                case "JUL": _month = "07"; break;
                case "AUG": _month = "08"; break;
                case "SEP": _month = "09"; break;
                case "OCT": _month = "10"; break;
                case "NOV": _month = "11"; break;
                case "DEC": _month = "12"; break;
                default:    _month = "00"; break;
            }

            sformated = s.Substring(11, 4) + "-";           //yyyy
            sformated += _month + "-";                      //MM
            sformated += s.Substring(8, 2) + " ";           //dd
            sformated += s.Substring(16, 2) + ":";          //HH
            sformated += s.Substring(19, 2) + ":";          //mm
            sformated += s.Substring(22, 2);                //ss

            dt = DateTime.ParseExact(sformated, "yyyy-MM-dd HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture);

            return dt;
        }
        public static ObjectId GetTimestampId(DateTime value)
        {
            string _s = value.ToString("yyyyMMddHHmmssffff");
            ObjectId _id = ObjectId.Parse(_s);
            return _id;
        }
        public static DateTime GetDateTimeFromMillisec(long l)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(l);
        }
        public static bool IsPositive<T>(T value) where T : struct, IComparable<T>
        {
            return value.CompareTo(default(T)) > 0;
        }
        public const int Sec60 = 60;
        public static decimal GetSmallestAmount(decimal d1, decimal d2, decimal d3)
        {
            decimal d = 0;
            if (d1 < d2) { d = d1; } else { d = d2; }
            if (d3 < d) { d = d3; }
            return d;
        }
        public static int GetMultiplier(decimal p)
        {
            switch ((int)p)
            {
                case 1:
                    return 10;
                case 2:
                    return 100;
                case 3:
                    return 1000;
                case 4:
                    return 10000;
                case 5:
                    return 100000;
                case 6:
                    return 1000000;
                default:
                    return 1;
            }
        }
        public static void Logger(Entry e)
        {
            Entry _e = e;

            //var v = e.doc["status"].ToInt32();
            Console.WriteLine("Timestamp: " + GetTimestamp(DateTime.Now) + " order #: "
                + " _id: " + e.doc["_id"]
                + " status: " + e.doc["status"]
                + " order_id: " + e.doc["order_id"]
                + " n_times: " + e.doc["n_times"]
                );
        }

        public static decimal ClampQuantity(decimal minQuantity, decimal maxQuantity, decimal stepSize, decimal quantity)
        {
            quantity = Math.Min(maxQuantity, quantity);
            quantity = Math.Max(minQuantity, quantity);
            quantity -= quantity % stepSize;
            quantity = Floor(quantity);
            return quantity;
        }

        public static decimal ClampPrice(decimal minPrice, decimal maxPrice, decimal price)
        {
            price = Math.Min(maxPrice, price);
            price = Math.Max(minPrice, price);
            return price;
        }



        public static decimal FloorPrice(decimal tickSize, decimal price)
        {
            price -= price % tickSize;
            price = Floor(price);
            return price;
        }

        private static decimal Floor(decimal number)
        {
            return Math.Floor(number * 100000000) / 100000000;
        }
        public static int GetPredictionPreisDirection(int slaveNumber, decimal weightedValue)
        {
            int nRet = 0;
            switch (slaveNumber)
            {
                //Slave 1
                case 1:// PredictionSlaveLineChart
                    {
                        decimal wv = weightedValue;

                        if (wv >= 10)
                        {
                            nRet = (int)PriceTrend.upfastfast;
                        }
                        else if (wv >= 1 && wv < 10m)
                        {
                            nRet = (int)PriceTrend.upfast;
                        }
                        else if (wv >= 0.25m && wv < 1)
                        {
                            nRet = (int)PriceTrend.up;
                        }
                        else if (wv >= 0.02m && wv < 0.15m)
                        {
                            nRet = (int)PriceTrend.upslow;
                        }
                        else if (wv > -0.02m && wv < 0.02m)
                        {
                            nRet = (int)PriceTrend.stable;
                        }
                        else if (wv <= -0.02m && wv > -0.15m)
                        {
                            nRet = (int)PriceTrend.downslow;
                        }
                        else if (wv <= -0.15m && wv > -1)
                        {
                            nRet = (int)PriceTrend.down;
                        }
                        else if (wv <= -1 && wv > -10)
                        {
                            nRet = (int)PriceTrend.downfast;
                        }
                        else if (wv <= -10)
                        {
                            nRet = (int)PriceTrend.downfastfast;
                        }
                    }
                    break;


                //Slave 2
                //Slave 3
                //Slave 4
                case 2:
                case 3:
                case 4:
                    {
                        int wv = (int)weightedValue;

                        if (wv == (int)WaveTrend.searchingbuy)
                        {
                            nRet = (int)PriceTrend.downfast;
                        }
                        else if (wv == (int)WaveTrend.sell)
                        {
                            nRet = (int)PriceTrend.down;
                        }
                        else if (wv == (int)WaveTrend.searchingsell)
                        {
                            nRet = (int)PriceTrend.upfast;
                        }
                        else if (wv == (int)WaveTrend.buy)
                        {
                            nRet = (int)PriceTrend.up;
                        }
                        else
                        {
                            //Debug.Assert(false);
                        }
                    }
                    break;


                //Slave 5
                case 5:// PredictionSlaveMACD
                    {
                        decimal wv = weightedValue;

                        if (wv >= 1 && wv < 10m)
                        {
                            nRet = (int)PriceTrend.upfast;
                        }
                        else if (wv > -0.02m && wv < 0.02m)
                        {
                            nRet = (int)PriceTrend.stable;
                        }
                        else if (wv <= 1 && wv > -10)
                        {
                            nRet = (int)PriceTrend.downfast;
                        }
                    }
                    break;

                default:
                    break;
            }
            return nRet;
        }



        public static int Max(int x, int y, int z)
        {
            return Math.Max(x, Math.Max(y, z));
        }
        public static decimal Max(decimal x, decimal y, decimal z)
        {
            return Math.Max(x, Math.Max(y, z));
        }



        #region Slave Methods
        public static SortedDictionary<DateTime, BinanceKline> SortoutOldElements(int maxSize, SortedDictionary<DateTime, BinanceKline> Dict)
        {
            int tooMuch = Dict.Count - maxSize;
            for (int i = maxSize; i < maxSize + tooMuch; i++)
            {
                var lastkey = Dict.Keys.Last();
                Dict.Remove(lastkey);
            }
            return Dict;
        }
        public static SortedDictionary<DateTime, SQZMOMIndicator> SortoutOldElements(int maxSize, SortedDictionary<DateTime, SQZMOMIndicator> Dict)
        {
            int tooMuch = Dict.Count - maxSize;
            for (int i = 0; i < tooMuch; i++)
            {
                var lastkey = Dict.Keys.Last();
                Dict.Remove(lastkey);
            }
            return Dict;
        }
        public static SortedDictionary<DateTime, MACDIndicator> SortoutOldElements(int maxSize, SortedDictionary<DateTime, MACDIndicator> Dict)
        {
            for (int i = maxSize; i < Dict.Count; i++)
            {
                var lastkey = Dict.Keys.Last();
                Dict.Remove(lastkey);
            }
            return Dict;
        }
        public static SortedDictionary<DateTime, SQZMOMIndicator> CalculateAndInsertSMA(
            int SlaveNumber,
            int BBLength, 
            decimal BBMultFactor, 
            int KCLength, 
            decimal KCMultFactor,
            SortedDictionary<DateTime, BinanceKline> SortedDictBinanceKlineReverse)
        {
            if (SortedDictBinanceKlineReverse.Count < BBLength)
            {
                return null;
            }
            // Calculate BB
            //basis = sma(source, length)
            var dictBasis = Get_N_MinsMovingAverage(BBLength, SortedDictBinanceKlineReverse);
            //dev = mult * stdev(source, length)
            var dictStdDev = Get_N_StandardDeviation(BBLength, BBMultFactor, SortedDictBinanceKlineReverse);
            //upperBB = basis + dev
            var dictUpperBB = Get_N_UpperLowerBB(BBLength, SortedDictBinanceKlineReverse, dictBasis, dictStdDev, 0);
            //lowerBB = basis - dev
            var dictLowerBB = Get_N_UpperLowerBB(BBLength, SortedDictBinanceKlineReverse, dictBasis, dictStdDev, 1);
            //---------------------------------------------------------------------------------------------
            // Calculate KC
            //ma = sma(source, lengthKC)
            var dictMA = Get_N_MinsMovingAverage(KCLength, SortedDictBinanceKlineReverse);
            //range = useTrueRange ? tr_t : (h - l)
            var dictRange = Fill_stTrueRange(SortedDictBinanceKlineReverse);
            //rangema = sma(range, lengthKC)
            var dictRangeMA = Get_N_MinsMovingAverage(KCLength, dictRange);
            //upperKC = ma + rangema * multKC
            var dictUpperKC = Get_N_UpperLowerKC(KCLength, KCMultFactor, dictMA, dictRangeMA, 0);//0 = +, upperKC
            //lowerKC = ma - rangema * multKC
            var dictLowerKC = Get_N_UpperLowerKC(KCLength, KCMultFactor, dictMA, dictRangeMA, 1);//1 = -, lowerKC
            //---------------------------------------------------------------------------------------------
            //sqzOn = (lowerBB > lowerKC) and(upperBB < upperKC)
            var dictSqzOn = Get_N_SqzOnOff(BBLength, dictLowerBB, dictLowerKC, dictUpperBB, dictUpperKC, 1);
            //sqzOff = (lowerBB < lowerKC) and(upperBB > upperKC)
            var dictSqzOff = Get_N_SqzOnOff(BBLength, dictLowerBB, dictLowerKC, dictUpperBB, dictUpperKC, 0);
            //noSqz = (sqzOn == false) and(sqzOff == false)
            var dictNoSqz = Get_N_NoSqz(BBLength, dictSqzOn, dictSqzOff); //Notice! noSqz is true, if no squeeze is found!
            //---------------------------------------------------------------------------------------------
            //val = linreg(source - avg(avg(highest(h, lengthKC), lowest(l, lengthKC)), sma(source, lengthKC)), lengthKC, 0)
            //The result of this function is calculated using the formula: 
            //linreg = intercept + slope * (length - 1 - offset), //where;
            //intercept and slope are the values calculated with the least squares method on source series (x argument),
            //length is the y argument, (lengthKC)
            //offset is the z argument, (0).
            //x = source - avg(avg(highest(h, lengthKC), lowest(l, lengthKC)), sma(source, lengthKC))
            var x = Get_N_PreLinReg(KCLength, SortedDictBinanceKlineReverse);
            /*
                for (int i = 0; i < x.Length; i++)
                {
                    Console.WriteLine("dt: " + SortedDictBinanceKlineReverse.ElementAt(i).Key +
                        " x: " + x[i]);
                }
            */
            var y = KCLength;
            int z = 0;
            //var dictLinReg = Get_N_LinRegLib1(x, y, z);//Linefit
            var dictLinReg = Get_N_LinRegLib2(SortedDictBinanceKlineReverse, x, y, z);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Ebuu'ylan test (validity if input data)
            //Tasso'ylan lib test (tel)
            /*
            if (SlaveNumber == 4)
            {
                for (int i = 0; i < dictLinReg.Count; i++)
                {
                    Console.WriteLine("dt: " + dictLinReg.ElementAt(i).Key +
                        " histogram: " + dictLinReg.ElementAt(i).Value.histogram +
                        " yintercept: " + dictLinReg.ElementAt(i).Value.yintercept +
                        " slope: " + dictLinReg.ElementAt(i).Value.slope +
                        " rsquared: " + dictLinReg.ElementAt(i).Value.rsquared);
                }
            }
            */
            
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //bcolor = iff( val > 0, 
            //                      iff(val > nz(val[1]), lime, green),
            //                                                          iff(val < nz(val[1]), red, maroon))
            var dictHistogramColor = Get_N_HistogramColor(dictLinReg);

            //scolor = noSqz ? blue : sqzOn ? black : gray
            var dictSqueezeColor = Get_N_SqueezeColor(dictNoSqz, dictSqzOn, dictSqzOff);

            var dictSqueezeMomentum = Get_N_SortedDictSqueezeMomentum(
                SortedDictBinanceKlineReverse, 
                SlaveNumber,
                dictLinReg, 
                dictHistogramColor, 
                dictSqueezeColor);
            if (dictSqueezeMomentum == null)
            {
                Environment.Exit(SlaveNumber);
            }
            return dictSqueezeMomentum;
        }

        private static SortedDictionary<DateTime, LinearRegression> Get_N_LinRegLib2(
            SortedDictionary<DateTime, BinanceKline> sortedDictBinanceKlineReverse, 
            decimal[] x, int y, int z)
        {
            //linreg = intercept + slope * (length - 1 - offset), //where;
            //intercept and slope are the values calculated with the least squares method on source series (x argument)

            SortedDictionary<DateTime, BinanceKline> dictY1 = new SortedDictionary<DateTime, BinanceKline>();
            SortedDictionary<DateTime, decimal> dictY2 = new SortedDictionary<DateTime, decimal>();
            int c = 0;
            foreach (var item in sortedDictBinanceKlineReverse)
            {
                DateTime dt = item.Key;
                dictY1.Add(dt, item.Value);
                decimal d = x.ElementAt(c);
                dictY2.Add(dt, d);
                c++;
            }

            SortedDictionary<DateTime, LinearRegression> SortedDictValOfLinReg = new SortedDictionary<DateTime, LinearRegression>();

            for (int counter = y; counter < dictY2.Count; counter++)
            {
                decimal[] input1 = new decimal[y];
                decimal[] input2 = new decimal[y];
                int innerLoopCounter = 0;
                int index = counter;
                while (innerLoopCounter < y)
                {
                    input1[innerLoopCounter] = dictY2.ElementAt(index - y).Value;
                    input2[innerLoopCounter] = innerLoopCounter;
                    innerLoopCounter += 1;
                    index += 1;
                }
                decimal rsquared = 0m;
                decimal yintercept = 0m;
                decimal slope = 0m;
                //LinearRegression2(input1, input2, 0, input2.Length, out rsquared, out yintercept, out slope);
                LinearRegression2(input2, input1, 0, input2.Length, out rsquared, out yintercept, out slope);
                var val = yintercept + slope * (y - 1 - z);

                LinearRegression lr = new LinearRegression();
                lr.DateTime = dictY1.ElementAt(counter - 1).Key;
                lr.rsquared = rsquared;
                lr.yintercept = yintercept;
                lr.slope = slope;
                lr.histogram = val;
                SortedDictValOfLinReg[lr.DateTime] = lr;
            }            
            /*
            for (int counter = 0; counter <= y; counter++)
            {
                LinearRegression lr = new LinearRegression();
                lr.DateTime = dictY2.ElementAt(counter).Key;
                lr.rsquared = 0m;
                lr.yintercept = 0m;
                lr.slope = 0m;
                lr.histogram = 0m;
                SortedDictValOfLinReg[lr.DateTime] = lr;
            }*/
            
            return SortedDictValOfLinReg;
        }
        /// <summary>
        /// ///////DONT CALL THIS FUNCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        private static decimal[] Get_N_LinRegLib1(decimal[] x, int y, int z)
        {

            /// ///////DONT CALL THIS FUNCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            /// 


            //linreg = intercept + slope * (length - 1 - offset), //where;
            //intercept and slope are the values calculated with the least squares method on source series (x argument)
            decimal[] avgValues = new decimal[x.Length];
            for (int counter = x.Length - y - 1; counter >= 0; counter--)
            {
                double[] input1 = new double[y];
                double[] input2 = new double[y];
                int innerLoopCounter = 0;
                int index = counter;
                while (innerLoopCounter < y)
                {
                    /// ///////DONT CALL THIS FUNCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    /// /// ///////DONT CALL THIS FUNCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    /// /// ///////DONT CALL THIS FUNCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    input1[innerLoopCounter] = (double)x.ElementAt(index + y);
                    input2[innerLoopCounter] = innerLoopCounter;
                    innerLoopCounter += 1;
                    index -= 1;
                }
                var val = Line(input1, input2);
                var val2 = val.Item1 + val.Item2 * (y - 1 - z);
                avgValues[counter] = (decimal)val2;
            }
            return avgValues;


            /// ///////DONT CALL THIS FUNCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            /// 

        }
        /// <summary>
        /// Least-Squares fitting the points (x,y) to a line through origin y : x -> b*x,
        /// returning a function y' for the best fitting line.
        /// </summary>
        public static Func<double, double> LineThroughOriginFunc(double[] x, double[] y)
        {
            double slope = SimpleRegression.FitThroughOrigin(x, y);
            return z => slope * z;
        }
        public static double LineThroughOrigin(double[] x, double[] y)
        {
            return SimpleRegression.FitThroughOrigin(x, y);
        }
        /// <summary>
        /// Least-Squares fitting the points (x,y) to a line y : x -> a+b*x,
        /// returning its best fitting parameters as [a, b] array,
        /// where a is the intercept and b the slope.
        /// </summary>
        public static Tuple<double, double> Line(double[] x, double[] y)
        {
            return SimpleRegression.Fit(x, y);
        }
        /// <summary>
        /// Least-Squares fitting the points (x,y) to a line y : x -> a+b*x,
        /// returning a function y' for the best fitting line.
        /// </summary>
        public static Func<double, double> LineFunc(double[] x, double[] y)
        {
            var parameters = SimpleRegression.Fit(x, y);
            double intercept = parameters.Item1, slope = parameters.Item2;
            return z => intercept + slope * z;
        }
        public static SortedDictionary<DateTime, SQZMOMIndicator> Get_N_SortedDictSqueezeMomentum(
            SortedDictionary<DateTime, BinanceKline> DictBinanceKlineReverse,
            int SlaveNumber,
            SortedDictionary<DateTime, LinearRegression> DictSM,
            SortedDictionary<DateTime, int> DictHC, 
            int[] DictSC)
        {
            SortedDictionary<DateTime, SQZMOMIndicator> _dictSM = 
                new SortedDictionary<DateTime, SQZMOMIndicator>(new ReverseComparer<DateTime>(Comparer<DateTime>.Default));

            for (int counter = 0; counter < DictHC.Count; counter++)
            {
                SQZMOMIndicator sm = new SQZMOMIndicator();
                sm.SlaveNumber = SlaveNumber;
                sm.DateTime = DictSM.ElementAt(counter).Key;
                sm.LinearRegression = DictSM.ElementAt(counter).Value;
                sm.LineColor = DictHC.ElementAt(counter).Value;
                sm.SqueezeColor = DictSC.ElementAt(counter);
                sm.CurrentPrice = DictBinanceKlineReverse[sm.DateTime].Close;

                _dictSM.Add(DictSM.ElementAt(counter).Key, sm);
            }

            //Console.WriteLine(DateTime.Now + ", Slave number: " + SlaveNumber);

            return _dictSM;
        }
        public static decimal[] Fill_stTrueRange(SortedDictionary<DateTime, BinanceKline> Dict)
        {
            //True range. It is max(high - low, abs(high - close[prev]), abs(low - close[prev]))
            decimal[] avgValues = new decimal[Dict.Count];
            for (int counter = Dict.Count - 2; counter >= 0; counter--)
            {
                //high - low
                decimal value1 = Dict.ElementAt(counter).Value.High - Dict.ElementAt(counter).Value.Low;
                //abs(high - close[prev])
                decimal value2 = Math.Abs(Dict.ElementAt(counter).Value.High - Dict.ElementAt(counter + 1).Value.Close);
                //abs(low - close[prev])
                decimal value3 = Math.Abs(Dict.ElementAt(counter).Value.Low - Dict.ElementAt(counter + 1).Value.Close);
                //max(high - low, abs(high - close[prev]), abs(low - close[prev]))
                decimal maxValue = Max(value1, value2, value3);
                avgValues[counter] = maxValue;
            }
            return avgValues;
        }
        public static decimal[] Get_N_MinsMovingAverage(int frameSize, SortedDictionary<DateTime, BinanceKline> Dict)
        {
            //The sma function returns the moving average, that is the sum of last y values of x, divided by y.
            decimal sum = 0;
            decimal[] avgValues = new decimal[Dict.Count];
            for (int counter = Dict.Count - frameSize; counter >= 0; counter--)
            {
                int innerLoopCounter = 0;
                int index = counter;
                while (innerLoopCounter < frameSize)
                {
                    sum += Dict.ElementAt(index).Value.Close;
                    innerLoopCounter += 1;
                    index += 1;
                }
                avgValues[counter] = sum / frameSize;
                sum = 0;
            }
            return avgValues;
        }
        public static decimal[] Get_N_MinsMovingAverage(int frameSize, decimal[] Dict)
        {
            //rangema = sma(range, lengthKC)
            decimal sum = 0;
            decimal[] avgValues = new decimal[Dict.Length];
            for (int counter = Dict.Length - frameSize; counter >= 0; counter--)
            {
                int innerLoopCounter = 0;
                int index = counter;
                while (innerLoopCounter < frameSize)
                {
                    sum += Dict.ElementAt(index);
                    innerLoopCounter += 1;
                    index += 1;
                }
                avgValues[counter] = sum / frameSize;
                sum = 0;
            }
            return avgValues;
        }

        public static decimal[] Get_N_StandardDeviation(int frameSize, decimal multiplier, SortedDictionary<DateTime, BinanceKline> Dict)
        {
            //dev = mult * stdev(source, length)
            decimal[] avgValues = new decimal[Dict.Count];
            for (int counter = Dict.Count - frameSize; counter >= 0; counter--)
            {
                decimal[] input1 = new decimal[frameSize];
                int innerLoopCounter = 0;
                int index = counter;
                while (innerLoopCounter < frameSize)
                {
                    input1[innerLoopCounter] = (Dict.ElementAt(index).Value.Close);
                    innerLoopCounter += 1;
                    index += 1;
                }
                decimal average = input1.Average();
                decimal sumOfSquaresOfDifferences = input1.Select(val => (val - average) * (val - average)).Sum();
                avgValues[counter] = multiplier * (decimal)Math.Sqrt((double)sumOfSquaresOfDifferences / input1.Length);
            }
            return avgValues;
        }
        public static decimal[] Get_N_UpperLowerBB(
            int frameSize, 
            SortedDictionary<DateTime, BinanceKline> Dict, 
            decimal[] dictBasis, 
            decimal[] dictStdDev, 
            int sign)
        {
            decimal sum = 0;
            decimal[] avgValues = new decimal[Dict.Count];
            for (int counter = 0; counter <= Dict.Count - frameSize; counter++)
            {
                if (sign == 0)//0, upperBB = basis + dev
                {
                    sum = (dictBasis.ElementAt(counter) + dictStdDev.ElementAt(counter));
                }
                else//1, lowerBB = basis - dev
                {
                    sum = (dictBasis.ElementAt(counter) - dictStdDev.ElementAt(counter));
                }
                avgValues[counter] = sum;
                sum = 0;
            }
            return avgValues;
        }
        public static decimal[] Get_N_UpperLowerKC(int frameSize, decimal factor, decimal[] dictMA, decimal[] dictRangeMA, int sign)
        {
            if (dictMA == null || dictRangeMA == null)
            {
                return null;
            }
            //upperKC = ma + rangema * multKC
            //lowerKC = ma - rangema * multKC
            decimal sum = 0;
            decimal[] avgValues = new decimal[dictMA.Length];
            for (int counter = 0; counter < dictMA.Length; counter++)
            {
                if (sign == 0)//0, upperKC = ma + rangema * multKC
                {
                    sum = (dictMA.ElementAt(counter) + (dictRangeMA.ElementAt(counter) * factor));
                }
                else//1, lowerKC = ma - rangema * multKC
                {
                    sum = (dictMA.ElementAt(counter) - (dictRangeMA.ElementAt(counter) * factor));
                }
                avgValues[counter] = sum;
                sum = 0;
            }
            return avgValues;
        }
        public static bool[] Get_N_SqzOnOff(
            int frameSize, 
            decimal[] dictLowerBB, 
            decimal[] dictLowerKC, 
            decimal[] dictUpperBB, 
            decimal[] dictUpperKC, 
            int sign)
        {
            //1, sqzOn = (lowerBB > lowerKC) and(upperBB < upperKC)
            //0, sqzOff = (lowerBB < lowerKC) and(upperBB > upperKC)
            bool b = false;
            bool[] avgValues = new bool[dictLowerKC.Length];
            for (int counter = 0; counter < dictLowerKC.Length; counter++)
            {
                if (sign == 1)//1, sqzOn = (lowerBB > lowerKC) and(upperBB < upperKC)
                {
                    if ((dictLowerBB.ElementAt(counter) > dictLowerKC.ElementAt(counter)) && (dictUpperBB.ElementAt(counter) < dictUpperKC.ElementAt(counter)))
                    {
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }
                }
                else//0, sqzOff = (lowerBB < lowerKC) and(upperBB > upperKC)
                {
                    if ((dictLowerBB.ElementAt(counter) < dictLowerKC.ElementAt(counter)) && (dictUpperBB.ElementAt(counter) > dictUpperKC.ElementAt(counter)))
                    {
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }
                }
                avgValues[counter] = b;
            }
            return avgValues;
        }
        public static bool[] Get_N_NoSqz(int frameSize, bool[] dictSqzOn, bool[] dictSqzOff)
        {
            //noSqz = (sqzOn == false) and(sqzOff == false)
            bool b = true;
            bool[] avgValues = new bool[dictSqzOn.Length];
            for (int counter = 0; counter < dictSqzOn.Length; counter++)
            {
                if ((dictSqzOn.ElementAt(counter) == false) && (dictSqzOff.ElementAt(counter) == false))
                {
                    b = true;
                }
                else
                {
                    b = false;
                }
                avgValues[counter] = b;
            }
            return avgValues;
        }
        public static decimal[] Get_N_PreLinReg(int frameSize, SortedDictionary<DateTime, BinanceKline> Dict)
        {
            //for x calc: linreg = intercept + slope * (length - 1 - offset), //where;
            //intercept and slope are the values calculated with the least squares method on source series (x argument)

            //x = linreg(source - avg(avg(highest(h, lengthKC), lowest(l, lengthKC)), sma(source, lengthKC)))
            //x = source - avg(avg(i3, i2), i1)
            // i1 = sma(source, lengthKC)
            // i2 = lowest(l, lengthKC)   
            // i3 = highest(h, lengthKC)
            //x = source - avg(avg(i3, i2), i1)
            //x = source - avg(i4, i1)
            // i4 = avg(i3, i2)
            //x = source - i5
            // i5 = avg(i4, i1)
            //x = source - i5
            // i6 = Subtraction(source - i5)
            decimal[] avgValues = new decimal[Dict.Count];
            var i1 = Get_N_MinsMovingAverage(frameSize, Dict);
            var i2 = Get_N_LowestLow(frameSize, Dict);
            var i3 = Get_N_HighestHigh(frameSize, Dict);
            var i4 = Get_N_Average(i3, i2);
            var i5 = Get_N_Average(i4, i1);
            var i6 = Get_N_Subtraction(Dict, i5, frameSize);
            return avgValues = i6;
        }
        public static decimal[] Get_N_LowestLow(int frameSize, SortedDictionary<DateTime, BinanceKline> Dict)
        {
            decimal lowest = 0m;
            decimal[] temp = new decimal[frameSize];
            decimal[] avgValues = new decimal[Dict.Count];
            for (int counter = Dict.Count - frameSize; counter >= 0 ; counter--)
            {
                int innerLoopCounter = 0;
                int index = counter;
                while (innerLoopCounter < frameSize)
                {
                    temp[innerLoopCounter] = Dict.ElementAt(index).Value.Low;
                    innerLoopCounter += 1;
                    index += 1;
                }
                lowest = temp.Min();
                avgValues[counter] = lowest;
                Array.Clear(temp, 0, temp.Length);
                lowest = 0;
            }
            return avgValues;
        }
        public static decimal[] Get_N_HighestHigh(int frameSize, SortedDictionary<DateTime, BinanceKline> Dict)
        {
            decimal highest = 0m;
            decimal[] temp = new decimal[frameSize];
            decimal[] avgValues = new decimal[Dict.Count];
            for (int counter = Dict.Count - frameSize; counter >= 0 ; counter--)
            {
                int innerLoopCounter = 0;
                int index = counter;
                while (innerLoopCounter < frameSize)
                {
                    temp[innerLoopCounter] = Dict.ElementAt(index).Value.High;
                    innerLoopCounter += 1;
                    index += 1;
                }
                highest = temp.Max();
                avgValues[counter] = highest;
                Array.Clear(temp, 0, temp.Length);
                highest = 0;
            }
            return avgValues;
        }
        public static decimal[] Get_N_Average(decimal[] array1, decimal[] array2)
        {
            decimal[] avgValues = new decimal[array1.Length];
            for (int counter = array1.Length - 1; counter >= 0; counter--)
            {
                avgValues[counter] = (array1[counter] + array2[counter]) / 2;
            }
            return avgValues;
        }
        public static decimal[] Get_N_Subtraction(SortedDictionary<DateTime, BinanceKline> Dict, decimal[] i5, int frameSize)
        {
            decimal[] avgValues = new decimal[i5.Length];
            for (int counter = i5.Length - frameSize; counter >= 0; counter--)
            {
                avgValues[counter] = Dict.ElementAt(counter).Value.Close - i5[counter];
            }
            return avgValues;
        }
        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        /// <param name="xVals">The x-axis values.</param>
        /// <param name="yVals">The y-axis values.</param>
        /// <param name="inclusiveStart">The inclusive inclusiveStart index.</param>
        /// <param name="exclusiveEnd">The exclusive exclusiveEnd index.</param>
        /// <param name="rsquared">The r^2 value of the line.</param>
        /// <param name="yintercept">The y-intercept value of the line (i.e. y = ax + b, yintercept is b).</param>
        /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
        public static void LinearRegression(decimal[] xVals, decimal[] yVals,
                                            int inclusiveStart, int exclusiveEnd,
                                            out decimal rsquared, out decimal yintercept,
                                            out decimal slope)
        {
            Debug.Assert(xVals.Length == yVals.Length);
            decimal sumOfX = 0;
            decimal sumOfY = 0;
            decimal sumOfXSq = 0;
            decimal sumOfYSq = 0;
            decimal ssX = 0;
            decimal ssY = 0;
            decimal sumCodeviates = 0;
            decimal sCo = 0;
            decimal count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                decimal x = xVals[ctr];
                decimal y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            decimal RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            decimal RDenom = (count * sumOfXSq - (sumOfX * sumOfX))
             * (count * sumOfYSq - (sumOfY * sumOfY));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            decimal meanX = sumOfX / count;
            decimal meanY = sumOfY / count;
            double d = (double)RDenom;
            if (d != 0)
            {
                decimal dblR = RNumerator / (decimal)(Math.Sqrt(d));
                rsquared = dblR * dblR;
                yintercept = meanY - ((sCo / ssX) * meanX);
                slope = sCo / ssX;
            }
            else
            {
                rsquared = 0;
                yintercept = 0;
                slope = 0;
            }

        }
        public static void LinearRegression2(decimal[] xVals, decimal[] yVals,
                                    int inclusiveStart, int exclusiveEnd,
                                    out decimal rsquared, out decimal yintercept,
                                    out decimal slope)
        {
            Debug.Assert(xVals.Length == yVals.Length);
            decimal sumOfX = 0;
            decimal sumOfY = 0;
            decimal sumOfXSq = 0;
            decimal sumOfYSq = 0;
            decimal ssX = 0;
            decimal ssY = 0;
            decimal sumCodeviates = 0;
            decimal sCo = 0;
            decimal count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                decimal x = xVals[ctr];
                decimal y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            decimal RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            decimal RDenom = (count * sumOfXSq - (sumOfX * sumOfX))
             * (count * sumOfYSq - (sumOfY * sumOfY));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            decimal meanX = sumOfX / count;
            decimal meanY = sumOfY / count;
            double d = (double)RDenom;
            if (d != 0)
            {
                decimal dblR = RNumerator / (decimal)(Math.Sqrt(d));
                rsquared = dblR * dblR;
                yintercept = meanY - ((sCo / ssX) * meanX);
                slope = sCo / ssX;

                //y anchors
                //double y1 = yintercept + (slope * (CurrentBar - startBarsAgo)); // y @ start
                //double y2 = yintercept + (slope * (CurrentBar - endBarsAgo)); // y @ end
            }
            else
            {
                rsquared = 0;
                yintercept = 0;
                slope = 0;
            }
        }
        public static SortedDictionary<DateTime, int> Get_N_HistogramColor(SortedDictionary<DateTime, LinearRegression> Dict)
        {
            //bcolor = iff( val > 0, 
            //                      iff(val > nz(val[1]), lime, green),
            //                                                          iff(val < nz(val[1]), red, maroon))
            int color = 0;

            SortedDictionary<DateTime, int> avgReturn= new SortedDictionary<DateTime, int>();

            int[] avgValues = new int[Dict.Count];

            for (int counter = 1; counter < Dict.Count; counter++)
            {
                if (Dict.ElementAt(counter).Value.histogram > 0m)
                {
                    if (Dict.ElementAt(counter).Value.histogram > Dict.ElementAt(counter - 1).Value.histogram)
                    {
                        color = (int)Enums.ColorSqzMom.GreenLight;
                    }
                    else
                    {
                        color = (int)Enums.ColorSqzMom.GreenDark;
                    }
                }
                else
                {
                    if (Dict.ElementAt(counter).Value.histogram < Dict.ElementAt(counter - 1).Value.histogram)
                    {
                        color = (int)Enums.ColorSqzMom.RedLight;
                    }
                    else
                    {
                        color = (int)Enums.ColorSqzMom.RedDark;
                    }
                }
                avgReturn.Add(Dict.ElementAt(counter).Key, color);
                color = 0;
            }
            avgReturn.Add(Dict.ElementAt(0).Key, 0);
            return avgReturn;
        }
        public static int[] Get_N_SqueezeColor(bool[] dictNoSqz, bool[] dictSqzOn, bool[] dictSqzOff)
        {
            //scolor = noSqz ? blue : sqzOn ? black : gray 
            int color = 0;
            int[] avgValues = new int[dictNoSqz.Length];
            for (int counter = 0; counter < dictNoSqz.Length; counter++)
            {
                if (dictNoSqz.ElementAt(counter) == true)
                {
                    color = (int)Enums.ColorSqzMom.Blue;
                }
                else
                {
                    if (dictSqzOn.ElementAt(counter) == true)
                    {
                        color = (int)Enums.ColorSqzMom.Black;
                    }
                    else
                    {
                        color = (int)Enums.ColorSqzMom.Grey;
                    }
                }
                avgValues[counter] = color;
                color = 0;
            }
            return avgValues;
        }
        #endregion
        #region Wave        
        internal static int IsSearchingSell(DateTime dt, SortedDictionary<DateTime, SQZMOMIndicator> Dict)
        {
            DateTime currentDT = dt;
            DateTime previousOneDT = dt.AddMinutes(-1);
            DateTime previousTwoDT = dt.AddMinutes(-2);
            var current = Dict[currentDT];
            var previousOne = Dict[previousOneDT];
            var previousTwo = Dict[previousTwoDT];
            decimal currentDeltaHistogram = current.LinearRegression.histogram - previousOne.LinearRegression.histogram;
            decimal previousDeltaHistogram = previousOne.LinearRegression.histogram - previousTwo.LinearRegression.histogram;

            int ret = (int)Enums.Boolean._unset;
            if (current.LineColor == (int)ColorSqzMom.GreenLight)
            {
                ret = (int)Enums.Boolean._true;
            }
            else
            {
                ret = (int)Enums.Boolean._false;
            }
            return ret;
        }
        internal static int IsSell(DateTime dt, SortedDictionary<DateTime, SQZMOMIndicator> Dict)
        {
            DateTime currentDT = dt;            
            DateTime previousOneDT = dt.AddMinutes(-1);
            DateTime previousTwoDT = dt.AddMinutes(-2);            
            var current = Dict[currentDT];
            var previousOne = Dict[previousOneDT];
            var previousTwo = Dict[previousTwoDT];
            decimal currentDeltaHistogram = current.LinearRegression.histogram - previousOne.LinearRegression.histogram;
            decimal previousDeltaHistogram = previousOne.LinearRegression.histogram - previousTwo.LinearRegression.histogram;
            
            int ret = (int)Enums.Boolean._unset;
            if ((current.LineColor == (int)ColorSqzMom.GreenDark))
            {
                if (currentDeltaHistogram <= previousDeltaHistogram)
                {
                    ret = (int)Enums.Boolean._false;
                }
                else
                {
                    ret = (int)Enums.Boolean._true;
                }
            }
            else
            {
                if (current.LinearRegression.histogram > 0 && currentDeltaHistogram <= previousDeltaHistogram)
                {
                    ret = (int)Enums.Boolean._true;
                }
                else
                {
                    ret = (int)Enums.Boolean._false;
                }
            }
            return ret;
        }
        internal static int IsSearchingBuy(DateTime dt, SortedDictionary<DateTime, SQZMOMIndicator> Dict)
        {
            DateTime currentDT = dt;
            DateTime previousOneDT = dt.AddMinutes(-1);
            DateTime previousTwoDT = dt.AddMinutes(-2);
            var current = Dict[currentDT];
            var previousOne = Dict[previousOneDT];
            var previousTwo = Dict[previousTwoDT];
            decimal currentDeltaHistogram = current.LinearRegression.histogram - previousOne.LinearRegression.histogram;
            decimal previousDeltaHistogram = previousOne.LinearRegression.histogram - previousTwo.LinearRegression.histogram;

            int ret = (int)Enums.Boolean._unset;
            if (current.LineColor == (int)ColorSqzMom.RedLight)
            {
                ret = (int)Enums.Boolean._true;
            }
            else
            {
                ret = (int)Enums.Boolean._false;
            }
            return ret;
        }
        internal static int IsBuy(DateTime dt, SortedDictionary<DateTime, SQZMOMIndicator> Dict)
        {
            DateTime currentDT = dt;
            DateTime previousOneDT = dt.AddMinutes(-1);
            DateTime previousTwoDT = dt.AddMinutes(-2);
            var current = Dict[currentDT];
            var previousOne = Dict[previousOneDT];
            var previousTwo = Dict[previousTwoDT];
            decimal currentDeltaHistogram = current.LinearRegression.histogram - previousOne.LinearRegression.histogram;
            decimal previousDeltaHistogram = previousOne.LinearRegression.histogram - previousTwo.LinearRegression.histogram;

            int ret = (int)Enums.Boolean._unset;
            if (current.LineColor == (int)ColorSqzMom.RedDark)
            {
                if (currentDeltaHistogram <= previousDeltaHistogram)
                {
                    ret = (int)Enums.Boolean._false;
                }
                else
                {
                    ret = (int)Enums.Boolean._true;
                }
            }
            else
            {
                if (current.LinearRegression.histogram < 0 && currentDeltaHistogram <= previousDeltaHistogram)
                {
                    ret = (int)Enums.Boolean._true;
                }
                else
                {
                    ret = (int)Enums.Boolean._false;
                }
            }
            return ret;
        }
        internal static decimal GetDeltaHistograme(DateTime dt, SortedDictionary<DateTime, SQZMOMIndicator> Dict)
        {
            DateTime currentDT = dt;
            DateTime previousOneDT = dt.AddMinutes(-1);
            var current = Dict[currentDT];
            var previousOne = Dict[previousOneDT];
            return (current.LinearRegression.histogram - previousOne.LinearRegression.histogram);
        }
        internal static int GetWaveTrend(Wave wave)
        {
            var IsSearchingSell = wave.Signals.Last().Value.SearchingSell;
            var IsSell = wave.Signals.Last().Value.Sell;
            var IsSearchingBuy = wave.Signals.Last().Value.SearchingBuy;
            var IsBuy = wave.Signals.Last().Value.Buy;

            int ret = (int)WaveTrend.stable;
            if (IsSearchingSell == (int)Enums.Boolean._true)
            {
                ret = (int)WaveTrend.searchingsell;
            }
            else if (IsSell == (int)Enums.Boolean._true)
            {
                ret = (int)WaveTrend.sell;
            }
            else if(IsSearchingBuy == (int)Enums.Boolean._true)
            {
                ret = (int)WaveTrend.searchingbuy;
            }
            else if(IsBuy == (int)Enums.Boolean._true)
            {
                ret = (int)WaveTrend.buy;
            }
            else
            {
                //Debug.Assert(false);
            }

            return ret;
        }
        internal static Wave UpdateWave(SortedDictionary<DateTime, SQZMOMIndicator> Dict, Wave wave)
        {
            for (int i = 0; i < Dict.Count - 2; i++)
            {
                //Update Wave
                var dt = Dict.ElementAt(i).Key;

                Signal s = new Signal();
                s.SearchingSell = IsSearchingSell(dt, Dict);
                s.Sell = IsSell(dt, Dict);
                s.SearchingBuy = IsSearchingBuy(dt, Dict);
                s.Buy = IsBuy(dt, Dict);
                s.DeltaHistograme = GetDeltaHistograme(dt, Dict);

                wave.AddSignal(dt, s);
            }
            wave.CurrentState = GetWaveTrend(wave);
            if (wave.CurrentState == (int)WaveTrend.stable)
            {
                //Debug.Assert(false);
            }
            return wave;
        }
        internal static Wave SortoutOldElements(int maxSize, Wave Dict)
        {
            int tooMuch = Dict.Signals.Count() - maxSize;
            for (int i = maxSize; i < maxSize + tooMuch; i++)
            {
                var lastkey = Dict.Signals.Keys.First();
                Dict.Signals.Remove(lastkey);
            }
            return Dict;
        }
        #endregion
        public static decimal UptoTwoDecimalPoints(decimal num)
        {
            var totalCost = Convert.ToDecimal(string.Format("{0:0.00}", num));
            return totalCost;
        }

        public const int ITERATIONS = 10000; // number of pbkdf2 iterations
        public static byte[] Encrypt(string clearText)
        {
            // https://stackoverflow.com/questions/42892719/comparing-hashed-password-with-salt-always-fails

            byte[] pass = new byte[] { 0x14, 0x16, 0x65, 0x7e, 0x25, 0x4d, 0x45, 0x64, 0x77, 0x85, 0x64, 0x25, 0x16 };
            byte[] salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4e, 0x65, 0x24, 0x76, 0x65, 0x34, 0x65, 0x76 };
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pass, salt, ITERATIONS);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
        }
        public static string Decrypt(byte[] input)
        {
            byte[] pass = new byte[] { 0x14, 0x16, 0x65, 0x7e, 0x25, 0x4d, 0x45, 0x64, 0x77, 0x85, 0x64, 0x25, 0x16 };
            byte[] salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4e, 0x65, 0x24, 0x76, 0x65, 0x34, 0x65, 0x76 };
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pass, salt, ITERATIONS);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(input, 0, input.Length);
                        cs.Close();
                    }
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }
    }
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> original;

        public ReverseComparer(IComparer<T> original)
        {
            //Apply validation rule(s), if needed
            this.original = original;
        }

        public int Compare(T left, T right)
        {
            return original.Compare(right, left);
        }
    }
    
}
