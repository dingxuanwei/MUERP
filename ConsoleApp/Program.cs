using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MU.DBWapper;
using log4net;
using System.Diagnostics;


namespace ConsoleApp
{
    class Program
    {
        private static ILog log = LogManager.GetLogger("ConsoleApp");
        static void Main(string[] args)
        {
            using (CYQ.Data.MAction action = new CYQ.Data.MAction("Users"))
            {
                var json = action.Select(1, 10, 10).ToJson();
                Console.WriteLine(json);
            }

            Console.ReadLine();
        }

        static string Test()
        {
            log.Info("This is debug info.");
            log.Error("This is error info.");
            return "Hello world!";
        }
    }

    class Users
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
