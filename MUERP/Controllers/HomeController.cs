using MUSystem.Core;
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
            return View( );
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

        
        public dynamic GetOrderList(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='a.BillDate desc'>
    <select>
        A.*,B.ShortName as CustomerName,s.OrderQty,D.Text AS BrandText
    </select>
    <from>
        oms_order A
        left join bas_customer     B  on     B.CustomerCode = A.CustomerCode
        left join (select BillNo ,sum(SizeQty) as OrderQty from oms_orderSizeList group by BillNo) S on A.BillNo =S.BillNo 
        left join bas_customerBrand D on D.Value = A.Brand 
        inner join (select distinct PatternBillNo from otk_ProofingReady) otk on A.BillNo=otk.PatternBillNo
    </from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='A.CustomerCode'        cp='equal'          ></field>
        <field name='A.CustOrder'           cp='equal'          ></field>
        <field name='A.ModelName'           cp='like'           ></field>
        <field name='A.Salesman'            cp='like'           ></field>
        <field name='A.StyleNo'             cp='like'           ></field>
        <field name='A.StyleName'           cp='like'           ></field>
        <field name='A.SeasonName'          cp='equal'          ></field>
        <field name='A.BillDate'            cp='daterange'      ></field>
        <field name='A.OrderStateCode'      cp='equal'          ></field>
        <field name='A.Brand'               cp='like'           ></field>
        <field name='A.Description'         cp='like'           ></field>
    </where>
</settings>");
            var masterService = new oms_orderService();
            var pQuery = query.ToParamQuery();
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public ActionResult Test1()
        {
            var list = GetOrderList(new RequestWrapper());
            return Json(list, JsonRequestBehavior.AllowGet);
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