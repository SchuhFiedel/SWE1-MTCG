using System;
using System.Collections.Generic;
using System.Text;
using MTCG;
using MTCG.Cards;
using Npgsql;


namespace MTCG.Util
{
    public class PostgreSqlClass
    {
        //private NpgsqlConnection connection;

        public PostgreSqlClass()
        {}

        public NpgsqlConnection SetConnect()
        {
            string sqlConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg;Pooling=false";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(sqlConnectionString);
                connection.Open();
                Console.WriteLine("No Error - Connection Established!");
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public void Insert(string sqlStatement)
        {
            NpgsqlConnection connection = SetConnect();

            NpgsqlCommand cmd = new NpgsqlCommand(sqlStatement, connection);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public void RegUser(string username, string pwd)
        {
            NpgsqlConnection connection = SetConnect();

            NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO usertable (username, password, coins, elo, num_games) " +
                "VALUES (@a,@b,20,0,0);", connection);

            cmd.Parameters.AddWithValue("a", username);
            cmd.Parameters.AddWithValue("b", pwd);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public List<Card> GetCardsFromDB()
        {
            List<Card> cards = new List<Card>();

            NpgsqlConnection connection = SetConnect();
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

            connection.Close();
            Console.WriteLine("Got all them cards Mate!");
            return cards;
        }

        public User GetUser(string username, string pwd)
        {
            NpgsqlConnection connection = SetConnect();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM usertable WHERE username = @a AND password = @b;", connection);
            cmd.Parameters.AddWithValue("a", username);
            cmd.Parameters.AddWithValue("b", pwd);
            cmd.Prepare();

            NpgsqlDataReader reader = cmd.ExecuteReader();

            User user = null;

            if (reader.HasRows)
            {
                reader.Read();
                Console.WriteLine(reader.GetPostgresType(0));
                if (reader.IsDBNull(6) && reader.IsDBNull(7))
                {
                    user = new User(reader.GetString(1), reader.GetString(2), "","",
                                         reader.GetInt32(0), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
                }
                else if (reader.IsDBNull(6))
                {
                    user = new User(reader.GetString(1), reader.GetString(2), "", reader.GetString(7),
                                         reader.GetInt32(0), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
                }
                else if (reader.IsDBNull(7))
                {
                    user = new User(reader.GetString(1), reader.GetString(2), reader.GetString(6),"",
                                         reader.GetInt32(0), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
                }
                else
                {
                    user = new User(reader.GetString(1), reader.GetString(2), reader.GetString(6), reader.GetString(7),
                                         reader.GetInt32(0), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
                }

                reader.Close();
                connection.Close();
                return user;
            }
            else
            {
                reader.Close();
                connection.Close();
                throw new ArgumentException("User data not found!");
            }

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

        public string GetToken(int user_id)
        {
            NpgsqlConnection connection = SetConnect();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT token FROM validtokens WHERE user_id = @a;", connection);
            cmd.Parameters.AddWithValue("a", user_id);
            cmd.Prepare();

            NpgsqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var ret = reader.GetString(0);
                connection.Close();
                return ret;
            }
            else
            {
                connection.Close();
                return "NULL";
            }
        }
    }
}
