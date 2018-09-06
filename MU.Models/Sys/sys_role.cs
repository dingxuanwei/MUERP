using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MU.Models
{
    public class sys_role
    {
        [Key]
        [MaxLength(20)]
        public string RoleCode { get; set; }
        [MaxLength(10)]
        public string RoleSeq { get; set; }
        [MaxLength(50)]
        public string RoleName { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string CreatePerson { get; set; }
        public DateTime CreateDate { get; set; }
        [MaxLength(50)]
        public string UpdatePerson { get; set; }
        public DateTime UpdateDate { get; set; }
        [MaxLength(50)]
        public string CheckType { get; set; }
        [MaxLength(50)]
        public string LoginPage { get; set; }
        [MaxLength(50)]
        public string LoginFirstPage { get; set; }
    }
}
