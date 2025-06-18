using Shop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Model.User
{
     public class UDBTable
     {
          [Key]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }

          [Required]
          [Display(Name = "username")]
          [StringLength(30, MinimumLength = 5, ErrorMessage ="The field minimum char is 5 and maximum is 30.")]
          public string UserName { get; set; }
          [Required]
          [Display(Name ="password")]
          [StringLength(50, MinimumLength = 8, ErrorMessage = "The field minimum char is 8 and maximum is 50.")]
          public string Password { get; set; }
          [Required]
          [Display(Name ="email")]
          [StringLength(50, MinimumLength = 8, ErrorMessage = "The field minimum char is 8 and maximum is 50.")]
          public string Email { get; set; }
          [Display(Name = "phone_number")]
          public string Phone { get; set; }
          [Display(Name = "address")]
          public string Address {  get; set; }
          [Display(Name = "ip")]
          public string IP {  get; set; }
          [Display(Name ="reg_dt")]
          public DateTime RegistrationDataTime { get; set; }
          [Display(Name ="login_dt")]
          public DateTime LastLoginDataTime { get; set; }
          public bool IsBanned {  get; set; }

          public URole Level { get; set; }

     }
}
