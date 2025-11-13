using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orders
{
    public class Limit
    {
        public Limit(long price)
        {
            Price = price;
        }
        public long Price { get; private set; }
        public OrderBookEntry Head { get; set; }
        public OrderBookEntry Tail { get; set; }
        public uint GetLevelOrderCount()
        {
            uint orderCount = 0;
            OrderBookEntry headPointer = Head;
            while (headPointer != null)
            {
                if (headPointer.CurrentOrder.CurrentQuantity != 0)
                    orderCount++;
                headPointer = headPointer.NextEntry;

            }
            return orderCount;

        }
        public uint GetLevelOrderQuantity()
        {
            uint orderQuantity = 0;
            OrderBookEntry headPointer = Head;
            while (headPointer != null)
            {
               
                    orderQuantity+= headPointer.CurrentOrder.CurrentQuantity;
                headPointer = headPointer.NextEntry;

            }
            return orderQuantity;

        }
        public List<OrderRecord> GetLevelOrderRecords()
        {
            List<OrderRecord> orderRecord = new List<OrderRecord>();
            OrderBookEntry headPointer = Head;
            uint theoreticalQueuePosition = 0;
            while (headPointer != null)
            {
                var currentOrder = headPointer.CurrentOrder;
                if (currentOrder.CurrentQuantity!=0)
                     orderRecord.Add(new OrderRecord(currentOrder.OrderId,currentOrder.CurrentQuantity,Price,currentOrder.Username, currentOrder.IsBuySide,currentOrder.SecurityId,theoreticalQueuePosition));
                headPointer = headPointer.NextEntry;
                theoreticalQueuePosition += 1;
            }
            return orderRecord;
        }
        //    public record OrderRecord(long OrderId,uint Quantity,int Price, string Username, bool IsBuySide, uint SecurityId,uint TheoreticalQueueposition)
        public bool IsEmpty
        {
            get
            {
                return Head == null && Tail == null;
            }
        }
        public Side Side
        {
            get
            {
                if (IsEmpty)
                    return Side.Unknown;
                else
                {
                    return Head.CurrentOrder.IsBuySide ? Side.Bid : Side.Ask;
                }
            }
        }
    }
}
