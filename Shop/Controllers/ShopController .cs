using AutoMapper;
using Shop.BusinessLogic;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Order;
using Shop.Domain.Model.Product;
using Shop.Domain.Enums;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Shop.Controllers
{
     public class ShopController : Controller
     {
          private readonly IProduct _product;
          private readonly IMapper _mapper;
          private readonly IOrder _order;
          private readonly ISession _session;

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
               _order = bl.GetOrderBL();
               _session = bl.GetSessionBL();
               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<ProductDO, Product>();
                    cfg.CreateMap<Product, ProductDO>();
                    cfg.CreateMap<CartItem, CartItemDO>();
                    cfg.CreateMap<CartItemDO, CartItem>();
                    cfg.CreateMap<CartItemDO, OrderItemDO>();
                    cfg.CreateMap<Review, ReviewDO>();
                    cfg.CreateMap<ReviewDO, Review>();
               });
               _mapper = config.CreateMapper();
          }

          public ActionResult Catalog(string search, string category, string company,
                                    decimal? minPrice, decimal? maxPrice, string sortBy)
          {
               SetUserData();

               var productDOList = _product.GetAllProducts();
               var products = _mapper.Map<List<Product>>(productDOList);

               if (!string.IsNullOrEmpty(search))
               {
                    products = products.Where(p =>
                        p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (!string.IsNullOrEmpty(p.Description) && p.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        p.CompanyName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                    ).ToList();
               }

               if (!string.IsNullOrEmpty(category) && category != "All Categories")
               {
                    if (Enum.TryParse<PCategories>(category, out PCategories selectedCategory))
                    {
                         products = products.Where(p => p.Category == selectedCategory).ToList();
                    }
               }

               if (!string.IsNullOrEmpty(company) && company != "All Companies")
               {
                    products = products.Where(p => p.CompanyName.Equals(company, StringComparison.OrdinalIgnoreCase)).ToList();
               }

               if (minPrice.HasValue)
               {
                    products = products.Where(p => p.Price >= minPrice.Value).ToList();
               }

               if (maxPrice.HasValue)
               {
                    products = products.Where(p => p.Price <= maxPrice.Value).ToList();
               }

               switch (sortBy)
               {
                    case "name_asc":
                         products = products.OrderBy(p => p.Name).ToList();
                         break;
                    case "name_desc":
                         products = products.OrderByDescending(p => p.Name).ToList();
                         break;
                    case "price_asc":
                         products = products.OrderBy(p => p.Price).ToList();
                         break;
                    case "price_desc":
                         products = products.OrderByDescending(p => p.Price).ToList();
                         break;
                    case "company":
                         products = products.OrderBy(p => p.CompanyName).ThenBy(p => p.Name).ToList();
                         break;
                    default:
                         products = products.OrderBy(p => p.Name).ToList();
                         break;
               }

               PopulateFilterDropdowns(productDOList, category, company);

               return View(products);
          }

          private void PopulateFilterDropdowns(List<ProductDO> allProducts, string selectedCategory = null, string selectedCompany = null)
          {
               var categories = Enum.GetValues(typeof(PCategories))
                                  .Cast<PCategories>()
                                  .Select(c => new SelectListItem
                                  {
                                       Value = c.ToString(),
                                       Text = GetCategoryDisplayName(c),
                                       Selected = selectedCategory == c.ToString()
                                  })
                                  .OrderBy(x => x.Text)
                                  .ToList();

               ViewBag.Categories = categories;

               var companies = allProducts
                              .Where(p => !string.IsNullOrEmpty(p.CompanyName))
                              .Select(p => p.CompanyName)
                              .Distinct()
                              .OrderBy(c => c)
                              .Select(c => new SelectListItem
                              {
                                   Value = c,
                                   Text = c,
                                   Selected = selectedCompany == c
                              })
                              .ToList();

               ViewBag.Companies = companies;
          }

          private string GetCategoryDisplayName(PCategories category)
          {
               var field = category.GetType().GetField(category.ToString());
               var displayAttribute = field?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                                           .FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;

               return displayAttribute?.Name ?? category.ToString();
          }

          [HttpGet]
          public JsonResult SearchProducts(string term)
          {
               if (string.IsNullOrEmpty(term) || term.Length < 2)
               {
                    return Json(new List<object>(), JsonRequestBehavior.AllowGet);
               }

               var productDOList = _product.GetAllProducts();
               var products = _mapper.Map<List<Product>>(productDOList);

               var searchResults = products
                   .Where(p => p.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                   .Take(10)
                   .Select(p => new
                   {
                        id = p.Id,
                        name = p.Name,
                        price = p.Price.ToString("C"),
                        image = !string.IsNullOrEmpty(p.ProductImagePath) ?
                              Url.Content("/Content/images/" + p.ProductImagePath) :
                              Url.Content("/Content/images/default-image.png")
                   })
                   .ToList();

               return Json(searchResults, JsonRequestBehavior.AllowGet);
          }

          [HttpGet]
          public JsonResult GetFilterCounts()
          {
               var productDOList = _product.GetAllProducts();
               var products = _mapper.Map<List<Product>>(productDOList);

               var categoryCounts = products
                   .GroupBy(p => p.Category)
                   .ToDictionary(g => g.Key.ToString(), g => g.Count());

               var companyCounts = products
                   .GroupBy(p => p.CompanyName)
                   .ToDictionary(g => g.Key, g => g.Count());

               return Json(new
               {
                    categories = categoryCounts,
                    companies = companyCounts,
                    totalProducts = products.Count
               }, JsonRequestBehavior.AllowGet);
          }

          [HttpGet]
          public ActionResult Checkout()
          {
               var cartDO = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();
               var cart = _mapper.Map<List<CartItem>>(cartDO);
               return View(new Order { CartItems = cart });
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Checkout(Order model)
          {
               var user = _session.GetUserByCookie(Request.Cookies["X-KEY"]?.Value);
               if (user == null) return RedirectToAction("Login", "Auth");

               if (!Enum.IsDefined(typeof(PaymentMethod), model.PaymentMethod))
               {
                    ModelState.AddModelError("PaymentMethod", "Please select a valid payment method");
               }


               if (string.IsNullOrEmpty(model.FirstName))
                    ModelState.AddModelError("FirstName", "First name is required");

               if (string.IsNullOrEmpty(model.LastName))
                    ModelState.AddModelError("LastName", "Last name is required");

               if (string.IsNullOrEmpty(model.Email))
                    ModelState.AddModelError("Email", "Email is required");

               if (string.IsNullOrEmpty(model.Address))
                    ModelState.AddModelError("Address", "Address is required");

               if (string.IsNullOrEmpty(model.Phone))
                    ModelState.AddModelError("Phone", "Phone is required");

               if (!ModelState.IsValid)
               {
                    var cartDO = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();
                    model.CartItems = _mapper.Map<List<CartItem>>(cartDO);
                    return View(model);
               }

               var cartItems = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();

               if (!cartItems.Any())
               {
                    TempData["Error"] = "Your cart is empty";
                    return RedirectToAction("Catalog");
               }

               var order = new OrderDO
               {
                    Username = user.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Address = model.Address,
                    Phone = model.Phone,
                    Payment = model.PaymentMethod,
                    Items = cartItems.Select(i => new OrderItemDO
                    {
                         OrderId = model.OrderId,
                         ProductId = i.ProductId,
                         ProductName = i.Name,
                         Quantity = i.Quantity,
                         UnitPrice = i.Price
                    }).ToList(),
                    TotalPrice = cartItems.Sum(item => item.Price * item.Quantity),
                    OrderStatus = "Pending"
               };

              var success = _order.PlaceOrderAction(order);

               if (success)
               {
                    Session["Cart"] = null;
                    TempData["Success"] = "Order placed successfully!";
                    return RedirectToAction("ThankYou");
               }
               else
               {
                    ModelState.AddModelError("", "Order placement failed. Please try again.");
                    model.CartItems = _mapper.Map<List<CartItem>>(cartItems);
                    return View(model);
               }
          }



          public ActionResult ProductPage(int ProductID)
          {
               SetUserData();

               var productDO = _product.GetProductById(ProductID);
               if (productDO == null)
                    return HttpNotFound();

               var product = _mapper.Map<Product>(productDO);

               var reviewsDO = _product.GetAllReviews(ProductID);
               product.Reviews = _mapper.Map<List<Review>>(reviewsDO);

               return View(product);

          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult SubmitReview(Review model)
          {
               if (!ModelState.IsValid)
               {
                    TempData["Error"] = "Invalid review.";
                    return RedirectToAction("ProductPage", new { ProductID = model.ProductId });
               }

               var user = _session.GetUserByCookie(Request.Cookies["X-KEY"]?.Value);
               if (user == null)
                    return RedirectToAction("Login", "Auth");

               model.Username = user.Username;

               _product.AddReview(_mapper.Map<ReviewDO>(model));

               TempData["Success"] = "Review submitted!";
               return RedirectToAction("ProductPage", new { ProductID = model.ProductId });
          }

          public ActionResult ThankYou()
          {
               return View();
          }
     }
}