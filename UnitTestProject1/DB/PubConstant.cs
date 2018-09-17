using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

public class PubConstant
{
    /// <summary>
    /// 获取连接字符串
    /// </summary>
    public static string ConnectionString
    {
        get
        {
            return ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;
        }
    }



}

