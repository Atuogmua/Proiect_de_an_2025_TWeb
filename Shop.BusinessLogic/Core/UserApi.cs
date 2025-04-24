using System;
using System.Collections.Generic;
using System.Linq;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;

namespace Shop.BusinessLogic.Core
{
     public class UserApi
     {
          //-----------------------------AUTH--------------------------------
          public ULoginResp UserLoginAction(UserLoginDO data) {
               UDBTable user;

               using (var db = new UserContext())
               {
                    user = db.Users.FirstOrDefault(u => u.UserName == data.Username);
               }

               using (var db = new UserContext())
               {
                    user = (from u in db.Users where u.UserName == data.Username select u).FirstOrDefault();
               }

               if (user != null)
               {

               }

               return new ULoginResp();
          }

     
     }
}
