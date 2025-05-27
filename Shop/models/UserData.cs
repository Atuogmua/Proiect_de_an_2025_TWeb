using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
     public class UserData
     {
          public string Username { get; set; }
          [DataType(DataType.Password)]
          public string Password { get; set; }
          public List<Product> Products { get; set; }
          public Product SingleProduct { get; set; }
     }
}