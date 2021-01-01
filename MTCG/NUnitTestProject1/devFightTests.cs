using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
//using Moq;
using MTCG;
using MTCG.Cards;
using MTCG.Util;


namespace Test
{
    [TestFixture]
    public class devFightTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMoq()
        {
            //var mockedA = new Mock<ICardType>();
            //var mockedB = new Mock<ICardType>();
            //var combat = new Combat(mockedA.Object, mockedB.Object);
        }

        
        [Test]
        public void Test1()
        {
           
        }

        [Test]
        public void HurricaneTest()
        {
            Card hur1 = new Hurricane();
            string name = "Hurricane";
            string actual = hur1.GetCardName();
            Assert.AreEqual(name, actual);
        }

        [Test]
        public void AttackTest()
        {
            Card hur1 = new Hurricane();
            Card hran1 = new Hransig();
            // 25 +10 - 20
            int expected = hran1.GetHP() - hur1.GetAP() + hran1.GetDP();
            int actual = hur1.Attack(ref hran1).GetHP();
            

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DbGetCardsTest()
        {
            PostgreSqlClass db = new PostgreSqlClass();
            List<Card> cardList = new List<Card>();
            cardList = db.GetAllCardsFromDB();
            Console.WriteLine();

            string expOne = "Hurricane";
            string actOne = cardList[0].GetCardName();

            Assert.AreEqual(expOne, actOne);
        }
    }
}