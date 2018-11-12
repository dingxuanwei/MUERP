using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MU.ERP.SSO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return PartialView();
        }

        public ActionResult datagrid_data(int page, int rows, string ItemID)
        {
            List<DataModel> list = new List<DataModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new DataModel()
                {
                    ItemID = "EST-" + i,
                    Product = "FI-SW-" + i.ToString("00"),
                    Price = 38.5m,
                    UnitCost = 10,
                    Status = "P"
                });
            }
            for (int i = 0; i < 20; i++)
            {
                list.Add(new DataModel()
                {
                    ItemID = "BEST-" + i,
                    Product = "K9-DL-" + i.ToString("00"),
                    Price = 450m,
                    UnitCost = 12,
                    Status = "P"
                });
            }

            DataModel[] tmparr = list.ToArray();
            if (!string.IsNullOrEmpty(ItemID)) tmparr = list.Where(p => p.ItemID.Contains(ItemID)).ToArray();

            DataGridModel<DataModel> grid = new DataGridModel<DataModel>();
            grid.total = tmparr.Length;
            grid.rows = tmparr.Skip((page - 1) * rows).Take(rows).ToArray();
            return Json(grid, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    class DataModel
    {
        public string ItemID { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int UnitCost { get; set; }
        public string Attribute { get; set; }
        public string Status { get; set; }
    }

    class DataGridModel<T>
    {
        public int total { get; set; }
        public T[] rows { get; set; }
        public string emptyMsg { get; set; }
    }
}