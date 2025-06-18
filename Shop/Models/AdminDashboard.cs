using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class AdminDashboard
     {
          public int TotalOrders { get; set; }
          public int TotalProducts { get; set; }
          public int TotalTransactions { get; set; }
          public int TotalUsers { get; set; }
          public decimal TotalRevenue { get; set; }
          public decimal NetRevenue { get; set; }
          public decimal PendingOrders { get; set; }
          public decimal DueAmount { get; set; }
          public decimal OverdueAmount { get; set; }
          public List<CustomerSummary> LatestCustomers { get; set; }
     }
}
