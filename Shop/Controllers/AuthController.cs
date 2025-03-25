using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Shop.Models;

namespace Shop.Controllers
{
    public class AuthController : Controller
    {
          // GET: Auth
          private readonly ISession _session;
          public AuthController()
          {
               var bl = new BussinessLogic();
               _session = bl.GetSession();
          }
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Register(UserLogin login)
          {
               if (ModelState.IsValid)
               {
                    UserLoginDO data = new UserLoginDO
                    {
                         Username = login.Username,
                         Password = login.Password,
                         LoginIp = login.LoginIp,
                         LoginDateTime = DateTime.Now

                    };

                    var userLogin = _session.UserLogin(data);
                    if (userLogin.Status)
                    {



                         return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                         ModelState.AddModelError("", userLogin.StatusMsg);
                         return View();
                    }
               } 



                    return View();
          }
          public ActionResult UserPage() 
          {
               return View();
          }
    }
}