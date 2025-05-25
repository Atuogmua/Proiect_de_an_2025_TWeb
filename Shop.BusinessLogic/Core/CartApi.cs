using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.Core
{
     public class CartApi : ICart
     {
          private readonly ShopContext _db = new ShopContext();

          public CartItemDO GetCartItemFromProduct(int productId, int quantity)
          {
               var product = _db.Products.FirstOrDefault(p => p.Id == productId);
               if (product == null) {
                    return null;
               }
               else
               {
                    return new CartItemDO
                    {
                         ProductId = product.Id,
                         Name = product.Name,
                         Price = product.Price,
                         Quantity = quantity
                    };
               }
          }


          public List<CartItemDO> UpdateCart(List<CartItemDO> cart, CartItemDO newItem)
          {
               var existItem = cart.FirstOrDefault(c => c.ProductId == newItem.ProductId);

               if (existItem != null)
               {
                    existItem.Quantity += newItem.Quantity;    
               }
               else
               {
                    cart.Add(newItem);
               }

               return cart;
          }

     }
}
