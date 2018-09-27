using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = DB.Entity().v_sys_menu.SelectMany(p => p.Code == "10");

            //var list = DB.SelectPage<v_sys_menu, int>(1, 10, s => s.Seq.Value, m => m.Code == "10");
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            //Console.WriteLine(json);

            //var db = new DemoEntities();
            //var result = db.();
            //foreach (var item in result)
            //{
            //    Console.WriteLine(item.IconCls);
            //}

            //list[0].Level = 10;
            //list[0].UpdateDate = DateTime.Now;
            //int rows = DB.Update(list[0], new string[] { "Level", "UpdateDate" });
            //Console.WriteLine("effect rows:" + rows);

            //string usercode = "admin";
            //var db = new DemoEntities();
            //var list = db.sys_user.Where(p => (p.UserCode == usercode || p.Phone == usercode || p.Email == usercode) && p.Password == "7FEF6171469E80D32C0559F88B377245").ToList();
            //if (list.Count() == 0) { Console.WriteLine("用户名或密码不存在"); return; }
            //var user = list.FirstOrDefault();
            //if (!user.Enable) { Console.WriteLine("该用户已经被禁用，请联系管理员");return; }

            //Console.WriteLine("登录成功");
            Console.ReadLine();
        }

        static int[] Range(int end, int start = 0)
        {
            var list = new List<int>();
            for (int i = start; i <= end; i++)
            {
                list.Add(i);
            }
            return list.ToArray();
        }
    }
}
