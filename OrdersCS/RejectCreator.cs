using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects
{
    public sealed  class RejectCreator
    {
        public static Reject GenerateOrderCoreReject(IOrderCore rejectionOrder,RejectionReason rr)
        {
            return new Reject(rejectionOrder, rr);

        }
    }
}
