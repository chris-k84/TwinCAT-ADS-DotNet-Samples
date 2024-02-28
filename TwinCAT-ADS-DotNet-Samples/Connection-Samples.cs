using System;
using System.Net.Sockets;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Connection_Samples : IDisposable
    {
        public AdsSession session { get; set; }
        AdsClient client { get; set; }
        AdsConnection connection { get; set; }
        public IAdsConnection adsConnection { get; set; }
        public ConnectionState connectionState { get; set; }
        SessionSettings settings = SessionSettings.Default;
        AmsAddress address;
        public StateInfo info { get; set; }
        public DeviceInfo deviceInfo { get; set; }
        public ISymbolLoader loader;
        public void ConnectionUsingAdsSession(string netid, int port)
        {
            address = new AmsAddress(netid, port);

            session = new AdsSession(address, settings);
            //session = new AdsSession(AmsNetId.Local, 851);

            connection = (AdsConnection)session.Connect(); // Establish the connection 
            StateInfo info = connection.ReadState();


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
            if (connection != null)
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
        ///<summary>
        ///Connect to plc port 851
        ///</summary>
        public void StopPLC()
        {
            StateInfo set = new StateInfo(AdsState.Stop, connection.ReadState().DeviceState);
            connection.WriteControl(set);
        }
        ///<summary>
        ///Connect to plc port 851
        ///</summary>
        public void StartPLC()
        {
            StateInfo set = new StateInfo(AdsState.Run, connection.ReadState().DeviceState);
            connection.WriteControl(set);
        }
        ///<summary>
        ///Connect to server port 10000
        ///</summary>
        public void RestartTwinCAT()
        {
            StateInfo set = new StateInfo(AdsState.Reset, connection.ReadState().DeviceState);
            connection.WriteControl(set);
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
