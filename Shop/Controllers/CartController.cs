using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.BusinessLogic.DBDataContext;
using Shop.Extension;
using Shop.Models;

namespace Shop.Controllers
{
     public class CartController : Controller
     {
          // GET: Home
          public ActionResult AddToCart(int id, int quantity)
          {
               // Fake DB lookup (replace with your EF context)
               var product = new ShopContext().Products.FirstOrDefault(p => p.Id == id);
               if (product == null)
                    return HttpNotFound();

               var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

               var existing = cart.FirstOrDefault(c => c.ProductId == id);
               if (existing != null)
               {
                    existing.Quantity += quantity;
               }
               else
               {
                    cart.Add(new CartItem
                    {
                         ProductId = id,
                         Name = product.Name,
                         Price = product.Price,
                         Quantity = quantity
                    });
               }

               Session["Cart"] = cart;

               return RedirectToAction("Index", "Cart");
          }

          public ActionResult Index()
          {
               var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
               return View(cart);
          }

     }
}