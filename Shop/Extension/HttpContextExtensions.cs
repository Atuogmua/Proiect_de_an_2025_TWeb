using Shop.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shop.Extension
{
     public static class HttpContextExtensions
     {
          public static UMinimal GetMySessionObject(this HttpContext current)
          {
               return (UMinimal)current?.Session["__SessionObject"];
          }

          public static void SetMySessionObject(this HttpContext current, UMinimal profile)
          {
               current.Session.Add("__SessionObject", profile);
          }

     }
}
