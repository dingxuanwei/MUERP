using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Strings()
        {
            return JavaScript("alert('OK')");
        }
    }
}