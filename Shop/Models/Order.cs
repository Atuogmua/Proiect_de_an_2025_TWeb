using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class Order
     {
          public string FirstName { get; set; }
          public string LastName { get; set; }
          public string Address { get; set; }
          public string Email { get; set; }
          public string Phone { get; set; }
          public string Notes { get; set; }

          public List<CartItem> CartItems { get; set; }
          public decimal TotalPrice => CartItems?.Sum(i => i.Price * i.Quantity) ?? 0;
     }
}
