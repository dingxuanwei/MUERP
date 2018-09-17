using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using MU.Extensions;
using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var d = DateTime.Now.ToOADate();
            Console.WriteLine(d);

            var t = DateTime.FromOADate(d);
            Console.WriteLine(t.ToString());
        }
    }
}

