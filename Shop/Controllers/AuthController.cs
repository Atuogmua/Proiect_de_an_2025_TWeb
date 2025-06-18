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
          private readonly IOrder _order;
          private readonly IMapper _mapper;

          public AuthController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _session = bl.GetSessionBL();
               _order = bl.GetOrderBL();

               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<UserLogin, UserLoginDO>();
                    cfg.CreateMap<UserRegister, URegisterDO>();
                    cfg.CreateMap<UserProfile, UserProfileDO>();
                    cfg.CreateMap<UserProfileDO, UserProfile>();
                    cfg.CreateMap<OrderDO, Order>()
                        .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.Items));

                    cfg.CreateMap<OrderItemDO, CartItem>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                        .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
                        .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

                    cfg.CreateMap<CartItem, OrderItemDO>();
                    cfg.CreateMap<Order, OrderDO>();
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
                    return RedirectToAction("Login");

               try
               {
                    string username = null;

                    if (Request.Cookies["X-KEY"]?.Value is string cookieValue && !string.IsNullOrWhiteSpace(cookieValue))
                    {
                         try
                         {
                              username = _session.GetUsernameFromCookie(cookieValue);
                         }
                         catch (Exception ex)
                         {
                              System.Diagnostics.Debug.WriteLine($"[OrderHistory] Cookie parsing failed: {ex.Message}");
                         }
                    }


                    if (string.IsNullOrEmpty(username))
                         return RedirectToAction("Login");

                    var profileDO = _session.GetUserProfile(username);
                    var ordersDO = _order.GetUserOrderHistory(username);

                    var model = new UserProfileViewModel
                    {
                         Profile = _mapper.Map<UserProfile>(profileDO),
                         Orders = _mapper.Map<List<Order>>(ordersDO)
                    };

                    return View(model);
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"UserPage error: {ex.Message}");
                    TempData["ErrorMessage"] = "Unable to load profile.";
                    return View(new UserProfileViewModel
                    {
                         Profile = new UserProfile(),
                         Orders = new List<Order>()
                    });
               }
          }


          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult UserPage(UserProfileViewModel model)
          {
               if (Session["LoginStatus"] != "login")
                    return RedirectToAction("Login");

               try
               {
                    if (ModelState.IsValid)
                    {
                         string username = null;

                         if (Request.Cookies["X-KEY"]?.Value is string cookieValue && !string.IsNullOrWhiteSpace(cookieValue))
                         {
                              try
                              {
                                   username = _session.GetUsernameFromCookie(cookieValue);
                              }
                              catch (Exception ex)
                              {
                                   System.Diagnostics.Debug.WriteLine($"[OrderHistory] Cookie parsing failed: {ex.Message}");
                              }
                         }
                         if (string.IsNullOrEmpty(username))
                              return RedirectToAction("Login");

                         var profileData = _mapper.Map<UserProfileDO>(model.Profile);
                         profileData.Username = username;
                         profileData.UpdateDateTime = DateTime.Now;

                         var result = _session.UpdateUserProfile(profileData);
                         if (result)
                         {
                              TempData["SuccessMessage"] = "Profile updated successfully!";
                              return RedirectToAction("UserPage");
                         }
                    }

                    return View(model);
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Profile update error: {ex.Message}");
                    ModelState.AddModelError("", "Unexpected error updating profile.");
                    return View(model);
               }
          }


          [HttpGet]
          public ActionResult OrderHistory()
          {
               if (Session["LoginStatus"] != "login")
                    return RedirectToAction("Login");
               UMinimal user = null;

               if (Request.Cookies["X-KEY"]?.Value is string cookieValue && !string.IsNullOrWhiteSpace(cookieValue))
               {
                    try
                    {
                         user = _session.GetUserByCookie(cookieValue);
                    }
                    catch (Exception ex)
                    {
                         System.Diagnostics.Debug.WriteLine($"[OrderHistory] Cookie parsing failed: {ex.Message}");
                    }
               }

               if (string.IsNullOrWhiteSpace(user.Username))
               {
                    System.Diagnostics.Debug.WriteLine("[OrderHistory] Username is null or empty.");
                    return RedirectToAction("Login");
               }

               var ordersDO = _order.GetUserOrderHistory(user.Username);
               var orders = _mapper.Map<List<Order>>(ordersDO);

               return View(orders);
               
          }

          [HttpGet]
          public ActionResult Logout()
          {
               try
               {
                    Session.Clear();
                    Session.Abandon();

                    if (Request.Cookies["X-KEY"] != null)
                    {
                         var expiredCookie = new HttpCookie("X-KEY")
                         {
                              Expires = DateTime.Now.AddDays(-1)
                         };
                         Response.Cookies.Add(expiredCookie);
                    }

                    return RedirectToAction("Index", "Home");
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Logout error: {ex.Message}");
                    return RedirectToAction("Index", "Home");
               }
          }


          [HttpPost]
          [ValidateAntiForgeryToken]
          [ActionName("Logout")]
          public ActionResult LogoutPost()
          {
               try
               {
                    Session.Clear();
                    Session.Abandon();

                    if (Request.Cookies["X-KEY"] != null)
                    {
                         var expiredCookie = new HttpCookie("X-KEY")
                         {
                              Expires = DateTime.Now.AddDays(-1)
                         };
                         Response.Cookies.Add(expiredCookie);
                    }

                    return RedirectToAction("Index", "Home");
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Logout error: {ex.Message}");
                    return RedirectToAction("Index", "Home");
               }
          }


          [HttpGet]
          public ActionResult ResetPassword()
          {
               if (Session["LoginStatus"] != "login")
                    return RedirectToAction("Login");

               string username = null;

               if (Request.Cookies["X-KEY"]?.Value is string cookieValue && !string.IsNullOrWhiteSpace(cookieValue))
               {
                    try
                    {
                         username = _session.GetUsernameFromCookie(cookieValue);
                    }
                    catch (Exception ex)
                    {
                         System.Diagnostics.Debug.WriteLine($"[OrderHistory] Cookie parsing failed: {ex.Message}");
                    }
               }
               if (string.IsNullOrEmpty(username))
                    return RedirectToAction("Login");

               var model = new ResetPasswordViewModel
               {
                    Username = username
               };

               return View(model);
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult ResetPassword(ResetPasswordViewModel model)
          {
               if (Session["LoginStatus"] != "login")
                    return RedirectToAction("Login");

               if (!ModelState.IsValid)
                    return View(model);

               try
               {
                    var changeResult = _session.ChangePassword(model.Username, model.CurrentPassword, model.NewPassword);
                    if (changeResult)
                    {
                         TempData["SuccessMessage"] = "Password changed successfully!";
                         return RedirectToAction("UserPage");
                    }
                    else
                    {
                         TempData["ErrorMessage"] = "Current password is incorrect.";
                         return View(model);
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"ResetPassword error: {ex.Message}");
                    TempData["ErrorMessage"] = "Something went wrong. Please try again.";
                    return View(model);
               }
          }



          [HttpGet]
          public ActionResult UpdateProfile() => RedirectToAction("UserPage");

          [HttpPost]
          public ActionResult UpdateProfile(UserProfileViewModel model) => UserPage(model);


     }
}