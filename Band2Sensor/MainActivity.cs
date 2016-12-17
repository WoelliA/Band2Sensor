using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Debug = System.Diagnostics.Debug;

namespace Band2Sensor
{
    public interface ILineWriter
    {
        Task WriteLineAsync(IDictionary<string, object> values);
    }

    public static class Program
    {
        public static async Task RunAsync() //da muss alles rein
        {
            var bandConnector = new BandConnector();
            var client = await bandConnector.ConnectToBandAsync();
            var heartRateLogger = new HeartRateLogger(client, new DebugLineWriter());
            await heartRateLogger.StartAsync();
            var tempLogger = new TempLogger(client, new DebugLineWriter());
            await tempLogger.StartAsync();
            var gsrLogger = new GsrLogger(client, new DebugLineWriter());
            await gsrLogger.StartAsync();

        }
    }

    public class DebugLineWriter : ILineWriter
    {
        public Task WriteLineAsync(IDictionary<string, object> values)
        {
            Debug.WriteLine(string.Join("/ ", values.Select(p => $"{p.Key}: {p.Value}")));
            return Task.FromResult(true);
        }
    }


    [Activity(Label = "Band2Sensor", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            await Program.RunAsync();

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}