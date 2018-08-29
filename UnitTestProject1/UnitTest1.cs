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
            var x = 10;
            var y = 100;
            Debug.Assert(x < y);
            Console.WriteLine("x<y");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine("TestMethod2");
        }

        [TestMethod]
        public void MyTestMethod()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10000000; i++)
            {
                var x = i;
            }
            sw.Stop();
            Debug.Assert(sw.ElapsedMilliseconds > 10);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}

