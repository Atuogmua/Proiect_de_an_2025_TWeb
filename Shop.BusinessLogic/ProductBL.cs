using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic
{
     public class ProductBL : ProductApi, IProduct
     {
          public bool AddProduct(ProductDO product)
          {
               return AddProductLogic(product);
          }

          public List<ProductDO> GetAllProducts()
          {
               return GetAllProductsAction();
          }

          public ProductDO GetProductById(int id) 
          { 
               return GetProductByIdAction(id);
          }
          public bool DeleteProduct(int id)
          {
               return DeleteProductAction(id);
          }
          public bool AddReview(ReviewDO review) 
          {
               return AddReviewAction(review);
          }
          public List<ReviewDO> GetAllReviews(int product)
          {
               return GetAllReviewsAction(product);
          }

          public List<ReviewDO> GetAllReviews()
          {
               return GetAllReviewsAction();
          }
          public bool DeleteReview(int reviewId)
          {
               return DeleteReviewAction(reviewId);
          }

          public List<ProductDO> Search(string searchTerm, int? id)
          {
               return SearchAction(searchTerm, id);
          }
     }
}
