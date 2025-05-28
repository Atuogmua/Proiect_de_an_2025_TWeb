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
                    cfg.CreateMap<CartItemDO, CartItem>();
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
               var cartDO = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();

               var cart = cartDO.Select(item => _mapper.Map<CartItem>(item)).ToList();

               return View(cart);
          }

          [HttpPost]
          public ActionResult UpdateCart(List<CartItem> cartItems)
          {
               if (cartItems != null)
               {
                    var cartDO = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();

                    foreach (var item in cartItems)
                    {
                         var existingItem = cartDO.FirstOrDefault(x => x.ProductId == item.ProductId);
                         if (existingItem != null)
                         {
                              existingItem.Quantity = item.Quantity;
                         }
                    }

                    Session["Cart"] = cartDO;
               }

               return RedirectToAction("Index");
          }

          public ActionResult RemoveFromCart(int id)
          {
               var cart = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();
               var itemToRemove = cart.FirstOrDefault(x => x.ProductId == id);

               if (itemToRemove != null)
               {
                    cart.Remove(itemToRemove);
                    Session["Cart"] = cart;
               }

               return RedirectToAction("Index");
          }

          
          public ActionResult ClearCart()
          {
               Session["Cart"] = new List<CartItemDO>();
               return RedirectToAction("Index");
          }

          public int GetCartItemCount()
          {
               var cart = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();
               return cart.Sum(x => x.Quantity);
          }

          public decimal GetCartTotal()
          {
               var cart = Session["Cart"] as List<CartItemDO> ?? new List<CartItemDO>();
               return cart.Sum(x => x.Price * x.Quantity);
          }
     }
}