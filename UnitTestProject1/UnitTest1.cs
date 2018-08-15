using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MUSystem.Core;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Data;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
        }

        [TestMethod]
        public void TestMethod2()
        {
            
        }

        /// <summary>
        /// 数据库中与C#中的数据类型对照
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ChangeToCSharpType(string type)
        {
            string reval = string.Empty;
            switch (type.ToLower( ))
            {
                case "int":
                    reval = "Int32";
                    break;
                case "text":
                    reval = "String";
                    break;
                case "bigint":
                    reval = "Int64";
                    break;
                case "binary":
                    reval = "System.Byte[]";
                    break;
                case "bit":
                    reval = "Boolean";
                    break;
                case "char":
                    reval = "String";
                    break;
                case "datetime":
                    reval = "System.DateTime";
                    break;
                case "decimal":
                    reval = "System.Decimal";
                    break;
                case "float":
                    reval = "System.Double";
                    break;
                case "image":
                    reval = "System.Byte[]";
                    break;
                case "money":
                    reval = "System.Decimal";
                    break;
                case "nchar":
                    reval = "String";
                    break;
                case "ntext":
                    reval = "String";
                    break;
                case "numeric":
                    reval = "System.Decimal";
                    break;
                case "nvarchar":
                    reval = "String";
                    break;
                case "real":
                    reval = "System.Single";
                    break;
                case "smalldatetime":
                    reval = "System.DateTime";
                    break;
                case "smallint":
                    reval = "Int16";
                    break;
                case "smallmoney":
                    reval = "System.Decimal";
                    break;
                case "timestamp":
                    reval = "System.DateTime";
                    break;
                case "tinyint":
                    reval = "System.Byte";
                    break;
                case "uniqueidentifier":
                    reval = "System.Guid";
                    break;
                case "varbinary":
                    reval = "System.Byte[]";
                    break;
                case "varchar":
                    reval = "String";
                    break;
                case "Variant":
                    reval = "Object";
                    break;
                default:
                    reval = "String";
                    break;
            }
            return reval;
        }
    }

    class MyClass
    {
        public int ID { get; set; }
        [Display(tableField:"UserName")]
        public string Name { get; set; }
        public string Password { get; set; }
    }

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class DisplayAttribute : Attribute
    {
        public string TableField { get; private set; }
        public DisplayAttribute(string tableField)
        {
            this.TableField = tableField;
        }
    }
}

