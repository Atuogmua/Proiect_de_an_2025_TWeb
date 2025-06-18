using Shop.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.Interface
{
     public interface IOrder
     {
          bool PlaceOrderAction(OrderDO order);
          List<OrderDO> GetUserOrderHistory(string username);
          List<OrderDO> GetAllOrders();
          bool DeleteOrder(int id);
     }
}
