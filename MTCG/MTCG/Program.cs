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
            Card card2 = new Hurricane();
            Card card3 = new Monster();
            card.attack(card2);
            
            Console.WriteLine(card2.attack(card3));

            Console.ReadKey();

        }
    }
}
