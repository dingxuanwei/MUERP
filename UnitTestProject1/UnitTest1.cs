using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MUSystem.Core;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using MU.Extensions;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string email = "dingdings02@163.com";
            Console.WriteLine(email.IsEmail( ));
            Console.WriteLine(email.IsNumeric());
        }

        [TestMethod]
        public void TestMethod2()
        {
            
        }
    }
}

