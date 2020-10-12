﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Cards
{
    /// <summary>
    /// PLS NOTE THAT THESE ARE ONLY GUIDELINES.
    /// THEY MAY VARY FROM COMPANY TO COMPANY!
    /// 
    /// - Use pascal casing for members (fields and methods)
    /// - Keep your variables in order: 
    ///     declare all member variables at the top of a class, with static variables at the very top
    ///     first public, then private
    /// - group by type
    ///     const/static/readonly
    ///     properties
    ///     class variables
    ///     
    /// </summary>

    abstract public class Card 
    {
        string cardType = null;



        public Card(string cardType)
        {
            if (cardType == "spell")
            {
                this.cardType = cardType;
            }
        }

    }


}
