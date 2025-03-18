using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
     public class HomeController : Controller
     {
        // GET: Home
          public ActionResult Index()
          {
               var product = Request.QueryString["p"];

               UserData u = new UserData();
               u.Username = "Costumer";
               u.Products = new List<string> {"Product #1", "Product #2", "Product #3" };
                 
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
          
    }
}