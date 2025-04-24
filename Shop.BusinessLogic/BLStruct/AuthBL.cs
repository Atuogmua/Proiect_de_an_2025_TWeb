using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;

namespace Shop.BusinessLogic.BLStruct
{
     public class AuthBL : UserApi, IAuth
     {
          public ULoginResp UserAuthLogic(UserLoginDO data)
          {
               return UserLoginAction(data);
          }
     }
}
