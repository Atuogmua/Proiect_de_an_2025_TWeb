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
               return View(new ProductDO()); // Initialize with empty model
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult AddProduct(ProductDO product, HttpPostedFileBase ProductImage)
          {
               if (!ModelState.IsValid)
               {
                    return View(product);
               }

               // Handle image upload
               if (ProductImage != null && ProductImage.ContentLength > 0)
               {
                    try
                    {
                         // Validate file type
                         string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                         string fileExtension = Path.GetExtension(ProductImage.FileName).ToLower();

                         if (!allowedExtensions.Contains(fileExtension))
                         {
                              ModelState.AddModelError("", "Please select a valid image file (jpg, jpeg, png, gif, bmp).");
                              return View(product);
                         }

                         // Create unique filename to avoid conflicts
                         string originalFileName = Path.GetFileNameWithoutExtension(ProductImage.FileName);
                         string uniqueFileName = $"{originalFileName}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

                         // Ensure the images directory exists
                         string imagesDirectory = Server.MapPath("/Content/images/");
                         if (!Directory.Exists(imagesDirectory))
                         {
                              Directory.CreateDirectory(imagesDirectory);
                         }

                         string path = Path.Combine(imagesDirectory, uniqueFileName);
                         ProductImage.SaveAs(path);

                         // Set the image path in the product model
                         product.ProductImagePath = uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                         ModelState.AddModelError("", "Error uploading image: " + ex.Message);
                         return View(product);
                    }
               }

               // Add the product to the database
               try
               {
                    bool success = _product.AddProduct(product);
                    if (success)
                    {
                         TempData["SuccessMessage"] = "Product added successfully!";
                         return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                         ModelState.AddModelError("", "Could not save the product to the database.");
                    }
               }
               catch (Exception ex)
               {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
               }

               return View(product);
          }

          // Additional method for editing products (if needed)
          [HttpGet]
          public ActionResult EditProduct(int id)
          {
               var product = _product.GetProductById(id);
               if (product == null)
               {
                    return HttpNotFound();
               }
               return View("AddProduct", product); // Reuse the same view
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult EditProduct(ProductDO product, HttpPostedFileBase ProductImage)
          {
               if (!ModelState.IsValid)
               {
                    return View("AddProduct", product);
               }

               // Handle image upload for editing
               if (ProductImage != null && ProductImage.ContentLength > 0)
               {
                    try
                    {
                         // Validate file type
                         string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                         string fileExtension = Path.GetExtension(ProductImage.FileName).ToLower();

                         if (!allowedExtensions.Contains(fileExtension))
                         {
                              ModelState.AddModelError("", "Please select a valid image file (jpg, jpeg, png, gif, bmp).");
                              return View("AddProduct", product);
                         }

                         // Delete old image if it exists
                         if (!string.IsNullOrEmpty(product.ProductImagePath))
                         {
                              string oldImagePath = Server.MapPath("/Content/images/" + product.ProductImagePath);
                              if (System.IO.File.Exists(oldImagePath))
                              {
                                   System.IO.File.Delete(oldImagePath);
                              }
                         }

                         // Save new image
                         string originalFileName = Path.GetFileNameWithoutExtension(ProductImage.FileName);
                         string uniqueFileName = $"{originalFileName}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

                         string imagesDirectory = Server.MapPath("/Content/images/");
                         if (!Directory.Exists(imagesDirectory))
                         {
                              Directory.CreateDirectory(imagesDirectory);
                         }

                         string path = Path.Combine(imagesDirectory, uniqueFileName);
                         ProductImage.SaveAs(path);

                         product.ProductImagePath = uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                         ModelState.AddModelError("", "Error uploading image: " + ex.Message);
                         return View("AddProduct", product);
                    }
               }

               try
               {
                    bool success = _product.UpdateProduct(product);
                    if (success)
                    {
                         TempData["SuccessMessage"] = "Product updated successfully!";
                         return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                         ModelState.AddModelError("", "Could not update the product in the database.");
                    }
               }
               catch (Exception ex)
               {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
               }

               return View("AddProduct", product);
          }
     }
}