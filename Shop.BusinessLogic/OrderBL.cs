using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic
{
     public class OrderBL : OrderApi, IOrder
     {

          public OrderDO PlaceOrderAction(OrderDO order)
          {
               return PlaceOrder(order);
          }

          public List<OrderDO> GetUserOrdersAction(string username)
          {
               return GetUserOrders(username);
          }



     }

}
