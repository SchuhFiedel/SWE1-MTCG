using System;
using System.Collections.Generic;
using System.Text;


namespace MTCG.Cards 
{
    class Monster : ICardType
    {
        public void attack()
        {
            Console.WriteLine("I DID THE ATTACK!");
        }
    }
}
