﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Models
{
     public class UserProfileViewModel
     {
          public UserProfile Profile { get; set; }

          public List<Order> Orders { get; set; }
     }
}
