using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;
using Shop.Extension;
using Shop.Models;

namespace Shop.Controllers
{
     public class HomeController : BaseController
     {
          private readonly ISession _session;
          private readonly IMapper _mapper;

          public HomeController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _session = bl.GetSessionBL();

               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<UMinimal, UserData>();
               });

               _mapper = config.CreateMapper();
          }

          public ActionResult Index()
          {
               var user = GetLoggedInUser();
               if (user == null) 
               {
                    return RedirectToAction("Login", "Auth");
               }

               var u = _mapper.Map<UserData>(user);
               return View(u);
          }

          public ActionResult Product()
          {
               var user = GetLoggedInUser();
               if (user == null)
               {
                    return RedirectToAction("Login", "Auth");
               }

               var product = Request.QueryString["p"];

               var u = _mapper.Map<UserData>(user);
               u.SingleProduct = product;

               return View(u);
          }

          [HttpPost]
          public ActionResult Product(string btn)
          {
               return RedirectToAction("Product", "Home", new { @p = btn });
          }
          public ActionResult AboutUs()
          {
               return View();
          }
          public ActionResult ContactUs()
          {
               return View();
          }
          public ActionResult Services()
          {
               return View();
          }

          public ActionResult Error404()
          {
               return View();
          }

          private UMinimal GetLoggedInUser()
          {
               var cookie = Request.Cookies["X-KEY"];
               if (cookie == null)
                    return null;

               return _session.GetUserByCookie(cookie.Value);
          }
     }
}