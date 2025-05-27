using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Shop.Models;
using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;
using AutoMapper;
using Shop.Filters;
using Shop.BusinessLogic.DBDataContext;
using System.IO;


namespace Shop.Controllers
{
     [AdminMod]
    public class AdminController : Controller
    {
          private readonly IProduct _product;
          private readonly IMapper _mapper;

          public AdminController()
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
          
          //Dashboard
          public ActionResult Index()
          {
               var products = _product.GetAllProducts();
               var viewProducts = _mapper.Map<List<Product>>(products);
               return View(viewProducts);
          }

          public ActionResult ManageUsers()
          {
               return View();
          }

          [HttpGet]
          public ActionResult AddProduct()
          {
               return View();
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult AddProduct(ProductDO product, HttpPostedFileBase ProductImage) 
          {
               if (!ModelState.IsValid)
               {
                    return View(product);
               }
               if (ProductImage != null && ProductImage.ContentLength > 0)
               {
                    string fileName = Path.GetFileName(ProductImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/ProductImages/"), fileName);
                    ProductImage.SaveAs(path);
                    product.ProductImagePath = "/ProductImages/" + fileName;
               }

               bool success = _product.AddProduct(product);
               if (success)
               {
                    return RedirectToAction("Index", "Admin");
               }

               ModelState.AddModelError("", "Could not save the product.");
               return View(product);
          }
     }
}