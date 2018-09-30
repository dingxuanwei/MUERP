using MU.ERP.Models;
using MU.DBWapper.Models;
using System.Web.Mvc;

namespace MU.ERP.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public sys_user Loginer { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User != null)
                Loginer = (User as MUser<sys_user>).UserData;
            else
                Redirect("/Login");
            base.OnActionExecuting(filterContext);
        }
    }
}