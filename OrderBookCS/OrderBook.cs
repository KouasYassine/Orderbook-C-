using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Instrument;
using TradingEngineServer.OrderBook;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class OrderBook : IRetrievalbook
    {
        //PRIVATE FIELDS//
        private readonly Dictionary<long, OrderBookEntry> orders = new Dictionary<long, OrderBookEntry>();
        private readonly Security _instrument;
        private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitComparer.Comparer);
        private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitComparer.Comparer);
        public OrderBook( Security instrument)
        {
       
            _instrument = instrument;
        }
        public int Count =>orders.Count;

       

        void AddOrder(Order order)
        {
            var baselimit=new Limit(order.Price);
            AddOrder(order, baselimit,order.IsBuySide ? _bidLimits:_askLimits,orders);
        }

        private void AddOrder(Order order, Limit baselimit, SortedSet<Limit> limits ,Dictionary<long,OrderBookEntry> internalBook)
        {
            if (limits.TryGetValue(baselimit, out Limit limit))
            {
                OrderBookEntry orderbookEntry = new OrderBookEntry(order, baselimit);
               if(limit.Head == null)
                {
                    limit.Head = orderbookEntry;
                    limit.Tail = orderbookEntry;
                }
                else
                {
                    OrderBookEntry tailPointer= limit.Tail;
                    tailPointer.NextEntry = orderbookEntry;
                    orderbookEntry.PrevEntry = tailPointer;
                    limit.Tail = orderbookEntry;
                }
               internalBook.Add(order.OrderId, orderbookEntry);

            }
            else
            {
                limits.Add(baselimit);
                // Limit does not exist, create a new limit and add order
                OrderBookEntry orderbookEntry = new OrderBookEntry(order,baselimit);
                baselimit.Head = orderbookEntry;
                baselimit.Tail = orderbookEntry;
                internalBook.Add(order.OrderId, orderbookEntry);

            }
        }

        void ChangeOrder(ModifyOrder modifyOrder) { 
         if (orders.TryGetValue(modifyOrder.OrderId,out OrderBookEntry obe))
            {
                RemoveOrder(modifyOrder.ToCancelOrder());
                AddOrder(modifyOrder.ToNewOrder(),obe.ParentLimit,modifyOrder.IsBuySide? _bidLimits: _askLimits , orders);
            }


                 }
        void RemoveOrder( CancelOrder cancelOrder) {
            if (orders.TryGetValue(cancelOrder.OrderId, out OrderBookEntry obe))
                { RemoveOrder(cancelOrder.OrderId ,obe, orders); }
                }
        private void RemoveOrder(long cancelOrder, OrderBookEntry obe, Dictionary<long, OrderBookEntry> internalBook)
        {
            //deal with the location of OverbookEntry within the linked List
            if(obe.PrevEntry!=null && obe.NextEntry !=null)
            {
                obe.PrevEntry.NextEntry = obe.NextEntry;
                obe.NextEntry.PrevEntry = obe.PrevEntry;
            }
            else if (obe.PrevEntry != null)
            { 
                obe.PrevEntry.NextEntry = null;
            }
            else if (obe.NextEntry !=null)
            {
                
                obe.NextEntry.PrevEntry = null;
            }
            //Deal with the limit if it is empty now
            if(obe.ParentLimit.Head == obe && obe.ParentLimit.Tail == obe)
            {
                //only one order on the level
                obe.ParentLimit.Head = null;         
                obe.ParentLimit.Tail = null;
            }
            else if (obe.ParentLimit.Head == obe)
            {// Moree than one order, but obe is first order on level
                obe.ParentLimit.Head = obe.NextEntry;
            }
            else if (obe.ParentLimit.Tail == obe)
            //More than one order, but obe is last order on level
            {
                obe.ParentLimit.Tail = obe.PrevEntry;
            }
            internalBook.Remove(cancelOrder);

        }
        public OrderBookSpread GetSpread()
        {
            long? bestAsk = null;
            long ?bestbid=null;    
            if (_askLimits.Count > 0 && !_askLimits.Min.IsEmpty)
            {
                bestAsk = _askLimits.Min.Price;
            }
            if (_bidLimits.Count > 0 && !_bidLimits.Min.IsEmpty)
            {
                bestbid = _bidLimits.Max.Price;
            }
           return new OrderBookSpread(bestbid, bestAsk);
        }

        public List<OrderBookEntry> GetAskOrders()
            
        {
            List<OrderBookEntry> orderbookEntries = new List<OrderBookEntry>();
            foreach ( var askLimit in _askLimits)
            {
                //traverse the linked list at each limit and collect orders*
                if (askLimit.IsEmpty)
                    continue;
                else {
                    OrderBookEntry askLimitPointer = askLimit.Head;
                    while(askLimitPointer != null)
                    {
                        orderbookEntries.Add(askLimitPointer);
                        askLimitPointer = askLimitPointer.NextEntry;
                    }
                }
            }
            return orderbookEntries;

        }

        public List<OrderBookEntry> GetBidOrders()
        {
            List<OrderBookEntry> orderbookEntries = new List<OrderBookEntry>();
            foreach (var bidLimit in _bidLimits)
            {
                //traverse the linked list at each limit and collect orders*
                if (bidLimit.IsEmpty)
                    continue;
                else
                {
                    OrderBookEntry bidLimitPointer = bidLimit.Head;
                    while (bidLimitPointer != null)
                    {
                        orderbookEntries.Add(bidLimitPointer);
                        bidLimitPointer = bidLimitPointer.NextEntry;
                    }
                }
            }
            return orderbookEntries;
        }

        void IOrderEntryOrderBook.AddOrder(Order order)
        {
            AddOrder(order);
        }

        void IOrderEntryOrderBook.ChangeOrder(ModifyOrder modifyOrder)
        {
            ChangeOrder(modifyOrder);
        }

        void IOrderEntryOrderBook.RemoveOrder(CancelOrder cancelOrder)
        {
            RemoveOrder(cancelOrder);
        }

        
        public bool ContainsOrder(long orderId)
        {
            return orders.ContainsKey(orderId);
        }
    }
}
