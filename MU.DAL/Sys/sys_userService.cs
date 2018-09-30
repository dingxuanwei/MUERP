using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MU.DBWapper;
using MU.DBWapper.Models;
using MU.Extensions;

namespace MU.DAL.Sys
{
    public class sys_userService
    {
        public sys_user LoginCheck(string usercode, string password)
        {
            password = password.MD5();
            var result = DB.Select<sys_user>(p => p.UserCode == usercode && p.Password == password);
            if (result.Count == 0) return null;
            return result.FirstOrDefault();
        }

        public void UpdateUserLoginCountAndDate(sys_user model)
        {
            model.LastLoginDate = DateTime.Now;
            model.LoginCount += 1;
            DB.Update(model);
        }
    }
}
