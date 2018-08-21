using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DB.Select<TUsers>(p => true).ToList().ForEach(x=>{
                Console.WriteLine(x.IsLogin);
            });

            Console.ReadLine();
        }
    }
}
