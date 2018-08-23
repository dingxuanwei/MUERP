using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fleck;
using System.ServiceModel;
using System.ServiceProcess;

namespace MU.Push
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    ServiceBase[] serviceToRun = new ServiceBase[] { new WinService() };
                    ServiceBase.Run(serviceToRun);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now.ToString() + "\tService Start Error:");
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                bool flag = true;
                do
                {
                    string name = "MU.Push";
                    string tip = "请选择你要执行的操作——0：控制台运行，1：自动部署服务，2：安装服务，3：卸载服务，4：查看服务状态，5：退出";
                    Console.WriteLine(tip);
                    Console.WriteLine("————————————————————");

                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.NumPad0:
                        case ConsoleKey.D0:
                            StartListen();
                            break;
                        case ConsoleKey.NumPad1:
                        case ConsoleKey.D1:
                            if (ServiceHelper.IsServiceExisted(name))
                            {
                                ServiceHelper.ConfigService(name, false);
                            }
                            if (!ServiceHelper.IsServiceExisted(name))
                            {
                                ServiceHelper.ConfigService(name, true);
                            }
                            ServiceHelper.StartService(name);
                            break;
                        case ConsoleKey.NumPad2:
                        case ConsoleKey.D2:
                            if (ServiceHelper.IsServiceExisted(name))
                            {
                                Console.WriteLine("\n服务已存在......");
                            }
                            else
                            {
                                ServiceHelper.ConfigService(name, true);
                            }
                            ServiceHelper.StartService(name);
                            break;
                        case ConsoleKey.NumPad3:
                        case ConsoleKey.D3:
                            if (ServiceHelper.IsServiceExisted(name))
                            {
                                ServiceHelper.ConfigService(name, false);
                                Console.WriteLine("\n卸载完成......");
                            }
                            else
                            {
                                Console.WriteLine("\n服务不存在......");
                            }
                            break;
                        case ConsoleKey.NumPad4:
                        case ConsoleKey.D4:
                            if (ServiceHelper.IsServiceExisted(name))
                            {
                                Console.WriteLine("\n服务状态：" + ServiceHelper.GetServiceStatus(name));
                            }
                            else
                            {
                                Console.WriteLine("\n服务不存在......");
                            }
                            break;
                        case ConsoleKey.NumPad5:
                        case ConsoleKey.D5:
                            flag = false;
                            break;
                    }
                } while (flag);
            }
        }

        static void StartListen()
        {
            string port = System.Configuration.ConfigurationManager.AppSettings["port"].ToString();
            ServiceHost host = new ServiceHost(typeof(MPService));
            host.Opened += (s, e) => { Console.WriteLine("WCF opened on " + host.BaseAddresses[0]); };
            host.Open();
            PushServer.Instance().StartWebSocket(port);
        }
    }
}
