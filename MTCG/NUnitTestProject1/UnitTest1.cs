using System;
using System.IO;
using MTCG;
using NUnit.Framework;
using MTCG.Cards;
using Moq;

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
        public void TestMoq()
        {
            var mockedA = new Mock<ICardType>();
            var mockedB = new Mock<ICardType>();
            //var combat = new Combat(mockedA.Object, mockedB.Object);
        }

        
        [Test]
        public void Test1()
        {
            CalcTestClass calc = new CalcTestClass();
            int value1 = 17;
            int value2 = 25;

            int actualValue = calc.xPlusY(value1, value2);
            int expectedVal = 17+25;

            Assert.AreEqual(expectedVal, actualValue);
        }

        [Test]
        public void HurricaneTest()
        {
            Card hur1 = new Hurricane();
            string name = "Hurricane";
            string actual = hur1.GetName();
            Assert.AreEqual(name, actual);
        }
        [Test]
        public void Test3()
        {
            Card hur1 = new Hurricane();
            Assert.Pass();
        }
    }
}