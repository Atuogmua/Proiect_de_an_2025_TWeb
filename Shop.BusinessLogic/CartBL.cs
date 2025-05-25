using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic
{
     public class CartBL : CartApi, ICart
     {
          CartItemDO GetCartItemFromProductAction(int productId, int quantity) 
          {
               return GetCartItemFromProduct(productId, quantity);
          }
          List<CartItemDO> UpdateCartAction(List<CartItemDO> cart, CartItemDO newItem)
          {
               return UpdateCart(cart, newItem);
          }

     }
}
