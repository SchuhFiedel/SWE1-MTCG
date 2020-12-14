using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using MTCG;
using MTCG.Cards;
using Npgsql;


namespace MTCG.Util
{
    public class PostgreSqlClass
    {
        private NpgsqlConnection connection;

        public PostgreSqlClass()
        {
            SetConnect();
        }

        public void SetConnect()
        {
            string sqlConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";
            try
            {
                connection = new NpgsqlConnection(sqlConnectionString);
                connection.Open();
                Console.WriteLine("No Error - Connection Established!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public void Insert(string sqlStatement)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sqlStatement,connection);
            cmd.ExecuteNonQuery();
        }




        public List<Card> GetCardsFromDB()
        {
            List<Card> cards = new List<Card>();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cards", connection);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Card tmp = new Card(reader.GetInt32(0), //CardID
                                    reader.GetString(1), //Cardname
                                    reader.GetString(2), //CardInfo
                                    (CardTypes)Convert.ToInt32(reader.GetBoolean(3)), //CardType
                                    (ElementTypes)reader.GetInt32(4), //ElementType
                                    (SpecialTypes)reader.GetInt32(5), //SpecialType
                                    reader.GetInt32(6), //MaxHP
                                    reader.GetInt32(7), //MaxAP
                                    reader.GetInt32(8), //MaxDP
                                    reader.GetBoolean(9)); //piercing
                cards.Add(tmp);
            }

            Console.WriteLine("Got all them cards Mate!");
            return cards;
        }


        /* BEISPIEL FÜR SPEICHERUNG (OHNE SQL INJECTION MÖGLIcHKEIT)
        public void InsertError()
        {
            // Insert
            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO usertable (firstname, givenname, cardid) VALUES (@p,@a,@b);", connection);

            cmd.Parameters.AddWithValue("p", "");
            cmd.Parameters.AddWithValue("a", "");
            cmd.Parameters.AddWithValue("b", 4);
            //cmd.Prepare();
            cmd.ExecuteNonQuery();

        }*/

    }
}
