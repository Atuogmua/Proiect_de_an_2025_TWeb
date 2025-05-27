using AutoMapper;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;
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
          private readonly IProduct _product;
          private readonly IMapper _mapper;

          private void SetUserData()
          {
               var userData = Session["UserData"] as UserData;
               if (userData != null) 
               {
                    ViewBag.UserData = userData;
               }
          }

          public ShopController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _product = bl.GetProductBL();

               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<ProductDO, Product>();
                    cfg.CreateMap<Product, ProductDO>();
               });

               _mapper = config.CreateMapper();
          }
          
          public ActionResult Catalog()
          {
               SetUserData();
               var productDOList = _product.GetAllProducts();
               var products = _mapper.Map<List<Product>>(productDOList);
               return View(products);
          }
          public ActionResult CheckOut()
          {
               SetUserData();
               var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
               return View();
          }

          public ActionResult ProductPage(int ProductID)
          {
               SetUserData();
               var product = _product.GetProductById(ProductID);

               if (product == null)
               {
                    return HttpNotFound();
               }

               var vm = _mapper.Map<Product>(product);

               return View(vm);
          }
    }
}