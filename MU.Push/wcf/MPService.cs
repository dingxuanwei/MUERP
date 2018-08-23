using System;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace MU.Push
{
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MPService : IMPService
    {
        public string DoWork(string name)
        {
            return "Hello " + name;
        }

        public string PushHtmlMessage(HtmlMsg msg)
        {
            try
            {
                bool suc = PushServer.Instance().PushHtml(msg.RegName, msg.Content);
                if (suc)
                {
                    #region 添加到已发送成功列表

                    using (var db = new MPModel())
                    {
                        MsgSent model = new MsgSent()
                        {
                            Title = msg.Title,
                            Content = msg.Content,
                            RequestTime = msg.RequestTime,
                            ExpriedTime = msg.ExpriedTime,
                            MType = (int)MsgType.Html,
                            RegName = msg.RegName,
                            Phone = "",
                            Address = "",
                            SendTime = DateTime.Now
                        };
                        db.MsgSents.Add(model);
                        int rows = db.SaveChanges();
                        return rows > 0 ? "SUCCESS" : "ERROR";
                    }

                    #endregion
                }
                else
                {
                    #region 添加到待发送成功列表

                    using (var db = new MPModel())
                    {
                        MsgToBeSent model = new MsgToBeSent()
                        {
                            Title = msg.Title,
                            Content = msg.Content,
                            RequestTime = msg.RequestTime,
                            ExpriedTime = msg.ExpriedTime,
                            MType = (int)MsgType.Html,
                            RegName = msg.RegName,
                            Phone = "",
                            Address = ""
                        };
                        db.MsgToBeSents.Add(model);
                        int rows = db.SaveChanges();
                        return rows > 0 ? "SUCCESS" : "ERROR";
                    }

                    #endregion
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string PushSmsMessage(SmsMsg msg)
        {
            try
            {
                bool suc = PushServer.Instance().PushSms(msg.Phone, msg.Content);

                if (suc)
                {
                    #region 添加到已发送成功列表

                    using (var db = new MPModel())
                    {
                        MsgSent model = new MsgSent()
                        {
                            Title = msg.Title,
                            Content = msg.Content,
                            RequestTime = msg.RequestTime,
                            ExpriedTime = msg.ExpriedTime,
                            MType = (int)MsgType.Html,
                            RegName = "",
                            Phone = msg.Phone,
                            Address = "",
                            SendTime = DateTime.Now
                        };
                        db.MsgSents.Add(model);
                        int rows = db.SaveChanges();
                        return rows > 0 ? "SUCCESS" : "ERROR";
                    }

                    #endregion
                }
                else
                {
                    #region 添加到待发送成功列表

                    using (var db = new MPModel())
                    {
                        MsgToBeSent model = new MsgToBeSent()
                        {
                            Title = msg.Title,
                            Content = msg.Content,
                            RequestTime = msg.RequestTime,
                            ExpriedTime = msg.ExpriedTime,
                            MType = (int)MsgType.Sms,
                            RegName = "",
                            Phone = msg.Phone,
                            Address = ""
                        };
                        db.MsgToBeSents.Add(model);
                        int rows = db.SaveChanges();
                        return rows > 0 ? "SUCCESS" : "ERROR";
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string PushEmailMessage(EmailMsg msg)
        {
            try
            {
                bool suc = PushServer.Instance().PushEmail(msg.Address, msg.Content);

                if (suc)
                {
                    #region 添加到已发送成功列表

                    using (var db = new MPModel())
                    {
                        MsgSent model = new MsgSent()
                        {
                            Title = msg.Title,
                            Content = msg.Content,
                            RequestTime = msg.RequestTime,
                            ExpriedTime = msg.ExpriedTime,
                            MType = (int)MsgType.Html,
                            RegName = "",
                            Phone = "",
                            Address = msg.Address,
                            SendTime = DateTime.Now
                        };
                        db.MsgSents.Add(model);
                        int rows = db.SaveChanges();
                        return rows > 0 ? "SUCCESS" : "ERROR";
                    }

                    #endregion
                }
                else
                {
                    #region 添加到待发送成功列表

                    using (var db = new MPModel())
                    {
                        MsgToBeSent model = new MsgToBeSent()
                        {
                            Title = msg.Title,
                            Content = msg.Content,
                            RequestTime = msg.RequestTime,
                            ExpriedTime = msg.ExpriedTime,
                            MType = (int)MsgType.Sms,
                            RegName = "",
                            Phone = "",
                            Address = msg.Address
                        };
                        db.MsgToBeSents.Add(model);
                        int rows = db.SaveChanges();
                        return rows > 0 ? "SUCCESS" : "ERROR";
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
