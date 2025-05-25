using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Product
{
     public class CartItemDO
     {
          public int ProductId { get; set; }
          public string Name { get; set; }
          public float Price { get; set; }
          public int Quantity { get; set; }
     }
}
