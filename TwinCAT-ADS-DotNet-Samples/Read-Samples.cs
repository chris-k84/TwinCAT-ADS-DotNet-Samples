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
        void ReadWithIdxOfs(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                AdsConnection connection = (AdsConnection)session.Connect();

                uint Index = 0;
                uint Offset = 0;
                byte[] readBuffer = new byte[8];
                connection.Read(Index, Offset, readBuffer);
                int i = BitConverter.ToInt32(readBuffer, 0);
            }
        }
        void ReadPrimativeWithSymbolicAccess(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                string uintValue = "MAIN.uintValue";

                AdsConnection connection = (AdsConnection)session.Connect();

                SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
                ISymbolLoader loader = SymbolLoaderFactory.Create(connection, loaderSettings);

                Symbol Test = (Symbol)loader.Symbols[uintValue];

                Test.ReadValue();
            }
        }
        void ReadPrimativeWithEvent(AmsAddress address, SessionSettings settings)
        {
            using (AdsSession session = new AdsSession(address, settings))
            {
                string uintValue = "MAIN.uintValue";

                AdsConnection connection = (AdsConnection)session.Connect();

                SymbolLoaderSettings loaderSettings = new SymbolLoaderSettings(SymbolsLoadMode.Flat);
                ISymbolLoader loader = SymbolLoaderFactory.Create(connection, loaderSettings);

                Symbol symbol = (Symbol)loader.Symbols[uintValue];

                symbol.NotificationSettings = new NotificationSettings(AdsTransMode.OnChange, 500, 0);
                symbol.ValueChanged += On_SymbolChange;
            }
        }
        static private void On_SymbolChange(object sender, ValueChangedEventArgs e)
        {
            Symbol symbol = (Symbol)e.Symbol;
            Console.WriteLine("The Var " + e.Symbol + " has value " + e.Value);
            Console.WriteLine(sender.ToString());
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

                SymbolIterator iterator = new SymbolIterator(symbols: loader.Symbols,
                                                                        recurse: true,
                                                                        predicate: filter);

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
