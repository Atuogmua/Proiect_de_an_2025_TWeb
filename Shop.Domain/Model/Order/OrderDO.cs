﻿using System;
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

          public DateTime OrderDate { get; set; }

          public string Address { get; set; }

          public decimal TotalPrice { get; set; }

          public List<OrderItemDO> Items { get; set; }
     }
}
