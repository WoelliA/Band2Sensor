using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.Band.Portable;
using System.Linq;
using System.Threading.Tasks;

namespace Band2Sensor
{
    
    [Activity(Label = "Band2Sensor", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        async Task ConnectToBandAsync()
        { 
        BandClientManager bandClientManager = BandClientManager.Instance;
        // query the service for paired devices
       
        var pairedBands = await bandClientManager.GetPairedBandsAsync();
        // connect to the first device
        var bandInfo = pairedBands.FirstOrDefault();
        var bandClient = await bandClientManager.ConnectAsync(bandInfo);
           
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

