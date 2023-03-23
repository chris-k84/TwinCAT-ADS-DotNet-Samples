using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.SumCommand;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Read_Samples : IDisposable
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
        public void CreateEventOnPrimativeType(ISymbolLoader loader, string symbol, EventHandler<ValueChangedEventArgs> method)
        {
            Symbol adsSymbol = (Symbol)loader.Symbols[symbol];
            
            adsSymbol.NotificationSettings = new NotificationSettings(AdsTransMode.OnChange, 1, 0);
            adsSymbol.ValueChanged += method;
        }
        private void On_SymbolChange(object sender, ValueChangedEventArgs e)
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
        public double[] ReadArrayWithSymbolicAccess(ISymbolLoader loader, string symbol)
        {
                Symbol arrayRead = (Symbol)loader.Symbols[symbol];

                return (double[])arrayRead.ReadValue();
        }
        public object[] SumReadPrimativeTypes(ISymbolLoader loader, IAdsConnection adsConnection, string[] symbols)
        {
            SymbolCollection symbolCollection = new SymbolCollection();

            foreach(String s in symbols)
            {
                Symbol symbol = (Symbol)loader.Symbols[s];
                symbolCollection.Add(symbol);
            }
            SumSymbolRead readCommand = new SumSymbolRead(adsConnection, symbolCollection);
            

            return readCommand.Read();
        }
        public object[] SumReadPrimativesByHandle(IAdsConnection adsConnection, string[] symbols)
        {
            SumCreateHandles createHandlesCommand = new SumCreateHandles(adsConnection, symbols);

            uint[] resultCreateHandles = createHandlesCommand.CreateHandles();
            Type[] valueTypes = new Type[] { typeof(bool), typeof(bool), typeof(bool),typeof(bool) };
            SumHandleRead readCommand = new SumHandleRead(adsConnection, resultCreateHandles, valueTypes);
            return readCommand.Read();
            
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
        public void Dispose()
        {
           
        }
    }
}
