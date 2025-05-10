using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class UserRegister
     {
          [Required(ErrorMessage = "Username is required")]
          [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
          public string Username { get; set; }

          [Required(ErrorMessage = "Email is required")]
          [EmailAddress(ErrorMessage = "Invalid email address")]
          public string Email { get; set; }

          [Required(ErrorMessage = "Password is required")]
          [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
          [DataType(DataType.Password)]
          public string Password { get; set; }

          [Compare("Password", ErrorMessage = "Passwords do not match")]
          [DataType(DataType.Password)]
          public string ConfirmPassword { get; set; }
     }
}
