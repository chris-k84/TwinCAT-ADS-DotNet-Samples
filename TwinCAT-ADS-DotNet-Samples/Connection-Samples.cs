using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Connection_Samples
    {
        void ConnectionUsingAdsSession(AmsAddress address,SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                AdsConnection connection = (AdsConnection)session.Connect(); // Establish the connection
                
                ConnectionState connectionState = connection.ConnectionState; // The actual connection state
            }
        }
        void ConnectionUsingAdsClient(AmsAddress address)
        {
            using (AdsClient client = new AdsClient())
            {
                client.Connect(address);

                StateInfo info = new StateInfo();

                info = client.ReadState();
            }
        }
    }
}
