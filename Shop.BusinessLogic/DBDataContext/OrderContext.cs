using Shop.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.DBDataContext
{
     public class OrderContext : DbContext
     {
          public OrderContext() : base("name=Shop") { }

          public virtual DbSet<ODbTable> Orders { get; set; }
     }
}
