using System;
using System.Collections.Generic;
using System.Text;
using MTCG.Cards;
using MTCG.Util;

namespace MTCG
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            PostgreSqlClass db = new PostgreSqlClass();
            List<Card> cardList = new List<Card>();
            cardList = db.GetCardsFromDB();
            Console.WriteLine();

            for (int i = 0; i < cardList.Count; i++)
            {
                Console.WriteLine(cardList[i].GetCardName() + " |Card Type: " + cardList[i].GetCardType() + " |Piercing Damage: " + cardList[i].GetPiercing());
                Console.WriteLine();
            }

            Console.WriteLine("Hransig HP : " + cardList[1].GetHP());
            cardList[0].Attack(cardList[1]);
            Console.WriteLine("Hransig HP : " + cardList[1].GetHP());


            Console.ReadKey();
            
        }
    }
}
