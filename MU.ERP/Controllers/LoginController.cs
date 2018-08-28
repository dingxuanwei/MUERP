using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MU.ERP.Controllers
{
    [App_Start.Localization]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Check(string usercode, string password, bool remember)
        {
            if (ModelState.IsValid)
                return Content("success");
            else
                return Content("error");
        }
    }
}