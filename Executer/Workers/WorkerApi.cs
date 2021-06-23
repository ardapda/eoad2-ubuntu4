using Executer.Enums;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Executer.Workers
{
    class WorkerApi : Worker
    {
        public WorkerApi()
        {        }

        public virtual string ApiBuy(string[] args) { return ""; }
        public virtual string ApiSell(string[] args) { return ""; }
        public virtual string ApiCheck(string[] args) { return ""; }
        public virtual string ApiBalance(string[] args) { return ""; }
        public virtual string ApiOrderCancel(string[] args) { return ""; }
        public virtual string ApiTicker(string[] args) { return ""; }
        


        public override void DoWork()
        {
            throw new NotImplementedException();
        }
        public override void DoWork2()
        {
            throw new NotImplementedException();
        }
        public override void DoWork3()
        {
            throw new NotImplementedException();
        }

        

    }
}