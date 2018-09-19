using MU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MU.ERP.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public sys_user Loginer { get; set; }
    }
}