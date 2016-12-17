using System.Linq;
using System.Threading.Tasks;
using Microsoft.Band.Portable;

namespace Band2Sensor
{
    public class BandConnector
    {
        private readonly BandClientManager bandClientManager = BandClientManager.Instance;

        public async Task<BandClient> ConnectToBandAsync()
        {
            BandDeviceInfo bandInfo = null;

            while (bandInfo == null)
            {
                var pairedBands = await bandClientManager.GetPairedBandsAsync();
                // connect to the first device
                bandInfo = pairedBands.FirstOrDefault();
            }
            var bandClient = await bandClientManager.ConnectAsync(bandInfo);
            return bandClient;
        }
    }
}