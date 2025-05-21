using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.Product
{
     public class PDBTable
     {
          [Key]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }
          [Required]
          public string Name { get; set; }
          [Required]
          public float Price { get; set; }
          [Required]
          public int Stock { get; set; }
          public string Description { get; set; }
          [Required]
          public DataType CreatedAtTime { get; set; }

     }
}
