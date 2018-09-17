using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using MU.Extensions;
using System.Diagnostics;
using System.Text;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var d = DateTime.Now.ToOADate();
            Console.WriteLine(d);

            var t = DateTime.FromOADate(d);
            Console.WriteLine(t.ToString());
        }

        [TestMethod]
        public void GetFields()
        {
            var name = "sys_menu";
            var ds = DbHelperSQL.Query(string.Format("sp_help '{0}'", name));
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("public class {0} \r\n{{", name);
            sb.AppendLine();
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                if (dr["Type"].ToString().IndexOf("char") >= 0)
                {
                    sb.AppendFormat("public string {0} {{get;set;}}", dr["Column_name"].ToString());
                }
                else if (dr["Type"].ToString().IndexOf("bit") >= 0)
                {
                    sb.AppendFormat("public bool {0} {{get;set;}}", dr["Column_name"].ToString());
                }
                else if (dr["Type"].ToString().IndexOf("datetime") >= 0)
                {
                    sb.AppendFormat("public DateTime {0} {{get;set;}}", dr["Column_name"].ToString());
                }
                else if (dr["Type"].ToString().IndexOf("int") >= 0)
                {
                    sb.AppendFormat("public int {0} {{get;set;}}", dr["Column_name"].ToString());
                }
                else
                {
                    sb.AppendFormat("public string {0} {{get;set;}}", dr["Column_name"].ToString());
                }

                sb.AppendLine();
            }
            
            sb.Append("}");

            Console.WriteLine(sb.ToString());
        }
    }
}

