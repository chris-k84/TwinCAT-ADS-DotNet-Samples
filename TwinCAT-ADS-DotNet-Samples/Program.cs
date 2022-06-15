using System;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");       

            Connection_Samples adsconnection = new Connection_Samples();

            adsconnection.ConnectionUsingAdsSession("10.0.2.15.1.1", 851);

            adsconnection.CheckConnection();

            adsconnection.LoadSymbolsFromTarget();

            

            
        }
    }
}
