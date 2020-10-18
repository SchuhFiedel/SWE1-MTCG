using System;
using System.Collections.Generic;
using System.Text;
using MTCG;
using MTCG.Cards;
using MySql.Data.MySqlClient;


namespace MTCG.Util
{
    public class MySqlDataClass
    {
        public MySqlConnection databaseConnection;

        public MySqlDataClass()
        {
            setConnect();
        }

        public void setConnect()
        {
            string mySQLConnectionString = "datasource=127.0.0.1;port=3306; username=root;password=;database=mtcg;";
            databaseConnection = new MySqlConnection(mySQLConnectionString);
        }

        public void runQuery(string queryToRun)
        {
            MySqlCommand commandDatabase = new MySqlCommand(queryToRun, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try{
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                if (myReader.HasRows){
                    Console.WriteLine("Query Generated result:");

                    while (myReader.Read()){

                        Console.WriteLine(myReader.GetValue(0).GetType() + " - " + myReader.GetString(1).GetType() + " - " + myReader.GetString(2).GetType() + " - " + myReader.GetValue(3).GetType());
                                                    //id                        //firstname                         //givename                          //cardid
                        Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " +  myReader.GetString(2) +  " - "  + myReader.GetValue(3));
                    }
                }
                else{
                    Console.WriteLine("Query Successfully executed!");
                }
                
            }
            catch(Exception e){
                Console.WriteLine("Query Error: " + e.Message);
            }
        }

        public List<Card> getCardsFromDB()
        {
            // TO DO

            //throw new NotImplementedException();

            List<Card> cards = new List<Card>();
            string query = "SELECT * FROM cards;";
            Console.WriteLine(query);

            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();
                Console.WriteLine(myReader);
                
                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");

                    while (myReader.Read())
                    {
                        //string newCardName, CardTypes newCardType, ElementTypes newElement, SpecialType newSpecial, int maxHP, int maxAP, int maxDP, bool newPiercing
                        //Console.WriteLine(myReader.GetValue(0).GetType() + " - " + myReader.GetString(1).GetType() + " - " + myReader.GetString(2).GetType() + " - " + myReader.GetValue(3).GetType());
                        //id                        //firstname                         //givename                          //cardid
                        Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3) + " - " + myReader.GetValue(4) + " - " + myReader.GetValue(5) + " - " + myReader.GetValue(6) + " - " + myReader.GetValue(7) + " - " + myReader.GetValue(8) + " - " + myReader.GetValue(9));
                        Card tmp = new Card((int)myReader.GetValue(0), (string)myReader.GetString(1), (string)myReader.GetString(2), (CardTypes)Convert.ToInt32(myReader.GetValue(3)), (ElementTypes)Convert.ToInt32(myReader.GetValue(4)), (SpecialTypes)(int)myReader.GetValue(5), (int)myReader.GetValue(6), (int)myReader.GetValue(7), (int)myReader.GetValue(8), Convert.ToBoolean(myReader.GetValue(9)));
                        cards.Add(tmp);
                    }
                }
                else
                {
                    Console.WriteLine("Query Successfully executed!");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
            }

            return cards;
        }

    }
}
