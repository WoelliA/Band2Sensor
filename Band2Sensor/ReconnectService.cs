using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Util;

namespace Band2Sensor
{
    [Service(Enabled = true)]
    internal class ReconnectService : Service
    {
        private readonly string tag = "backgroundservice";

        public async void OnCOnnectionLost(SensorEvent e)
        {
            var bandConnector = new BandConnector();
            await bandConnector.ConnectToBandAsync();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Info(tag, $"OnStart");
            try
            {
                Program.RunAsync();
                Log.Info(tag, $"Started Service.");
            }
            catch
            {
                Log.Info(tag, $"Failed to start Service.");
            }

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            Program.Disconnect();
            base.OnDestroy();
        }


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}