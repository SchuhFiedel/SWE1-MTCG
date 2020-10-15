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
            
            //card.attack(card2);

            Console.ReadKey();

        }
    }
}
