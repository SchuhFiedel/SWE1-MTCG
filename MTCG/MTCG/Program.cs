using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MTCG.Cards;
using MTCG.Util;
//using MTCG.Server;

namespace MTCG.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            PostgreSqlClass db = new PostgreSqlClass();
            //List<Card> cardList = new List<Card>();
            //cardList = db.GetCardsFromDB();
            //Console.WriteLine();
            

            //for (int i = 0; i < cardList.Count; i++)
            //{
            //    Console.WriteLine(cardList[i].GetCardName() + " |Card Type: " + cardList[i].GetCardType() + " |Piercing Damage: " + cardList[i].GetPiercing());
            //    Console.WriteLine();
            //}

            //Console.WriteLine("Hransig HP : " + cardList[1].GetHP());
            //cardList[0].Attack(cardList[1]);
            //Console.WriteLine("Hransig HP : " + cardList[1].GetHP());


            Console.WriteLine("Server Started...!");
            Server myserver = new Server("127.0.0.1", 8000);
            
            Console.ReadKey();
        }
    }
}
