//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFConsole
{
    using System;
    using System.Collections.Generic;
    
    public partial class sys_user
    {
        public string UserCode { get; set; }
        public int Salt { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string QQ { get; set; }
        public string WechatID { get; set; }
        public Nullable<bool> Sex { get; set; }
        public bool Enable { get; set; }
        public string Sign { get; set; }
        public string RoleCode { get; set; }
        public string GroupID { get; set; }
        public string CompanyID { get; set; }
        public string DeptID { get; set; }
        public int LoginCount { get; set; }
        public System.DateTime LoginTime { get; set; }
        public string Token { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdatePerson { get; set; }
    }
}