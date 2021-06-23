using Executer.Core;
using System;
using System.Collections.Generic;

namespace Executer.Documents
{
    public class Wave
    {
        public Wave()
        {
            Signals = new SortedDictionary<DateTime, Signal>();
            CurrentState = (int)Enums.WaveTrend.stable;
            LastTimeUpdated = DateTime.Now.AddHours(-1);//Substract time from server
        }
        //public DateTime LastDateTime { get; set; }
        public SortedDictionary<DateTime, Signal> Signals { get; set; }
        public int CurrentState { get; set; }
        public DateTime LastTimeUpdated { get; set; }
        public void WaveLastTimeUpdated() { LastTimeUpdated = DateTime.Now.AddHours(-1); }
        public void AddSignal(DateTime dt, Signal s)
        {
            Signals[dt] = s;
            WaveLastTimeUpdated();
        }
    }
}
