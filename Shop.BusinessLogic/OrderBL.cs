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

          public bool PlaceOrderAction(OrderDO order)
          {
               return PlaceOrder(order);
          }

          public List<OrderDO> GetUserOrderHistory(string username)
          {
               return GetUserOrderHistoryAction(username);
          }
          public List<OrderDO> GetAllOrders()
          {
               return GetAllOrdersAction();
          }

          public bool DeleteOrder(int id)
          {
               return DeleteOrderAction(id);
          }
     }

}
