using MU.Extensions;
using MUSystem.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MUERP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index( )
        {
            Email.Send("dingdings02@163.com", "<h1>Hello dingdings02@163.com</h1>");

            return View();
        }

        public ActionResult About( )
        {
            ViewBag.Message = "Your application description page.";

            return View( );
        }

        public ActionResult Contact( )
        {
            ViewBag.Message = "Your contact page.";

            return View( );
        }

        public ActionResult Test2()
        {
            var sql = string.Format("select top 10 * from wxpaylife");
            var table = DbHelperSQL.Query(sql).Tables[0];
            var list = DBUtils<wxpaylife>.Data2Entity(table);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            return Content(json, "text/json");
        }

        public ActionResult Order( )
        {
            var data = new DataGrid<Employee>( );
            data.page = 1;
            data.size = 10;
            data.total = 125;
            data.rows = new List<Employee>( ) { new Employee("dingxw", 33), new Employee("dingxuanwei", 30) };
            return View(data);
        }
    }

    public class DataGrid<Entity>
    {
        public IEnumerable<Entity> rows { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int size { get; set; }
    }

    public class Employee
    {
        public string name { get; set; }
        public int age { get; set; }
        public Employee( ) { }
        public Employee(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }
}