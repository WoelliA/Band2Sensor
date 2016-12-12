using System;
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
        BandClientManager bandClientManager = BandClientManager.Instance;
        async Task ConnectToBandAsync()
        {
            BandDeviceInfo bandInfo = null;
            while (bandInfo == null)
            {
                var pairedBands = await bandClientManager.GetPairedBandsAsync();
                // connect to the first device
                bandInfo = pairedBands.FirstOrDefault();
            }
            var bandClient = await bandClientManager.ConnectAsync(bandInfo);
            var sensormanager = bandClient.SensorManager;
            var heartRate = sensormanager.HeartRate;
            heartRate.ReadingChanged += (o, args) =>
            {
                var quality = args.SensorReading.Quality;
                DateTime timeStamp = System.DateTime.UtcNow;
                System.Diagnostics.Debug.WriteLine($"time:{timeStamp} / bpm: {args.SensorReading.HeartRate} / quality:{quality}");
            
            if (heartRate.UserConsented == UserConsent.Unspecified)
            {
                bool granted = await heartRate.RequestUserConsent();
            }
            if (heartRate.UserConsented == UserConsent.Granted)
            {
                await heartRate.StartReadingsAsync(Microsoft.Band.Portable.Sensors.BandSensorSampleRate.Ms128); //SampleRate MS128 = 1(sec)/128; so this is the most detailed one
            }
            else
            {

            }
            // await heartRate.StopReadingsAsync();
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            await ConnectToBandAsync();

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

