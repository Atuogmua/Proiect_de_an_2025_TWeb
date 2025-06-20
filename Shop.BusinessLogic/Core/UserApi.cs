﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shop.BusinessLogic.DBDataContext;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;
using Shop.Helpers;
using System.Data.Entity;
using System.Web;
using AutoMapper;
using Shop.Domain.Enums;
using Shop.Domain.Model.Order;
using System.Net;


namespace Shop.BusinessLogic.Core
{
     public class UserApi
     {

          private readonly IMapper _mapper;


          public UserApi()
          {
               var config = new MapperConfiguration(cfg =>
               {
                    cfg.CreateMap<UDBTable, UMinimal>();
               });

               _mapper = config.CreateMapper();

          }
          //-----------------------------AUTH--------------------------------
          internal ULoginResp UserLoginAction(UserLoginDO data)
          {
               if (data == null || string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
               {
                    return new ULoginResp { Status = false, StatusMsg = "Invalid login data." };
               }
               var isEmail = new EmailAddressAttribute().IsValid(data.Username);
               var hashedPassword = LoginHelper.HashGen(data.Password);

               using (var db = new UserContext())
               {
                    var user = isEmail
                        ? db.Users.FirstOrDefault(u => u.Email == data.Username && u.Password == hashedPassword)
                        : db.Users.FirstOrDefault(u => u.UserName == data.Username && u.Password == hashedPassword);

                    if (user == null || user.IsBanned == true)
                    {
                         return new ULoginResp { Status = false, StatusMsg = "The Username or Password is Incorrect or You have been Banned" };
                    }

                    user.IP = data.UserIP;
                    user.LastLoginDataTime = data.LoginDateTime;

                    db.SaveChanges();

                    return new ULoginResp { Status = true, StatusMsg = "Login successful." };
               }
          }


          internal ULoginResp UserRegisterAction(URegisterDO data)
          {
               try
               {
                    using (var db = new UserContext())
                    {
                         var existingUser = db.Users.FirstOrDefault(u => u.UserName == data.Username || u.Email == data.Email);

                         if (existingUser != null)
                         {
                              if (existingUser.UserName == data.Username)
                              {
                                   return new ULoginResp { Status = false, StatusMsg = "Username already exists." };
                              }
                              else if (existingUser.Email == data.Email)
                              {
                                   return new ULoginResp { Status = false, StatusMsg = "Email already exists." };
                              }
                         }

                         var hashedPass = LoginHelper.HashGen(data.Password);

                         var user = new UDBTable()
                         {
                              UserName = data.Username,
                              Email = data.Email,
                              RegistrationDataTime = DateTime.Now,
                              Password = hashedPass,
                              IP = data.UserIP,
                              LastLoginDataTime = DateTime.Now,
                              Level = URole.User
                         };
                         db.Users.Add(user);
                         var result = db.SaveChanges();

                         if (result > 0)
                         {
                              return new ULoginResp { Status = true, StatusMsg = "Registration successful." };
                         }
                         else
                         {
                              return new ULoginResp { Status = false, StatusMsg = "Registration failed." };
                         }
                    }
               }
               catch (Exception ex) 
               {
                    System.Diagnostics.Debug.WriteLine($"Registration error: {ex.Message}");
                    return new ULoginResp { Status = false, StatusMsg = "An error occurred during registration" };
               }
          }

          internal HttpCookie Cookie(string loginUsername)
          {
               var apiCookie = new HttpCookie("X-KEY")
               {
                    Value = CookieGenerator.Create(loginUsername)
               };

               using (var db = new SessionContext())
               {
                    Session curent;

                    var validate = new EmailAddressAttribute();

                    if (validate.IsValid(loginUsername))
                    {
                         curent = (from e in db.Sessions where e.UserName == loginUsername select e).FirstOrDefault();
                    }
                    else
                    {
                         curent = (from e in db.Sessions where e.UserName == loginUsername select e).FirstOrDefault();
                    }

                    if (curent != null)
                    {
                         curent.CookieString = apiCookie.Value;
                         curent.ExpireTime = DateTime.Now.AddMinutes(60);

                         using(var todo = new SessionContext())
                         {
                              todo.Entry(curent).State = EntityState.Modified;
                              todo.SaveChanges();
                         }
                    }
                    else
                    {
                         db.Sessions.Add(new Session
                         {
                              UserName = loginUsername,
                              CookieString = apiCookie.Value,
                              ExpireTime = DateTime.Now.AddMinutes(60)
                         });
                         db.SaveChanges();
                    }

               }

               return apiCookie;
          }


          internal UMinimal UserCookie(string cookie)
          {
               if (string.IsNullOrEmpty(cookie)) return null;

               Session session;

               using (var db = new SessionContext())
               {
                    session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);
               }

               if (session == null) return null;

               using (var db = new UserContext())
               {
                    var currentUser = db.Users.FirstOrDefault(u =>
                         u.UserName == session.UserName || u.Email == session.UserName);

                    if (currentUser == null || currentUser.IsBanned == true) return null;

                    return _mapper.Map<UMinimal>(currentUser);
               }
          }


          public UserProfileDO GetUserProfileAction(string username)
          {
               using (var db = new UserContext())
               {
                    var user = db.Users.FirstOrDefault(u => u.UserName == username);
                    if (user == null || user.IsBanned == true) return null;

                    return new UserProfileDO
                    {
                         Username = user.UserName,
                         Email = user.Email,
                         PhoneNumber = user.Phone,
                    };
               }
          }


          public bool UpdateUserProfileAction(UserProfileDO profile)
          {
               using (var db = new UserContext())
               {
                    var user = db.Users.FirstOrDefault(u => u.UserName == profile.Username);
                    if (user == null || user.IsBanned == true) return false;

                    user.Email = profile.Email;
                    user.Phone = profile.PhoneNumber;

                    db.SaveChanges();
                    return true;
               }
          }


          public string GetUsernameFromCookieAction(string cookieValue)
          {
               if (string.IsNullOrEmpty(cookieValue)) return null;

               Session session;

               using (var db = new SessionContext())
               {
                    session = db.Sessions.FirstOrDefault(s => s.CookieString == cookieValue && s.ExpireTime > DateTime.Now);
               }

               if (session == null) return null;

               using (var db = new UserContext())
               {
                    var currentUser = db.Users.FirstOrDefault(u =>
                         u.UserName == session.UserName || u.Email == session.UserName);

                    if (currentUser == null || currentUser.IsBanned == true) return null;

                    return currentUser.UserName;
               }
          }

          public bool ChangePasswordAction(string username, string oldPassword, string newPassword)
          {
               using (var _context = new UserContext())
               {
                    var user = _context.Users.FirstOrDefault(u => u.UserName == username);

                    if (user == null || user.IsBanned == true)
                         return false;

                    // Assuming passwords are stored hashed:
                    string oldPasswordHash = LoginHelper.HashGen(oldPassword);
                    if (user.Password != oldPasswordHash)
                         return false;

                    user.Password = LoginHelper.HashGen(newPassword);

                    _context.SaveChanges();
                    return true;
               }
          }

          public List<UMinimal> GetAllUsersAction()
          {
               using (var _db = new UserContext())
               {
                    return _mapper.Map<List<UMinimal>>(_db.Users.ToList());
               }
          }

          public bool ToggleBanUserAction(int userId)
          {
               using (var _db = new UserContext())
               {
                    var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                    if (user == null)
                         return false;

                    user.IsBanned = !user.IsBanned;
                    _db.SaveChanges();
                    return true;
               }
          }



     }
}
