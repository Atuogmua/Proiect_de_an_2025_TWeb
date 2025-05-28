using AutoMapper;
using Shop.BusinessLogic.DBDataContext;
using Shop.Domain.Model.Order;
using Shop.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BusinessLogic.Core
{
     public class OrderApi
     {
          private readonly ShopContext _context;
          private readonly IMapper _mapper;

          public OrderApi()
          {
               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<OrderDO, OrderItemDO>();
               });

               _mapper = config.CreateMapper();
          }

          public OrderDO PlaceOrder(OrderDO order)
          {
               var newOrder = new OrderDO
               {
                    Username = order.Username,
                    OrderDate = DateTime.Now,
                    Address = order.Address,
                    Notes = order.Notes,
                    TotalPrice = order.TotalPrice,
                    Items = order.Items.Select(i => new OrderItemDO
                    {
                         ProductId = i.ProductId,
                         ProductName = i.ProductName,
                         Quantity = i.Quantity,
                         UnitPrice = i.UnitPrice
                    }).ToList()
               };

               _context.Orders.Add(newOrder);
               _context.SaveChanges();
               return newOrder;
          }

          public List<OrderDO> GetUserOrders(string username)
          {
               return _context.Orders
                   .Where(o => o.Username == username)
                   .Select(o => new OrderDO
                   {
                        Id = o.Id,
                        Username = o.Username,
                        OrderDate = o.OrderDate,
                        Address = o.Address,
                        Notes = o.Notes,
                        TotalPrice = o.TotalPrice,
                        Items = o.Items.Select(i => new OrderItemDO
                        {
                             ProductId = i.ProductId,
                             ProductName = i.ProductName,
                             Quantity = i.Quantity,
                             UnitPrice = i.UnitPrice
                        }).ToList()
                   }).ToList();
          }
     }
}

