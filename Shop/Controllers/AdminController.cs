using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Shop.Models;
using Shop.BusinessLogic.Core;
using Shop.Domain.Enums;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;
using Shop.Domain.Model.User;
using Shop.Domain.Model.Order;
using AutoMapper;
using Shop.Filters;
using System.IO;
using Shop.BusinessLogic;

namespace Shop.Controllers
{
     [AdminMod]
     public class AdminController : Controller
     {
          private readonly IProduct _product;
          private readonly IOrder _order;
          private readonly ISession _session;
          private readonly IMapper _mapper;

          public AdminController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _product = bl.GetProductBL();
               _order = bl.GetOrderBL();
               _session = bl.GetSessionBL();
               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<ProductDO, Product>();
                    cfg.CreateMap<Product, ProductDO>();
                    cfg.CreateMap<UMinimal, UserData>();
                    cfg.CreateMap<Review, ReviewDO>();
                    cfg.CreateMap<ReviewDO, Review>();
                    cfg.CreateMap<Order, OrderDO>();
                    cfg.CreateMap<OrderDO, Order>();
               });
               _mapper = config.CreateMapper();
          }

          //Dashboard
          public ActionResult Index()
          {
               var products = _product.GetAllProducts();
               var viewProducts = _mapper.Map<List<Product>>(products);

               var orders = _order.GetAllOrders(); // Assuming you have this method
               var customers = _session.GetAllUsers(); // Assuming you have this method

               var model = new AdminDashboard
               {
                    TotalProducts = products.Count(),
                    TotalUsers = customers.Count(),
                    TotalOrders = orders.Count(),
                    TotalTransactions = orders.Count(),
                    TotalRevenue = orders.Sum(o => o.TotalPrice),
                    NetRevenue = orders.Sum(o => o.TotalPrice * 0.98m), // assume 2% 
                    PendingOrders = orders.Sum(o => o.TotalPrice)
               };

               return View(model);
          }

          [HttpGet]
          public ActionResult Products(string searchTerm = null, int? id = null)
          {
               var productDOs = _product.Search(searchTerm, id);
               var products = _mapper.Map<List<Product>>(productDOs);
               return View(products);
          }
          public ActionResult Orders()
          {
               var orders = _order.GetAllOrders();
               var model = _mapper.Map<List<Order>>(orders);
               return View(model);
          }

          public ActionResult Customers()
          {
               var users = _session.GetAllUsers();
               var model = _mapper.Map<List<UserData>>(users).Select(u => new UserData
               {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsBanned = u.IsBanned
               }).ToList();

               return View(model);
          }

          [HttpGet]
          public ActionResult AddProduct()
          {
               var model = new ProductDO();
               PopulateCategoryDropdown();
               return View(model);
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult AddProduct(ProductDO product, HttpPostedFileBase ProductImage)
          {
               if (!ModelState.IsValid)
               {
                    PopulateCategoryDropdown(product.Category);
                    return View(product);
               }

               if (ProductImage != null && ProductImage.ContentLength > 0)
               {
                    try
                    {
                         string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                         string fileExtension = Path.GetExtension(ProductImage.FileName).ToLower();

                         if (!allowedExtensions.Contains(fileExtension))
                         {
                              ModelState.AddModelError("", "Please select a valid image file (jpg, jpeg, png, gif, bmp).");
                              return View(product);
                         }

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
                         return View(product);
                    }
               }

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
          [HttpPost]
          public ActionResult DeleteProduct(int id)
          {
               _product.DeleteProduct(id);
               return RedirectToAction("Products");
          }
          [HttpGet]
          public ActionResult EditProduct(int id)
          {
               var product = _product.GetProductById(id);
               if (product == null)
               {
                    return HttpNotFound();
               }
               PopulateCategoryDropdown(product.Category);
               return View("AddProduct", product); 
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult EditProduct(ProductDO product, HttpPostedFileBase ProductImage)
          {
               if (!ModelState.IsValid)
               {
                    return View("AddProduct", product);
               }

               if (ProductImage != null && ProductImage.ContentLength > 0)
               {
                    try
                    {
                         string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                         string fileExtension = Path.GetExtension(ProductImage.FileName).ToLower();

                         if (!allowedExtensions.Contains(fileExtension))
                         {
                              ModelState.AddModelError("", "Please select a valid image file (jpg, jpeg, png, gif, bmp).");
                              return View("AddProduct", product);
                         }

                         if (!string.IsNullOrEmpty(product.ProductImagePath))
                         {
                              string oldImagePath = Server.MapPath("/Content/images/" + product.ProductImagePath);
                              if (System.IO.File.Exists(oldImagePath))
                              {
                                   System.IO.File.Delete(oldImagePath);
                              }
                         }

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


          private void PopulateCategoryDropdown(PCategories? selectedCategory = null)
          {
               var categories = Enum.GetValues(typeof(PCategories))
                                   .Cast<PCategories>()
                                   .Select(c => new SelectListItem
                                   {
                                        Value = ((int)c).ToString(),
                                        Text = c.ToString(),
                                        Selected = selectedCategory.HasValue && selectedCategory.Value == c
                                   })
                                   .ToList();

               ViewBag.Categories = categories;
          }

          [HttpPost]
          public ActionResult ToggleBan(int id)
          {
               var result = _session.ToggleBanUser(id);
               if (!result)
               {
                    TempData["Error"] = "User not found or error occurred.";
               }
               else
               {
                    TempData["Success"] = "User status updated.";
               }

               return RedirectToAction("Customers");
          }

          [HttpPost]
          public ActionResult DeleteOrder(int id)
          {
               _order.DeleteOrder(id);
               return RedirectToAction("ManageOrders");
          }


          [HttpGet]
          public ActionResult Reviews()
          {
               var reviewDOs = _product.GetAllReviews();
               var reviews = _mapper.Map<List<Review>>(reviewDOs);
               return View(reviews);
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult DeleteReview(int id)
          {
               var success = _product.DeleteReview(id);

               if (success)
               {
                    TempData["Success"] = "Review deleted successfully.";
               }
               else
               {
                    TempData["Error"] = "Failed to delete review.";
               }

               return RedirectToAction("ManageReviews");
          }

     }


}