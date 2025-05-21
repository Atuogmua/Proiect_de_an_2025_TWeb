using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shop.BusinessLogic.DBDataContext;
using Shop.Domain.Model.Product;

namespace Shop.BusinessLogic.Core
{
     public class ProductApi
     {
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

          public bool AddProduct(ProductDO product)
          {
               using (var db = new ShopContext())
               {
                    var entity = _mapper.Map<PDBTable>(product);
                    db.Products.Add(entity);
                    return db.SaveChanges() > 0;
               }   
          }
     }
}
