using Shop.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.Interface
{
     public interface ISession
     {
          ULoginResp UserLogin(UserLoginDO data);
     }
}
