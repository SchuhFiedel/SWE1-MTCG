using NUnit.Framework;
using System;
using System.IO;
using MTCG;

namespace Test
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            CalcTestClass calc = new CalcTestClass();
            int value1 = 17;
            int value2 = 25;

            int actualValue = calc.xPlusY(value1, value2);
            int expectedVal = 25;

            Assert.AreEqual(expectedVal, actualValue);
        }
    }
}