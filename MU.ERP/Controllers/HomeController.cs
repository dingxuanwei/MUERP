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
            return View();
        }


        public ActionResult Users(int page, int rows)
        {
            var grid = DB.SelectPage<sys_user, string>(page, rows, o => o.UserCode, x => x.UserCode == "dingxuanwei");
            return Json(grid);
        }
    }
}