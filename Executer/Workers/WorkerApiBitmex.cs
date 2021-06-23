using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Executer.Workers
{
    class WorkerApiBitmex : WorkerApi
    {
        private string pytonbin = string.Empty;
        private static string localIP = string.Empty;
        private static ProcessStartInfo PSI;
        public WorkerApiBitmex()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            if (localIP.Contains("<your_ip_here>"))
            {
                pytonbin = @"/usr/bin/python3";//ubuntu Python-Interpreter
            }
            else
            {
                Console.WriteLine("no valid ip localIP: " + localIP);
                throw new NotImplementedException();
            }
            Console.WriteLine("localIP: " + localIP);


            if (PSI == null)
            {
                PSI = new ProcessStartInfo(pytonbin);
                PSI.RedirectStandardInput = false;
                PSI.RedirectStandardOutput = true;
                PSI.UseShellExecute = false;
                PSI.CreateNoWindow = true;
            }
        }
        public override void DoWork()
        {
            while (IsAlive)
            {
            }
        }
        public override void DoWork2()
        { }
        public override void DoWork3()
        { }
        public override string ApiBuy(string[] args)
        {
            return RunFromCmd(args);
        }
        public override string ApiSell(string[] args)
        {
            return RunFromCmd(args);
        }
        public override string ApiCheck(string[] args)
        {
            return RunFromCmd(args);
        }
        public override string ApiBalance(string[] args)
        {
            return RunFromCmd(args);
        }
        public static string RunFromCmd(string[] args)
        {
            string result = string.Empty;
            string script = string.Empty;
            if (localIP.Contains("<your_ip_here>"))
            {
                script = @"/usr/local/lib/python3.8/dist-packages/ccxt/executebin.py";//ubuntu
            }
            else
            {
                Console.WriteLine("no valid ip localIP: " + localIP);
                Environment.Exit(0);
            }
            if (args.Length != 6)
            {
                Console.WriteLine("not valid api call: " + args);
                throw new NotImplementedException();
            }
            if (PSI == null)
            {
                return result;
            }

            try
            {
                PSI.Arguments = string.Format("{0} {1} {2} {3} {4} {5} {6}", script, args[0], args[1], args[2], args[3], args[4], args[5]);
                using (var proc = new Process())
                {
                    proc.StartInfo = PSI;
                    proc.Start();
                    proc.WaitForExit();
                    if (proc.ExitCode == 0)
                    {
                        result = proc.StandardOutput.ReadToEnd();
                    }
                    else
                    {
                        proc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("R Script failed: " + result, ex);
            }
            return result;
        }

    }
}