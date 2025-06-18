using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class Review
     {
          public int ReviewId {  get; set; }
          public int ProductId { get; set; }
          public string Username { get; set; }
          public string Content { get; set; }
          public int Rating { get; set; }
          public DateTime CreatedAt { get; set; }
     }
}
