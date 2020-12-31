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
        //using ICardType newCardType = new Monster();
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
        public SpecialTypes GetSpecial() { return specialType; }
        public string GetCardName() { return cardName; }
        public string GetCardInfo() { return cardInfo; }
        public int GetCardId() { return cardID; }
        public int GetHP() { return healthPoints; }
        public int GetAP() { return attackPoints; }
        public int GetDP() { return defensePoints; }
        public bool GetPiercing() { return piercing; }


        //other functions
        public Card Attack(ref Card other)
        {
            Console.WriteLine(this.GetCardName() + " is Attacking: " + other.GetCardName());
            other.TakeDamage(this);
            return other;
        }

        //This card is beeing attacked
        public void TakeDamage(Card attacker)
        {
            double remAP = attacker.GetAP(); // remaining Attack Points

            remAP = CheckBuffs(remAP, attacker);

            // Dont deduct attack points because its piercing damage
            if (attacker.GetPiercing() == false){
                //DEFENDING ACTION -- deduct dp from ap
                (this.defensePoints, remAP) = this.Defend((int)remAP, this.defensePoints);
            }

            Console.WriteLine(this.cardName + " takes " + remAP.ToString() + " Dmg!");
            int HP = this.healthPoints - (int)remAP;
            

            //if HP <  0 set to 0 else set Card.Healthpoints to HP
            if (HP <= 0){this.healthPoints = 0;}
            else{this.healthPoints = HP;}
            Console.WriteLine(this.cardName + " remaining HP: " + this.healthPoints.ToString()+"!");
        }

        //DEFENDING ACTION -- deduct dp from ap
        (int, int) Defend(int attackerAP, int defenderDP)
        {
            int remAP = attackerAP - defenderDP;
            int remDP = defenderDP - attackerAP;

            if (remAP <= 0) remAP = 0;
            if (remDP <= 0) remDP = 0;

            return (remDP, remAP);
        }

        double Buff(double atp)
        {
            return (atp * 1.5);
        }

        double Debuff(double atp)
        {
            return (atp * 0.5);
        }

        double CheckBuffs(double atp, Card attacker)
        {

            if(attacker.GetCardType() != CardTypes.Monster || this.GetCardType() != CardTypes.Monster)
            {

                //Buffs
                /// - Water good vs Fire AND Air 
                if (attacker.GetElementType() == ElementTypes.Water && (this.elementType == ElementTypes.Fire || this.elementType == ElementTypes.Air)) { atp = Buff(atp); }
                /// - Fire good vs Normal AND Ice 
                if (attacker.GetElementType() == ElementTypes.Fire && (this.elementType == ElementTypes.Normal || this.elementType == ElementTypes.Ice)) { atp = Buff(atp); }
                /// - Normal good vs Water AND Earth
                if (attacker.GetElementType() == ElementTypes.Normal && (this.elementType == ElementTypes.Water || this.elementType == ElementTypes.Earth)) { atp = Buff(atp); }
                /// - Earth good vs Fire AND Ice
                if (attacker.GetElementType() == ElementTypes.Earth && (this.elementType == ElementTypes.Fire || this.elementType == ElementTypes.Ice)) { atp = Buff(atp); }
                /// - Ice good vs Electro AND Air
                if (attacker.GetElementType() == ElementTypes.Ice && (this.elementType == ElementTypes.Electro || this.elementType == ElementTypes.Air)) { atp = Buff(atp); }
                /// - Electro good vs normal AND Earth
                if (attacker.GetElementType() == ElementTypes.Electro && (this.elementType == ElementTypes.Normal || this.elementType == ElementTypes.Earth)) { atp = Buff(atp); }
                /// - Air good vs Electro
                if (attacker.GetElementType() == ElementTypes.Air && this.elementType == ElementTypes.Electro) { atp = Buff(atp); }

                //Spell-Monster Specials
                /// Knight -> Dies if hit by water spell
                if (attacker.GetCardType() == CardTypes.Spell && attacker.GetElementType() == ElementTypes.Water && this.GetSpecial() == SpecialTypes.Knight) { atp = 100; }
                /// Kraken -> Immune to all spells
                if (attacker.GetCardType() == CardTypes.Spell && this.GetSpecial() == SpecialTypes.Kraken) { atp = 0; }

                //Debuffs
                ///Water inefficient agains Ice
                if (attacker.GetElementType() == ElementTypes.Water && this.elementType == ElementTypes.Ice) { atp = Debuff(atp); }
                ///(Fire inefficient agains Air)
                if (attacker.GetElementType() == ElementTypes.Fire && this.elementType == ElementTypes.Air) { atp = Debuff(atp); }

            }
            else
            {
                //Monser-Monster Specials
                /// Ork -> can not attack Wizzard
                if (attacker.GetSpecial() == SpecialTypes.Ork && this.GetSpecial() == SpecialTypes.Wizzard) { atp = 0; }
                /// Goblin -> can not attack Dragon
                if (attacker.GetSpecial() == SpecialTypes.Goblin && this.GetSpecial() == SpecialTypes.Dragon) { atp = 0; }
                /// Dragon -> can not attack Fire Elves
                if (attacker.GetSpecial() == SpecialTypes.Dragon && this.GetSpecial() == SpecialTypes.FireElf) { atp = 0; }
            }


            return atp;
        }

        public bool CheckAlive()
        {
            if(this.GetHP() >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
