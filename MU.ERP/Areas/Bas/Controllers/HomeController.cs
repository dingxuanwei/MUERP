﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MU.ERP.Areas.Bas.Controllers
{
    public class HomeController : Controller
    {
        // GET: Bas/Home
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}