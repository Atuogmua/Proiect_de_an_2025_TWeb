using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Shop.Models;
using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;


namespace Shop.Controllers
{
    public class AuthController : Controller
    {
          private readonly ISession _session;
          public AuthController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _session = bl.GetSessionBL();
          }
          // GET: Auth
          public ActionResult Index()
          {
               return View();
          }
          [HttpGet]
          public ActionResult Register()
          {
               return View();
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
                         UserIP = Request.UserHostAddress,
                         LoginDateTime = DateTime.Now

                    };

                    var userLogin = _session.UserLogin(data);
                    /*if (userLogin.Status)
                    {



                         return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                         ModelState.AddModelError("", userLogin.StatusMsg);
                         return View();
                    }*/
               } 



                    return View();
          }

          public ActionResult Login()
          {
               return View();
          }
          public ActionResult UserPage() 
          {
               return View();
          }
    }
}