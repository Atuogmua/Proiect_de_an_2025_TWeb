using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
          public ActionResult Register()
          {
                    return View();
          }
          public ActionResult UserPage() 
          {
               return View();
          }
    }
}