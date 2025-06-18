using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class CustomerSummary
     {
          public string FullName { get; set; }
          public int OrderCount { get; set; }
          public decimal TotalSpent { get; set; }
          public bool Onboarded { get; set; }
          public DateTime RegisterDate { get; set; }
     }
}
