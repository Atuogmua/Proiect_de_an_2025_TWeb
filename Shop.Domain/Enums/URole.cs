using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Enums
{
     public enum URole
     {



          None = 0,
          Banned = 1,
          FraudalentUser = 2,
          Deleted = 3,

          User = 100,

          Moderator = 200,
          Administrator = 400
     }
}
