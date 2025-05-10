using Shop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.User
{
     public class UMinimal
     {
          public int Id { get; set; }
          public string Username { get; set; }
          public string Email { get; set; }
          public DateTime LastLogIn { get; set; }
          public string LastIp {  get; set; }
          public URole Level { get; set; }
     }
}
