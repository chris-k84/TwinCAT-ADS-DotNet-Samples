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
            Write_Samples writer = new Write_Samples(); 
            List<string> symbols = new List<string>();
            using(Connection_Samples adsconnection = new Connection_Samples())
            {
                //adsconnection.ConnectionUsingAdsSession("169.254.61.77.1.1", 353);
                // adsconnection.ConnectionUsingAdsSession("169.254.127.143.2.1", 1001);

                // adsconnection.CheckConnection();

                // Console.WriteLine(adsconnection.info.ToString());
                // Console.WriteLine(adsconnection.deviceInfo.ToString());
                // Console.WriteLine(adsconnection.connectionState);

                //adsconnection.LoadSymbolsFromTarget(1);

                //Console.ReadLine();


                /////////////////READ SUM DATA FROM IO///////////////////
                adsconnection.ConnectionUsingAdsSession("169.254.61.77.1.1", 301);
                adsconnection.LoadSymbolsFromTarget(1);

                foreach (Symbol s in adsconnection.loader.Symbols)
                {
                    Console.WriteLine(s.InstancePath);
                }
                Console.ReadLine();

                //string[] targets = { "Term 3 (EL9227-5500).OCP Inputs Channel 1.Load",
                //                        "Term 4 (EL1008).Channel 1.Input",
                //                        "Term 3 (EL9227-5500).OCP Inputs Channel 2.Current",
                //                        "Term 10 (ELX1054).Channel 1.Input"};
                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[0]));
                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[1]));
                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[2]));
                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[3]));

                string[] targets = { "Task 4.Inputs.Var 464",
                                        "Task 4.Inputs.Var 469",
                                        "Task 4.Inputs.Var 470",
                                        "Task 4.Inputs.Var 471"};
                string[] outputs = {"Task 4.Outputs.Var 347",
                                        "Task 4.Outputs.Var 348" };
                object[] values = { false, false };

                Object[] result =  reader.SumReadPrimativeTypes(adsconnection.loader, adsconnection.adsConnection, targets);

                Console.WriteLine(result[0] + " " + result[1]+ " " + result[2] + " "+ result[3]);
                Console.ReadLine();

                writer.SumWritePrimativeTypes(adsconnection.loader, adsconnection.adsConnection, outputs, values);

                // methods.DisplayRpcMethods(adsconnection.loader,"MAIN.fbMath");
                //Console.WriteLine("Array Access");
                //double[] buffer = new double[8];
                //buffer = reader.ReadArrayWithSymbolicAccess(adsconnection.loader, 
                //    "Object1 (MyTestModel).MyTestModel_B.Constant5" );
                //foreach (double d in buffer)
                //{
                //    Console.WriteLine(d.ToString());
                //}

                //Console.WriteLine("Single Access");
                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader,
                //                   "Object1 (MyTestModel).MyTestModel_B.Constant5[2]"));
                // methods.CallMethodInPLC(adsconnection.adsConnection, 
                //      "MAIN.fbMath","mAdd",new object[] { (short)1, (short)2 });
                //                                                 
                //methods.CallMethodInPLC(adsconnection.adsConnection);
                // adsconnection.LoadSymbolsFromTarget(0);

                // symbols = reader.FilterSymbols(adsconnection.loader, "Untitled3_Obj1 (Module1)");

                // foreach (Symbol s in adsconnection.loader.Symbols)
                // {
                //     Console.WriteLine(s.InstancePath);
                // }

                //Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, "MAIN.iInt"));

                // byte[] val1 = reader.ReadComplexWithSymbolicAccess(adsconnection.loader, "MAIN.testStruct");

                //object value = reader.ReadComplexWithDynamicSymbolAccess(adsconnection.loader, "MAIN.testStruct");

                //reader.CreateEventOnPrimativeType(  adsconnection.loader, "MAIN.iCounter", OnChange);

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
