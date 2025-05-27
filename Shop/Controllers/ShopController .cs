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
               var productDOList = _product.GetAllProducts();
               var products = _mapper.Map<List<Product>>(productDOList);
               return View(products);
          }
          public ActionResult CheckOut()
          {
               var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
               return View();
          }

          public ActionResult ProductPage(int ProductID)
          {
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