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
                adsconnection.ConnectionUsingAdsSession("10.0.2.15.1.1", 851);
                //adsconnection.ConnectionUsingAdsSession("169.254.127.143.2.1", 1001);

                adsconnection.CheckConnection();

                adsconnection.LoadSymbolsFromTarget(2);

                // byte[] val1 = reader.ReadComplexWithSymbolicAccess(adsconnection.loader, "MAIN.testStruct");

                object value = reader.ReadComplexWithDynamicSymbolAccess(adsconnection.loader, "MAIN.testStruct");

                // reader.CreateEventOnPrimativeType(adsconnection.loader,"MAIN.iCounter");

                // Console.ReadLine();

                // reader.RemoveEventOnPrimative(adsconnection.loader,"MAIN.iCounter");

                // byte[] readBuffer = new byte[8];
                // reader.ReadWithIdxOfs("F302","10180001", adsconnection.adsConnection,readBuffer);

                Console.ReadLine();
            }
        }
    }
}
