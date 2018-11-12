using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NPOI.XSSF.UserModel;
using System.Text;

namespace MU.ERP.Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult EleTag()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ImportExcel()
        {
            return View();
        }

        public ActionResult ExcelToString()
        {
            string filename = Server.MapPath("~/Upload/rp.xlsx");
            using (var excel = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var workbook = new XSSFWorkbook(excel);
                var sheet = workbook.GetSheetAt(0);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    for (int j = 0; j < sheet.GetRow(i).Cells.Count(); j++)
                    {
                        sb.AppendFormat("{0},", sheet.GetRow(i).Cells[j].StringCellValue);
                    }
                    sb.AppendLine();
                }
                return Content("<pre>" + sb.ToString() + "</pre>", "text/html");
            }
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string ext = Path.GetExtension(file.FileName).ToLower();
                if (ext == ".xls" || ext == ".xlsx")
                {
                    string fileSave = Server.MapPath("~/Upload");
                    if (!Directory.Exists(fileSave)) Directory.CreateDirectory(fileSave);

                    file.SaveAs(Path.Combine(fileSave, file.FileName));
                }
            }
            return Content("");
        }
    }
}