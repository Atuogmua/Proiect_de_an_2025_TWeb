using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Order
{
     public class OrderDO
     {
          [Key]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }
          [Required]
          public string Username { get; set; }
          [Required]
          public DateTime OrderDate { get; set; }
          [Required]
          public string Address { get; set; }
          public string Notes { get; set; }
          [Required]
          public decimal TotalPrice { get; set; }
          [Required]
          public List<OrderItemDO> Items { get; set; }

     }
}
