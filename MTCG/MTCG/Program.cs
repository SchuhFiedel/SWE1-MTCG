using System;
using MTCG.Cards;

namespace MTCG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Hurricane card = new Hurricane();
            card.attack();

            Console.ReadKey();

        }
    }
}
