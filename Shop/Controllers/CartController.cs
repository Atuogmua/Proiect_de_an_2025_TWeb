using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;
using Shop.Extension;
using Shop.Models;

namespace Shop.Controllers
{
     public class CartController : Controller
     {
          private readonly ICart _cart;
          private readonly IMapper _mapper;

          public CartController()
          {
               var bl = new BusinessLogic.BusinessLogic();
               _cart = bl.GetCartBL();

               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<CartItem, CartItemDO>();
                    cfg.CreateMap<CartItemDO, CartItemDO>();

               });

               _mapper = config.CreateMapper();
          }

          public ActionResult AddToCart(int id, int quantity)
          {
               var product = _cart.GetCartItemFromProduct(id, quantity);
               if (product == null)
               {
                    return HttpNotFound();
               }

               var newItem = _mapper.Map<CartItemDO>(product);
               newItem.Quantity = quantity;

               var cart = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();
               cart = _cart.UpdateCart(cart, newItem);

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