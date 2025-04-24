using Shop.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.DBDataContext
{
     public class UserContext : DbContext
     {
          public UserContext():base("name=Shop") { }
          public virtual DbSet<UDBTable> Users { get; set; }



     }
}
