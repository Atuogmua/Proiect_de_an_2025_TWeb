using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.User
{
     public class UserProfileDO
     {
          public string Username { get; set; }
          public string FullName { get; set; }
          public string Email { get; set; }
          public string PhoneNumber { get; set; }
          public DateTime? UpdateDateTime { get; set; }
     }

}
