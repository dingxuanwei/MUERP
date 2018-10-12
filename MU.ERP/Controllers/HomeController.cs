using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MU.ERP.Models;
using Qiniu.Util;
using Qiniu.Storage;

namespace MU.ERP.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Type = "info";
            return View(Loginer);
        }
    }
}