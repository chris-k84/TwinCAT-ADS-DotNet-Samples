using System;
using System.Collections.Generic;
using TwinCAT.Ads.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Program
    {
        static void Main(string[] args)
        {  
            Console.WriteLine("Hello World");
            Read_Samples reader = new Read_Samples();
            List<string> symbols = new List<string>();
            using(Connection_Samples adsconnection = new Connection_Samples())
            {
                adsconnection.ConnectionUsingAdsSession("10.0.2.15.1.1", 350);
                //adsconnection.ConnectionUsingAdsSession("169.254.127.143.2.1", 1001);

                //adsconnection.CheckConnection();

                adsconnection.LoadSymbolsFromTarget(0);

                symbols = reader.FilterSymbols(adsconnection.loader, "Untitled3_Obj1 (Module1)");

                foreach (Symbol s in adsconnection.loader.Symbols)
                {
                    Console.WriteLine(s.InstancePath);
                }

                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, "MAIN.iInt"));

                // byte[] val1 = reader.ReadComplexWithSymbolicAccess(adsconnection.loader, "MAIN.testStruct");

                //object value = reader.ReadComplexWithDynamicSymbolAccess(adsconnection.loader, "MAIN.testStruct");

                // reader.CreateEventOnPrimativeType(adsconnection.loader,"MAIN.iInt");

                // Console.ReadLine();

                // reader.RemoveEventOnPrimative(adsconnection.loader,"MAIN.iInt");

                // byte[] readBuffer = new byte[8];
                // reader.ReadWithIdxOfs("F302","10180001", adsconnection.adsConnection,readBuffer);

                Console.ReadLine();
            }
        }
    }
}
