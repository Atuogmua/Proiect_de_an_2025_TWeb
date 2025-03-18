using System.Collections.Generic;

namespace Shop.Models
{
     public class UserData
     {
          public string Username { get; set; }
          public List<string> Products { get; set; }
          public string SingleProduct { get; set; }
     }
}