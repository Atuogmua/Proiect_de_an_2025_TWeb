using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;

namespace Shop.BusinessLogic.BLStruct
{
     internal class AuthBL : UserApi, IAuth
     {
          public string UserAuthLogic(UserLoginDO data)
          {
               return UserAuthLogicAction(data);
          }
     }
}
