using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Cards
{
    public class Hurricane : Spell, ICardType//, IElementType
    {
        public new void attack()
        {
            base.attack();
        }
        
    }
}
