using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MU.DBWapper;
using log4net;
using System.Diagnostics;
using CSRedis;
using System.Web.Mvc;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = GetUserExcept(new string[] { "a", "b", "c", "d" });
            Console.WriteLine(result);
            
            Console.ReadLine();
        }

        static string GetUserExcept(params string[] users)
        {
            

            return string.Join(",", users);
        }
    }

    class Tot
    {
        public string MyProperty { get; set; }

        public Tot()
        {

        }
    }
}
