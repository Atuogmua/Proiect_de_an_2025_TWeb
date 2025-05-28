using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                    ProductImagePath = p.ProductImagePath
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
                    ProductImagePath = product.ProductImagePath
               };

          }

     }
}
