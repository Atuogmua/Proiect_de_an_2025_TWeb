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
using Shop.Domain.Model.Order;

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
                    cfg.CreateMap<UserProfile, UserProfileDO>();
                    cfg.CreateMap<UserProfileDO, UserProfile>();
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

               try
               {
                    // Get current user information
                    string username = GetCurrentUsername();
                    if (string.IsNullOrEmpty(username))
                    {
                         return RedirectToAction("Login");
                    }

                    var userProfile = _session.GetUserProfile(username);
                    if (userProfile != null)
                    {
                         var profileModel = _mapper.Map<UserProfile>(userProfile);
                         return View(profileModel);
                    }
                    else
                    {
                         // Create empty profile for new users
                         var emptyProfile = new UserProfile { Username = username };
                         return View(emptyProfile);
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"UserPage error: {ex.Message}");
                    TempData["ErrorMessage"] = "Unable to load profile. Please try again.";
                    return View(new UserProfile());
               }
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult UserPage(UserProfile model)
          {
               if (Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login");
               }

               try
               {
                    if (ModelState.IsValid)
                    {
                         string username = GetCurrentUsername();
                         if (string.IsNullOrEmpty(username))
                         {
                              return RedirectToAction("Login");
                         }

                         var profileData = _mapper.Map<UserProfileDO>(model);
                         profileData.Username = username;
                         profileData.UpdateDateTime = DateTime.Now;

                         var updateResult = _session.UpdateUserProfile(profileData);
                         if (updateResult)
                         {
                              TempData["SuccessMessage"] = "Profile updated successfully!";
                              return RedirectToAction("UserPage");
                         }
                         else
                         {
                              return View(model);
                         }
                    }

                    return View(model);
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Profile update error: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                    return View(model);
               }
          }

          [HttpGet]
          public ActionResult OrderHistory()
          {
               if (Session["LoginStatus"] != "login")
               {
                    return RedirectToAction("Login");
               }

               try
               {
                    string username = GetCurrentUsername();
                    if (string.IsNullOrEmpty(username))
                    {
                         return RedirectToAction("Login");
                    }

                    var orders = _session.GetUserOrderHistory(username);
                    return View(orders);
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Order history error: {ex.Message}");
                    TempData["ErrorMessage"] = "Unable to load order history. Please try again.";
                    return View(new List<OrderHistoryDO>());
               }
          }

          [HttpPost]
          public ActionResult Logout()
          {
               try
               {
                    Session.Clear();
                    Session.Abandon();

                    if (Request.Cookies["ShopAuth"] != null)
                    {
                         var cookie = new HttpCookie("ShopAuth")
                         {
                              Expires = DateTime.Now.AddDays(-1)
                         };
                         Response.Cookies.Add(cookie);
                    }

                    return RedirectToAction("Index", "Home");
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Logout error: {ex.Message}");
                    return RedirectToAction("Index", "Home");
               }
          }

          private string GetCurrentUsername()
          {
               if (Session["Username"] != null)
               {
                    return Session["Username"].ToString();
               }

               if (Request.Cookies["ShopAuth"] != null)
               {
                    try
                    {
                         var cookieValue = Request.Cookies["ShopAuth"].Value;
                         return _session.GetUsernameFromCookie(cookieValue);
                    }
                    catch (Exception ex)
                    {
                         System.Diagnostics.Debug.WriteLine($"Cookie parsing error: {ex.Message}");
                    }
               }

               return null;
          }
     }
}