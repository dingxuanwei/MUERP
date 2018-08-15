using System;
using System.Collections.Generic;
using System.Text;

namespace MUSystem.Utils.NPOI.HSSF
{
    [Serializable]
    public class OldExcelFormatException:Exception
    {
        public OldExcelFormatException(String s)
            : base(s)
        { }

    }
}
