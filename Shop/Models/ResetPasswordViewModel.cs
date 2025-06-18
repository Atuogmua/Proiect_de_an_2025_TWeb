using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class ResetPasswordViewModel
     {
          public string Username { get; set; }

          [Required(ErrorMessage = "Current password is required")]
          [DataType(DataType.Password)]
          public string CurrentPassword { get; set; }

          [Required(ErrorMessage = "New password is required")]
          [DataType(DataType.Password)]
          public string NewPassword { get; set; }
     }
}
