using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MU.DBWapper;
using MU.Models;

namespace MU.ERP.App_Start
{
    public class MvcMenuFilter: ActionFilterAttribute
    {
        private bool _isEnable = true;

        public MvcMenuFilter()
        {
            _isEnable = true;
        }

        public MvcMenuFilter(bool IsEnable)
        {
            _isEnable = IsEnable;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_isEnable)
            {
                var list = new List<string>();
                var route = filterContext.RouteData.Values;
                var url = string.Format("/{0}/{1}/{2}", route["area"], route["controller"], route["action"]);

                list.Add(url);

                if (url.EndsWith("/Index"))
                {
                    url = url.Substring(0, url.Length - 6);
                    list.Add(url);
                }

                if (url.EndsWith("/Home"))
                {
                    url = url.Substring(0, url.Length - 5);
                    list.Add(url);
                }

                if (DB.Select<sys_user>(p => p.UserCode == "admin").Count == 0)
                    filterContext.Result = new ContentResult() { Content = "你没有访问此功能的权限，请联系管理员！" };
            }

            base.OnActionExecuting(filterContext);
        }
    }
}