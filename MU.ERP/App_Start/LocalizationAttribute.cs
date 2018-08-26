using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

namespace MU.ERP.App_Start
{
    public class LocalizationAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lang = filterContext.RouteData.Values["lang"]?.ToString();
            if (!string.IsNullOrWhiteSpace(lang))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                var cookie = filterContext.HttpContext.Request.Cookies["MU.ERP.CurrentUICulture"];
                var langHeader = cookie?.Value;
                if (string.IsNullOrEmpty(langHeader)) langHeader = filterContext.HttpContext.Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(langHeader);
            }
            HttpCookie _cookie = new HttpCookie("MU.ERP.CurrentUICulture", Thread.CurrentThread.CurrentUICulture.Name);
            _cookie.Expires = DateTime.Now.AddYears(1);
            filterContext.HttpContext.Response.SetCookie(_cookie);

            base.OnActionExecuting(filterContext);
        }
    }
}