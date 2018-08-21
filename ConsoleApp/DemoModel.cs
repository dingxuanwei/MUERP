using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class TUsers
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? IsLogin { get; set; }

        public virtual List<TUsersRoles> TUsersRolesList { get; set; }
    }

    public class TRoles
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public string RoleRemark { get; set; }

        public virtual List<TUsersRoles> TRolesUsersList { get; set; }
    }

    public class TUsersRoles
    {
        public int Id { get; set; }

    }
}
