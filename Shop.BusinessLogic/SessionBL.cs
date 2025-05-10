using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;
using System.Web;

namespace Shop.BusinessLogic
{
     public class SessionBL : UserApi, ISession
     {
          public ULoginResp UserLogin(UserLoginDO data)
          {
               return UserLoginAction(data);
          }

          public HttpCookie GenCookie(string loginUsername)
          {
               return Cookie(loginUsername);
          }

          public UMinimal GetUserByCookie(string apiCookieValue) 
          {
               return UserCookie(apiCookieValue);
          }
          public ULoginResp RegisterUser(URegisterDO data)
          {
               return UserRegisterAction(data);
          }

     }
}
