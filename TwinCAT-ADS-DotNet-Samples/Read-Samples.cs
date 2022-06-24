using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.SumCommand;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Read_Samples
    {
        public void ReadWithIdxOfs(string Index, String Offset, IAdsConnection connection,byte[] ReadBuffer)
        {
                uint Idx = Convert.ToUInt32(Index,16);
                uint Oft = Convert.ToUInt32(Offset,16);
                connection.Read(Idx, Oft, ReadBuffer);
        }
        public string ReadPrimativeWithSymbolicAccess(ISymbolLoader loader, string symbol)
        {
            Symbol Test = (Symbol)loader.Symbols[symbol];
            
            return Test.ReadValue().ToString();
            
        }
        public byte[] ReadComplexWithSymbolicAccess(ISymbolLoader loader, string symbol)
        {
            Symbol Test = (Symbol)loader.Symbols[symbol];
            
            return (byte[])Test.ReadValue();
        }
        public object ReadComplexWithDynamicSymbolAccess(ISymbolLoader loader, string symbol)
        {
            DynamicSymbol Test = (DynamicSymbol)loader.Symbols[symbol];

            return Test.ReadValue();
        }
        public void CreateEventOnPrimativeType(ISymbolLoader loader, string symbol)
        {
            Symbol adsSymbol = (Symbol)loader.Symbols[symbol];

            adsSymbol.NotificationSettings = new NotificationSettings(AdsTransMode.OnChange, 1, 0);
            adsSymbol.ValueChanged += On_SymbolChange;
            
        }
        static private void On_SymbolChange(object sender, ValueChangedEventArgs e)
        {
            Symbol symbol = (Symbol)e.Symbol;
            Console.WriteLine("The Var " + e.Symbol + " has value " + e.Value);
            Console.WriteLine(sender.ToString());
        }
        public void RemoveEventOnPrimative(ISymbolLoader loader, string symbol)
        {
            Symbol adsSymbol = (Symbol)loader.Symbols[symbol];
            adsSymbol.ValueChanged -= On_SymbolChange;
        }
        void ReadArrayWithSymbolicAccess(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                string arrayTest = "MAIN.arrayTest";

                AdsConnection connection = (AdsConnection)session.Connect();

                SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
                ISymbolLoader loader = SymbolLoaderFactory.Create(connection, loaderSettings);

                Symbol arrayRead = (Symbol)loader.Symbols[arrayTest];

                short[] readBuffer = (short[])arrayRead.ReadValue();
            }
        }
        void SumReadPrimativeTypes(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                string uintValue = "MAIN.uintValue";
                string dintValue = "MAIN.dintValue";

                AdsConnection connection = (AdsConnection)session.Connect();

                SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
                ISymbolLoader loader = SymbolLoaderFactory.Create(connection, loaderSettings);

                Symbol var1 = (Symbol)loader.Symbols[uintValue];
                Symbol var2 = (Symbol)loader.Symbols[dintValue];

                SymbolCollection symbols = new SymbolCollection() { var1, var2 };

                SumSymbolRead readCommand = new SumSymbolRead(connection, symbols);


                var resultSumRead = readCommand.Read();
            }
        }
        public List<string> FilterSymbols(ISymbolLoader loader, string path)
        {
                Regex filterExpression = new Regex(pattern: @"^"+path+".*"); // Everything that starts with "MAIN"

                Func<ISymbol, bool> filter = s => filterExpression.IsMatch(s.InstancePath);

                SymbolIterator iterator = new SymbolIterator(loader.Symbols, true, filter);

                List<string> symbolList = new List<string>();

                foreach (ISymbol filteredSymbol in iterator)
                {
                    symbolList.Add(filteredSymbol.InstancePath);
                }
                return symbolList;
        }
        void ReadArrayofStructs(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                AdsConnection connection = (AdsConnection)session.Connect();

                SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
                ISymbolLoader loader = SymbolLoaderFactory.Create(connection, loaderSettings);

                ArrayInstance var1 = (ArrayInstance)loader.Symbols["MAIN.arrTest"];
                Symbol var2 = (Symbol)loader.Symbols["MAIN.iCounter"];

                SymbolCollection sumSymbols = new SymbolCollection() { var1, var2 };

                SumSymbolRead readCommand = new SumSymbolRead(connection, sumSymbols);

                var sumReadResult = readCommand.Read();
            }
        }
    }
}
