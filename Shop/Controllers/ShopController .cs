using AutoMapper;
using Shop.BusinessLogic;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Order;
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
          [HttpGet]
          public ActionResult Checkout()
          {
               var cartDO = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();
               var cart = _mapper.Map<List<CartItem>>(cartDO);
               return View(new Order { CartItems = cart });

          }

          [HttpPost]
          public ActionResult Checkout(Order model)
          {
               var user = _session.GetUserByCookie(Request.Cookies["login"]?.Value);
               if (user == null) return RedirectToAction("Login", "User");

               var order = new OrderDO
               {
                    Username = user.Username,
                    Address = model.Address,
                    Notes = model.Notes,
                    Items = model.CartItems.Select(i => new OrderItemDO
                    {
                         ProductId = i.ProductId,
                         ProductName = i.Name,
                         Quantity = i.Quantity,
                         UnitPrice = i.Price
                    }).ToList(),
                    TotalPrice = model.TotalPrice
               };

               _order.PlaceOrderAction(order);
               Session["Cart"] = null;
               return RedirectToAction("ThankYou");
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

          public ActionResult ThankYou() 
          {
               return View();
          }
    }
}