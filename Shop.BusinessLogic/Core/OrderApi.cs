using AutoMapper;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.Order;
using Shop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Shop.Domain.Model.User;
using static System.Collections.Specialized.BitVector32;

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
                         if (string.IsNullOrEmpty(orderData.Username))
                              throw new ArgumentException("Username is required");

                         if (!Enum.IsDefined(typeof(PaymentMethod), orderData.Payment))
                              throw new ArgumentException("Invalid payment method");

                         if (orderData.Items == null || !orderData.Items.Any())
                              throw new ArgumentException("Order must contain at least one item");

                         var newOrder = _mapper.Map<ODbTable>(orderData);
                         newOrder.OrderDate = DateTime.Now;

                         if (string.IsNullOrEmpty(newOrder.Status))
                              newOrder.Status = "Pending";

                         db.Orders.Add(newOrder);
                         db.SaveChanges();

                         System.Diagnostics.Debug.WriteLine($"Order placed successfully for user: {orderData.Username}, Payment Method: {orderData.Payment}");

                         return true;
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Order placement failed: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    return false;
               }
          }

          public OrderDO GetOrderById(int orderId)
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         var order = db.Orders
                             .Include(o => o.Items)
                             .FirstOrDefault(o => o.Id == orderId);
                         return _mapper.Map<OrderDO>(order);
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Get order by ID failed: {ex.Message}");
                    return null;
               }
          }

          public bool UpdateOrderStatus(int orderId, string status)
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
                         if (order != null)
                         {
                              order.Status = status;
                              db.SaveChanges();
                              return true;
                         }
                         return false;
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Order status update failed: {ex.Message}");
                    return false;
               }
          }

          public List<OrderDO> GetOrdersByPaymentMethod(PaymentMethod paymentMethod)
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         var orders = db.Orders
                             .Include(o => o.Items)
                             .Where(o => o.Payment == paymentMethod)
                             .OrderByDescending(o => o.OrderDate)
                             .ToList();
                         return _mapper.Map<List<OrderDO>>(orders);
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Get orders by payment method failed: {ex.Message}");
                    return new List<OrderDO>();
               }
          }

          public Dictionary<PaymentMethod, int> GetPaymentMethodStatistics()
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         return db.Orders
                             .GroupBy(o => o.Payment)
                             .ToDictionary(g => g.Key, g => g.Count());
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Get payment method statistics failed: {ex.Message}");
                    return new Dictionary<PaymentMethod, int>();
               }
          }

          public List<OrderDO> GetOrdersByStatus(string status)
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         var orders = db.Orders
                             .Include(o => o.Items)
                             .Where(o => o.Status == status)
                             .OrderByDescending(o => o.OrderDate)
                             .ToList();
                         return _mapper.Map<List<OrderDO>>(orders);
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Get orders by status failed: {ex.Message}");
                    return new List<OrderDO>();
               }
          }

          public bool DeleteOrder(int orderId)
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         var order = db.Orders.FirstOrDefault(o => o.Id == orderId);
                         if (order != null)
                         {
                              db.Orders.Remove(order);
                              db.SaveChanges();
                              return true;
                         }
                         return false;
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Delete order failed: {ex.Message}");
                    return false;
               }
          }

          public List<OrderDO> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
          {
               try
               {
                    using (var db = new OrderContext())
                    {
                         var orders = db.Orders
                             .Include(o => o.Items)
                             .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                             .OrderByDescending(o => o.OrderDate)
                             .ToList();
                         return _mapper.Map<List<OrderDO>>(orders);
                    }
               }
               catch (Exception ex)
               {
                    System.Diagnostics.Debug.WriteLine($"Get orders by date range failed: {ex.Message}");
                    return new List<OrderDO>();
               }
          }

          public List<OrderDO> GetUserOrderHistoryAction(string username)
          {
               using (var db = new OrderContext())
               {
                    var orders = db.Orders
                        .Include(o => o.Items)
                        .Where(o => o.Username == username)
                        .OrderByDescending(o => o.OrderDate)
                        .ToList();
                    return _mapper.Map<List<OrderDO>>(orders);
               }
          }

          public List<OrderDO> GetAllOrdersAction()
          {
               using (var _db = new OrderContext())
               {
                    return _mapper.Map<List<OrderDO>>(_db.Orders);
               }
          }

          public bool DeleteOrderAction(int id)
          {
               using(var _db = new OrderContext())
               {
                    var order = _db.Orders.FirstOrDefault(o => o.Id == id);
                    if (order != null)
                    {
                         _db.Orders.Remove(order);
                         _db.SaveChanges();
                         return true;
                    }
                    return false;
               }
          }


     }

}