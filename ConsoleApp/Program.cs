using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MailMessage mail = new MailMessage( );
            mail.Subject = "测试邮件";
            mail.From = new MailAddress("dingxuanwei@163.com", "Ding");
            mail.To.Add(new MailAddress("dingdings02@163.com", "Dingxw"));
            mail.CC.Add(new MailAddress("dingxuanwei@shtmu.com", "August"));
            mail.Body = "验证码："+ new Random( ).Next(1000000).ToString( );
            mail.BodyEncoding = Encoding.UTF8;
            //mail.IsBodyHtml = true;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            SmtpClient client = new SmtpClient( );
            client.Host = "smtp.163.com";
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("dingxuanwei@163.com", "38158738a");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);

            Console.ReadLine( );
        }
    }
}
