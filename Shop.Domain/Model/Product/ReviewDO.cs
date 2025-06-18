using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Product
{
     public class ReviewDO
     {
          public int Id { get; set; }
          public int ProductId { get; set; }
          public string Username { get; set; }
          public string Content { get; set; }
          public int Rating { get; set; }
          public DateTime CreatedAt { get; set; }

          public virtual PDBTable Product { get; set; }
     }
}
