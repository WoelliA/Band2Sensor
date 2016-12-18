using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using Debug = System.Diagnostics.Debug;

namespace Band2Sensor
{
    public interface ILineWriter
    {
        Task WriteLineAsync(IDictionary<string, object> values);
    }

    public class DebugLineWriter : ILineWriter
    {
        public Task WriteLineAsync(IDictionary<string, object> values)
        {
            var csv = new StringBuilder();
            //var newLine = string.Format();
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

            Program.RunAsync();
            //Intent reconnectIntent = new Intent(Application.Context, typeof(ReconnectService));

            //Application.Context.StartService(reconnectIntent);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}