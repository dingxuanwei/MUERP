using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MU.Push
{
    public class PushServer
    {
        #region 单例模式

        private static readonly PushServer _instance = new PushServer();
        public static PushServer Instance()
        {
            return _instance;
        }

        private PushServer()
        {
            timer = new Timer((obj) =>
            {
                MPModel db = new MPModel();
                db.MsgToBeSents.ToList().ForEach(s =>
                {
                    //判断消息是否已经超时，超时suc=true
                    bool suc = DateTime.Parse(s.ExpriedTime) < DateTime.Now;
                    //如果超时，则不再推送此消息
                    if (!suc)
                    {
                        //暂定为每条消息只能指定一种推送方式，将来可以改成多种方式一起推送
                        if (s.MType == (int)MsgType.Html)
                        {
                            suc = PushHtml(s.RegName, s.Content);
                        }
                        else if (s.MType == (int)MsgType.Sms)
                        {
                            suc = PushSms(s.Phone, s.Content);
                        }
                        else if (s.MType == (int)MsgType.Email)
                        {
                            suc = PushEmail(s.Address, s.Content);
                        }
                    }
                    if (suc)
                    {
                        db.MsgSents.Add(new MsgSent()
                        {
                            Title = s.Title,
                            Content = s.Content,
                            RequestTime = s.RequestTime,
                            ExpriedTime = s.ExpriedTime,
                            MType = s.MType,
                            RegName = s.RegName,
                            Phone = s.Phone,
                            Address = s.Address,
                            SendTime = DateTime.Now
                        });
                        if (db.SaveChanges() > 0)
                        {
                            db.MsgToBeSents.Remove(s);
                            db.SaveChanges();
                        }
                    }
                });
            }, null, 5000, 1000 * 60 * 5); //5分钟一次轮询
        }

        #endregion

        private Timer timer = null;
        HashSet<OnLineWebSocket> tmpSocket = new HashSet<OnLineWebSocket>();
        WebSocketServer server = null;

        public void StartWebSocket(string port, LogLevel level = LogLevel.Debug)
        {
            try
            {
                FleckLog.Level = level;
                server = new WebSocketServer("ws://0.0.0.0:" + port);
                server.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        Console.WriteLine("Open!" + socket.ConnectionInfo.Id.ToString());
                        tmpSocket.Add(new OnLineWebSocket(socket));
                    };
                    socket.OnClose = () =>
                    {
                        Console.WriteLine("Close!" + socket.ConnectionInfo.Id.ToString());
                        tmpSocket.RemoveWhere(p => p.Socket == socket);
                    };
                    socket.OnMessage = name =>
                    {
                        Console.WriteLine($"Regist:{name},has connected {tmpSocket.Count}");
                        var sk = tmpSocket.Where(p => p.Socket == socket).ToList();
                        sk.ForEach(s => s.Name = name);
                        #region 用户上线后，查询否有待推送消息

                        MPModel db = new MPModel();
                        var query = db.MsgToBeSents.Where(p => p.MType == (int)MsgType.Html && p.RegName == name).ToList();
                        query.ForEach(s =>
                        {
                            bool suc = PushHtml(s.RegName, s.Content);
                            if (suc)
                            {
                                db.MsgSents.Add(new MsgSent()
                                {
                                    Title = s.Title,
                                    Content = s.Content,
                                    RequestTime = s.RequestTime,
                                    ExpriedTime = s.ExpriedTime,
                                    MType = s.MType,
                                    RegName = s.RegName,
                                    Phone = s.Phone,
                                    Address = s.Address,
                                    SendTime = DateTime.Now
                                });
                                if (db.SaveChanges() > 0)
                                {
                                    db.MsgToBeSents.Remove(s);
                                    db.SaveChanges();
                                }
                            }
                        });

                        #endregion
                    };
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void StopWebSocket()
        {
            server.ListenerSocket.Close();
            server = null;
        }

        /// <summary>
        /// 推送Html
        /// </summary>
        /// <param name="name">websocket注册名称</param>
        /// <param name="content">推送内容</param>
        public bool PushHtml(string name, string content)
        {
            var list = tmpSocket.Where(p => p.Name == name).ToList();
            if (list.Count == 0) return false;
            list.ForEach((s) => { s.Send(content); });
            return true;
        }

        /// <summary>
        /// 推送短消息
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="content">推送内容</param>
        public bool PushSms(string phone, string content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 推送Email消息
        /// </summary>
        /// <param name="address">邮箱地址</param>
        /// <param name="content">邮件内容</param>
        public bool PushEmail(string address, string content)
        {
            try
            {
                Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                if (!regex.IsMatch(address)) throw new Exception("邮件地址无效");
                string servicemail = System.Configuration.ConfigurationManager.AppSettings["mailname"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();

                MailMessage mail = new MailMessage();
                mail.Subject = "测试邮件";
                mail.From = new MailAddress(servicemail, "上海秒优");
                mail.To.Add(new MailAddress(address));
                mail.Body = content;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.ym.163.com";
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(servicemail, password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

    public class OnLineWebSocket
    {
        public string Name { get; set; }
        public IWebSocketConnection Socket { get; set; }

        public OnLineWebSocket(IWebSocketConnection socket)
        {
            this.Socket = socket;
        }

        public void Send(string content)
        {
            Socket.Send(content);
        }
    }
}
