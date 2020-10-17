using System;
using MTCG.Cards;

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

            Console.ReadKey();

        }
    }
}
