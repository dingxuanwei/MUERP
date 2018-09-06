using System;
using System.ComponentModel.DataAnnotations;

namespace MU.Models
{
    public class sys_user
    {
        [Key]
        [MaxLength(20)]
        public string UserCode { get; set; }
        [MaxLength(10)]
        public string UserSeq { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string RoleName { get; set; }
        [MaxLength(50)]
        public string OrganizeName { get; set; }
        [MaxLength(50)]
        public string DepCode { get; set; }
        [MaxLength(500)]
        public string ConfigJSON { get; set; }
        public bool IsEnable { get; set; }
        public int LoginCount { get; set; }
        public DateTime LastLoginDate { get; set; }
        [MaxLength(50)]
        public string CreatePerson { get; set; }
        public DateTime CreateDate { get; set; }
        [MaxLength(50)]
        public string UpdatePerson { get; set; }
        public DateTime UpdateDate { get; set; }
        [MaxLength(50)]
        public string FactoryCode { get; set; }
        [MaxLength(50)]
        public string DepNo { get; set; }
        [MaxLength(50)]
        public string Token { get; set; }
    }
}
