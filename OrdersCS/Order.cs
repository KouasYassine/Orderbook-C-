using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class Order : IOrderCore
    {
        public Order(IOrderCore orderCore, long price, uint quantity, bool isBuySide)
        {
            //PROPERTIES//
           Price= price;
            InitialQuantity = quantity;
            CurrentQuantity = quantity;
            IsBuySide = isBuySide;
            //FIELDS//
            _orderCore = orderCore;


        }
        public Order (ModifyOrder modifyOrder):this(modifyOrder,modifyOrder.Price, modifyOrder.Quantity, modifyOrder.IsBuySide)
        {

        }
        //PROPERTIES//
        

        public long Price { get; }
        public uint InitialQuantity { get; private set; }
        public uint CurrentQuantity { get; private set; }
        public bool IsBuySide { get; }
        public long OrderId=>_orderCore.OrderId;
        public string Username=>_orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;


        //METHODS//
        public void IncreaseQuantity(uint quantityDelta)
        {
            CurrentQuantity += quantityDelta;
        }
        public void DecreaseQuantity(uint quantityDelta)
        {
            if (quantityDelta <= CurrentQuantity)
                CurrentQuantity -= quantityDelta;
            else
                throw new InvalidOperationException($" QuantityDelta> CurrentQuantity for OrderId={OrderId}");
        }
        //FIELDS//
        private readonly IOrderCore _orderCore;
    }
}
