using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
     public class UserData
     {
          public int Id { get; set; }
          public string Username { get; set; }
          public string Email { get; set; }
          public string FullName { get; set; }
          [DataType(DataType.Password)]
          public string Password { get; set; }
          public List<Product> Products { get; set; }
          public bool IsBanned {  get; set; }
          public Product SingleProduct { get; set; }
     }
}