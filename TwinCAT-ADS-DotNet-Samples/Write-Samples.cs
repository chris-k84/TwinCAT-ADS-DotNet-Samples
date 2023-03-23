using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.SumCommand;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Write_Samples : IDisposable
    {
        public void WriteIntWithSymbolicAccess(ISymbolLoader loader, string symbol, int value)
        {
            Symbol Test = (Symbol)loader.Symbols[symbol];
            
            Test.WriteValue(value);
        }
        public void SumWritePrimativeTypes(ISymbolLoader loader,IAdsConnection adsConnection, string[] symbols, object[] values)
        {
            SymbolCollection writeSymbols = new SymbolCollection();
            foreach(string s in symbols)
            {
                writeSymbols.Add((Symbol)loader.Symbols[s]);
            }

            SumSymbolWrite writeCommand = new SumSymbolWrite(adsConnection,writeSymbols);

            writeCommand.Write(values);
        }
        void SumWriteArrayOfStruct(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                AdsConnection connection = (AdsConnection)session.Connect();

                SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
                ISymbolLoader loader = SymbolLoaderFactory.Create(connection, loaderSettings);

                ArrayInstance var1 = (ArrayInstance)loader.Symbols["MAIN.arrTest"];
                Symbol var2 = (Symbol)loader.Symbols["MAIN.iCounter"];

                SymbolCollection sumSymbols = new SymbolCollection() { var1, var2 };

                TestStruct[] arrOfStructs = new TestStruct[10];
                arrOfStructs[0].Flag = true;

                SumSymbolWrite writeCommand = new SumSymbolWrite(connection, sumSymbols);

                object[] writeValues = new object[] { arrOfStructs, (int)32 };

                try
                {
                    writeCommand.Write(writeValues);
                }
                catch (AdsSumCommandException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    struct TestStruct
    {
        public bool Flag { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public ushort Scale { get; set; } 
    }
}