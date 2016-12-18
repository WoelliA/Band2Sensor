using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Util;
using Microsoft.Band.Portable;

namespace Band2Sensor
{
    public static class Program
    {
        private static bool isCheckRunning;
        static List<IDisposable> loggers = new List<IDisposable>();
        private static BandClient _bandClient;

        public static void Disconnect()
        {
            isCheckRunning = false;
            _bandClient?.DisconnectAsync();
            
        }

        public static async Task RunAsync() //da muss alles rein
        {
            try
            {
                var bandConnector = new BandConnector();
                _bandClient = await bandConnector.ConnectToBandAsync();

            }
            catch (Exception e)
            {
                Log.Error("Programm", e.Message);
            }

            if (_bandClient != null && _bandClient.IsConnected == true)
            {
                foreach (var disposable in loggers)
                {
                    disposable.Dispose();

                }
                loggers.Clear();

                var heartRateLogger = new HeartRateLogger(_bandClient, CreateLineWriter("heart"));
                loggers.Add(heartRateLogger);
                await heartRateLogger.StartAsync();
                var tempLogger = new TempLogger(_bandClient, CreateLineWriter("temp"));
                loggers.Add(tempLogger);
                await tempLogger.StartAsync();
                var gsrLogger = new GsrLogger(_bandClient, CreateLineWriter("gsr"));
                loggers.Add(gsrLogger);
                await gsrLogger.StartAsync();
            }
            CheckIsConnected();
        }

        private static ILineWriter CreateLineWriter(string name)
        {
            return new FileLineWriter(name);
            //return new DebugLineWriter();
        }

        private static async void CheckIsConnected() //Endlosschleife die checkt ob das Band verbunden ist
        {
            if (isCheckRunning) return;
            isCheckRunning = true;
            while (isCheckRunning)
            {
                await Task.Delay(200);
                Log.Debug("Programm", "Running");
                if (_bandClient != null && _bandClient.IsConnected == true)
                    continue;
                else await RunAsync();
            }
        }
    }
}