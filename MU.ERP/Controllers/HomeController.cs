using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MU.Models;
using MU.DBWapper;

namespace MU.ERP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DB.Insert(new Book() { Name = "pro csharp 5.0", Author = "dingxw", Price = 128.99m });
            //DB.Insert(new Book() { Name = "pro csharp 5.0", Author = "dingxw"});
            return View();
        }
    }
}