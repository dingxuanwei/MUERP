using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Resources;

namespace MU.ERP.Controllers
{
    [App_Start.Localization]
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            ViewBag.Title = Gloable.Title;
            ViewBag.Name = Gloable.Name;
            ViewBag.Password = Gloable.Password;
            return View();
        }
    }
}