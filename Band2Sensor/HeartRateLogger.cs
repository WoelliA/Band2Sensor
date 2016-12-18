using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Sensors;

namespace Band2Sensor
{
    public class HeartRateLogger : IDisposable
    {
        private readonly BandClient _bandClient;
        private readonly ILineWriter _lineWriter;
        private BandHeartRateSensor _heartRate;

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
            _heartRate = sensormanager.HeartRate;
            _heartRate.ReadingChanged += HeartRateOnReadingChanged;

            if (_heartRate.UserConsented == UserConsent.Unspecified)
            {
                var granted = await _heartRate.RequestUserConsent();
            }
            if (_heartRate.UserConsented == UserConsent.Granted)
                await _heartRate.StartReadingsAsync(BandSensorSampleRate.Ms16);
        }

        public void Dispose()
        { 
            _heartRate.ReadingChanged -= HeartRateOnReadingChanged;
        }
    }
}