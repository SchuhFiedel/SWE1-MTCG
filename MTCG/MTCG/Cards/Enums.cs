using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Cards
{

    //no summary needed
    public enum CardTypes
    {
        Spell = 0,
        Monster
    }

    /// <summary>
    /// Effectiveness of Elements
    /// - Water good vs Fire AND Air (Water inefficient agains Ice)
    /// - Fire good vs Normal AND Ice (Fire inefficient agains Air)
    /// - Normal good vs Water AND Earth
    /// - Earth good vs Fire AND Ice
    /// - Ice good vs Electro AND Air (Water inefficient agains Ice)
    /// - Electro good vs normal AND Earth
    /// - Air good vs Electro (Fire inefficient agains Air)
    /// </summary>

    public enum ElementTypes
    {
        Normal = 0,
        Fire,
        Water,
        Ice,
        Earth,
        Air,
        Electro
    }

    /// <summary>
    /// Dragon ->  can not attack FireElf (Can not be attacked by Goblin)
    /// FireElf -> (Can not be attacked by Dragon)
    /// Wizzard -> (Can not be attacked by Ork)
    /// Knight -> Dies if hit by water spell
    /// Kraken -> Immune to all spells
    /// Goblin -> can not attack Dragon
    /// Ork -> can not attack Wizzard
    /// </summary>

    public enum SpecialTypes
    {
        None = 0,
        Dragon,
        Goblin,
        Knight,
        Wizzard,
        Ork,
        Kraken,
        FireElf
    }
}
