using MU.DBWapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.DAL.Sys
{
    public class DataBaseService
    {
        public static string TestConnection()
        {
            return DB.Test();
        }
    }
}
