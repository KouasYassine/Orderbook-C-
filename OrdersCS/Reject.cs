using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;   

namespace TradingEngineServer.Rejects

{
    public class Reject:IOrderCore
    {
        public Reject(IOrderCore rejectOrder, RejectionReason  rejectionReason)
        {//PROPERTIES//

            RejectionReason = rejectionReason;


            //FIELDS//
            _orderCore = rejectOrder;
            

        }
        //FEILDS//

        private readonly IOrderCore _orderCore;

        //PROPERTIES//
        public RejectionReason RejectionReason { get; }
        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;

    }
}
