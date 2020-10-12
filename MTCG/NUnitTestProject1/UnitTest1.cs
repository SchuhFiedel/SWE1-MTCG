using NUnit.Framework;
using System;
using System.IO;
using MTCG;
using MTCG.Cards;

namespace Test
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        /*
        [Test]
        public void Test1()
        {
            CalcTestClass calc = new CalcTestClass();
            int value1 = 17;
            int value2 = 25;

            int actualValue = calc.xPlusY(value1, value2);
            int expectedVal = 17+25;

            Assert.AreEqual(expectedVal, actualValue);
        }*/

        [Test]
        public void Test2()
        {
            Card hur1 = new Hurricane();
            Card hur2 = new Hurricane();
            hur1.attack(hur2);
            Assert.Pass();
        }
        [Test]
        public void Test3()
        {
            Card hur1 = new Hurricane();
            Card mon = new Monster();
            hur1.attack(mon);
            Assert.Pass();
        }
    }
}