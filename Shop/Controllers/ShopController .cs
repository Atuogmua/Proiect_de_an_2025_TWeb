using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
     public class ShopController : Controller
     {
        // GET: Shop
          public ActionResult Cart()
          {
                 return View();
          }
          public ActionResult Catalog()
          {
               return View();
          }
          public ActionResult CheckOut()
          {
               return View();
          }
    }
}