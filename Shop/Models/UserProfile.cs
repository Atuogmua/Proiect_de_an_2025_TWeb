using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class UserProfile
     {
          public string Username { get; set; }
          public string FullName { get; set; }
          public string Email { get; set; }
          public string PhoneNumber { get; set; }
          public List<Order> Orders { get; set; }

     }

}
