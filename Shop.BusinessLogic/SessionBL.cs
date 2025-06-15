using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Order;
using Shop.Domain.Model.User;
using System.Collections.Generic;
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

          public UserProfileDO GetUserProfile(string username) 
          {
               return GetUserProfileAction(username);     
          }
          public bool UpdateUserProfile(UserProfileDO profile)
          {
               return UpdateUserProfileAction(profile);

          }
          public List<OrderHistoryDO> GetUserOrderHistory(string username)
          {
               return GetUserOrderHistoryAction(username);

          }
          public string GetUsernameFromCookie(string cookieValue)
          {
               return GetUsernameFromCookieAction(cookieValue);
          }

     }
}
