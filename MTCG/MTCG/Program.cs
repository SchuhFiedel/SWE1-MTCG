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
            Console.WriteLine("Hello World!");
            List<Card> cards = new List<Card>();

            PostgreSqlClass db = new PostgreSqlClass();

            //db.Insert("INSERT INTO usertable (firstname, givenname, cardid) VALUES ((SELECT firstname from usertable where i_uid = 3), 'B' , 4);");
            //Console.WriteLine("Insert Done");

            cards = db.GetCardsFromDB();

            foreach (Card x in cards){
                Console.WriteLine(x.GetCardName());
            }
            
            Console.ReadKey();
            
        }
    }
}
