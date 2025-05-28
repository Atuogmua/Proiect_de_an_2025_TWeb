using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic
{
     public class BusinessLogic
     {
          public IProduct GetProductBL()
          {
               return new ProductBL();
          }

          public ISession GetSessionBL()
          {
               return new SessionBL();
          }

          public ICart GetCartBL()
          {
               return new CartBL();
          }

          public IOrder GetOrderBL()
          {
               return new OrderBL();
          }
     }
}
