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


    //abstract 
    public class Card //: ICardType, IElementType
    {
        
        CardTypes cardType;
        ElementTypes elementType;
        SpecialTypes specialType;
        string cardName;
        string cardInfo;
        int healthPoints;
        int attackPoints;
        int defensePoints;
        int cardID;
        bool piercing;

        public Card(int newCardID,string newCardName, string newCardInfo, CardTypes newCardType, ElementTypes newElement, SpecialTypes newSpecial, int maxHP, int maxAP, int maxDP, bool newPiercing)
        {
            this.cardID = newCardID;
            this.cardType = newCardType;
            this.cardName = newCardName;
            this.cardInfo = newCardInfo;
            this.elementType = newElement;
            this.specialType = newSpecial;
            this.healthPoints = maxHP;
            this.attackPoints = maxAP;
            this.defensePoints = maxDP;
            this.piercing = newPiercing;
        }

        //get functions
        public ElementTypes GetElementType() { return elementType; }
        public CardTypes GetCardType() { return cardType; }
        public SpecialTypes getSpecial() { return specialType; }
        public string GetCardName() { return cardName; }
        public int GetHP() { return healthPoints; }
        public int GetAP() { return attackPoints; }
        public int GetDP() { return defensePoints; }
        public bool GetPiercing() { return piercing; }


        //other functions
        public Card Attack(Card other)
        {
            Console.WriteLine("Attacking: " + other.GetCardName());
            other.TakeDamage(this, false);
            return other;
        }

        public void TakeDamage(Card attacker, bool piercing)
        {
            int remAP;
            if (piercing == true)
            {
                remAP = attacker.attackPoints;
            }
            else
            {
                //DEFENDING ACTION
                (this.defensePoints, remAP) = this.Defend(attacker.attackPoints, this.defensePoints);
            }

            int HP = this.healthPoints - remAP;

            if (HP <= 0)
            {
                this.healthPoints = 0;
            }
            else
            {
                this.healthPoints = HP;
            }
            
        }

        (int, int) Defend(int attackerAP, int defenderDP)
        {
            int remAP = attackerAP - defenderDP;
            int remDP = defenderDP - attackerAP;

            if (remAP <= 0) remAP = 0;
            if (remDP <= 0) remDP = 0;

            return (remDP, remAP);
        }
    }
}
