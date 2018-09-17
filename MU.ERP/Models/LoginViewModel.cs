using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace MU.ERP.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "用户名称不能为空")]
        [Display(Name = "用户名称")]
        [MaxLength(18)]
        public string usercode { get; set; }

        [Required(ErrorMessage = "用户密码不能为空")]
        [Display(Name = "用户密码")]
        [MaxLength(18)]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

    public class MUser<TUserData> : IPrincipal where TUserData: class, new ()
    {
        private IIdentity _identity;
        private TUserData _userData;

        public MUser(FormsAuthenticationTicket ticket, TUserData userData)
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
            if (userData == null) throw new ArgumentNullException("userData");

            _identity = new FormsIdentity(ticket);
            _userData = userData;
        }

        public TUserData UserData
        {
            get { return _userData; }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            IPrincipal principal = _userData as IPrincipal;
            if (principal == null)
                throw new Exception("非法访问");
            else
                return principal.IsInRole(role);
        }

        public static void SignIn(string loginName, TUserData userData, int expiration)
        {
            if (string.IsNullOrEmpty(loginName)) throw new ArgumentNullException("loginName");
            if (userData == null) throw new ArgumentNullException("userData");

            string data = null;
            if (userData != null) data = JsonConvert.SerializeObject(userData);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, loginName, DateTime.Now, DateTime.Now.AddDays(1), true, data);
            string cookieValue = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
            cookie.HttpOnly = true;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (expiration > 0) cookie.Expires = DateTime.Now.AddMinutes(expiration);

            HttpContext context = HttpContext.Current;
            if (context == null) throw new InvalidOperationException();

            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
        }

        public static void TrySetUserInfo(HttpContext context)
        {
            if (context == null) throw new ArgumentException("context");

            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return;
            try
            {
                TUserData userData = null;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    userData = JsonConvert.DeserializeObject<TUserData>(ticket.UserData);
                    if (userData != null)
                    {
                        context.User = new MUser<TUserData>(ticket, userData);
                    }
                }
            }
            catch { }
        }
    }
}
