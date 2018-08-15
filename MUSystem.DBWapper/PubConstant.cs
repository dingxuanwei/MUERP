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
            //string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];       
            //string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
            //if (ConStringEncrypt == "true")
            //{
            //    _connectionString = DESEncrypt.Decrypt(_connectionString);
            //}
            //return _connectionString; 
            return ConfigurationManager.ConnectionStrings["MES"].ConnectionString;
        }
    }



}

