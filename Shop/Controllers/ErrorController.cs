using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Shop.Models;
using Shop.BusinessLogic.Core;
using Shop.BusinessLogic.Interface;
using Shop.Domain.Model.User;
using AutoMapper;


namespace Shop.Controllers
{
    public class ErrorController : Controller
    {
          public ActionResult Index()
          {
               return View();
          }
          public ActionResult Error404()
          {
               return View();
          }
     }
}