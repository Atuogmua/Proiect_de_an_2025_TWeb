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
     public class ProductBL : ProductApi, IProduct
     {
          public bool AddProductLogic(ProductDO product)
          {
               return AddProduct(product);
          }

          public List<ProductDO> GettAllProductsAction()
          {
               return GetAllProducts();
          }
     }
}
