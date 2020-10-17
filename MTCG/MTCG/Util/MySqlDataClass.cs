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

        public void setConnect()
        {
            string mySQLConnectionString = "datasource=127.0.0.1;port=3306; username=root;password=;database=test;";
            MySqlConnection databaseConnection = new MySqlConnection(mySQLConnectionString);
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

            throw new NotImplementedException();

            List<Card> cards = new List<Card>();
            string query = "SELECT * FROM cards;";

            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");

                    while (myReader.Read())
                    {
                        //string newCardName, CardTypes newCardType, ElementTypes newElement, SpecialType newSpecial, int maxHP, int maxAP, int maxDP, bool newPiercing
                        //Console.WriteLine(myReader.GetValue(0).GetType() + " - " + myReader.GetString(1).GetType() + " - " + myReader.GetString(2).GetType() + " - " + myReader.GetValue(3).GetType());
                        //id                        //firstname                         //givename                          //cardid
                        //Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3));
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
