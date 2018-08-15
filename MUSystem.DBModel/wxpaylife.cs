using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUSystem.DBModel
{
    public class wxpaylife
    {
        public int id { get; set; }
        public string sendxml { get; set; }
        public string recvxml { get; set; }
        public DateTime addtime { get; set; }
        public string used { get; set; }
    }
}
