using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MU.ERP.Models;
using MU.DAL;
using MU.Models;
using MU.Extensions;

namespace MU.ERP.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView();
        }

        /// <summary>
        /// 测试数据库连接是否正常
        /// </summary>
        [AllowAnonymous]
        public ActionResult ConnectionTest()
        {
            var info = DB.Test();
            return Content(info);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }


            int LoginEffectiveHours = Convert.ToInt32(AppConfig.Get("LoginEffectiveHours"));
            MUser<sys_user>.SignIn(result[0].UserName, result[0], 60 * LoginEffectiveHours);

            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return Redirect("/Home");
            
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Login");
        }
    }
}