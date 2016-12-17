using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Sensors;

namespace Band2Sensor
{
    public class TempLogger
    {
        private readonly BandClient _bandClient;
        private readonly ILineWriter _lineWriter;

        public TempLogger(BandClient bandClient, ILineWriter lineWriter)
        {
            _bandClient = bandClient;
            _lineWriter = lineWriter;
        }

        private async void SkinTempOnReadingChangedAsync(object sender,
            BandSensorReadingEventArgs<BandSkinTemperatureReading> args)
        {
            var skinTemp = args.SensorReading.Temperature;
            var timeStamp = DateTime.UtcNow;

            IDictionary<string, object> values = new Dictionary<string, object>();
            values["Time"] = timeStamp;
            values["Temp"] = skinTemp;
            await _lineWriter.WriteLineAsync(values);
        }

        public async Task StartAsync()
        {
            var sensormanager = _bandClient.SensorManager;
            var skinTemp = sensormanager.SkinTemperature;
            skinTemp.ReadingChanged += SkinTempOnReadingChangedAsync;
            await skinTemp.StartReadingsAsync();
        }
    }
}