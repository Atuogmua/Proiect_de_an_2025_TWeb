using Shop.Domain.Model.Order;
using Shop.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shop.BusinessLogic.Interface
{
     public interface ISession
     {
          ULoginResp UserLogin(UserLoginDO data);
          HttpCookie GenCookie(string loginUsername);
          UMinimal GetUserByCookie(string cookieValue);
          ULoginResp RegisterUser(URegisterDO data);

          UserProfileDO GetUserProfile(string username);
          bool UpdateUserProfile(UserProfileDO profile);
          List<OrderHistoryDO> GetUserOrderHistory(string username);
          string GetUsernameFromCookie(string cookieValue);

     }
}
