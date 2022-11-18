using System;
using System.Collections.Generic;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Program
    {
        static void Main(string[] args)
        {  
            Console.WriteLine("Hello World");
            Read_Samples reader = new Read_Samples();
            Method_Call_Samples methods = new Method_Call_Samples();
            List<string> symbols = new List<string>();
            using(Connection_Samples adsconnection = new Connection_Samples())
            {
                adsconnection.ConnectionUsingAdsSession("172.18.138.71.1.1", 851);
                //adsconnection.ConnectionUsingAdsSession("169.254.127.143.2.1", 1001);

                adsconnection.CheckConnection();

                Console.WriteLine(adsconnection.info.ToString());
                Console.WriteLine(adsconnection.deviceInfo.ToString());
                Console.WriteLine(adsconnection.connectionState);

                adsconnection.LoadSymbolsFromTarget(2);

                Console.ReadLine();

                methods.DisplayRpcMethods(adsconnection.loader,"MAIN.fbMachine");

                Console.ReadLine();

                // methods.CallMethodInPLC(adsconnection.adsConnection, "MAIN.fbMachine","SetPackML",new object[]
                //                                                 { (short)1 });
                methods.CallMethodInPLC(adsconnection.adsConnection);
                // adsconnection.LoadSymbolsFromTarget(0);

                // symbols = reader.FilterSymbols(adsconnection.loader, "Untitled3_Obj1 (Module1)");

                // foreach (Symbol s in adsconnection.loader.Symbols)
                // {
                //     Console.WriteLine(s.InstancePath);
                // }

                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, "MAIN.iInt"));

                // byte[] val1 = reader.ReadComplexWithSymbolicAccess(adsconnection.loader, "MAIN.testStruct");

                //object value = reader.ReadComplexWithDynamicSymbolAccess(adsconnection.loader, "MAIN.testStruct");
                
                //reader.CreateEventOnPrimativeType(  adsconnection.loader, "MAIN.iInt", OnChange);

                // Console.ReadLine();

                // reader.RemoveEventOnPrimative(adsconnection.loader,"MAIN.iInt");

                // byte[] readBuffer = new byte[8];
                // reader.ReadWithIdxOfs("F302","10180001", adsconnection.adsConnection,readBuffer);

                Console.ReadLine();
            }
        }

        static public void OnChange(object sender, ValueChangedEventArgs e)
        {
            Symbol symbol = (Symbol)e.Symbol;
            Console.WriteLine("The Var " + e.Symbol + " has value " + e.Value);
            Console.WriteLine(sender.ToString());
        }
        
        
    }
}
