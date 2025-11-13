using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class ModifyOrder:IOrderCore

    {
        public ModifyOrder(IOrderCore orderCore, 
            long modifiyPrice,uint modifyquantity,bool isBuyside)
        {//PROPERTIES//
            Price=modifiyPrice;
            Quantity=modifyquantity;    
            IsBuySide=isBuyside;
            //FIELDS//
            _orderCore = orderCore;

            
        }
        //METHODS//
        public CancelOrder ToCancelOrder()
        {
            return new CancelOrder(this);
        }
        public Order ToNewOrder()
        {
            return new Order(this);
        }

        public long Price { get; }
        public uint Quantity { get; private set; }
        public bool IsBuySide { get; private set; }
        private readonly IOrderCore _orderCore;
    }
}
