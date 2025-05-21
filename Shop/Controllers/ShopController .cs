using Shop.BusinessLogic.DBDataContext;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
     public class ShopController : Controller
     {
          private readonly ShopContext _shopContext;
        // GET: Shop
          
          public ActionResult Catalog()
          {
               var products = _shopContext.Products
               .Select(p => new Product
               {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ProductQuantity = p.Stock,
                    Description = p.Description,
                    ProductImagePath = "/images/products/default.png"
               }).ToList();

               return View(products);
          }
          public ActionResult CheckOut()
          {
               return View();
          }

          public ActionResult ProductPage(int ProductID)
          {
               var product = _shopContext.Products.FirstOrDefault(p => p.Id == ProductID);

               if (product == null)
               {
                    return HttpNotFound();
               }

               var vm = new Product
               {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ProductQuantity = product.Stock,
                    Description = product.Description,
                    ProductImagePath = "/images/products/default.png"
               };

               return View(vm);
          }
    }
}