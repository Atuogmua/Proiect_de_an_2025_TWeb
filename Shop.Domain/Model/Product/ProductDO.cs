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
          public int ID { get; set; }
          public string Name { get; set; }
          public decimal Price { get; set; }
          public int Stock { get; set; }
          public string Description { get; set; }
          public string ProductImagePath { get; set; }
     }
}
