﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class CartItem
     {
          public int ProductId { get; set; }
          public string Name { get; set; }
          public decimal Price { get; set; }
          public int Quantity { get; set; }
          public string ProductImagePath { get; set; }

     }
}
