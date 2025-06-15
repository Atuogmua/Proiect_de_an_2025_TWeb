using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Order
{
     public class OrderHistoryDO
     {
          public int OrderId { get; set; }
          public DateTime OrderDate { get; set; }
          public string Address { get; set; }
          public decimal TotalPrice { get; set; }
          public List<OrderItemDO> Items { get; set; }
     }
}
