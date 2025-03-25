using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
     public class UserLogin
     {
          public string Username { get; set; }
          public string Password { get; set; }
          public string UserIP { get; set; }
          public DataSetDateTime LoginDateTime { get; set; }

     }
}