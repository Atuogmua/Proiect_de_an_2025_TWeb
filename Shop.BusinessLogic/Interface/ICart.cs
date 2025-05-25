using Shop.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.Interface
{
     public interface ICart
     {
          CartItemDO GetCartItemFromProduct(int productId, int quantity); 
          List<CartItemDO> UpdateCart(List<CartItemDO> cart, CartItemDO newItem);
     }
}
