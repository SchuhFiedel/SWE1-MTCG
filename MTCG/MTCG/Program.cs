using System;
using MTCG.Cards;
using MTCG.Util;

namespace MTCG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Card card = new Hurricane();
            Card card2 = new Hransig();

            Console.WriteLine("Hransig HP : " + card2.GetHP());
            card.Attack(card2);
            Console.WriteLine("Hransig HP : " + card2.GetHP());

            MySqlDataClass db = new MySqlDataClass();
            string queryString = "";
            while (queryString != ".") {
                queryString = Console.ReadLine();
                if (queryString != ".") { db.runQuery(queryString); };
            }
            Console.ReadKey();

        }
    }
}
