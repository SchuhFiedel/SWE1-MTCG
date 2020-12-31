using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class MatchmakingListWrapper
    {
        public List<int> userIDsForMatchmaking = new List<int>();

        public MatchmakingListWrapper()
        {

        }
    }

    public class FightingTupleWrapper
    {
        public FightingTupleWrapper()
        {

        }

        public Tuple<int, int, bool, bool> fighting = Tuple.Create<int, int, bool, bool>(0, 0, false, false); // userid, userid, fighting, draw

    }

   

    
}
