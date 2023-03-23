using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TwinCAT_ADS_DotNet_Samples
{
    internal class Method_Call_Samples
    {
        public void CallMethodInPLC(IAdsConnection adsConnection)
        {               
            short result = (short)adsConnection.InvokeRpcMethod("MAIN.fbMachine",
                                                                "SetPackML",
                                                                new object[]
                                                                { (short)3});
            
        }
        public void CallMethodInPLC(IAdsConnection adsConnection, string className, string methodName)
        {               
            short result = (short)adsConnection.InvokeRpcMethod(className,
                                                                methodName,
                                                                null);
            
        }
        public void CallMethodInPLC(IAdsConnection adsConnection, string className, string methodName, object[] methodParameters )
        {               
            short result = (short)adsConnection.InvokeRpcMethod(className,
                                                                methodName,methodParameters);
            
        }
        public void DisplayRpcMethods(ISymbolLoader loader, string target)
        {
            IStructInstance main = (IStructInstance)loader.Symbols[target];
            foreach(IRpcMethod method in main.RpcMethods)
            {
                Console.WriteLine(method.Name.ToString());
            }
        }
    }
}
