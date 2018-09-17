using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MU.ERP.Models;
using MU.DBWapper;
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
            model.password = model.password.MD5();
            var result = DB.Select<sys_user>(p => p.UserCode == model.usercode && p.Password == model.password);
            if (result.Count != 1) { ModelState.AddModelError("", "用户名或密码不正确"); return PartialView(model); }
            if (!result[0].IsEnable) { ModelState.AddModelError("", "用户已经被禁用，请联系管理员"); return PartialView(model); }
            result[0].LastLoginDate = DateTime.Now;
            result[0].LoginCount += 1;
            int row = DB.Update(result[0]);
            MUser<sys_user>.SignIn(result[0].UserName, result[0], 60 * 24);

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