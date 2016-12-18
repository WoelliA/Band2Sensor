using System.Linq;
using System.Threading.Tasks;
using Microsoft.Band.Portable;

namespace Band2Sensor
{
    public class BandConnector
    {
        public async Task<BandClient> ConnectToBandAsync()
        {
            BandDeviceInfo bandInfo = null;

            var bandClientManager1 = BandClientManager.Instance;

            var pairedBands = await bandClientManager1.GetPairedBandsAsync();
            // connect to the first device
            bandInfo = pairedBands.FirstOrDefault();

            if (bandInfo != null)
            {
                var bamdclient = await bandClientManager1.ConnectAsync(bandInfo);
                return bamdclient;
            }
            return null;
        }
    }
}