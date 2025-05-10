using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.User
{
     public class URegisterDO
     {
          public string Username { get; set; }
          public string Email { get; set; }
          public string Password { get; set; }
          public string UserIP { get; set; }
          public DateTime RegisterDateTime { get; set; }

     }
}
