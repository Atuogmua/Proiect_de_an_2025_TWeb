using Shop.Domain.Model.Order;
using Shop.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.DBDataContext
{
     public class ShopContext : DbContext
     {
          public ShopContext() : base("name=Shop") { }
          public DbSet<PDBTable> Products { get; set; }
          public DbSet<OrderDO> Orders { get; set; }
          public DbSet<OrderItemDO> OrderItems { get; set; }
     }
}
