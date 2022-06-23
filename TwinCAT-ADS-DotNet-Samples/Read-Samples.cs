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
        public void ReadPrimativeWithSymbolicAccess(ISymbolLoader loader, string symbol)
        {
            Symbol Test = (Symbol)loader.Symbols[symbol];

            Test.ReadValue();
            
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
        void FilterSymbols(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                AdsConnection connection = (AdsConnection)session.Connect();

                SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
                ISymbolLoader loader = SymbolLoaderFactory.Create(connection, loaderSettings);

                Regex filterExpression = new Regex(pattern: @"^MAIN.*"); // Everything that starts with "MAIN"

                Func<ISymbol, bool> filter = s => filterExpression.IsMatch(s.InstancePath);

                SymbolIterator iterator = new SymbolIterator(loader.Symbols, true, filter);
                
                foreach (ISymbol filteredSymbol in iterator)
                {
                    Console.WriteLine(filteredSymbol.InstancePath);
                }

            }
            
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
