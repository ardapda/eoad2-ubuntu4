using Executer.Workers;
using System.Threading;

namespace Executer
{
    class Program
    {
        //Worker Threads
        private static WorkerDatabase workerDatabaseObject;
        private static Thread workerDatabaseThread;
        private static WorkerExecuter workerExecuterObject;
        private static Thread workerExecuterThread;
        private static WorkerOnline workerOnlineObject;
        private static Thread workerOnlineThread;

        private static WorkerTVSignals workerTVSignalsObject;
        private static Thread workerTVSignalsThread;
        

        static void Main(string[] args)
        {
            InitializeThreads();
        }

        private static void InitializeThreads()
        {
            //--------------------------------------------------------------
            //Workers

            //online client
            workerOnlineObject = new WorkerOnline();
            workerOnlineObject.ConnectDB();
            workerOnlineObject.IsAlive = false;
            workerOnlineThread = new Thread(workerOnlineObject.DoWork);
            workerOnlineThread.Start();
            Thread.Sleep(1);


            //executer long
            workerExecuterObject = new WorkerExecuter();
            workerExecuterObject.IsAlive = true;
            workerExecuterThread = new Thread(workerExecuterObject.DoWork);
            workerExecuterThread.Start();
            Thread.Sleep(1);

            //reader
            workerDatabaseObject = new WorkerDatabase();
            workerDatabaseObject.ConnectDB();
            //workerDatabaseObject.ResetInternalTask();
            //workerDatabaseObject.CorrectionDateTimeFormat();
            workerDatabaseObject.IsAlive = true;
            workerDatabaseThread = new Thread(workerDatabaseObject.DoWork);
            workerDatabaseThread.Start();
            Thread.Sleep(1);

            
            //tvsignals
            workerTVSignalsObject = new WorkerTVSignals();
            workerTVSignalsObject.IsAlive = true;
            workerTVSignalsThread = new Thread(workerTVSignalsObject.DoWork);
            workerTVSignalsThread.Start();
            Thread.Sleep(1);

        }
    }
}
