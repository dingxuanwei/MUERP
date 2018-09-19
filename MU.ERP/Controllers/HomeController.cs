using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MU.Models;
using MU.ERP.Models;

namespace MU.ERP.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //var loginer = (User as MUser<sys_user>).UserData;
            return View();
        }
    }
}