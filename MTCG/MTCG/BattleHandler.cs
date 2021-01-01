using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MTCG.Util;
using MTCG.Cards;

namespace MTCG
{
    public class BattleHandler
    {
        private static Mutex mut = new Mutex();
        List<Card> playerOneHand = new List<Card>();
        List<Card> playerTwoHand = new List<Card>();
        List<Card> playerOneTable = new List<Card>();
        List<Card> playerTwoTable = new List<Card>();
        User playerOne;
        User playerTwo;
        Random rand = new Random();

        public static int SPELLUSES = 3;

        public BattleHandler()
        {
            List<Card> playerOneHand = new List<Card>();
            List<Card> playerTwoHand = new List<Card>();
            List<Card> playerOneTable = new List<Card>();
            List<Card> playerTwoTable = new List<Card>();
            User playerOne = null;
            User playerTwo = null;
        }

        public void PrepareBattle(ref FightingTupleWrapper fighting, ref MatchmakingListWrapper userIDsForMatchmaking)
        {
            if(userIDsForMatchmaking.userIDsForMatchmaking.Count >= 2)
            {
                mut.WaitOne(); // user values without other threads interfering

                int userOneID = userIDsForMatchmaking.userIDsForMatchmaking[0]; // get ids from matchmaking queue
                int userTwoID = userIDsForMatchmaking.userIDsForMatchmaking[1];

                fighting.fighting = Tuple.Create<int, int, bool, bool>(userOneID, userTwoID, true, false); // set status for serverinfo

                userIDsForMatchmaking.userIDsForMatchmaking.Remove(userOneID); // remove userIDs from the matchmakin queue
                userIDsForMatchmaking.userIDsForMatchmaking.Remove(userTwoID);

                mut.ReleaseMutex();

                //Coinflip
                bool turn = Convert.ToBoolean(RandoFunct(0,1));
                if (turn)
                {
                    int tmp = userOneID;
                    userOneID = userTwoID;
                    userTwoID = tmp;
                }

                PostgreSqlClass DB = new PostgreSqlClass();
                playerOneHand = DB.GetUserDeckCards(userOneID);
                playerTwoHand = DB.GetUserDeckCards(userTwoID);
                playerOne = DB.GetUser(userOneID);
                playerTwo = DB.GetUser(userTwoID);

                Tuple<int, int, bool> winLoseDraw = Battle(turn); // winner, loser, draw(true) or not draw(false)

                mut.WaitOne();
                fighting.fighting = Tuple.Create<int, int, bool, bool>(winLoseDraw.Item1, winLoseDraw.Item2, false, winLoseDraw.Item3);
                mut.ReleaseMutex();
            }
        }

        private int RandoFunct(int inclusiveFrom, int inclusiveTo)
        {
            int number = rand.Next(inclusiveFrom, inclusiveTo + 1);
            return number;
        }

        private Tuple<int,int,bool> Battle(bool turn)
        {
            Tuple<int, int, bool> retVal = Tuple.Create(0, 0, false); // winner, loser, draw(true) or not draw(false)
            Dictionary<int, int> playerOneSpellUses = new Dictionary<int, int>(); // cardID, uses
            Dictionary<int, int> playerTwoSpellUses = new Dictionary<int, int>();  // cardID, uses

            bool end = false;
            int numberOfRounds = 0;
            while (end == false)
            {
                RemoveDeadCards(ref playerOneSpellUses, ref playerTwoSpellUses);

                if (turn == false)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine(playerOne.username +"'s Turn:");
                    Console.ResetColor();
                    if (playerOneHand.Count > 0)
                    {
                        //play card from hand to table
                        playerOneTable.Add(playerOneHand[0]);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.WriteLine(playerOne.username + " plays: " + playerOneHand[0].GetCardName());
                        Console.ResetColor();
                        if (CardTypes.Spell!=playerOneHand[0].GetCardType())
                        {
                            playerOneSpellUses.Add(playerOneHand[0].GetCardId(), SPELLUSES);
                        }
                        playerOneHand.RemoveAt(0);
                    }

                    //if card on table attack Card on oponent table randomly
                    if (playerOneTable.Count > 0 && playerTwoTable.Count > 0)
                    {
                        int oponentNumberOfCards = playerTwoTable.Count;
                        foreach (Card x in playerOneTable)
                        {
                            Card cardToAttack = playerTwoTable[RandoFunct(0, oponentNumberOfCards - 1)]; //randomly choose which card to attack
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.WriteLine(x.GetCardName() + " Attacks " + cardToAttack.GetCardName());
                            Console.ResetColor();
                            if (!(cardToAttack.GetCardType() == CardTypes.Spell))
                            {
                                x.Attack(ref cardToAttack); //dmg is dealt to the other card
                                if (x.GetCardType() == CardTypes.Spell)
                                {
                                    int uses = playerOneSpellUses[x.GetCardId()];
                                    playerOneSpellUses.Add(x.GetCardId(), uses - 1);
                                }
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                Console.WriteLine("Can not attack Spell! - No attack happens!");
                                Console.ResetColor();
                            }
                            
                        }
                    }
                    numberOfRounds++;
                    turn = true;
                }
                else
                {
                    Console.WriteLine(playerTwo.username + "'s Turn:");

                    if (playerTwoHand.Count > 0)
                    {
                        //play card from hand to table
                        playerTwoTable.Add(playerTwoHand[0]);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.WriteLine(playerTwo.username + " plays: " + playerTwoHand[0].GetCardName());
                        Console.ResetColor();
                        if (CardTypes.Spell != playerTwoHand[0].GetCardType())
                        {
                            playerTwoSpellUses.Add(playerTwoHand[0].GetCardId(), SPELLUSES);
                        }
                        playerTwoHand.RemoveAt(0);
                    }

                    //if card on table attack Card on oponent table randomly
                    if (playerTwoTable.Count > 0 && playerOneTable.Count > 0)
                    {
                        int oponentNumberOfCards = playerOneTable.Count;
                        foreach (Card x in playerTwoTable)
                        {
                            Card cardToAttack = playerOneTable[RandoFunct(0, oponentNumberOfCards - 1)]; //randomly choose which card to attack
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.WriteLine(x.GetCardName() + " Attacks " + cardToAttack.GetCardName());
                            Console.ResetColor();
                            if (!(cardToAttack.GetCardType() == CardTypes.Spell))
                            {
                                x.Attack(ref cardToAttack); //dmg is dealt to the other card
                                if (x.GetCardType() == CardTypes.Spell)
                                {
                                    int uses = playerTwoSpellUses[x.GetCardId()];
                                    playerTwoSpellUses.Add(x.GetCardId(), uses - 1);
                                }
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                Console.WriteLine("Can not attack Spell! - No attack happens!");
                                Console.ResetColor();
                            }
                        }
                    }
                    turn = false;
                    numberOfRounds++; 
                }

                Console.BackgroundColor = ConsoleColor.Blue;
                if ((playerOneHand.Count == 0 && playerOneTable.Count == 0) && (playerTwoHand.Count > 0 || playerTwoTable.Count > 0))
                {
                    //player One wins
                    retVal = Tuple.Create(playerOne.user_id, playerTwo.user_id, false);
                    Console.WriteLine("Winner: User " + playerOne.username);
                    end = true;
                    PostgreSqlClass DB = new PostgreSqlClass();
                    DB.AddElo(3, playerOne.user_id);
                    DB.DeductElo(5, playerTwo.user_id);
                    DB.Insert("UPDATE usertable SET num_games = num_games + 1 WHERE user_id=" + playerOne.user_id + ";");
                    DB.Insert("UPDATE usertable SET num_games = num_games + 1 WHERE user_id=" + playerTwo.user_id + ";");
                    DB.Insert("UPDATE usertable SET loss = loss + 1 WHERE user_id = " + playerTwo.user_id+";");
                    DB.Insert("UPDATE usertable SET win = win + 1 WHERE user_id = " + playerOne.user_id + ";");
                }
                else if ((playerTwoHand.Count == 0 && playerTwoTable.Count == 0) && (playerOneHand.Count > 0 || playerOneTable.Count > 0))
                {
                    //Player Two wins
                    retVal = Tuple.Create(playerTwo.user_id, playerOne.user_id, false);
                    Console.WriteLine("Winner: User "+ playerTwo.username);
                    end = true;
                    PostgreSqlClass DB = new PostgreSqlClass();
                    DB.AddElo(3, playerTwo.user_id);
                    DB.DeductElo(5, playerOne.user_id);
                    DB.Insert("UPDATE usertable SET num_games = num_games + 1 WHERE user_id=" + playerOne.user_id + ";");
                    DB.Insert("UPDATE usertable SET num_games = num_games + 1 WHERE user_id=" + playerTwo.user_id + ";");
                    DB.Insert("UPDATE usertable SET loss = loss + 1 WHERE user_id = " + playerOne.user_id + ";");
                    DB.Insert("UPDATE usertable SET win = win + 1 WHERE user_id = " + playerTwo.user_id + ";");
                }
                else if (numberOfRounds > 100 || (playerTwoHand.Count == 0 && playerOneHand.Count == 0 && CheckForSpells()))
                {
                    //Draw
                    retVal = Tuple.Create(playerOne.user_id, playerTwo.user_id, true);
                    Console.WriteLine("DRAW!!");
                    PostgreSqlClass DB = new PostgreSqlClass();
                    DB.Insert("UPDATE usertable SET num_games = num_games + 1 WHERE user_id=" + playerOne.user_id + ";");
                    DB.Insert("UPDATE usertable SET num_games = num_games + 1 WHERE user_id=" + playerTwo.user_id + ";");
                    end = true;
                }
                Console.ResetColor();

            }
            return retVal;
        }

        private void RemoveDeadCards(ref Dictionary<int, int> playerOneSpellUses, ref Dictionary<int, int> playerTwoSpellUses)
        {
            for (int i = playerOneTable.Count - 1; i >= 0; i--)
            {
                if (playerOneTable[i].GetCardType() == CardTypes.Spell && playerOneSpellUses.ContainsKey(playerOneTable[i].GetCardId()) && playerOneSpellUses[playerOneTable[i].GetCardId()] <= 0)
                {
                    Console.WriteLine(playerOneTable[i].GetCardName() + " is removed from the Table!");
                    playerOneTable.RemoveAt(i);
                }
                else if (playerOneTable[i].GetCardType() == CardTypes.Monster && playerOneTable[i].CheckAlive())
                {
                    Console.WriteLine(playerOneTable[i].GetCardName() + " is removed from the Table!");
                    playerOneTable.RemoveAt(i);
                }
            }
            for (int i = playerTwoTable.Count - 1; i >= 0; i--)
            {
                if (playerTwoTable[i].GetCardType() == CardTypes.Spell && playerTwoSpellUses.ContainsKey(playerTwoTable[i].GetCardId()) && playerTwoSpellUses[playerTwoTable[i].GetCardId()] <= 0)
                {
                    Console.WriteLine(playerTwoTable[i].GetCardName() + " is removed from the Table!");
                    playerTwoTable.RemoveAt(i);
                }
                else if (playerTwoTable[i].GetCardType() == CardTypes.Monster && playerTwoTable[i].CheckAlive())
                {
                    Console.WriteLine(playerTwoTable[i].GetCardName() + " is removed from the Table!");
                    playerTwoTable.RemoveAt(i);
                }
            }
        }

        private bool CheckForSpells()
        {
            bool noMonstersAlive = true;

            foreach(Card x in playerOneTable)
            {
                if(x.GetCardType() == CardTypes.Monster)
                {
                    noMonstersAlive = false;
                    break;
                }       
            }
            if (!noMonstersAlive)
            {
                foreach (Card x in playerTwoTable)
                {
                    if (x.GetCardType() == CardTypes.Monster)
                    {
                        noMonstersAlive = false;
                        break;
                    }
                }
            }
            return noMonstersAlive;
        }
    }
}

/*MUTEX Syntax : 
   private static Mutex mut = new Mutex();
   mut.WaitOne();
   mut.ReleaseMutex();
 */
