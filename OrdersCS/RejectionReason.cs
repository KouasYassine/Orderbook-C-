using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects
{
    public enum RejectionReason
    {
        Unknown,
        OrderNotfound,
        InstrumentNotFound,
        AttemptingtoModifyWrongSide
    }
}
