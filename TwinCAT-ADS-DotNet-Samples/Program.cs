﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;
using System.Xml.Linq;
using System.Net.Sockets;
using TwinCAT.Ads;
using TwinCAT;
using System.Linq;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Really simple example with adsclient (outdated), add an iCounter to your MAIN and increment, run this little section to get the value
            //using(AdsClient adsClient = new AdsClient())
            //{
            //    //int intToRead = 0;
            //    Boolean valueToRead;
            //    int targetAxis = 2;

            //    //adsClient.Connect("10.97.0.23.1.1", 851);
            //    adsClient.Connect("Local", 851);

            //    string target = string.Format("MAIN.fbAxis{0}.bBusy", targetAxis.ToString());
            //    //intToRead = (int)adsClient.ReadValue("MAIN.iCounter", typeof(int));
            //    valueToRead = (Boolean)adsClient.ReadValue(target,typeof(Boolean));

            //    //Console.WriteLine(intToRead.ToString());
            //    Console.WriteLine(valueToRead.ToString());
            //}
            //Really simple sample

            //Reall simple sample with Adssession, that allows connection diagnostics through the adsession class,
            //adsconneciton provides the same functionality as adsclient
            //SessionSettings settings = SessionSettings.Default;
            //AmsAddress address = new AmsAddress("192.168.56.1.1.1", 851);
            //AdsConnection adsConnection;
            //using (AdsSession adsSession = new AdsSession(address, settings))
            //{
            //    int intToRead = 0;
            //    adsConnection = (AdsConnection)adsSession.Connect();
            //    intToRead = (int)adsConnection.ReadValue("MAIN.sTest", typeof(int));
            //    Console.WriteLine(intToRead.ToString());
            //}
            //really simple sample

            // ADSReadwithSymbolicAccessDemo();
            // ADSTwinCATStateDemo();
            // ADSIndexVSSymbolReadSpeedDemo();
            // ADSEtherCATCoEReadDemo();
            // ADSEventDrivenReadDemo();
            // ADSTcCOMArrayReadDemo();
            // ADSClassMethodCallDemo();
            // ADSReadPDOIOTerminalDemo();
            // ADSSumReadWriteDemo();
            //ADSSumReadWriteMarshallingDemo();
            // ADSConnectionDiagnosticDemo();
            // ADSREadRPCMethodsFound();
            //GetAllSymbolAndShow();
        }

        static public void GetAllSymbolAndShow()
        {
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                adsconnection.ConnectionUsingAdsSession("199.4.42.250.1.1", 351);
                adsconnection.LoadSymbolsFromTarget(1);
                foreach (Symbol symbol in adsconnection.loader.Symbols)
                {
                    ///Only need to go down 1 layer with PLC////
                    if (symbol.InstanceName == "MAIN")
                    {
                        foreach (Symbol symbol1 in symbol.SubSymbols)
                        {
                            Console.WriteLine(symbol1.InstanceName.ToString());

                        }
                    }
                    ///Need to go down 1 extra layer with TMCs////
                    
                    //foreach (Symbol symbol1 in symbol.SubSymbols)
                    //{
                    //    if (symbol1.InstanceName == "IntegerTest_P")
                    //    {
                    //        foreach (Symbol symbol2 in symbol1.SubSymbols)
                    //        {
                    //            Console.WriteLine(symbol1.InstanceName.ToString());
                    //        }
                    //    }
                    //}


                }
            }
        }
        static public void OnChangePrimative(object sender, ValueChangedEventArgs e)
        {
            Symbol symbol = (Symbol)e.Symbol;
            Console.WriteLine("The Var " + e.Symbol + " has value " + e.Value);
            Console.WriteLine(sender.ToString());
        }
        static public void OnChangeArray(object sender, ValueChangedEventArgs e)
        {
            Symbol symbol = (Symbol)e.Symbol;
            double[] values = (double[])e.Value;
            foreach (double value in values)
            {
                Console.WriteLine(value.ToString());
            }
            Console.WriteLine(sender.ToString());
        }
        static public void ADSConnectionDiagnosticDemo()
        {
            ///////////////////Simple Connection sample, checks the status of connection//////////////
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                adsconnection.ConnectionUsingAdsSession("169.254.61.77.1.1", 851);
                adsconnection.CheckConnection();
                Console.WriteLine(adsconnection.info.ToString());
                Console.WriteLine(adsconnection.deviceInfo.ToString());
                Console.WriteLine(adsconnection.connectionState);
                Console.ReadLine();
            }
        }
        static public void ADSSumReadWriteDemo()
        {
            Read_Samples reader = new Read_Samples();
            Write_Samples writer = new Write_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                /////////////////Read/Write SUM Data From an IO Task///////////////////
                adsconnection.ConnectionUsingAdsSession("169.254.61.77.1.1", 301);
                adsconnection.LoadSymbolsFromTarget(1);
                string[] targets = { "Task 4.Inputs.Var 464",
                                    "Task 4.Inputs.Var 469",
                                    "Task 4.Inputs.Var 470",
                                    "Task 4.Inputs.Var 471"};
                string[] outputs = {"Task 4.Outputs.Var 347",
                                    "Task 4.Outputs.Var 348" };
                object[] values = { false, false };
                Object[] result = reader.SumReadPrimativeTypes(adsconnection.loader, adsconnection.adsConnection, targets);
                Console.WriteLine(result[0] + " " + result[1] + " " + result[2] + " " + result[3]);
                Console.ReadLine();
                writer.SumWritePrimativeTypes(adsconnection.loader, adsconnection.adsConnection, outputs, values);
                Console.ReadLine();
            }
        }
        static public void ADSSumReadWriteMarshallingDemo()
        {
            Read_Samples reader = new Read_Samples();
            Write_Samples writer = new Write_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                /////////////////Read/Write SUM Data with Marshalling class///////////////////
                adsconnection.ConnectionUsingAdsSession("199.4.42.250.1.1", 351);
                adsconnection.LoadSymbolsFromTarget(1);

                //////////////////SUM READ/////////////////////////////////
                //string[] targets = { "MAIN.adsBool",
                //                       "MAIN.adsInt",
                //                       "MAIN.adsWord",
                //                       "MAIN.adsLreal"};


                //object[] result = reader.SumReadPrimativeTypes(adsconnection.loader, adsconnection.adsConnection, targets);
                //Console.WriteLine(result[0].GetType().Name);
                //Console.WriteLine(result[1].GetType().Name);
                //Console.WriteLine(result[2].GetType().Name);
                //Console.WriteLine(result[3].GetType().Name);

                //Console.WriteLine(result[0] + " " + result[1] + " " + result[2]);

                //ParseAdsData data = new ParseAdsData();

                //data.ParseData(result);

                //Console.WriteLine(data.ToString());

                //Console.WriteLine(data.Data.ToString());

                //////////////////SUM WRITE/////////////////////////////////
                byte[] var1 = new byte[4];
                byte[] var2 = new byte[4];

                var1[1] = 1;
                var2[1] = 2;

                object[] writevalues = new object[2];

                writevalues[0] = var1;
                writevalues[1] = var2;
                
                string[] targets = { "Object1 (IntegerTest).IntegerTest_P.testParamUint16_1", "Object1 (IntegerTest).IntegerTest_P.testParamUint16_2" };
              //  string[] targets = { "MAIN.arrByte1", "MAIN.arrByte2" };
                writer.SumWritePrimativeTypes(adsconnection.loader, adsconnection.adsConnection, targets, writevalues);

                Console.ReadLine();
            }
        }
        static public void ADSReadPDOIOTerminalDemo()
        {
            Read_Samples reader = new Read_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                ///////////////////////Read direct IO data from EtherCAT Master ADS device////////////////////////
                adsconnection.ConnectionUsingAdsSession("10.112.0.23.1.1", 27907);
                
                adsconnection.LoadSymbolsFromTarget(0);
                string[] targets = { "Term 3 (EL9227-5500).OCP Inputs Channel 1.Load",
                                       "Term 4 (EL1008).Channel 1.Input",
                                       "Term 3 (EL9227-5500).OCP Inputs Channel 2.Current",
                                        "Term 10 (ELX1054).Channel 1.Input"};
                reader.PrintAllSymbols(adsconnection.loader);
                Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[0]));
                Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[1]));
                Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[2]));
                Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, targets[3]));
                Console.ReadLine();
            }
        }
        static public void ADSClassMethodCallDemo()
        {
            Method_Call_Samples methods = new Method_Call_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                ////////////////////////Call Method inside PLC///////////////////////////////////////////////////
                methods.DisplayRpcMethods(adsconnection.loader, "MAIN.fbMath");
                methods.CallMethodInPLC(adsconnection.adsConnection,
                     "MAIN.fbMath", "mAdd", new object[] { (short)1, (short)2 });
                Console.ReadLine();
            }
        }
        static public void ADSTcCOMArrayReadDemo()
        {
            Read_Samples reader = new Read_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                //////////////////Read array data from TcCOM object in TwinCAT/////////////////////////////
                Console.WriteLine("Array Access");
                double[] buffer = new double[8];
                buffer = reader.ReadArrayLrealsWithSymbolicAccess(adsconnection.loader,
                   "Object1 (MyTestModel).MyTestModel_B.Constant5");
                foreach (double d in buffer)
                {
                    Console.WriteLine(d.ToString());
                }
                Console.ReadLine();
                Console.WriteLine("Single Access");
                Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader,
                                  "Object1 (MyTestModel).MyTestModel_B.Constant5[2]"));
                Console.ReadLine();
            }
        }
        static public void ADSEventDrivenReadDemo()
        {
            Read_Samples reader = new Read_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                //////////////////////////Event reading - set up Event handler on symbol/////////////////////////////
                adsconnection.ConnectionUsingAdsSession("192.168.56.1.1.1", 851);
                adsconnection.LoadSymbolsFromTarget(1);
                reader.CreateEventOnPrimativeType(adsconnection.loader, "MAIN.sTest", OnChangePrimative);
                Console.ReadLine();
                reader.RemoveEventOnPrimative(adsconnection.loader, "MAIN.sTest");
                Console.ReadLine();
            }
        }
        static public void ADSEtherCATCoEReadDemo()
        {
            Read_Samples reader = new Read_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                ////////////////////////////Read CoE index of EtherCAT device//////////////////////////////////////
                adsconnection.ConnectionUsingAdsSession("169.254.127.143.2.1", 1001);
                byte[] readBuffer = new byte[8];
                reader.ReadWithIdxOfs("F302", "10180001", adsconnection.adsConnection, readBuffer);
                Console.ReadLine();
            }
        }
        static public void ADSIndexVSSymbolReadSpeedDemo()
        {
            Read_Samples reader = new Read_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                //////////////////////////Timing ADS read cycles/////////////////////////////
                adsconnection.ConnectionUsingAdsSession("169.254.61.77.1.1", 851);
                adsconnection.LoadSymbolsFromTarget(0);

                foreach (IAdsSymbol symbol in adsconnection.loader.Symbols)
                {
                    Console.WriteLine(symbol.InstancePath.ToString());
                    Console.WriteLine(symbol.IndexGroup.ToString());
                    Console.WriteLine(symbol.IndexOffset.ToString());
                }
                byte[] buffer = new byte[8000];
                Stopwatch stopwatch = Stopwatch.StartNew();


                //double[] val1 = reader.ReadArrayLrealsWithSymbolicAccess(adsconnection.loader, "MAIN.arrLreal");
                reader.ReadWithIdxOfs("4040", "5DFF0", adsconnection.adsConnection, buffer);


                stopwatch.Stop();


                //double[] buffer2 = new double[buffer.Length / 8];
                //Buffer.BlockCopy(buffer, 0, buffer2, 0, buffer2.Length * 8);
                double[] buffer2 = new double[buffer.Length / 8];
                for (int i = 0; i < buffer2.Length; i++)
                    buffer2[i] = BitConverter.ToDouble(buffer, i * 8);

                Console.WriteLine(stopwatch.ElapsedMilliseconds);
            }
        }
        static public void ADSTwinCATStateDemo()
        {
            Read_Samples reader = new Read_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                //////////////////////////Set Run Mode sample/////////////////////////////
                adsconnection.ConnectionUsingAdsSession("169.254.61.77.1.1", 10000);
                adsconnection.CheckConnection();
                Console.WriteLine(adsconnection.info.AdsState.ToString());
                Console.WriteLine(adsconnection.info.DeviceState.ToString());
                Console.WriteLine(adsconnection.connectionState);

                Console.ReadLine();

                switch ("restart")
                {
                    case "restart": adsconnection.RestartTwinCAT(); ; break;
                    case "run": adsconnection.StartPLC(); break;
                    case "stop": adsconnection.StopPLC(); break;
                    default: Console.WriteLine("Please choose \"Run\" or \"Stop\" and confirm with enter.."); break;
                }
            }
        }
        static public void ADSReadwithSymbolicAccessDemo()
        {
            Read_Samples reader = new Read_Samples();
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                ////////////////////////Read samples for primitive and structs/////////////////////////////////////
                Console.WriteLine(reader.ReadPrimativeWithSymbolicAccess(adsconnection.loader, "MAIN.iInt"));
                byte[] val1 = reader.ReadComplexWithSymbolicAccess(adsconnection.loader, "MAIN.testStruct");
                object value = reader.ReadComplexWithDynamicSymbolAccess(adsconnection.loader, "MAIN.testStruct");
                Console.ReadLine();
            }
        }
        static public void ADSREadRPCMethodsFound()
        {
            using (Connection_Samples adsconnection = new Connection_Samples())
            {
                adsconnection.ConnectionUsingAdsSession("127.0.0.1.1.1", 851);
                adsconnection.LoadSymbolsFromTarget(2);
                List<TestCase> testCases = new List<TestCase>();
                List<ISymbol> TestSuites = new List<ISymbol>();
                foreach (ISymbol symbol in adsconnection.loader.Symbols)
                {
                    RecursiveInterfaceSearch(symbol, TestSuites);
                }
                
                foreach (ISymbol s in TestSuites)
                {

                    IStructInstance rpcStructInstance = s as IStructInstance;
                    foreach (var rpc in rpcStructInstance.RpcMethods)
                    {
                        Console.WriteLine(rpc.Name);
                    }
                   
                    
                }
            }
        }
        static public void RecursiveInterfaceSearch(ISymbol symbol, List<ISymbol> TestSuites)
        {
            if (symbol.SubSymbols.Count == 0)
            {
                return;
            }
            foreach (ISymbol symbol1 in symbol.SubSymbols)
            {
                if (symbol1.Category == DataTypeCategory.Struct)
                {
                    if ((symbol1.DataType as StructType).InterfaceImplementationNames.Contains("I_SomeInterface"))
                    {
                        //foreach()
                        TestSuites.Add(symbol1);
                        //TestCase test = new TestCase();
                        ////Tests.Add(symbol1.Attributes[0].Name, symbol1.InstancePath);
                        ////Tests.Add(symbol1.Attributes[0].Name);
                        //test.Name = symbol1.Attributes[0].Name;
                        //test.Path = symbol1.InstancePath;
                        //Tests.Add(test);
                    }
                }
                RecursiveInterfaceSearch(symbol1, TestSuites);
            }
        }
        public class TestCase
        {
            public string Name { get; set; } = "";
            public string Path { get; set; } = "";
            public string[] Tests { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
        public struct MyData
        {
            public Boolean myBool;
            public Int16 myInt;
            public UInt32 myDword;
            public Double myLreal;

            public override string ToString() => $"({myBool}, {myInt}, {myDword}, {myLreal})";
        }
        class ParseAdsData
        {
            private Boolean myBool;
            private Int16 myInt;
            private UInt32 myDword;
            private Double myLreal;
            public MyData Data;
            public ParseAdsData()
            {

            }

            public void ParseData(Object[] data)
            {
                myBool = (Boolean)data[0];
                myInt = (Int16)data[1];
                myDword = (UInt32)data[2];
                myLreal = (Double)data[3];
                Data.myBool = (Boolean)data[0];
                Data.myInt = (Int16)data[1];
                Data.myDword = (UInt32)data[2];
                Data.myLreal = (Double)data[3];
            }

            public override string ToString()
            {
                return "Data: " + myBool + " " + myInt + " " + myDword + " " + myLreal;
            }
        }
    }
}
