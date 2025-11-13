using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders    
{
    public sealed class OrderStatusCreator
    {
        public static CancelOrderStatus GenerateCancrlOrderStatus(CancelOrderStatus co)
        {
            return new CancelOrderStatus();
        }
        public static NewOrderstatus GenerateNewOrderStatus(NewOrderstatus no)
        {
            return new NewOrderstatus();
        }
        public static ModifyOrderStatus GenerateModifyOrderStatus(ModifyOrderStatus mo)
        {
            return new ModifyOrderStatus();
        }
    }
}
