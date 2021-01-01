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
    public class devDBTests
    {
        List<Card> cardList = new List<Card>();

        [SetUp]
        public void Setup()
        {
            PostgreSqlClass db = new PostgreSqlClass();
            cardList = db.GetAllCardsFromDB();
        }


        [Test]
        public void AttackTest_Hurricane_Hransig()
        {
            Card hur1 = new Hurricane();
            Card hran1 = new Hransig();
            // 25 +10 - 20
            int expected = hran1.GetHP() - hur1.GetAP() + hran1.GetDP();
            int actual = hur1.Attack(ref hran1).GetHP();
            

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
            Card cardToAttack = cardList[1];
            cardList[0].Attack(ref cardToAttack);
            int actual = cardList[1].GetHP();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DBAttackTest_PercingDMG()
        {
            int expected;

            if(cardList[1].GetHP() - cardList[2].GetAP() >= 0 && cardList[1].GetPiercing())
            {
                expected = cardList[1].GetHP() - cardList[2].GetAP();
            }
            else if(cardList[1].GetHP() - cardList[2].GetAP() + cardList[1].GetDP() >= 0 && cardList[1].GetPiercing())
            {
                expected = cardList[1].GetHP() - cardList[2].GetAP() + cardList[1].GetDP();
            }
            else { expected = 0; }

            Card cardToAttack = cardList[1];
            Console.WriteLine(cardToAttack.GetCardName() + " HP: " + cardToAttack.GetHP() +"Is attacked by " + cardList[2].GetCardName() + cardToAttack.GetAP());
            cardList[2].Attack(ref cardToAttack);
            int actual = cardToAttack.GetHP();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DBAttackTest_ElementBuff()
        {
            int expected = (cardList[1].GetHP() + cardList[1].GetDP() - (int)(cardList[3].GetAP() * 1.5));

            Card cardToAttack = cardList[1];
            cardList[3].Attack(ref cardToAttack);
            int actual = cardList[1].GetHP();

            Assert.AreEqual(expected, actual);
        }
    }
}