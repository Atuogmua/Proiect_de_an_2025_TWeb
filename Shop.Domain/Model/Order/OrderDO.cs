using Shop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Order
{
     public class OrderDO
     {
          public int Id { get; set; }

          public string Username { get; set; }
          public string FirstName { get; set; }
          public string LastName { get; set; }
          public string Email { get; set; }
          public string Phone { get; set; }
          public string OrderStatus { get; set; } = "Pending";
          public DateTime OrderDate { get; set; }
          public string Address { get; set; }
          public decimal TotalPrice { get; set; }
          public List<OrderItemDO> Items { get; set; }
          public string Status { get; set; }
          public PaymentMethod Payment { get; set; }
     }
}
