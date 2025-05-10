using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Model.User;

namespace Shop.BusinessLogic.DBDataContext
{
     internal class SessionContext : DbContext
     {
          public SessionContext() : base("name=Shop") { }

          public virtual DbSet<Session> Sessions { get; set; }
     }
     
}
