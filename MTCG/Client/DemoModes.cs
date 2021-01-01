using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class DemoModes
    {
        int mode = 0;
        TcpClient client;
        NetworkStream stream;
        bool isLoggedIn = false;
        string authentication = "";
        string uri = "";
        string username = "";
        string password = "";

        public DemoModes(TcpClient t_client, NetworkStream stream, int mode, string uri)
        {
            client = t_client;
            this.stream = stream;
            this.mode = mode;
            this.uri = uri;
        }

        public void runDemo()
        {
            /*Console.WriteLine("DEMO MODE ONE (1) - Register new User, Login USer, Buy Coins, Buy Card Packs, Show All Cards, Set Deck, Battle, show Stats");
                Console.WriteLine("DEMO MODE TWO (2) - Login Herold25 with Credentials, Show Stats, show Scoreboard, ");
            */
            if(mode == 1)
            {
                username = "Barney";
                password = "Barney";

                DemoOne();
            }else if (mode == 2)
            {
                username = "Herold25";
                password = "Herold25";
                DemoTwo();
            }
        }

        public void DemoOne() // DEMO MODE ONE (1) - Register new User, Login USer, Buy Coins, Buy Card Packs, Show All Cards, Set Deck, Battle, show Stats
        {
            Tuple<string, string, string> sendInfo;
            int input = 0;
            Thread.Sleep(500);

            //Register User
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Register User:" + username +" " + password+ "!");
            Console.ResetColor();
            sendInfo = RegUser();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);
            
            //Login User
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Login User:" + username + " " + password + "!");
            Console.ResetColor();
            sendInfo = LoginUser();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Authentication Token set to:" + authentication + "!");
            Thread.Sleep(2000);

            //Get User Info

            Console.WriteLine("Get User Info for:" + username + " " + password + "!");
            Console.ResetColor();
            sendInfo = GetUserInfo();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(2000);

            //Update User Info
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Update User:" + username + "!");
            Console.ResetColor();
            sendInfo = UpdateUserInfo();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            //Get User Info
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get User Info for:" + username + " " + password + "!");
            Console.ResetColor();
            sendInfo = GetUserInfo();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(2000);

            //Buy More Coins
            string amount = "40";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Buy More Coins for:" + username + " Amount: " + amount + "!");
            Console.ResetColor();
            sendInfo = BuyCoins(amount);
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            //GEt User Stack
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Cardstack for:" + username + "!");
            Console.ResetColor();
            sendInfo = GetStacK();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //Buy Cardpack 1
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Buy Cardpack 1 for:" + username + "!");
            Console.ResetColor();
            sendInfo = BuyCardPacks("1");
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            //Buy Cardpack 3
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Buy Cardpack 3 for:" + username + "!");
            Console.ResetColor();
            sendInfo = BuyCardPacks("3");
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            //GEt User Stack
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Cardstack for:" + username + "!");
            Console.ResetColor();
            sendInfo = GetStacK();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //Get Deck 
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Carddeck for:" + username + "!");
            Console.ResetColor();
            sendInfo = GetDeck();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(2000);

            //put Cards into Deck
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Put Card 2 in Carddeck for:" + username + "!");
            Console.ResetColor();
            sendInfo = PutCardInDeck("2");
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Put Card 5 in Carddeck for:" + username + "!");
            Console.ResetColor();
            sendInfo = PutCardInDeck("5");
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Put Card 13 in Carddeck for:" + username + "!");
            Console.ResetColor();
            sendInfo = PutCardInDeck("13");
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Put Card 12 in Carddeck for:" + username + "!");
            Console.ResetColor();
            sendInfo = PutCardInDeck("12");
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(1000);

            //GEt Card deck
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get changed Carddeck for:" + username + "!");
            Console.ResetColor();
            sendInfo = GetDeck();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //Get Scoreboard
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Scoreboard!");
            Console.ResetColor();
            sendInfo = ScoreBoard();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //Go into Battle Mode
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Start matchmaking and Battle!");
            Console.ResetColor();
            do
            {
                sendInfo = BattleMode();
                SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
                ReadStream(ref input);
                Thread.Sleep(2000);
            } while (input == 12);
            Thread.Sleep(2000);

            //Get Scoreboard
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Scoreboard!");
            Console.ResetColor();
            sendInfo = ScoreBoard();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //Logout 
            SendToServer("DELETE", "/sessions", "");
            ReadStream(ref input);
            client.Close();
            Console.WriteLine("END OF DEMO ONE!");
        }

        public void DemoTwo()//DEMO MODE TWO(2) - Login Herold25 with Credentials, Show Stats, show Scoreboard,
        {

            Tuple<string, string, string> sendInfo;
            int input = 0;
            Thread.Sleep(500);

            //Login User
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Login User:" + username + " " + password + "!");
            Console.ResetColor();
            sendInfo = LoginUser();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Authentication Token set to:" + authentication + "!");
            Thread.Sleep(2000);

            //Get User Info
            Console.WriteLine("Get User Info for:" + username + " " + password + "!");
            Console.ResetColor();
            sendInfo = GetUserInfo();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //GET DECK
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Carddeck for:" + username + "!");
            Console.ResetColor();
            sendInfo = GetDeck();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //Get Scoreboard
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Scoreboard!");
            Console.ResetColor();
            sendInfo = ScoreBoard();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(4000);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Start matchmaking and Battle!");
            Console.ResetColor();
            do
            {
                sendInfo = BattleMode();
                SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
                ReadStream(ref input);
                Thread.Sleep(2000);
            } while (input == 12);
            Thread.Sleep(3000);

            //Get Scoreboard
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Get Scoreboard!");
            Console.ResetColor();
            sendInfo = ScoreBoard();
            SendToServer(sendInfo.Item1, sendInfo.Item2, sendInfo.Item3);
            ReadStream(ref input);
            Thread.Sleep(3000);

            //Logout 
            SendToServer("DELETE", "/sessions", "");
            ReadStream(ref input);
            Console.WriteLine("END OF DEMO TWO!");
        }


        public void SendToServer(string reqType, string path, string message)
        {
            //SEND MESSAGE TO SERVER
            string answerString = "";
            if (message.Length == 0)
            {
                answerString =
                        reqType + " " + path + " " + "HTTP/1.1\n" +
                        "Host: " + uri + "\n" +
                        "Authentication: " + authentication + "\n" +
                        "Content-Type: application/json; charset=utf-8\n";

                Console.WriteLine("Sent: \n {0}", answerString);
            }
            else
            {
                answerString =
                        reqType + " " + path + " " + "HTTP/1.1\n" +
                        "Host: " + uri + "\n" +
                        "Authentication: " + authentication + "\n" +
                        "Content-Length:" + message.Length + " \n" +
                        "Content-Type: application/json; charset=utf-8\n" +
                        "\n" + message;
                Console.WriteLine("Sent: \n {0}", message);
            }

            Byte[] reply = Encoding.ASCII.GetBytes(answerString);
            try
            {
                stream.Write(reply, 0, reply.Length);
                stream.Flush();
            }
            catch (System.IO.IOException e)
            {
                Console.Write("Server ended connection!", e);
            }
        }

        public string ReadStream(ref int input)
        {
            String response = String.Empty;
            if (client.Connected)
            {
                // Bytes Array to receive Server Response.
                Byte[] data = new Byte[200000];
                // Read the Tcp Server Response Bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                string[] messageLines = response.Split('\n');
                string[] tokens = messageLines[0].Split(' '); // first row
                int rowcounter = SearchDataRow(messageLines); // find row where Header ends
                Tuple<string[], string> allData = SplitData(rowcounter, messageLines); // split messageLines in HeaderArray and Payload string
                Dictionary<string, string> headers = GetHeaders(allData.Item1); //Split Header Array into Dictinary with Attributes and Values

                if (!isLoggedIn)
                {
                    Tuple<bool, string> auth = CheckAuthenticationData(allData.Item2);
                    isLoggedIn = auth.Item1;
                    authentication = auth.Item2;
                }
                if (Logout(allData.Item2))
                {
                    client.Close();
                }
                else if (InBattleMode(allData.Item2))
                {
                    input = 12;
                }
                else
                {
                    input = 0;
                }

                Console.WriteLine("Received:\n");
                for (int i = rowcounter; i < messageLines.Length; i++)
                {
                    Console.WriteLine(messageLines[i]);
                }
                Console.WriteLine("\n\n");
                //Thread.Sleep(2);
            }
            return response;
        }

        //search for row in which the HTTP Payload is
        private static int SearchDataRow(string[] messageLines)
        {
            int rowcounter = 0;
            foreach (string x in messageLines)
            {
                rowcounter += 1;
                if (x == "\r" || x == "\n" || x == "") // if any kind of new line, break
                {
                    break;
                }
            }
            return rowcounter;
        }

        //Make Json format strings more readable
        private static string RemoveUnnecessaryChars(string info)
        {
            info = info.Replace("\n", "").Replace("\r", "").Replace("\t", "")
                        .Replace("{", "").Replace("}", "").Replace(" \"", "").Replace("\"", "");
            return info;
        }

        //Split data into Headers and Payload
        private static Tuple<string[], string> SplitData(int rowcounter, string[] messageLines)
        {
            string[] a = new string[] { };
            string b = "";
            List<string> tmp = new List<string>();

            for (int i = 0; i < rowcounter - 1; i++)
            {
                tmp.Add(messageLines[i].Replace(" ", ""));
            }
            for (int i = 0; i + rowcounter < messageLines.Length; i++)
            {
                b += messageLines[i + rowcounter];
            }

            a = tmp.ToArray();
            Tuple<string[], string> addMessage = Tuple.Create(a, b);

            return addMessage;
        }

        //get all headers into a dictionary
        private static Dictionary<string, string> GetHeaders(string[] data)
        {
            Dictionary<string, string> Headers = new Dictionary<string, string>();

            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] tmp = data[i].Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "").Split(":");
                Headers.Add(tmp[0], tmp[1]);
            }
            return Headers;
        }

        //set  authentication Token which is received from the server
        private static Tuple<bool, string> CheckAuthenticationData(string info)
        {
            info = RemoveUnnecessaryChars(info);
            string[] attr = info.Split(',');

            bool success = false;
            string token = "";
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':');
                if (rowinfo[0] == "Login" && rowinfo[1] == "Success") { success = true; }
                if (rowinfo[0] == "Token") { token = rowinfo[1]; }
            }

            return Tuple.Create(success, token);

        }

        //Check if Matchmaking or Batteling
        public static bool InBattleMode(string info)
        {
            info = RemoveUnnecessaryChars(info);
            string[] attr = info.Split(',');

            bool success = false;
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':');
                if (rowinfo[0] == "Matchmaking" && rowinfo[1] != "Unsuccessful") { success = true; }
                if (rowinfo[0] == "Battle" && rowinfo[1] != "Over") { success = false; }
            }

            return success;
        }

        //REG DONE
        public Tuple<string,string,string> RegUser()
        {
            string message = "";
            string path = "/";
            string reqType = "";

            // Register User 
            //DONE
            reqType = "POST";
            path += "users";

            message = "{\n" +
                      "\"Username\": \"" + username + "\",\n" +
                      "\"Password\": \"" + password + "\"\n" +
                      "}";
            //END REGISTER

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //login DONE
        public Tuple<string,string,string> LoginUser()
        {
            string message = "";
            string path = "/";
            string reqType = "";

            //Login 
            //DONE
            reqType = "POST";
            path += "sessions";
            message = "{\n" +
                      "\"Username\": \"" + username + "\",\n" +
                      "\"Password\": \"" + password + "\"\n" +
                    "}";
            //END LOGIN   
            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //GET USER INFO DONE
        public Tuple<string,string,string> GetUserInfo()
        {

            string message = "";
            string path = "/";
            string reqType = "";
            // Get User Info 
            //DONE
            reqType = "GET";
            path += "users";
            //END GET USER INFO

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //Update User INFO DONE
        public Tuple<string, string,string> UpdateUserInfo()
        {
            string message = "";
            string path = "/";
            string reqType = "";

            // Update User Info
            //DONE
            reqType = "PUT";
            path += "users/";
            string bio = "Hey there I am Noob!";
            string image = "owO";
            string newUsername = "-";

            message = "{\n" +
                        "\"Username\": \"" + newUsername + "\",\n" +
                        "\"Bio\": \"" + bio + "\",\n" +
                        "\"Image\": \"" + image + "\"\n" +
                        "}";
            //END UPDATE USER INFO

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //BUY COINS DONE
        public Tuple<string,string,string> BuyCoins(string coins)
        {
            string message = "";
            string path = "/";
            string reqType = "";

            // Buy More Coins
            //DONE
            reqType = "POST";
            path += "transactions/coins";

            message = "{\n" +
                        "\"BuyAmount\": \"" + coins + "\"\n" +
                        "}";
            //END BUY MORE COINS

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //BUY CARD PACKS DONE
        public Tuple<string,string,string> BuyCardPacks(string packID)
        {
            string message = "";
            string path = "/";
            string reqType = "";

            // Buy Card  Packs
            //DONE
            reqType = "POST";
            path += "transactions/packages";

            message = "{\n" +
                        "\"PackageID\": \"" + packID + "\"\n" +
                        "}";
            //END BUY CARD PACKS

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //GET PLAYER CARD STACK DONE
        public Tuple<string,string,string> GetStacK()
        {
            string message = "";
            string path = "/";
            string reqType = "";
            // Show All Owned Cards
            //DONE
            reqType = "GET";
            path += "cards";
            //END SHOW ALL OWNED CARDS
            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //GET CARD DECK DONE
        public Tuple<string,string,string> GetDeck()
        {
            string message = "";
            string path = "/";
            string reqType = "";

            //Get User Card Deck
            //DONE
            reqType = "GET";
            path += "decks";
            //END SHOW ALL CARDS IN DECK

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //PUT CARD IN DECK
        public Tuple<string,string,string> PutCardInDeck(string cardID)
        {

            string message = "";
            string path = "/";
            string reqType = "";
            // Put Owned Card in Deck
            //DONE
            reqType = "PUT";
            path += "decks";

            message = "{\n" +
                        "\"CardId\": \"" + cardID + "\"\n" +
                        "}";
            //END PUT CARD IN DECK

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //Show ScoreBOard DONE
        public Tuple<string,string,string> ScoreBoard()
        {
            string message = "";
            string path = "/";
            string reqType = "";
            // Show ScoreBoard
            //DONE
            reqType = "GET";
            path += "score";
            //END GET SCOREBOARD

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        //SEND BATTLE MESSAGE DONE
        public Tuple<string, string, string> BattleMode()
        {
            string message = "";
            string path = "/";
            string reqType = "";

            // Battle Handler
            //TO-DO
            reqType = "POST";
            path += "battle";

            return Tuple.Create<string, string, string>(reqType, path, message);
        }

        public static bool Logout(string info)
        {
            info = RemoveUnnecessaryChars(info);
            string[] attr = info.Split(',');

            bool success = false;
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':');
                if (rowinfo[0] == "Logout" && rowinfo[1] == "Success") { success = true; }
            }

            return success;
        }
    }
}



    