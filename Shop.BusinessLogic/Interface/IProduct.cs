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
          ProductDO GetProductById(int id);
          bool UpdateProduct(ProductDO product);
          bool DeleteProduct(int id);
          bool AddReview(ReviewDO review);
          List<ReviewDO> GetAllReviews(int product);
          List<ReviewDO> GetAllReviews();
          bool DeleteReview(int reviewId);
          List<ProductDO> Search(string searchTerm, int? id);
     }
}
