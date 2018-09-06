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
        //private static ILog log = LogManager.GetLogger("ConsoleApp");
        static void Main(string[] args)
        {
            Order order = new Order() { Id = 1, Name = "lee", Count = 10, Price = 100, Desc = "订单测试" };
            IOrderProcessor orderprocessor = new OrderProcessorDecorator(new OrderProcessor());
            orderprocessor.Submit(order);
            Console.ReadLine();
        }
    }

    class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
    }

    interface IOrderProcessor
    {
        void Submit(Order order);
    }

    class OrderProcessor:IOrderProcessor
    {
        public void Submit(Order order)
        {
            Console.WriteLine("提交订单");
        }
    }

    class OrderProcessorDecorator:IOrderProcessor
    {
        public IOrderProcessor OrderProcessor { get; set; }
        public OrderProcessorDecorator(IOrderProcessor orderprocessor)
        {
            OrderProcessor = orderprocessor;
        }

        public void Submit(Order order)
        {
            PrePreoceed(order);
            OrderProcessor.Submit(order);
            PostProceed(order);
        }

        public void PrePreoceed(Order order)
        {
            Console.WriteLine("提交订单前，进行订单数据校验...");
            if (order.Price < 0)
            {
                Console.WriteLine("订单总价有误，请重新核对订单。");
            }
        }

        public void PostProceed(Order order)
        {
            Console.WriteLine("提交订单后，进行订单日志记录......");
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "提交订单，名称：" + order.Name + ",订单价格：" + order.Price);
        }
    }
}
