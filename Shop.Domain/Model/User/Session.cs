using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Shop.Domain.Model.User
{
     public class Session
     {
          [Key]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int SessionId { get; set; }
          [Required]
          [StringLength(30)]
          public string UserName { get; set; }
          [Required]
          public string CookieString { get; set; }
          [Required]
          public DateTime ExpireTime { get; set; }
     }
}
