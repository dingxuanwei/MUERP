using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MU.Extensions
{
    public class Email
    {
        public static void Send(string emailAddress, string html)
        {
            var frommail= "dingxuanwei@shtmu.com";
            var password = "38158738a";
             MailMessage mail = new MailMessage();
            mail.Subject = "测试邮件";
            mail.From = new MailAddress(frommail, "上海秒优");
            mail.To.Add(new MailAddress(emailAddress, "noreply"));
            mail.Body = html;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.ym.163.com";
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(frommail, password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
        }
    }
}
