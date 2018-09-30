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

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var redis = new RedisClient("localhost"))
            {
                redis.Connected += (s, e) => { Console.WriteLine("Connected"); };

                var b = redis.SMembers("bbs");
                Array.ForEach(b, (s) => { Console.WriteLine(s); });
            }
            Console.ReadLine();
        }
    }
}
