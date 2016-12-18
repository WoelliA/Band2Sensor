using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Sensors;

namespace Band2Sensor
{
    public class GsrLogger : IDisposable
    {
        private readonly BandClient _bandClient;
        private readonly ILineWriter _lineWriter;
        private BandGsrSensor _gsr;

        public GsrLogger(BandClient bandClient, ILineWriter lineWriter)
        {
            _bandClient = bandClient;
            _lineWriter = lineWriter;
        }

        private async void GsrOnReadingChangedAsync(object sender,
            BandSensorReadingEventArgs<BandGsrReading> args)
        {
            var gsr = args.SensorReading.Resistance;
            var timeStamp = DateTime.UtcNow;

            IDictionary<string, object> values = new Dictionary<string, object>();
            values["Time"] = timeStamp;
            values["GSR"] = gsr;
            await _lineWriter.WriteLineAsync(values);
        }

        public async Task StartAsync()
        {
            var sensormanager = _bandClient.SensorManager;
            _gsr = sensormanager.Gsr;
            _gsr.ReadingChanged += GsrOnReadingChangedAsync;

            await _gsr.StartReadingsAsync();
        }

        public void Dispose()
        {
            _gsr.ReadingChanged -= GsrOnReadingChangedAsync;
        }
    }
}