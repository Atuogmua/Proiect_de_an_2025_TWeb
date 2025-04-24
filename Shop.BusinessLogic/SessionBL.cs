using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;

namespace Shop.BusinessLogic
{
     public class SessionBL : UserApi, ISession
     {
          public ULoginResp UserLogin(UserLoginDO data)
          {
               return UserLoginAction(data);
          }

     }
}
