using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;

namespace WebClientTool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddApplicant()
        {
            var ctx = new
            {
                ID = Guid.NewGuid().ToString(),                                         //消息ID，手动生成，字符串
                Auditor = "admin",                                                      //审核人UserCode
                AntiAuditReason = "反审核理由，会显示到首页上",                              //会显示到首页的通知事项中，以时间+反审理由的形式
                Notice = new List<string>() { "test2" },                                //通知人UserCode，可多人
                Applicant = "test1",                                                    //申请人UserCode
                MenuCode = "2107",                                                      //菜单编号，用于获取跳转的菜单Url，如2107为采购反审
                SubmitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),              //当前提交时间
                OrderNo = "8f0615edcd3f47348f9f16c872a2a2d4"                            //大货订单号，用于反审链接的查询参数
            };

            string content = Newtonsoft.Json.JsonConvert.SerializeObject(ctx);

            var model = new HtmlMsg()
            {
                Title = "推送测试",                                                       //标题，不会反映到ERP系统，消息系统查询使用
                Content = content,                                                      //待推送消息的主体内容，JSON格式
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),             //请求时间
                ExpriedTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),  //超时时间，超时后，消息系统不再推送该条消息
                RegName = "admin"                                                       //审核人UserCode,与Auditor相同，用于接收消息
            };
            string s = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/json");
            var data = wc.UploadData("http://localhost:9918/MPService/PushHtmlMessage", Encoding.UTF8.GetBytes(s));
            var result = Encoding.UTF8.GetString(data);
            return Content(result);
        }
    }

    /// <summary>
    /// Html推送，用于WebSocket
    /// </summary>
    class HtmlMsg
    {
        /// <summary>
        /// 标题，暂不列入推送，用于后台
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 具体推送内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string RequestTime { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public string ExpriedTime { get; set; }
        /// <summary>
        /// WebSocket注册名称，当WebSocket上线后，立即发送自己的名称即可在后续接收到推送消息
        /// </summary>
        public string RegName { get; set; }
    }
}