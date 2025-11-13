using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
     public class CancelOrder :IOrderCore 
    {
        public CancelOrder(IOrderCore orderCore)
        {
            //FIELDS//
            _orderCore = orderCore;
        }
        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;
        private readonly IOrderCore _orderCore;

    }
}
