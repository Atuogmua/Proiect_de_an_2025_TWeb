using AutoMapper;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Shop.BusinessLogic.Core
{
     public class OrderApi
     {
          private readonly IMapper _mapper;

          public OrderApi()
          {
               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<OrderDO, ODbTable>();
                    cfg.CreateMap<ODbTable, OrderDO>();
                    
               });

               _mapper = config.CreateMapper();
          }

          public bool PlaceOrder(OrderDO orderData)
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         var newOrder = _mapper.Map<ODbTable>(orderData);
                         newOrder.OrderDate = DateTime.Now; 

                         db.Orders.Add(newOrder);
                         db.SaveChanges();
                         return true;
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Order placement failed: {ex.Message}");
                    return false;
               }
          }

          public List<OrderDO> GetUserOrders(string username)
          {
               using (var db = new OrderContext())
               {
                    var orders = db.Orders
                        .Include(o => o.Items) 
                        .Where(o => o.Username == username)
                        .ToList();

                    return _mapper.Map<List<OrderDO>>(orders);
               }
          }

     }
}
