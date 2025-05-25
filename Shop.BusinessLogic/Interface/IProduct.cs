using Shop.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.Interface
{
     public interface IProduct
     {
          bool AddProduct(ProductDO product);
          List<ProductDO> GetAllProducts();
     }
}
