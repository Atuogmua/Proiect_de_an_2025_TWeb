using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Order;
using Shop.Domain.Model.User;
using System;
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
          public List<UMinimal> GetAllUsers()
          {
               return GetAllUsersAction();
          }

          public UserProfileDO GetUserProfile(string username) 
          {
               return GetUserProfileAction(username);     
          }
          public bool UpdateUserProfile(UserProfileDO profile)
          {
               return UpdateUserProfileAction(profile);

          }
          public string GetUsernameFromCookie(string cookieValue)
          {
               return GetUsernameFromCookieAction(cookieValue);
          }

          public bool ChangePassword(string username, string oldPassword, string newPassword)
          {
               return ChangePasswordAction(username, oldPassword, newPassword);
          }
          public bool ToggleBanUser(int userId)
          {
               return ToggleBanUserAction(userId);
          }
          

     }
}
