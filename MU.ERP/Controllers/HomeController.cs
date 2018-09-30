using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MU.ERP.Models;

namespace MU.ERP.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(Loginer);
        }
    }
}