using ExchangeSharp;
using Executer.Core;
using Executer.Documents;
using MongoDB.Bson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executer.Workers
{
    abstract class Worker
    {
        public Worker()
        { }
        protected volatile bool _isAlive;
        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }
        abstract public void DoWork();
        abstract public void DoWork2();
        abstract public void DoWork3();
        protected volatile bool _hasToWait = true;
        public bool HasToWait
        {
            get { return _hasToWait; }
            set { _hasToWait = value; }
        }
        protected volatile bool _oneTimeFlag = false; // Start indicator
        public bool OneTimeFlag
        {
            get { return _oneTimeFlag; }
            set { _oneTimeFlag = value; }
        }
    }
}