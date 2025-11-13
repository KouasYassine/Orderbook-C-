using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{/// <summary> 
 ///ReadOnly Representation of our order
 ///</summary>
    public record OrderRecord(long OrderId, uint Quantity, long Price, string Username, bool IsBuySide, int SecurityId, uint TheoreticalQueueposition)
    {}
       namespace System.Runtime.CompilerServices
    {
       internal class IsExternalInit
        {
        }
    }
}
