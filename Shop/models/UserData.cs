using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
     public class UserData
     {
          public string Username { get; set; }
          [DataType(DataType.Password)]
          public string Password { get; set; }
          public List<string> Products { get; set; }
          public string SingleProduct { get; set; }
     }
}