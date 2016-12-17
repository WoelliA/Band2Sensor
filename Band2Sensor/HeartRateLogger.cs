using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Sensors;

namespace Band2Sensor
{
    public class HeartRateLogger
    {
        private readonly BandClient _bandClient;
        private readonly ILineWriter _lineWriter;

        public HeartRateLogger(BandClient bandClient, ILineWriter lineWriter)
        {
            _bandClient = bandClient;
            _lineWriter = lineWriter;
        }

        private async void HeartRateOnReadingChanged(object sender,
            BandSensorReadingEventArgs<BandHeartRateReading> args)
        {
            var quality = args.SensorReading.Quality;
            var timeStamp = DateTime.UtcNow;

            IDictionary<string, object> values = new Dictionary<string, object>();
            values["Time"] = timeStamp;
            values["Quality"] = quality;
            values["HR"] = args.SensorReading.HeartRate;
            await _lineWriter.WriteLineAsync(values);
        }

        public async Task StartAsync()
        {
            var sensormanager = _bandClient.SensorManager;
            var heartRate = sensormanager.HeartRate;
            heartRate.ReadingChanged += HeartRateOnReadingChanged;

            if (heartRate.UserConsented == UserConsent.Unspecified)
            {
                var granted = await heartRate.RequestUserConsent();
            }
            if (heartRate.UserConsented == UserConsent.Granted)
                await heartRate.StartReadingsAsync(BandSensorSampleRate.Ms16);
        }
    }
}