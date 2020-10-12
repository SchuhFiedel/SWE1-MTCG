using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Cards
{
    public class Spell : Card
    {
        public void attack()
        {
            Console.WriteLine("I DID THE SPELL!");
        }

        public Spell() : base("spell")
        {

        }

    }
}
