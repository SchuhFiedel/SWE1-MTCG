using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using MTCG;
using MTCG.Cards;
using MTCG.Util;


namespace Test
{
    [TestFixture]
    public class Tests
    {
        List<Card> cardList = new List<Card>();

        [SetUp]
        public void Setup()
        {
            MySqlDataClass db = new MySqlDataClass();
            cardList = db.getAllCardsFromDB();
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
        }

        [Test]
        public void HurricaneTest()
        {
            Card hur1 = new Hurricane();
            string name = "Hurricane";
            string actual = hur1.GetCardName();
            Assert.AreEqual(name, actual);
        }
        */


        [Test]
        public void AttackTest_Hurricane_Hransig()
        {
            Card hur1 = new Hurricane();
            Card hran1 = new Hransig();
            // 25 +10 - 20
            int expected = hran1.GetHP() - hur1.GetAP() + hran1.GetDP();
            int actual = hur1.Attack(hran1).GetHP();
            

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void dbGetCardsTest()
        {
            string expOne = "Hurricane";
            string actOne = cardList[0].GetCardName();
            Assert.AreEqual(expOne, actOne);
        }

        [Test]
        public void DBAttackTest_Hurricane_Hransig()
        {
            int expected = cardList[1].GetHP() - cardList[0].GetAP() + cardList[1].GetDP();
            cardList[0].Attack(cardList[1]);
            int actual = cardList[1].GetHP();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DBAttackTest_PercingDMG()
        {
            int expected;
            if(cardList[1].GetHP() - cardList[2].GetAP() >= 0)
            {
                expected = cardList[1].GetHP() - cardList[2].GetAP();
            }
            else { expected = 0; }


            cardList[2].Attack(cardList[1]);
            int actual = cardList[1].GetHP();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DBAttackTest_ElementBuff()
        {
            int expected = (cardList[1].GetHP() + cardList[1].GetDP() - (int)(cardList[3].GetAP() * 1.5));

            cardList[3].Attack(cardList[1]);
            int actual = cardList[1].GetHP();

            Assert.AreEqual(expected, actual);
        }
    }
}