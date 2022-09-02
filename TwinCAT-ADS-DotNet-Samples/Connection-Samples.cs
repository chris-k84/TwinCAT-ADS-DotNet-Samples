using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Connection_Samples : IDisposable
    {
        AdsSession session { get; set; }
        AdsClient client { get; set; }
        AdsConnection connection { get; set; }
        public IAdsConnection adsConnection {get; set;}
        public ConnectionState connectionState { get; set; }
        SessionSettings settings = SessionSettings.Default;
        AmsAddress address;
        public StateInfo info { get; set; }
        public DeviceInfo deviceInfo {get;set;}
        public ISymbolLoader loader;
        public void ConnectionUsingAdsSession(string netid, int port)
        {        
            address = new AmsAddress(netid, port);

            session = new AdsSession(address,settings);
                 
            connection = (AdsConnection)session.Connect(); // Establish the connection 

            adsConnection = connection;
        }
        public void ConnectionUsingAdsClient(string netid, int port)
        {
                client = new AdsClient();
                address = new AmsAddress(netid, port);

                client.Connect(address);

                adsConnection = connection;
        }
        public void ConnectToIOServer(AmsAddress address)
        {
            connection = (AdsConnection)session.Connect();
        }
        public void CheckConnection()
        {
            if(connection != null)
            {
                info = connection.ReadState();
                deviceInfo = connection.ReadDeviceInfo();
                connectionState = connection.ConnectionState; // The actual connection state
            }
        }
        public void LoadSymbolsFromTarget(int SymbolMode)
        {
            SymbolsLoadMode loadMode = (SymbolsLoadMode)SymbolMode;
            SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(loadMode);
            loader = SymbolLoaderFactory.Create(connection, loaderSettings);
        }
        public void Dispose()
        {
            if(session != null)
            {
                session.Dispose();
                connection.Dispose();
            }
            if(client != null)
            {
                client.Dispose();
            }
        }
    }
}
