using System;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Program
    {
        static void Main(string[] args)
        {  
            Console.WriteLine("Hello World");
            Read_Samples reader = new Read_Samples();
            using(Connection_Samples adsconnection = new Connection_Samples())
            {
                adsconnection.ConnectionUsingAdsSession("169.254.127.143.1.1", 851);

                adsconnection.CheckConnection();

                adsconnection.LoadSymbolsFromTarget();

                reader.CreateEventOnPrimativeType(adsconnection.loader,"MAIN.iCounter");

                Console.ReadLine();

                reader.RemoveEventOnPrimative(adsconnection.loader,"MAIN.iCounter");

                Console.ReadLine();
            }
        }
    }
}
