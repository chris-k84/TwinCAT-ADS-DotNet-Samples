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
    internal class Write_Samples
    {
        void SumWritePrimativeTypes(AmsAddress address, SessionSettings settings)
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

                SumSymbolWrite writeCommand = new SumSymbolWrite(connection, symbols);
                
                object[] writeValues = new object[]{(ushort)42, (int)32};

                writeCommand.Write(writeValues);
            }

        }
    }
}
