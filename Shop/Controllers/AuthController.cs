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
using AutoMapper;


namespace Shop.Controllers
{
    public class AuthController : Controller
    {
          private readonly ISession _session;
          private readonly IMapper _mapper;
          public AuthController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _session = bl.GetSessionBL();

               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<UserLogin, UserLoginDO>();
                    cfg.CreateMap<UserRegister, URegisterDO>();

               });

               _mapper = config.CreateMapper();
          }
          public ActionResult Index()
          {
               return View();
          }
          [HttpGet]
          public ActionResult Register()
          {
               if (Session["LoginStatus"] == "login")
               {
                    return RedirectToAction("Index", "Home");
               }
               return View();
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Register(UserRegister register)
          {
               try
               {
                    if (ModelState.IsValid)
                    {
                         var data = _mapper.Map<URegisterDO>(register);

                         data.UserIP = Request.UserHostAddress;
                         data.RegisterDateTime = DateTime.Now;

                         var userRegister = _session.RegisterUser(data);
                         if (userRegister.Status)
                         {
                              HttpCookie cookie = _session.GenCookie(register.Username);
                              ControllerContext.HttpContext.Response.Cookies.Add(cookie);


                              return RedirectToAction("Index", "Home");
                         }
                         else
                         {
                              ModelState.AddModelError("", userRegister.StatusMsg);
                              return View(register);
                         }
                    }
                    return View(register);
               }
               catch (Exception ex) 
               {
                    System.Diagnostics.Debug.WriteLine($"Registration error: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                    return View(register);
               }
               
          }

          [HttpGet]
          public ActionResult Login()
          {
               if (Session["LoginStatus"] == "login")
               {
                    return RedirectToAction("Index", "Home");
               }
               return View();
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Login(UserLogin model)
          {
               try
               {
                    if (ModelState.IsValid)
                    {
                         var data = _mapper.Map<UserLoginDO>(model);
                         data.UserIP = Request.UserHostAddress;
                         data.LoginDateTime = DateTime.Now;

                         var loginResult = _session.UserLogin(data);

                         if (loginResult.Status)
                         {
                              HttpCookie cookie = _session.GenCookie(model.Username);
                              ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                              return RedirectToAction("Index", "Home");
                         }
                         else
                         {
                              ModelState.AddModelError("", loginResult.StatusMsg);
                              return View(model);
                         }
                    }

                    return View(model);
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                    return View(model);
               }
          }

          [HttpGet]
          public ActionResult UserPage()
          {
               if (Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login");
               }
               return View();
          }
     }
}