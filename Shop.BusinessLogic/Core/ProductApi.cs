using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Product;

namespace Shop.BusinessLogic.Core
{
     public class ProductApi
     {
          private readonly ShopContext _db = new ShopContext();
          private readonly IMapper _mapper;

          public ProductApi()
          {
               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<ProductDO, PDBTable>();
                    cfg.CreateMap<PDBTable, ProductDO>();
               });

               _mapper = config.CreateMapper();
          }

          public bool AddProductLogic(ProductDO product)
          {
               using (var db = new ShopContext())
               {
                    var entity = _mapper.Map<PDBTable>(product);
                    db.Products.Add(entity);
                    return db.SaveChanges() > 0;
               }   
          }

          public List<ProductDO> GetAllProductsAction() 
          {
               return _db.Products.Select(p => new ProductDO
               {
                    ID = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Stock = p.Stock,
                    ProductImagePath = p.ProductImagePath,
                    Category = p.Category,
                    CompanyName = p.CompanyName
               }).ToList();
          }

          public bool UpdateProduct(ProductDO product)
          {
               var entityProduct = _db.Products.Find(product.ID);

               if (entityProduct == null)
               {
                    return false;
               }

               entityProduct.Name = product.Name;
               entityProduct.Description = product.Description;
               entityProduct.Price = product.Price;
               entityProduct.Stock = product.Stock;
               entityProduct.CompanyName = product.CompanyName;
               entityProduct.Category = product.Category;

               if (!string.IsNullOrEmpty(product.ProductImagePath))
               {
                    entityProduct.ProductImagePath = product.ProductImagePath;
               }

               _db.Entry(entityProduct).State = EntityState.Modified;
               _db.SaveChanges();

               return true;              
               
          }


          public ProductDO GetProductByIdAction(int id)
          {
               var product = _db.Products.FirstOrDefault(p => p.Id == id);
               if (product == null)
                    return null;

               return new ProductDO
               {
                    ID = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    ProductImagePath = product.ProductImagePath,
                    Category = product.Category,
                    CompanyName = product.CompanyName
               };

          }

          public bool DeleteProductAction(int id)
          {
               var product = _db.Products.FirstOrDefault(p => p.Id == id);
               if (product != null)
               {
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                    return true;
               }
               return false;
          }

          public bool AddReviewAction(ReviewDO review)
          {
               using (var context = new ShopContext())
               {
                    var newReview = new ReviewDO
                    {
                         ProductId = review.ProductId,
                         Username = review.Username,
                         Content = review.Content,
                         Rating = review.Rating,
                         CreatedAt = DateTime.Now
                    };

                    context.Reviews.Add(newReview);
                    context.SaveChanges();
                    return true;
               }
               return false;
          }

          public List<ReviewDO> GetAllReviewsAction(int product)
          {
               using (var _db = new ShopContext())
               {
                    return _db.Reviews
                      .Where(r => r.ProductId == product)
                      .OrderByDescending(r => r.CreatedAt)
                      .ToList();
               }

          }
          public List<ReviewDO> GetAllReviewsAction()
          {
               using (var _db = new ShopContext())
               {
                    return _db.Reviews.OrderByDescending(r => r.CreatedAt).ToList();
               }
          }
          public bool DeleteReviewAction(int reviewId)
          {
               using (var db = new ShopContext())
               {
                    var review = db.Reviews.FirstOrDefault(r => r.Id == reviewId);
                    if (review == null)
                         return false;

                    db.Reviews.Remove(review);
                    db.SaveChanges();
                    return true;
               }
          }

          public List<ProductDO> SearchAction(string searchTerm, int? id)
          {
               using (var context = new ShopContext())
               {
                    var query = context.Products.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                         query = query.Where(p => p.Name.Contains(searchTerm));
                    }

                    if (id.HasValue)
                    {
                         query = query.Where(p => p.Id == id.Value);
                    }

                    return _mapper.Map<List<ProductDO>>(query.ToList());
               }
          }

     }
}
