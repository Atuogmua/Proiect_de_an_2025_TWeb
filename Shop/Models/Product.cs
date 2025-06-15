using Shop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class Product
     {
          
          public int Id { get; set; }
          public string Name { get; set; }
          public string Description { get; set; }
          public string ProductImagePath {  get; set; }
          public decimal Price {  get; set; }
          public DateTime LastTimeUpdated { get; set; }
          public string CompanyName { get; set; }
          public string Status { get; set; }
          public int Stock { get; set; }
          public PCategories Category { get; set; }


     }
}
