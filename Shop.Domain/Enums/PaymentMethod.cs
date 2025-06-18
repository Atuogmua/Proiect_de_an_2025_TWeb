using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Enums
{
     public enum PaymentMethod
     {
          [Display(Name = "Credit Card")]
          CreditCard = 1,

          [Display(Name = "Debit Card")]
          DebitCard = 2,

          [Display(Name = "PayPal")]
          PayPal = 3,

          [Display(Name = "Cash on Delivery")]
          CashOnDelivery = 4,

          [Display(Name = "Bank Transfer")]
          BankTransfer = 5
     }
}
