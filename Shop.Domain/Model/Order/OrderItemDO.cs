﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Order
{
     public class OrderItemDO
     {
          public int Id { get; set; }
          public int OrderId { get; set; }
          public int ProductId { get; set; }
          public string ProductName { get; set; }
          public int Quantity { get; set; }
          public decimal UnitPrice { get; set; }
     }

}
