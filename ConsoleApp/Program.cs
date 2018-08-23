using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MU.DBWapper;
using log4net;

namespace ConsoleApp
{
    class Program
    {
        private static ILog log = LogManager.GetLogger("ConsoleApp");
        static void Main(string[] args)
        {
            Console.WriteLine(Test());
            Console.ReadLine();
        }

        static string Test()
        {
            log.Info("This is debug info.");
            log.Error("This is error info.");
            return "Hello world!";
        }
    }
}
