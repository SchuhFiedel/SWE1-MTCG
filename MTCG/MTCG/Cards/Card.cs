using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Cards
{
    /// <summary>
    ///
    /// Monster cards: Cards with active attacks and damage based on an element type. The element type does not affect pure monster fights.
    /// 
    /// Spell cards: Cards can attack with an element based spell which either:
    ///        - is effective
    ///        - is not effective
    ///        - has no effect
    ///
    /// Specials: 
    ///     - Goblins are afraid of Dragons -> Goblin can not attack Dragon
    ///     - Wizzards can control Orks -> orc can not attack wizzard
    ///     - Knight armor is so heavy that they drown -> Knight dies instantly through water spell
    ///     - Kraken is immune against spells
    ///     - FireElves know Dragons -> Dragons can not attack FireElves
    ///</summary>


    abstract public class Card //: ICardType, IElementType
    {
        CardTypes cardType;
        ElementTypes elementType;
        SpecialType specialType;
        string cardName;
        int healthPoints;
        int attackPoints;
        int defensePoints;
        

        public Card(string newCardName, CardTypes newCardType, ElementTypes newElement, SpecialType newSpecial, int maxHP, int maxAP, int maxDP)
        {
            this.cardType = newCardType;
            this.cardName = newCardName;
            this.elementType = newElement;
            this.specialType = newSpecial;
            this.healthPoints = maxHP;
            this.attackPoints = maxAP;
            this.defensePoints = maxDP;
        }

        public Card Attack(Card other)
        {
            Console.WriteLine("Attacking: " + other);
            return other;
        }

    }


}
