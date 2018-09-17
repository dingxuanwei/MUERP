using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.Models
{
    public class sys_menu
    {
        public string MenuCode { get; set; }
        public string ParentCode { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public string IconClass { get; set; }
        public string IconURL { get; set; }
        public string MenuSeq { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
