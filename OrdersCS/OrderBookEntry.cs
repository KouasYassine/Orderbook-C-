using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders {
    
    public class OrderBookEntry
    {
        public OrderBookEntry(Order currentOrder, Limit parentLimit)
        {
            if (currentOrder is null) throw new ArgumentNullException(nameof(currentOrder));
            if (parentLimit is null) throw new ArgumentNullException(nameof(parentLimit));

            CurrentOrder = currentOrder;
            ParentLimit = parentLimit;
            CreationTime = DateTime.UtcNow;
        }
       
        public Order CurrentOrder { get; }
        public Limit ParentLimit { get; }
        public DateTime CreationTime { get; }
        public OrderBookEntry NextEntry { get; set; }
        public OrderBookEntry PrevEntry { get; set; }

    }


}


