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
          private readonly IProduct _product;
          private readonly IMapper _mapper;
          
          public HomeController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _session = bl.GetSessionBL();
               _product = bl.GetProductBL();

               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<UMinimal, UserData>();
               });

               _mapper = config.CreateMapper();
          }

          public ActionResult Index()
          {
               SessionStatus();

               var cookieValue = Request.Cookies["X-KEY"];
               if(cookieValue == null || string.IsNullOrEmpty(cookieValue.Value))
               {
                    return RedirectToAction("Login", "Auth");
               }

               var user = _session.GetUserByCookie(cookieValue.Value);
               if (user == null) 
               {
                    return RedirectToAction("Login", "Auth");
               }

               var products = _product.GetAllProducts();
               var mappedProducts = _mapper.Map<List<Product>>(products);

               var u = new UserData
               {
                    Username = user.Username,
                    Products = mappedProducts
               };

               return View(u);
          }

          public ActionResult Product()
          {
               var cookie = Request.Cookies["X-KEY"];
               var user = _session.GetUserByCookie(cookie?.Value);

               if (!int.TryParse(Request.QueryString["p"], out int productId))
               {
                    return RedirectToAction("Error404");
               }

               var product = _product.GetProductById(productId);
               if (product == null)
               {
                    return RedirectToAction("Error404");
               }

               var mappedProduct = _mapper.Map<Product>(product);
               
               UserData u = new UserData
               {
                    Username = user?.Username ?? "Customer",
                    SingleProduct = mappedProduct
               };

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
     }
}