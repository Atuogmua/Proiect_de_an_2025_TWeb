using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Domain.Model.Order
{
     public class OrderItemDb
     {
          [Key]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }

          public int ProductId { get; set; }

          public string ProductName { get; set; }

          public int Quantity { get; set; }

          public decimal UnitPrice { get; set; }

          // Foreign Key
          public int ODbTableId { get; set; }

          [ForeignKey("ODbTableId")]
          public virtual ODbTable ODbTable { get; set; }
     }
}
