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
            string subject = "测试邮件";
            string displayname = "上海秒优";
            string frommail = "dingxuanwei@shtmu.com";
            string password = "*******";
            string host = "smtp.ym.163.com";
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.From = new MailAddress(frommail, displayname);
            mail.To.Add(new MailAddress(emailAddress));
            mail.Body = html;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(frommail, password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
        }
    }
}
