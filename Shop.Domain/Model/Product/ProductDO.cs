using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Product
{
     public class ProductDO
     {
          public string Name { get; set; }
          public float Price { get; set; }
          public int Stock { get; set; }
          public string Description { get; set; }
          public string ProductImagePath { get; set; }
     }
}
